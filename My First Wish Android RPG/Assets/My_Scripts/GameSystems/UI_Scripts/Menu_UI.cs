using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_UI : MonoBehaviour {
    public GameObject MenuUI;
    public GameObject WorldInteractObject;
    public GameObject MainCharacter;
    public GameObject HomeOption;
    public GameObject InventoryOption;
    public GameObject QuestsOption;
    public GameObject BioOption;
    public GameObject StatsOption;
    
    //Method to enable the menu
    public void MenuEnable() {
        MenuUI.GetComponent<Canvas>().enabled = true;
        MenuUI.transform.Find("MenuCanvas").Find("MenuHead").GetComponent<Canvas>().enabled = true;
        //WorldInteractObject.GetComponent<Interact>().WorldInteractsActive = false;
        OpenHomeOption();

    }

    //Method to disable the menu
    public void MenuDisable() {
        MenuUI.GetComponent<Canvas>().enabled = false;
        MenuUI.transform.Find("MenuCanvas").Find("MenuHead").GetComponent<Canvas>().enabled = false;
        Debug.Log("Closing menu");
        //WorldInteractObject.GetComponent<Interact>().WorldInteractsActive = true;

        //methods for deactivating UI Specifics, so they can be reset on close
        InventoryOption.GetComponent<Inventory_UI_Manager>().CloseAndResetInventoryUIGrid();

        //deactivating the canvases
        HomeOption.GetComponent<Canvas>().enabled = false;
        InventoryOption.GetComponent<Canvas>().enabled = false;
        QuestsOption.GetComponent<Canvas>().enabled = false;
        BioOption.GetComponent<Canvas>().enabled = false;
        StatsOption.GetComponent<Canvas>().enabled = false;

        
    }

    //Method to open the Home option
    public void OpenHomeOption() {
        HomeOption.GetComponent<Canvas>().enabled = true;
        InventoryOption.GetComponent<Canvas>().enabled = false;
        QuestsOption.GetComponent<Canvas>().enabled = false;
        BioOption.GetComponent<Canvas>().enabled = false;
        StatsOption.GetComponent<Canvas>().enabled = false;
        InventoryOption.GetComponent<Inventory_UI_Manager>().CloseAndResetInventoryUIGrid();

        //filling in Details Section
        HomeOption.transform.Find("CurrentLevel").gameObject.GetComponent<Text>().text = MainCharacter.GetComponent<CharacterStats>().CharacterLevel.ToString();
        HomeOption.transform.Find("CurrentHP").gameObject.GetComponent<Text>().text = (MainCharacter.GetComponent<CharacterStats>().getCurrentHP().ToString()+"/"
            + MainCharacter.GetComponent<CharacterStats>().getModifiedStatValue("HP").ToString());

        HomeOption.transform.Find("currentEXP").gameObject.GetComponent<Text>().text = (MainCharacter.GetComponent<CharacterStats>().Experience_Points.ToString() + "/"
    + MainCharacter.GetComponent<CharacterStats>().Level_Experience_Required.ToString());

    }

    //Method to open the Inventory option
    public void OpenInventoryOption() {
        HomeOption.GetComponent<Canvas>().enabled = false;
        InventoryOption.GetComponent<Canvas>().enabled = true;
        QuestsOption.GetComponent<Canvas>().enabled = false;
        BioOption.GetComponent<Canvas>().enabled = false;
        StatsOption.GetComponent<Canvas>().enabled = false;
        InventoryOption.GetComponent<Inventory_UI_Manager>().RenderInventoryUIGrid(MainCharacter.GetComponent<Player_Inventory>());
    }

    //Method to open the Quest option
    public void OpenQuestOption() {
        HomeOption.GetComponent<Canvas>().enabled = false;
        InventoryOption.GetComponent<Canvas>().enabled = false;
        QuestsOption.GetComponent<Canvas>().enabled = true;
        BioOption.GetComponent<Canvas>().enabled = false;
        StatsOption.GetComponent<Canvas>().enabled = false;
        QuestsOption.GetComponent<Quest_UI_Manager>().initializeQuestInfo(MainCharacter.GetComponent<QuestsManager>());
    }

    //Method to open the Bio option
    public void OpenBioOption() {
        HomeOption.GetComponent<Canvas>().enabled = false;
        InventoryOption.GetComponent<Canvas>().enabled = false;
        QuestsOption.GetComponent<Canvas>().enabled = false;
        BioOption.GetComponent<Canvas>().enabled = true;
        StatsOption.GetComponent<Canvas>().enabled = false;
    }

    //Method to open the Stats option
    public void OpenStatsOption() {
        HomeOption.GetComponent<Canvas>().enabled = false;
        InventoryOption.GetComponent<Canvas>().enabled = false;
        QuestsOption.GetComponent<Canvas>().enabled = false;
        BioOption.GetComponent<Canvas>().enabled = false;
        StatsOption.GetComponent<Canvas>().enabled = true;
        SetStatMenuData();
    }

    public void SetStatMenuData() {
        string StatDisplayLabelID = "";
        string StatDisplayValueID = "";

        //setting Character Stats
        StatsOption.transform.Find("StatsDisplay").Find("HPValue").gameObject.GetComponent<Text>().text = MainCharacter.GetComponent<CharacterStats>().getModifiedStatValue("HP").ToString();
        StatsOption.transform.Find("StatsDisplay").Find("AttackValue").gameObject.GetComponent<Text>().text = MainCharacter.GetComponent<CharacterStats>().getModifiedStatValue("Attack").ToString();
        StatsOption.transform.Find("StatsDisplay").Find("DefenceValue").gameObject.GetComponent<Text>().text = MainCharacter.GetComponent<CharacterStats>().getModifiedStatValue("Defence").ToString();
        StatsOption.transform.Find("StatsDisplay").Find("AgilityValue").gameObject.GetComponent<Text>().text = MainCharacter.GetComponent<CharacterStats>().getModifiedStatValue("Agility").ToString();
        StatsOption.transform.Find("StatsDisplay").Find("LevelValue").gameObject.GetComponent<Text>().text = MainCharacter.GetComponent<CharacterStats>().CharacterLevel.ToString();

        //setting exp bar length
        float currentEXP = MainCharacter.GetComponent<CharacterStats>().Experience_Points;
        float maxEXP = MainCharacter.GetComponent<CharacterStats>().Level_Experience_Required;
        float percent = currentEXP/maxEXP;
        StatsOption.transform.Find("StatsDisplay").Find("EXPGauge").gameObject.GetComponent<Image>().fillAmount = percent;


        List <StatModifierClass> unifiedBonuses = new List<StatModifierClass>();

        //Unifying Bonuses into one List
        unifiedBonuses.AddRange(MainCharacter.GetComponent<CharacterStats>().getModifierList("HP"));
        unifiedBonuses.AddRange(MainCharacter.GetComponent<CharacterStats>().getModifierList("Attack"));
        unifiedBonuses.AddRange(MainCharacter.GetComponent<CharacterStats>().getModifierList("Defence"));
        unifiedBonuses.AddRange(MainCharacter.GetComponent<CharacterStats>().getModifierList("Agility"));

        //Printing Bonuses and Values
        for(int i=0; ((i<unifiedBonuses.Count)&&(i<9)); i++) {
            StatDisplayLabelID = ("Bonus" + (i + 1).ToString() + "Label");
            StatDisplayValueID = ("Bonus" + (i + 1).ToString() + "Value");

            StatsOption.transform.Find("BonusesDisplay").Find(StatDisplayLabelID).gameObject.GetComponent<Text>().text = unifiedBonuses[i].getModifierName();
            StatsOption.transform.Find("BonusesDisplay").Find(StatDisplayValueID).gameObject.GetComponent<Text>().text = unifiedBonuses[i].getModifierValue().ToString();

        }

        //sending SkillManager to skills menu
        StatsOption.transform.Find("SkillMenu").gameObject.GetComponent<SkillMenuUIController>().assignSkillManager(MainCharacter.GetComponent<SkillsManager>());
    }

    public void toggleActiveSkillIconInfo(int i) {
        GameObject slotLabel = StatsOption.transform.Find("SkillDisplay").Find("ActiveSkillIcons").Find("Slot" + i.ToString() + "Label").gameObject;
        GameObject callerButton = StatsOption.transform.Find("SkillDisplay").Find("ActiveSkillIcons").Find("ActiveSkill" + i.ToString()).gameObject;

        if ((!slotLabel.GetComponent<Image>().enabled)&& !MainCharacter.GetComponent<SkillsManager>().checkIfSpecificSkillSlotIsNull(i)) {
            slotLabel.GetComponent<Image>().enabled = true;
            slotLabel.transform.Find("Text").gameObject.GetComponent<Text>().enabled = true;

            slotLabel.transform.Find("Text").gameObject.GetComponent<Text>().text = MainCharacter.GetComponent<SkillsManager>()
                .findSkillByIconReference(callerButton.GetComponent<Image>().sprite.name).getSkillName();
        } else {

            slotLabel.GetComponent<Image>().enabled = false;
            slotLabel.transform.Find("Text").gameObject.GetComponent<Text>().enabled = false;

        }

    }

    //Shortcut key to the SkillMenu
    public void shortcutKey() {
        MenuEnable();
        OpenStatsOption();
        StatsOption.transform.Find("SkillMenu").gameObject.GetComponent<SkillMenuUIController>().openSkillsMenu();

    }

}
