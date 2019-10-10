using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class forms the basis of the Skills
[System.Serializable]
public class SkillClass{

    //Skill Parameters, need to be public to be serializable JSON
    public string skillTyping;
    public int accuracy;
    public string cooldownTriggerName;
    public float cooldownTime;
    public string skillAnimationTrigger;
    public float multiplier;
    public string skillName;
    public string skillDescription;
    public bool locked;
    public string iconSpriteName;
    public string secondaryEffectType;
    public float secondaryEffectMultiplier;
    public string StatEffectedByPrimaryEffect;
    public string StatEffectedBySecondaryEffect;
    public float SkillDuration;
    public float secondaryEffectSkillDuration;

    //Skill Types constants
    public const string defensive_Keyword = "Defensive";
    public const string offensive_Keyword = "Offensive";

    //Empty constructor
    public SkillClass() {
        skillName = "";
    }

    //Skill creation method
    public SkillClass(string skillClass_Constant_Offensive_or_Defensive, int accuracy, bool isSkilllocked, float cooldownTime, string iconSpriteName,
        string cooldownAnimationTrigger, string skillAnimationTrigger, float multiplier, string optional_StatEffectedByPrimaryEffect,
        string skillName, string skillDescription, float SkillDuration) {

        this.cooldownTriggerName = cooldownAnimationTrigger;
        this.multiplier = multiplier;
        this.skillName = skillName;
        this.skillDescription = skillDescription;
        locked = isSkilllocked;
        this.skillAnimationTrigger = skillAnimationTrigger;
        this.cooldownTime = cooldownTime;
        this.accuracy = accuracy;
        this.iconSpriteName = iconSpriteName;
        StatEffectedByPrimaryEffect = optional_StatEffectedByPrimaryEffect;
        skillTyping = skillClass_Constant_Offensive_or_Defensive;
        this.SkillDuration = SkillDuration;

        //optional Secondary Effect
        this.secondaryEffectType = "";
        secondaryEffectMultiplier = 0.0f;
        this.StatEffectedBySecondaryEffect = "";
        secondaryEffectSkillDuration = 0.0f;
    }

    //used to configure the secondary skill effect
    public void configureSkillSecondaryEffect(string secondaryEffectType, float secondaryEffectMultiplier, string StatEffectedBySecondaryEffect,
        float secondaryEffectSkillDuration) {

        this.secondaryEffectType = secondaryEffectType;
        this.secondaryEffectMultiplier = secondaryEffectMultiplier;
        this.StatEffectedBySecondaryEffect = StatEffectedBySecondaryEffect;
        this.secondaryEffectSkillDuration = secondaryEffectSkillDuration;

    }

    //fetch Methods
    public int getAccuracy() {
        return accuracy;
    }

    public string getCooldownAnimationTrigger() {
        return cooldownTriggerName;
    }

    public string getIconSpriteName() {
        return iconSpriteName;
    }

    public string getskillAnimationTrigger() {
        return skillAnimationTrigger;
    }

    public float getRequiredCooldownTime() {
        return cooldownTime;
    }

    public float getMultiplierValue() {
        return multiplier;
    }

    public string getSkillName() {
        return skillName;
    }

    public string getSkillDescription() {
        return skillDescription;
    }

    public string getStatEffectedByPrimaryEffect() {
        return StatEffectedByPrimaryEffect;
    }

    public string OffensiveOrDefensive() {
        return skillTyping;
    }


    public bool isSkillLocked() {
        return locked;
    }

    public void unlockSkill() {
        locked = false;
    }

    public float getSkillDuration() {
        return SkillDuration;
    }

    public string getSecondaryEffectType() {
        return secondaryEffectType;
    } 

    public float getSecondaryEffectMultiplier() {
        return secondaryEffectMultiplier;
    }

    public string getStatEffectedBySecondaryEffect() {
        return StatEffectedBySecondaryEffect;
    }

    public float getSecondaryEffectDuration() {
        return secondaryEffectSkillDuration;
    }

}
