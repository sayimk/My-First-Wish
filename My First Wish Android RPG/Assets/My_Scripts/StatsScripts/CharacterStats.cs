using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour {

    //public values used to set the stat values in the inspector
    public int Inspector_Character_Level;
    public int Inspector_Required_Experience_Points;

    //GameObjects for UI Elements
    public GameObject UI_HP_Gauge;
    public GameObject UI_AttackUp;
    public GameObject UI_SpeedUp;
    public GameObject UI_DefenceUp;

    //Private StatClasses
    private StatClass HP;
    private StatClass Attack;
    private StatClass Defence;
    private StatClass Agility;

    //class increment Values
    private int maxStatPointsReceivable = 10;

    //Public character Information
    public int CharacterLevel { get; set; }
    public int Experience_Points { get; set; }
    public int Level_Experience_Required { get; set; }
    private int currentHP;
    private int StatPointsAvailable=0;

    //non-stacking buffs
    //skill Effect Vars
    //Speed Buffs
    bool agilityBuffActive = false;
    float agilityBuffEndTime;
    int agilityBuffedValue;

    //DefenceBuffs
    bool defenceBuffActive = false;
    float defenceBuffEndTime;
    int defenceBuffedValue;

    //AttackBuffs
    bool attackBuffActive = false;
    float attackBuffEndTime;
    int attackBuffedValue;

    //HPBuff
    bool HPBuffActive = false;
    float HPBuffEndTime;
    int HPBuffedValue;

	// Use this for initialization
	void Start () {

        //setting players Level
        CharacterLevel = Inspector_Character_Level;

        //Setting up stat Classes for the Player
        HP = new StatClass("HP","This is the total amount of health points that the user has and damage taken will reduce the amount of HP, " +
            "but can be restored through potions. When the health points hit 0, the user will faint and it will be Game Over.", Convert.ToInt32(((CharacterLevel * 60) / 1.7)));

        Attack = new StatClass("Attack", "This is the attack power that the user has, this affect amount of damage the user can inflict on the enemy. The higher the stat, the more damage can be inflicted on the enemy.", Convert.ToInt32(((CharacterLevel * 60) / 5.7)));

        Defence = new StatClass("Defence", "This is the amount of Defensive power the user has, this will reduce the amount of damage inflicted on the user when attacked. A higher Defence means more damage reduction.", Convert.ToInt32(((CharacterLevel * 60) / 6.5)));

        Agility = new StatClass("Agility", "This is how quick the user is, it affects skill cooldown time as well as frequency of the users attacks. The higher the stat, the more attacks the user can do and a further reduction in cooldown time", Convert.ToInt32(((CharacterLevel * 60) / 4.8)));

        //Setting Players Base Exp Requirement
        Level_Experience_Required = Inspector_Required_Experience_Points;

        //setting the players current HP to Max
        currentHP = HP.getModifiedStatValue();

        //updating UI HP Gauge
        updateUIHPGauge();
    }

    void Update() {

        if (agilityBuffActive) {

            if (Time.time > agilityBuffEndTime) {
                agilityBuffActive = false;
                agilityBuffedValue = 0;
                UI_SpeedUp.GetComponent<Image>().enabled = false;
                Debug.Log("Buff ended " + Time.time);
            }


        }

    }

    public void addExperiencePoints(int EXPAmount) {
        //figure out a formula for dynamic experience gain
        int experiencePointsGained = EXPAmount;

        if (EXPAmount!=0)
            DialogueSystem.Main.displayEXP(experiencePointsGained);

        //checks if experience points gain causes level up, if so gets exact points required and causes level up
        if ((Experience_Points + experiencePointsGained) >= Level_Experience_Required) {

            //used for leveling up
            DialogueSystem.Main.displayNotification("Level Up");

            if (experiencePointsGained != 0) {
                int exactExpRequired = (Level_Experience_Required - Experience_Points);
                experiencePointsGained = (experiencePointsGained - exactExpRequired);
                Experience_Points = Experience_Points + experiencePointsGained;

            } else {

                // this is for handling the recursive call for leveling up
                if (Experience_Points >= Level_Experience_Required)
                    Experience_Points = Experience_Points - Level_Experience_Required;
                else {
                    int exactExpRequired = (Level_Experience_Required - Experience_Points);
                    Experience_Points = (Experience_Points - exactExpRequired);
                }

            }

            //this will increase the level of the character and increase the exp cap
            CharacterLevel = CharacterLevel + 1;
            Level_Experience_Required = Convert.ToInt32(Level_Experience_Required*1.3);
            Debug.Log("Current exp has been reset");
            StatPointsAvailable = StatPointsAvailable + maxStatPointsReceivable;

            //increasing stat values from leveling up
            HP.statBaseValue = Convert.ToInt32(((CharacterLevel*60)/1.7));
            Attack.statBaseValue = Convert.ToInt32(((CharacterLevel*60)/5.7));
            Defence.statBaseValue = Convert.ToInt32(((CharacterLevel * 60) / 6.5));
            Agility.statBaseValue = Convert.ToInt32(((CharacterLevel * 60) / 4.8));

            //updating the HPGauge UI because of HP Stat change
            updateUIHPGauge();

            //this is to handle when the amount of EXP is too much and overflows to the next level requirement
            if (Experience_Points >= Level_Experience_Required)
                addExperiencePoints(0);

        } else {
            Experience_Points = Experience_Points + experiencePointsGained;
        }

        //updating the HPGauge UI 
        updateUIHPGauge();

    }

    public int getAvalibleStatPoints() {
        int temp = StatPointsAvailable;
        return temp;
    }

    //method called for increasing stats using checked and error checked skill points
    public void increaseStatValues(int HPAmount, int AttackAmount, int DefenceAmount, int AgilityAmount) {
        HP.statBaseValue = HP.statBaseValue + HPAmount;
        Attack.statBaseValue = Attack.statBaseValue + AttackAmount;
        Defence.statBaseValue = Defence.statBaseValue + DefenceAmount;
        Agility.statBaseValue = Agility.statBaseValue + AgilityAmount;

        StatPointsAvailable = StatPointsAvailable - (HPAmount + AttackAmount + DefenceAmount + AgilityAmount);
    }

    public int getCurrentHP() {
        return currentHP;
    }

    public List<StatModifierClass> getModifierList(string statName) {

        if (statName == "HP") {
            return HP.getModifierList();
        } else if (statName == "Attack") {
           return Attack.getModifierList();
        } else if (statName == "Defence") {
           return Defence.getModifierList();
        } else if (statName == "Agility") {
           return Agility.getModifierList();
        }

        return null;
    }


    //Adding StatBonuses
    public void addBonusModifier(List<StatModifierClass> modifiersList){
        string Modifiertype;

        //for each modifier get the type of Stat then add individually to that stat, if player HP is full then increase currentHP when HP is increased
        foreach (var modifier in modifiersList) {
            Modifiertype = modifier.getModifierType();

            if (Modifiertype == "HP") {
                if (currentHP == HP.getModifiedStatValue()) {
                    HP.addModifier(modifier);
                    currentHP = HP.getModifiedStatValue();
                }else
                    HP.addModifier(modifier);
            } else if (Modifiertype == "Attack") {
                Attack.addModifier(modifier);
            }else if (Modifiertype == "Defence") {
                Defence.addModifier(modifier);
            }else if (Modifiertype == "Agility") {
                Agility.addModifier(modifier);
            }
        }
    }

    public void removeBonusModifiers(List<StatModifierClass> modifiersList){
        string Modifiertype;

        //for each modifier get the type of Stat then remove individually to that stat
        foreach (var modifier in modifiersList) {
            Modifiertype = modifier.getModifierType();

            if (Modifiertype == "HP") {
                HP.removeModifier(modifier);
            } else if (Modifiertype == "Attack") {
                Attack.removeModifier(modifier);
            } else if (Modifiertype == "Defence") {
                Defence.removeModifier(modifier);
            } else if (Modifiertype == "Agility") {
                Agility.removeModifier(modifier);
            }
        }
    }

    public int getExperiencePoints() {
        int exp = Experience_Points;
        return exp;
    }

    //get Base Stat Values
    public int getStatBaseValue(string StatName){

        if (StatName == "HP") {
            return HP.statBaseValue;
        } else if (StatName == "Attack") {
            return Attack.statBaseValue;
        } else if (StatName == "Defence") {
           return Defence.statBaseValue;
        } else if (StatName == "Agility") {
            return Agility.statBaseValue;
        }

        return 0;
    }

    //get Stat values after modifiers have been applied
    public int getModifiedStatValue(string StatName){

        if (StatName == "HP") {
            return HP.getModifiedStatValue();
        } else if (StatName == "Attack") {
            return Attack.getModifiedStatValue();
        } else if (StatName == "Defence") {
            return Defence.getModifiedStatValue();
        } else if (StatName == "Agility") {
            return Agility.getModifiedStatValue();
        }

        return 0;
    }

    //get Stat Description
    public string getStatDescription(string statName) {
        if (statName == "HP") {
            return HP.statDescription;
        } else if (statName == "Attack") {
            return Attack.statDescription;
        } else if (statName == "Defence") {
            return Defence.statDescription;
        } else if (statName == "Agility") {
            return Agility.statDescription;
        }

        return "";
    }

    //method for adding damage to the player
    public void addDamageToPlayer(int enemyAttackStat, int enemyAgilityStat) {

        gameObject.GetComponent<MC_Script>().enterCombat();


        //Calculating damage reduction from player defence
        float totalDA = enemyAttackStat + Defence.getModifiedStatValue();
        float pAttack = enemyAttackStat;
        float fDamage = (pAttack * (pAttack / totalDA)) * 1.0f;

        //checking evasion chance based on Agility

        int agilityCal;

        if (agilityBuffActive) {
            agilityCal = agilityBuffedValue;
            Debug.Log("Used Buffed " + agilityBuffedValue);
        } else {
            agilityCal = Agility.getModifiedStatValue();
            Debug.Log("Used base" + agilityCal);
        }


        float total = enemyAgilityStat + agilityCal;
        Debug.Log(total);

        float percent = agilityCal / total;
        Debug.Log(percent);

        float evasionPercent = percent * 100;
        Debug.Log(evasionPercent);

        if (!willHitChance(((int)evasionPercent))) {
            fDamage = 0f;
            Debug.Log("------------ Player Evaded ---------------");
        }


        currentHP = currentHP - ((int) fDamage);

        if (currentHP < 0)
            currentHP = 0;

        updateUIHPGauge();

        if (currentHP == 0) {
            //Do something for Game Over
            Debug.Log("Game over !");
        }
    }

    //use this method to determine if the attack will hit
    public bool willHitChance(int percentageChance) {

        if (percentageChance > 100)
            percentageChance = 100;

        if (percentageChance < 0)
            percentageChance = 0;

        System.Random rnd = new System.Random();

        Debug.Log("Player Percentage was " + percentageChance);

        int genNo = rnd.Next(0, 101);

        Debug.Log("Player Number generated for Hit Chance was " + genNo);


        if (genNo > percentageChance)
            return true;
        else
            return false;
    }


    //calling this method will update the UI HP Gauge
    public void updateUIHPGauge() {

        //calculate the percentage of the current HP and the Total HP(HP Stat)

        float currentFloatHP = (float)currentHP;
        float MaxHPFloat = (float)HP.getModifiedStatValue();
        float HP_Percent = (currentFloatHP/ MaxHPFloat);
      
        UI_HP_Gauge.transform.Find("Bar").gameObject.GetComponent<Image>().fillAmount = HP_Percent;
        UI_HP_Gauge.transform.Find("CurrentHP").gameObject.GetComponent<Text>().text = currentHP.ToString();
        UI_HP_Gauge.transform.Find("MaxHP").gameObject.GetComponent<Text>().text = HP.getModifiedStatValue().ToString();

    }

    public void healPlayerHP(int amount) {

        if (currentHP + amount <= HP.getModifiedStatValue()) {
            currentHP = currentHP + amount;
        }else {
            currentHP = HP.getModifiedStatValue();
        }
    }

    //this will load the saved data after loading to a new floor
    public void loadData(PlayerObjectData playerObjectData) {
        HP.statBaseValue = playerObjectData.HPValue;
        currentHP = playerObjectData.currentHP;
        Attack.statBaseValue = playerObjectData.AttackValue;
        Defence.statBaseValue = playerObjectData.DefenceValue;
        Agility.statBaseValue = playerObjectData.AgilityValue;
        CharacterLevel = playerObjectData.CharacterLevel;
        Level_Experience_Required = playerObjectData.Level_Experience_Required;

        updateUIHPGauge();

        Debug.Log("Character Stats Loaded");
    }

    public void applySkillEffect(string statEffected, float multiplier, float durationInSeconds) {

        switch (statEffected) {
            default:
                break;

            case "Agility" : {

                    float fBaseAgility = Agility.getModifiedStatValue();

                    float f_agilityBuffedValue = ((fBaseAgility * multiplier));
                    agilityBuffedValue = (int)f_agilityBuffedValue;

                    agilityBuffEndTime = Time.time + durationInSeconds;
                    UI_SpeedUp.GetComponent<Image>().enabled = true;
                    agilityBuffActive = true;

                    Debug.Log("Buff Activated " + agilityBuffEndTime);
                
            }
                break;

            case "HP" : {
                    Debug.Log("Not Handled Yet, because no skills this at the moment");
            }
                break;

            case "Attack": {
                    Debug.Log("Not Handled Yet, because no skills this at the moment");

                }
                break;

            case "Defence": {
                    Debug.Log("Not Handled Yet, because no skills this at the moment");
                }
                break;
        }


    }
}
