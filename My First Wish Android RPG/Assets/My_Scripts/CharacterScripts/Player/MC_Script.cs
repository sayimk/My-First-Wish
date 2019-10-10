using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MC_Script: MonoBehaviour {

    NavMeshAgent playerMove;
    GameObject equippedWeapon;
    GameObject weaponHolder;

    //used for checking player movement status and interactions
    bool pendingAction = false;
    bool moving = false;
    string actionTag = "";
    int minDistance = 4;
    public int WeaponAttackRange;
    bool inCombat = false;
    GameObject CollidedObject = null;

    //used for attack timings
    public float standardAttackIntervalInSeconds;
    private float lastAttackTime;

    void Awake(){
        playerMove = gameObject.transform.parent.GetComponent<NavMeshAgent>();
    }

    //the Update method will check every frame to see if the distance between the player and
    //the interactable targets distance is less than 4, so they are close enough to start a dialogue
    void Update() {

        gameObject.transform.rotation = new Quaternion(0, 0, 0,0);

        if (moving&&(Vector3.Distance(gameObject.transform.parent.position, playerMove.destination) < ((float)minDistance))) {

            gameObject.GetComponent<Animator>().SetTrigger("stop");

            moving = false;

        }

        //checking if there is a pending action
        if (pendingAction) {

            //Checking distance

            try {

                 if (Vector3.Distance(gameObject.transform.position, CollidedObject.transform.position) < minDistance) {

                      //NPC Interaction
                        if (actionTag.Equals("NPC")) {
                            CollidedObject.transform.GetChild(0).GetComponent<NPC_Interface>().interact();

                            //if the NPC is a quest target, check to see if they have the NPCMarker script
                            if (CollidedObject.transform.GetChild(0).GetComponent<FindNPCQuestMarker>() != null) {
                                CollidedObject.transform.GetChild(0).GetComponent<FindNPCQuestMarker>().talkToQuestTargetNPC();
                            }

                            pendingAction = false;
                            actionTag = "";

                            //Item Chest Interaction
                        } else if (actionTag.Equals("ItemChest")) {
                            CollidedObject.GetComponent<ItemsHeld>().AddToPlayerInventory();
                            pendingAction = false;
                            actionTag = "";

                        //when the player arrives at the enemy, set inCombat to true and look at Enemy
                        } else if (actionTag.Equals("SubCharacter")) {
                            CollidedObject.GetComponent<SubCharactersBase>().activatePlayerStoryEvent();
                        gameObject.transform.parent.LookAt(CollidedObject.transform.parent);
                            pendingAction = false;
                            actionTag = "";

                        //when the player arrives at the enemy, set inCombat to true and look at Enemy
                        } else if (actionTag.Equals("Enemy")) {
                            //gameObject.transform.parent.LookAt(CollidedObject.transform.position);
                            inCombat = true;
                            pendingAction = false;
                            actionTag = "";
                        }
                 }

            } catch (NullReferenceException) {
                pendingAction = false;
                actionTag = "";
            }
        }

        //When the player is in combat the character will look and face the enemy
        if (inCombat) {

            try {
                gameObject.transform.parent.LookAt(CollidedObject.transform.parent.Find("FocusPoint").position);
                //gameObject.transform.parent.LookAt(new Vector3(CollidedObject.transform.position.x, CollidedObject.transform.position.y-2.6f, CollidedObject.transform.position.z));
            } catch (NullReferenceException) {
              //  inCombat = false;
              //  leaveCombat();
            } catch (MissingReferenceException) {
              //  inCombat = false;
             //   leaveCombat();
            }




            //Used for timing attacks and while in combat, with a global interval while in combat
            if (CollidedObject != null) {

                if (Vector3.Distance(gameObject.transform.parent.position, CollidedObject.transform.parent.position) < WeaponAttackRange) {
                
                    if (Time.time > (lastAttackTime + standardAttackIntervalInSeconds)) {

                        gameObject.GetComponent<Animator>().SetTrigger("standardAttack");

                        lastAttackTime = Time.time;
                    }
                }
            }
        }
    }

        //this handles moving the players character via touch and raycasting
        public void MoveCharacter(){
        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            CollidedObject = hit.collider.gameObject;
            playerMove.stoppingDistance = 0f;
            playerMove.destination = hit.point;



            if (!moving) {
                moving = true;
                gameObject.GetComponent<Animator>().SetTrigger("walk");
                minDistance = 2;
            }

            if (!(CollidedObject.tag == "Enemy")) {
                checkIfEnemiesInRange();
            }

            //checking if an interactable object has been touched or 'collided' with
            if ((CollidedObject.tag == "QuestNPC")) {

                //set the player stopping distance and then set a pending action and tag, then wait for 
                //player to arrive at destination
                playerMove.stoppingDistance = 3f;
                pendingAction = true;
                actionTag = "NPC";
                minDistance = 5;

                //now checking if the object is an ItemChest
            } else if (CollidedObject.tag == "SubCharacter") {
                playerMove.stoppingDistance = 3f;
                pendingAction = true;
                actionTag = "SubCharacter";
                minDistance = 5;

            } else if (CollidedObject.tag == "ItemChest") {
                playerMove.stoppingDistance = 3f;
                pendingAction = true;
                actionTag = "ItemChest";
                minDistance = 5;

                //setting enemy distance
            } else if (CollidedObject.tag == "Enemy") {
                playerMove.stoppingDistance = 5f;
                pendingAction = true;
                actionTag = "Enemy";
                minDistance = 6;
            
            }


            //Checks if still in combat, if not clicked on an enemy
            Collider[] objects = Physics.OverlapSphere(gameObject.transform.position, 4f);

            bool stillInCombat = false;

            if (objects.Length > 0) {

                foreach (var item in objects) {

                    if (item.tag.Equals("Enemy")) {
                        stillInCombat = true;
                        CollidedObject = item.gameObject;
                    }
                }
            }

            if (stillInCombat) {
                inCombat = true;
                Debug.Log("I am still in Combat");
                gameObject.transform.LookAt(CollidedObject.transform.position);
            }

        }
    }

    //call when in range of an enemy
    public void enterCombat() {
     //   try {
            equippedWeapon.GetComponent<WeaponsInterface>().enterCombat();
            gameObject.GetComponent<Animator>().SetTrigger("engageCombat");
            lastAttackTime = Time.time;
            World_System_Interface.main.stopAmbientAudio();
            World_System_Interface.main.playBattleAudio();
       // } catch (NullReferenceException) {

        //    DialogueSystem.Main.displayNotification("You do not have a weapon equipped");
      //  }
    }

    //call when in leaving range of an enemy
    public void leaveCombat() {
        try {
            gameObject.GetComponent<Animator>().SetTrigger("disengageCombat");
            World_System_Interface.main.stopBattleAudioWithFadeOut();
            World_System_Interface.main.playAmbientAudio();

        } catch (NullReferenceException) {
            //Doesn't need Any Handling
            //just means a weapon isn't equipped
        }
    }

    //weapon interaction methods
    //Button Event for a Quick attack
    public void quickAttack() {
        equippedWeapon.GetComponent<WeaponsInterface>().performQuickAttack(gameObject.GetComponent<Player_Log>());

    }

    //Button Event for a Heavy attack
    public void heavyAttack() {
        Debug.Log("Calling heavy");
        equippedWeapon.GetComponent<WeaponsInterface>().performHeavyAttack(gameObject.GetComponent<Player_Log>());
    }

    //this method is the event for the skill slot 1
    public void skillSlot1() {
        Debug.Log("Used Skill 1");
        gameObject.GetComponent<SkillsManager>().activateSkill1();
    }

    //this method is the event for the skill slot 2
    public void skillSlot2() {
        Debug.Log("Used Skill 2");
        gameObject.GetComponent<SkillsManager>().activateSkill2();
    }


    //this method changes the currently equipped weapon
    public void setCurrentWeapon(GameObject weapon) {
        equippedWeapon = weapon;
    }

    //this method changes the currently equipped weapon
    public void setCurrentWeaponHolder(GameObject holder) {
        weaponHolder = holder;
    }

    //Physics Engine When Enemy Enters Players Range
    void OnTriggerEnter(Collider col) {
        if (col.tag == "Enemy") {
            if (!inCombat) {
                enterCombat();
                CollidedObject = col.gameObject;
                inCombat = true;
            }
        }
    }

    //Physics Engine when Enemy Exits Players Range
    void OnTriggerExit(Collider col) {
        if (col.tag == "Enemy") {
            checkIfEnemiesInRange();
        }
    }

    //this method will check if the player is detected in a certain range by sending a pulse from the center outwards and will check the 
    //tags of them for the 'Player' tag
    public void checkIfEnemiesInRange() {

        bool enemyInRange = false;

        Debug.Log("Searching for Enemies");

        Collider[] objects = Physics.OverlapSphere(gameObject.transform.position, gameObject.GetComponent<SphereCollider>().radius);

        if (objects.Length > 0) {
            foreach (var item in objects) {

                if (item.tag.Equals("Enemy"))
                    enemyInRange = true;

            }
        }


        if (!enemyInRange) {

            leaveCombat();
           inCombat = false;
            Debug.Log("Enemies not in range");
        }

    }

    public void finishedAttackCallback(string attackAnimationName) {
        gameObject.transform.Find("QuickRigCharacter_RightHand").GetChild(0).gameObject.GetComponent<WeaponsInterface>().attackMotionFinishedCallback(attackAnimationName);
    }

    //this is used for catching animation events and disabling the right hand collider for enemy damage
    public void disableRightHandCollider() {
        gameObject.transform.Find("QuickRigCharacter_RightHand").GetComponent<BoxCollider>().enabled = false;
    }

    //this is used for catching animation events and enabling the right hand collider for enemy attacks
    public void enableRightHandCollider() {
        gameObject.transform.Find("QuickRigCharacter_RightHand").GetComponent<BoxCollider>().enabled = true;
    }

    //this is used for logic behind the engage animation and will enable the sword in the 
    //right hand and disable the holder or scabbach in the lefthand
    public void pullSwordFromHolder() {
        equippedWeapon.SetActive(true);
        weaponHolder.SetActive(false);
    }

    //does logic for disengage and will do the opposite of pullSwordFromHolder
    public void putSwordinHolder() {
        equippedWeapon.SetActive(false);
        weaponHolder.SetActive(true);
    }


}
