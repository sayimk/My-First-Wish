using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this is the skill manager and it will handle the players skills
public class SkillsManager : MonoBehaviour {

    //List of players Available Skills, contains 2 different types of skills, Offensive and Defensive skills
    List<SkillClass> playerSkills = new List<SkillClass>();

    //these are references to the UI Slots and can be used to call cooldowns and switch images for buttons
    public GameObject UISkillSlot1;
    public GameObject UISkillSlot2;

    //Used for calling a skill Attack on the weapon in the players Right Hand

    //these are the skill slots of the players active skills
    SkillClass activeSkill1;
    SkillClass activeSkill2;

    CharacterStats playerStats;

    void Start() {

        playerStats = gameObject.GetComponent<CharacterStats>();

        activeSkill1 = new SkillClass();
        activeSkill2 = new SkillClass();

        //player skills

        //SecretArts: 2nd Form Fatal, renamed to Gale Return
        SkillClass Fatal_Return = new SkillClass(SkillClass.defensive_Keyword, 100, false, 5f, "Fatal_Return_Icon", "FatalReturn", "FatalReturn", 5f, "Agility", "SecretArts 2nd Form: Fatal Return",
            "The 2nd form of the SecretArts school, a quick and powerful counter attack, " +
            "where the user pull the blade to the side horizontally, then after evading incoming attack, unleashes a powerful slash, " +
            "along with powerful wind blades", 3f);

        Fatal_Return.configureSkillSecondaryEffect(SkillClass.offensive_Keyword, 1.2f, "", 0.0f);
   
        //SecretArts:6th Form Fuji Stance


        //SecretArts: 5th Form Descent of the Dragon




        addNewSkill(Fatal_Return);

    }


    public void activateSkill1() {

        bool performedMove = false;

        //handling primary skill effect
        if (activeSkill1.OffensiveOrDefensive().Equals(SkillClass.offensive_Keyword)&& (activeSkill1!=null)) {

            if (gameObject.transform.Find("QuickRigCharacter_RightHand").childCount > 0) {

                gameObject.transform.Find("QuickRigCharacter_RightHand").GetChild(0).gameObject.GetComponent<WeaponsInterface>()
                        .performSkillAttack(activeSkill1.getSkillName(), calculateOffensiveSkillDamage(activeSkill1.getMultiplierValue()), activeSkill1.getAccuracy());
                performedMove = true;

            }

        }else if(activeSkill1.OffensiveOrDefensive().Equals(SkillClass.defensive_Keyword) && (activeSkill1 != null)) {


                gameObject.GetComponent<CharacterStats>().applySkillEffect(activeSkill1.getStatEffectedByPrimaryEffect(), 
                    activeSkill1.getMultiplierValue(), activeSkill1.getSkillDuration());

            performedMove = true;


        }


        //Handle second effects here

        if (activeSkill1.getSecondaryEffectType().Equals(SkillClass.offensive_Keyword)) {

            gameObject.transform.Find("QuickRigCharacter_RightHand").GetChild(0).gameObject.GetComponent<WeaponsInterface>()
                        .performSkillAttack(activeSkill1.getSkillName(), calculateOffensiveSkillDamage(activeSkill1.getSecondaryEffectMultiplier()), activeSkill1.getAccuracy());

            performedMove = true;


        } else if (activeSkill1.getSecondaryEffectType().Equals(SkillClass.defensive_Keyword)) {

            gameObject.GetComponent<CharacterStats>().applySkillEffect(activeSkill1.getStatEffectedBySecondaryEffect(),
                activeSkill1.getSecondaryEffectMultiplier(), activeSkill1.getSecondaryEffectDuration());

            performedMove = true;

        }






        if (performedMove) {
            UISkillSlot1.GetComponent<Animator>().SetTrigger(activeSkill1.getCooldownAnimationTrigger());
            Debug.Log(activeSkill1.getskillAnimationTrigger());
            gameObject.GetComponent<Animator>().SetTrigger(activeSkill1.getskillAnimationTrigger());
        }


    }

    public void activateSkill2() {

        bool performedMove = false;

        //handling primary skill effect
        if (activeSkill2.OffensiveOrDefensive().Equals(SkillClass.offensive_Keyword) && (activeSkill2 != null)) {

            if (gameObject.transform.Find("QuickRigCharacter_RightHand").childCount > 0) {

                gameObject.transform.Find("QuickRigCharacter_RightHand").GetChild(0).gameObject.GetComponent<WeaponsInterface>()
                        .performSkillAttack(activeSkill2.getSkillName(), calculateOffensiveSkillDamage(activeSkill2.getMultiplierValue()), activeSkill2.getAccuracy());
                performedMove = true;

            }

        } else if (activeSkill2.OffensiveOrDefensive().Equals(SkillClass.defensive_Keyword) && (activeSkill2 != null)) {

            gameObject.GetComponent<CharacterStats>().applySkillEffect(activeSkill2.getStatEffectedByPrimaryEffect(),
                activeSkill2.getMultiplierValue(), activeSkill2.getSkillDuration());

            performedMove = true;


        }


        //Handle second effects here

        if (activeSkill2.getSecondaryEffectType().Equals(SkillClass.offensive_Keyword)) {

            gameObject.transform.Find("QuickRigCharacter_RightHand").GetChild(0).gameObject.GetComponent<WeaponsInterface>()
                        .performSkillAttack(activeSkill2.getSkillName(), calculateOffensiveSkillDamage(activeSkill2.getSecondaryEffectMultiplier()), activeSkill2.getAccuracy());

            performedMove = true;


        } else if (activeSkill2.getSecondaryEffectType().Equals(SkillClass.defensive_Keyword)) {

            gameObject.GetComponent<CharacterStats>().applySkillEffect(activeSkill2.getStatEffectedBySecondaryEffect(),
                activeSkill2.getSecondaryEffectMultiplier(), activeSkill2.getSecondaryEffectDuration());

            performedMove = true;

        }






        if (performedMove) {
            UISkillSlot2.GetComponent<Animator>().SetTrigger(activeSkill2.getCooldownAnimationTrigger());
            Debug.Log(activeSkill2.getskillAnimationTrigger());
            gameObject.GetComponent<Animator>().SetTrigger(activeSkill2.getskillAnimationTrigger());
        }


    }

    public void setSkill1(string skill_Name) {

        bool set = false;

        UISkillSlot1.GetComponent<Image>().enabled = true;

        for (int i = 0; i < playerSkills.Count; i++) {
            if (playerSkills[i].getSkillName().Equals(skill_Name)) {
                activeSkill1 = playerSkills[i];
                UISkillSlot1.GetComponent<Image>().sprite = Resources.Load<Sprite>("Skill_Icons/" + playerSkills[i].getIconSpriteName());
                set = true;
            }
        }

        if (!set)
            throw new System.Exception("SkillSlot1: Skill Does Not Exist");



    }

    public void setSkill2(string skill_Name) {

        bool set = false;

        UISkillSlot2.GetComponent<Image>().enabled = true;

        for (int i = 0; i < playerSkills.Count; i++) {
            if (playerSkills[i].getSkillName().Equals(skill_Name)) {
                activeSkill2 = playerSkills[i];
                UISkillSlot2.GetComponent<Image>().sprite = Resources.Load<Sprite>("Skill_Icons/" + playerSkills[i].getIconSpriteName());
                set = true;
            }
        }

        if (!set)
            throw new System.Exception("SkillSlot2: Skill Does Not Exist");

    }

    //used for adding a new skill
    public bool addNewSkill(SkillClass newSkill) {

        for(int i = 0; i <playerSkills.Count; i++) {
            if (playerSkills[i].getSkillName().Equals(newSkill.getSkillName()))
                return false;

            if (playerSkills[i].getIconSpriteName().Equals(newSkill.getIconSpriteName()))
                return false;
        }

        playerSkills.Add(newSkill);
        return true;

    }

    //used to get the total amount of skills
    public int getTotalSkillCount() {

        return playerSkills.Count;

    }

    public SkillClass getSkillInfoAt(int location) {

        return playerSkills[location];

    }

    public SkillClass findSkillByIconReference(string iconName) {

        int pos = -1;

        for (int i = 0; i < playerSkills.Count; i++) {

            if (playerSkills[i].getIconSpriteName().Equals(iconName))
                pos = i;
        }

        if (pos==-1)
            throw new System.Exception("Skill Not Found by Icon Reference");

        return playerSkills[pos];

    }

    public SkillClass getSlot1Skill() {
        return activeSkill1;
    }

    public SkillClass getSlot2Skill() {
        return activeSkill2;
    }

    public bool checkIfSpecificSkillSlotIsNull(int i) {

        if ((i == 1) && (activeSkill1.getSkillName().Equals("")))
            return true;
        else if ((i == 2) && (activeSkill2.getSkillName().Equals("")))
            return true;


        return false;
    }


    private int calculateOffensiveSkillDamage(float multiplier) {
        //fill in
        
        float damage = 0;

            damage = playerStats.getModifiedStatValue("Attack") * multiplier;
        Debug.Log(playerStats.getModifiedStatValue("Attack")+" "+multiplier+" "+damage);

        return ((int)damage);
    }

    public List<SkillClass> getAllPlayerSkills() {
        List<SkillClass> temp = playerSkills;
        return temp;
    }

    //this method will load all the skill data from the playerObjectData
    public void loadData(PlayerObjectData playerObjectData) {

        playerSkills.Clear();
        playerSkills.AddRange(playerObjectData.playerSkills);

        //checking if the saved skills are not null then fetch and equip
        if (!playerObjectData.activeSkill1.skillName.Equals(""))
            setSkill1(playerObjectData.activeSkill1.skillName);

        if (!playerObjectData.activeSkill2.skillName.Equals(""))
            setSkill2(playerObjectData.activeSkill2.skillName);


        Debug.Log("Skill Manager Loaded");
    }

}
