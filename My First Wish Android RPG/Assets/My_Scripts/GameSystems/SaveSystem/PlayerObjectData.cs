using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerObjectData : ScriptableObject {

    public PlayerObjectData() {

    }

    //Data From Character Stats and saving method
    public int HPValue = 0;
    public int currentHP = 0;
    public int AttackValue = 0;
    public int DefenceValue = 0;
    public int AgilityValue = 0;
    public int CharacterLevel = 0;
    public int Experience_Points = 0;
    public int Level_Experience_Required = 0;

    public void saveCharacterStatData(int HPValue, int currentHP, int AttackValue, int DefenceValue, int AgilityValue,
        int CharacterLevel, int Experience_Points, int Level_Experience_Required) {

        this.HPValue = HPValue;
        this.currentHP = currentHP;
        this.AttackValue = AttackValue;
        this.DefenceValue = DefenceValue;
        this.AgilityValue = AgilityValue;
        this.CharacterLevel = CharacterLevel;
        this.Experience_Points = Experience_Points;
        this.Level_Experience_Required = Level_Experience_Required;
    }

    //Data from PlayerInventory and saving method
    public Item[] InventoryItems;

    public void saveInventoryItems(List<Item> Inventory) {
        InventoryItems = Inventory.ToArray();
    }

    //Data from SkillManager and saving method
    public SkillClass[] playerSkills;
    public SkillClass activeSkill1;
    public SkillClass activeSkill2;

    public void saveSkillManager(List<SkillClass> playerSkills, SkillClass activeSkill1, SkillClass activeSkill2) {

        this.playerSkills = playerSkills.ToArray();
        this.activeSkill1 = activeSkill1;
        this.activeSkill2 = activeSkill2;
    }

    //Quests are not Carried Over between floors
    //public void saveQuestManager(){}

    //Data from Player_Log and save methods

    public string[] attackLog;

    //ADD ADDITIONAL ENEMY TYPES HERE, public to access var
    public int totalSpirits;

    //Saving the attackLog
    public void savePlayerLog(Player_Log player_Log) {
        this.attackLog = player_Log.getAttackLog().ToArray();
        totalSpirits = player_Log.getTotalDefeatedViaEnemyType(Player_Log.SpiritEnemy);
    }
}
