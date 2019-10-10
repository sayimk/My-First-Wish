using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyBaseClass : MonoBehaviour {

    //Spirits Initial Parameters
    public int Level;
    public int baseExpGain;
    protected int MaxHP;
    protected int currentHP;
    protected int AttackStat;
    protected int DefenceStat;
    protected int AgilityStat;

    //access to UI
    public GameObject HealthDisplay;
    public GameObject DamageDisplay;
    public GameObject TellSystem;
    public GameObject MainCamera;

    //Settings for Patrolling (startPatrol and resumePatrol)
    public bool enablePatrolling;
    public bool randomPatrolling;
    protected bool currentlyPatrolling;
    public List<Vector3> patrolCoordinates = new List<Vector3>();
    protected int lastPatrolPosition = 0;
    protected Vector3 patrolPosVector;
    protected bool movingToPatrolPos = false;


    //holding player instance on contact
    GameObject player;

    //used for monitoring player position and following
    protected bool inCombat = false;
    protected bool attack = false;
    protected bool stop = true;
    protected Vector3 playerLastPosition;
    protected Vector3 spiritsStartPosition;

    //cross method variables for active search (initActiveSearch) and (resumeActiveSearch)
    public bool enablePlayerActiveSearch;
    public float searchRadius;
    protected List<Vector3> searchAreas = new List<Vector3>();
    protected bool currentlySearching = false;
    protected bool movingToSearchArea = false;
    protected int areaSelector = 0;
    protected float savedStoppingDistance;

    //used for attack type prediction percentages
    protected float lastAttackTimeSeconds = 0;
    protected Dictionary<string, int> percentages = new Dictionary<string, int>();

    //Optional Action to perform on defeat
    Action actionOnDefeat;

    // Use this for initialization
    public virtual void Start() {

        HealthDisplay.transform.Find("LevelDisplay").gameObject.GetComponent<Text>().text =  Level.ToString();
        //storing the spirits initial position
        spiritsStartPosition = transform.position;
        savedStoppingDistance = gameObject.transform.parent.GetComponent<NavMeshAgent>().stoppingDistance;
        //disabling box collider to prevent interference between the two colliders
        gameObject.GetComponent<BoxCollider>().enabled = false;

        //setting values and keys for all possible attacks
        percentages.Add(Player_Log.QuickAttack, 0);
        percentages.Add(Player_Log.HeavyAttack, 0);
        percentages.Add(Player_Log.StandardAttack, 0);
        //----Add new Attacks here-----

        //setting up patrolling settings
        if (enablePatrolling && (patrolCoordinates.Count == 0))
            enablePatrolling = false;

        if (enablePatrolling) Patrol();
    }

    void Update() {
        //using Update to change the position of the HealthDisplay in relation to the enemy position every frame
        HealthDisplay.transform.LookAt(MainCamera.transform);
        TellSystem.transform.LookAt(MainCamera.transform);
        DamageDisplay.transform.LookAt(MainCamera.transform);


        //monitoring navMeshAgent
        if (movingToSearchArea && !inCombat) {


            if (gameObject.transform.parent.GetComponent<NavMeshAgent>().stoppingDistance != 0) {
                savedStoppingDistance = gameObject.transform.parent.GetComponent<NavMeshAgent>().stoppingDistance;
                gameObject.transform.parent.GetComponent<NavMeshAgent>().stoppingDistance = 0;
            }

            if (Vector3.Distance(searchAreas[areaSelector], gameObject.transform.parent.transform.position) < 2) {
                //gameObject.transform.parent.GetComponent<NavMeshAgent>().stoppingDistance = savedStoppingDistance;
                gameObject.GetComponent<Animator>().SetTrigger("startAreaSearch");

                searchAreas.RemoveAt(areaSelector);
                movingToSearchArea = false;
            }
        }

        //this will only check the distance between the player and a patrol position when not in combat and when moving
        if (movingToPatrolPos && !inCombat) {


            if (gameObject.transform.parent.GetComponent<NavMeshAgent>().stoppingDistance != 0) {
                savedStoppingDistance = gameObject.transform.parent.GetComponent<NavMeshAgent>().stoppingDistance;
                gameObject.transform.parent.GetComponent<NavMeshAgent>().stoppingDistance = 0;
            }

            if (Vector3.Distance(gameObject.transform.parent.transform.position, patrolPosVector) < 2) {


                movingToPatrolPos = false;
                gameObject.transform.parent.GetComponent<NavMeshAgent>().stoppingDistance = savedStoppingDistance;
                gameObject.GetComponent<Animator>().SetTrigger("waitAtPoint");


            }
        }


        // only face the player continuously when they are in combat
        if (inCombat && player != null)
            gameObject.transform.parent.transform.LookAt(new Vector3(player.transform.parent.position.x, 
                player.transform.parent.position.y+.7f, player.transform.parent.position.z));

    }


    //when the player enters range, start attacking and make them the target
    void OnTriggerEnter(Collider col) {
        if ("Player".Equals(col.tag)) {

            if (currentlySearching) {
                gameObject.GetComponent<Animator>().SetTrigger("attack_Player");
                currentlySearching = false;
            }

            if (currentlyPatrolling) {
                gameObject.GetComponent<Animator>().SetTrigger("endPatrol");
                currentlyPatrolling = false;
            }

            player = col.gameObject;
            TellSystem.transform.Find("CuriousSign").gameObject.GetComponent<Text>().enabled = false;
            TellSystem.gameObject.GetComponent<Animator>().SetTrigger("alerted");
            gameObject.transform.parent.GetComponent<NavMeshAgent>().stoppingDistance = savedStoppingDistance;
            attack = false;
            stop = true;
            inCombat = true;
            movingToSearchArea = false;
        }
    }

    /* this is the TriggerStay collider which is used to monitor the player when they are within range
     * this method will also follow the player and attack, if they try to run away
     */
    void OnTriggerStay(Collider col) {
        if ("Player".Equals(col.tag)) {
            player = col.gameObject;

            if (Vector3.Distance(gameObject.transform.parent.transform.position, player.transform.parent.transform.position) < 5) {

                if (!attack) {
                    gameObject.GetComponent<Animator>().SetTrigger("attack_Player");
                    attack = true;
                    stop = false;
                }
            } else {
                if (!stop) {
                    gameObject.GetComponent<Animator>().SetTrigger("stop_Attack");
                    attack = false;
                    stop = true;
                }
                moveToPlayer();
            }
        }
    }

    //when the player leaves range stop targeting them
    void OnTriggerExit(Collider col) {
        if ("Player".Equals(col.tag)) {
            playerLastPosition = player.transform.position;
            player = null;
            inCombat = false;
            checkIfPlayerInRange();
            if ((!currentlySearching) && enablePlayerActiveSearch)
                initActiveSearch();
        }
    }

    /*Method for adding Damage to the Enemy, also checks to see if the gameObject has 0 health and destroys if true
    *this method need the damage recieved in integer and the AttackName must be in the format of 'QuickAttack'
    *
    */
    public int addEnemyDamage(int damageReceived, string AttackName, bool isSkill, int skillAccuracy) {

        //this will log the players new attack

        Debug.Log(isSkill);

        if (!isSkill) {
            player.GetComponent<Player_Log>().logPlayersAttack(AttackName);
        } else {
            //if the Attack is a skill then do some other type of logging
        }
        //this is whether or not the attack from the player will hit the enemy

        float hpPercent = ((float)currentHP / (float)MaxHP) * 100;



        //if the enemy is less than 40% health then enemy will become more defensive and the evasion chance will increase
        if (hpPercent < 40) {

            if (!isSkill) {
                int percentChange = (percentages[AttackName]);

                //Double Check this
                if (!willHitChance(percentChange + 20)) {
                    damageReceived = 0;
                }
            } else {

                if (!willHitChance(100 - (skillAccuracy - 10))) {
                    damageReceived = 0;
                }


            }

        } else {

            if (!isSkill) {

                if (!willHitChance((percentages[AttackName] - 30))) {
             
                    damageReceived = 0;
                }
            } else {
                if (!willHitChance(100 - skillAccuracy)) {
                    damageReceived = 0;
                }
            }
        }



        //this method is used to calculate the chances of which attacks will come next, these percentages will be used to 

        if (!isSkill)
            getHitEvadePercentage();

        currentHP = currentHP - damageReceived;

        if (currentHP < 0)
            currentHP = 0;

        DamageDisplay.transform.Find("Text").GetComponent<Text>().text = "";
        DamageDisplay.transform.Find("Text").GetComponent<Animator>().SetTrigger("display");

        if (damageReceived != 0)
            DamageDisplay.transform.Find("Text").GetComponent<Text>().text = damageReceived.ToString();
        else {
            DamageDisplay.transform.Find("Text").GetComponent<Text>().text = "Missed";
        }
        updateHPBar();

        if (currentHP == 0) {
            if (player != null) {

                //Calculating the Exp Gain
                //baseExpGain

                if (player.GetComponent<CharacterStats>().CharacterLevel == Level)
                    baseExpGain = baseExpGain/10;
                else if ((Level - player.GetComponent<CharacterStats>().CharacterLevel) <= 2)
                    baseExpGain = baseExpGain / 2;

                player.GetComponent<CharacterStats>().addExperiencePoints(baseExpGain);
                player.GetComponent<Player_Log>().incrementTotalEnemyDefeated("Spirit");

                if (actionOnDefeat != null) {
                    actionOnDefeat.DynamicInvoke();
                    actionOnDefeat = null;
                }
            }
            Destroy(gameObject.transform.parent.gameObject);


        }

        return currentHP;
    }



    //this method will Add Enemey damage to the Player and decrease their health
    public void addDamageToPlayer() {

        if (player != null) {
            player.GetComponent<CharacterStats>().addDamageToPlayer(AttackStat, AgilityStat);
        }

    }

    //method for updating the HP Bar in World Space
    void updateHPBar() {

        float currentHPFloat = (float)currentHP;
        float maxHPFloat = (float)MaxHP;
        float percentageHP = (float)(currentHPFloat / maxHPFloat);

        HealthDisplay.transform.Find("Bar").gameObject.GetComponent<Image>().fillAmount = percentageHP;
    }

    //this method will make the enemy approach the player
    public void moveToPlayer() {

        if (player != null) {
            gameObject.transform.parent.GetComponent<NavMeshAgent>().stoppingDistance = savedStoppingDistance;
            gameObject.transform.parent.GetComponent<NavMeshAgent>().SetDestination(
                new Vector3(player.transform.parent.position.x, player.transform.parent.position.y,
                player.transform.parent.position.z));
        }
    }

    //this is a 'tell' method and will warn the player of an attack, in attempt to give the player time to prepare
    public void warnPlayerOfStandardAttack() {
        TellSystem.gameObject.GetComponent<Animator>().SetTrigger("standard_attack_warning");
    }

    //use this method to determine if the attack will hit
    public bool willHitChance(int percentageChance) {

        if (percentageChance > 100)
            percentageChance = 100;

        if (percentageChance < 0)
            percentageChance = 0;

        System.Random rnd = new System.Random();

        int genNo = rnd.Next(0, 101);

        if (genNo > percentageChance)
            return true;
        else
            return false;
    }

    //this method is used for calculating the percentage that the attack will hit, 
    //based on the current hit change and the previous attack
    /*
    *Attack cooldowns:
    * - Heavy Attack = 10s, 15s with 5s offset
    * - Quick Attack = 3s, 5s with 2s offset
    */
    public int getHitEvadePercentage() {

        //Initializing required datatypes
        Player_Log player_Log = player.GetComponent<Player_Log>();
        Dictionary<string, int> temp = new Dictionary<string, int>(); //used for temperarily holding values 


        //this for each uses the player's combat history to calculate the base percentage changes
        foreach (var attackType in percentages) {

            float tempCount = player_Log.getLogDataForSpecificAttack(attackType.Key);
            float tempTotal = player_Log.getTotalLoggedAttacks();
            float initialPercentage = (tempCount / tempTotal) * 100;

            temp.Add(attackType.Key, (int)initialPercentage);
        }

        //moving temp values into main Dictionary
        foreach (var newVal in temp) {
            percentages[newVal.Key] = newVal.Value;
        }

        //emptying temp holder
        temp.Clear();

        //these are percentage adjustments based on simple tactics and cooldown times

        //Heavy attack checks and adjustments

        //if it was the last attack
        if (player_Log.getLastAttack().Equals(Player_Log.HeavyAttack))
            if (percentages[Player_Log.HeavyAttack] > 0) {

                int half = percentages[Player_Log.HeavyAttack] / 2;
                percentages[Player_Log.HeavyAttack] = percentages[Player_Log.HeavyAttack] - half;

                if (percentages[Player_Log.QuickAttack] + half < 100)
                    percentages[Player_Log.QuickAttack] = percentages[Player_Log.QuickAttack] + half;
            }

        //if the last attack is within 15s, normally would be 100 percent, but allows a lucky attack to the player, hence 15%
        if (player_Log.getLastAttack().Equals(Player_Log.HeavyAttack) && (getAttackTimeDifference() < 10)) {
            percentages[Player_Log.HeavyAttack] = 0;
            percentages[Player_Log.QuickAttack] = 85;
            percentages[Player_Log.StandardAttack] = 15;
        }


        //Quick attack checks and adjustments

        //if the last attack is within 5s
        if (player_Log.getLastAttack().Equals(Player_Log.QuickAttack) && (getAttackTimeDifference() <= 5)) {
            percentages[Player_Log.HeavyAttack] = 85;
            percentages[Player_Log.QuickAttack] = 0;
            percentages[Player_Log.StandardAttack] = 25;
        }

        //      use method check 2nd last attack
        if (player_Log.get2ndLastAttack().Equals(Player_Log.QuickAttack) && (getAttackTimeDifference() > 5)) {
            percentages[Player_Log.HeavyAttack] = percentages[Player_Log.HeavyAttack] - 10;
            percentages[Player_Log.QuickAttack] = percentages[Player_Log.QuickAttack] + 30;
            percentages[Player_Log.StandardAttack] = percentages[Player_Log.StandardAttack] - 10;
        }

        lastAttackTimeSeconds = Time.time;
        return 0;
    }


    //this method is used to get the about of time elapsed since the last attack, to help calculate percentages for attack evasion
    public float getAttackTimeDifference() {

        return Time.time - lastAttackTimeSeconds;

    }

    //calling this method will start an active search to look for player within a certain range
    public void initActiveSearch() {

        //generate the search coordinates and save them in an list of vectors3
        // use a random number gen to check pick a coordinate

        //use animation rotateSearch to slowly spin around and looks for player, interrupt when found the player
        //if completed search using a callback method from the anaimation, remove position from array and pick a new number from
        //the new range and repeat.
        //if finish randomly chose to either return to old position or wait at the last location

        //changing passive colider to long range search collider

        currentlySearching = true;

        //setting the search locations

        searchAreas.Clear();

        //adding search areas
        searchAreas.Add(new Vector3((playerLastPosition.x + searchRadius), playerLastPosition.y, playerLastPosition.z));
        searchAreas.Add(new Vector3((playerLastPosition.x - searchRadius), playerLastPosition.y, playerLastPosition.z));

        searchAreas.Add(new Vector3(playerLastPosition.x, playerLastPosition.y, (playerLastPosition.z + searchRadius)));
        searchAreas.Add(new Vector3(playerLastPosition.x, playerLastPosition.y, (playerLastPosition.z - searchRadius)));
        //can add more points later



        gameObject.GetComponent<Animator>().SetTrigger("lostTarget");
    }



    public void resumeActiveSearch() {


        gameObject.GetComponent<Animator>().SetTrigger("stopAreaSearch");

        if (!inCombat) {
            TellSystem.transform.Find("CuriousSign").gameObject.GetComponent<Text>().enabled = true;

            System.Random rnd = new System.Random();

            if (searchAreas.Count > 0) {

                areaSelector = rnd.Next(0, searchAreas.Count);



                gameObject.transform.parent.GetComponent<NavMeshAgent>().SetDestination(searchAreas[areaSelector]);
                //savedStoppingDistance = gameObject.transform.parent.GetComponent<NavMeshAgent>().stoppingDistance;
                //gameObject.transform.parent.GetComponent<NavMeshAgent>().stoppingDistance = 0;

                movingToSearchArea = true;



            } else {

                TellSystem.transform.Find("CuriousSign").gameObject.GetComponent<Text>().enabled = false;
                TellSystem.GetComponent<Animator>().SetTrigger("thinking");

                if (enablePatrolling)
                    Patrol();
                else {

                    //change to continue patrolling if it is enabled
                    if (rnd.Next(0, 2) == 0) {
                        gameObject.transform.parent.GetComponent<NavMeshAgent>().stoppingDistance = 0;
                        gameObject.transform.parent.GetComponent<NavMeshAgent>().SetDestination(spiritsStartPosition);
                    } else {
                        gameObject.transform.parent.GetComponent<NavMeshAgent>().stoppingDistance = 0;
                        gameObject.GetComponent<NavMeshAgent>().SetDestination(playerLastPosition);
                    }

                    currentlySearching = false;
                }

            }
        } else {
            gameObject.GetComponent<Animator>().SetTrigger("stopAreaSearch");
            searchAreas.Clear();
        }
    }


    public void Patrol() {

        currentlyPatrolling = true;

        gameObject.GetComponent<Animator>().SetTrigger("endPatrol");

        if (!inCombat) {

            System.Random rnd = new System.Random();

            bool validNext = false;
            int newPosition = 0;

            if (randomPatrolling) {
                while (!validNext) {

                    newPosition = rnd.Next(0, patrolCoordinates.Count);



                    if (lastPatrolPosition != newPosition)
                        validNext = true;

                    lastPatrolPosition = newPosition;

                }

            } else {
                if (lastPatrolPosition < (patrolCoordinates.Count - 1))
                    lastPatrolPosition = lastPatrolPosition + 1;
                else
                    lastPatrolPosition = 0;
            }



            patrolPosVector = patrolCoordinates[lastPatrolPosition];


            gameObject.transform.parent.GetComponent<NavMeshAgent>().stoppingDistance = 0;
            gameObject.transform.parent.GetComponent<NavMeshAgent>().SetDestination(patrolPosVector);


            movingToPatrolPos = true;



        }
    }


    public void checkIfPlayerInRange() {

        bool playerInRange = false;

        Collider[] objects = Physics.OverlapSphere(gameObject.transform.parent.transform.position, gameObject.GetComponent<SphereCollider>().radius);

        if (objects.Length > 0) {
            foreach (var item in objects) {

                if (item.tag.Equals("Player"))
                    playerInRange = true;

            }
        }


        if (!playerInRange) {

            inCombat = false;

            gameObject.GetComponent<Animator>().SetTrigger("stop_Attack");
        }

    }

    public void enableThinkingTell() {

        TellSystem.GetComponent<Animator>().SetTrigger("thinking");

    }

    public int getDefenceStat() {
        return DefenceStat;
    }

    public int getAttackStat() {
        return AttackStat;
    }

    public int getAgility() {
        return AgilityStat;
    }

    public void setActionToPerformOnDefeat(Action actionToPerform) {
        actionOnDefeat = actionToPerform;
    }
}
