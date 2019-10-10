
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

//Used to storing data for the player class
public class PlayerObjectSave : MonoBehaviour {

    Player_Log player_log;
    CharacterStats characterStats;
    Player_Inventory player_Inventory;
    SkillsManager skillsManager;
    int transitionFloorInt = -1;

    PlayerObjectData Data;
    public void savePlayerDataToFile() {

        //fetching and creating references to players current scripts
        player_log = gameObject.GetComponent<Player_Log>();
        characterStats = gameObject.GetComponent<CharacterStats>();
        player_Inventory = gameObject.GetComponent<Player_Inventory>();
        skillsManager = gameObject.GetComponent<SkillsManager>();

        //creating scriptable instance of the PlayerObjectData that will be saved as JSON
        Data = (PlayerObjectData)ScriptableObject.CreateInstance("PlayerObjectData");

        //adding Data from different classes

        //adding Data from CharacterStats
        Data.saveCharacterStatData(characterStats.getStatBaseValue("HP"), characterStats.getCurrentHP(), characterStats.getStatBaseValue("Attack"), characterStats.getStatBaseValue("Defence"),
            characterStats.getStatBaseValue("Agility"), characterStats.CharacterLevel, characterStats.getExperiencePoints(), characterStats.Level_Experience_Required);

        //adding Data from inventory
        Data.saveInventoryItems(player_Inventory.getInventory());

        //Adding Data from SkillManager
        Data.saveSkillManager(skillsManager.getAllPlayerSkills(), skillsManager.getSlot1Skill(), skillsManager.getSlot2Skill());

        //Adding Data from Attacklog
        Data.savePlayerLog(player_log);
        
        //Saving instance to new file
        File.WriteAllText(Application.dataPath + "/Resources/SaveData/FloorTransitionData.txt",
            JsonUtility.ToJson(Data));

        if (!transitionFloorInt.Equals(-1)) {
            SceneManager.LoadScene(transitionFloorInt, LoadSceneMode.Single);
        }
    }

    //this method is used to fetch and load data from the JSON text file
    public void loadPlayerDataFromFile() {

        if (File.Exists(Application.dataPath + "/Resources/SaveData/FloorTransitionData.txt")) {

            //fetching and creating references to players current scripts
            player_log = gameObject.GetComponent<Player_Log>();
            characterStats = gameObject.GetComponent<CharacterStats>();
            player_Inventory = gameObject.GetComponent<Player_Inventory>();
            skillsManager = gameObject.GetComponent<SkillsManager>();

            Data = (PlayerObjectData)ScriptableObject.CreateInstance("PlayerObjectData");
            TextAsset jsonFile = Resources.Load("SaveData/FloorTransitionData") as TextAsset;
            JsonUtility.FromJsonOverwrite(jsonFile.text, Data);

            characterStats.loadData(Data);
            skillsManager.loadData(Data);
            player_log.loadData(Data);
            player_Inventory.loadData(Data);

            Debug.Log("Data Loaded from previous Floor");

        } else {
            Debug.Log("Transition Save Data not found");
        }
    }

    public void LoadSceneAfterSaveOption(int SceneBuildint) {
        transitionFloorInt = SceneBuildint;
    }

}
