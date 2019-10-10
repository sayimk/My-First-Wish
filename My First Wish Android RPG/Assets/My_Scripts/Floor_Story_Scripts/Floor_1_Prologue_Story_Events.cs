using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Floor_1_Prologue_Story_Events : MonoBehaviour {

    public Floor_1_Prologue_Story_Events currentStory { get; set; }

    //Characters
    public GameObject Neru;
    public GameObject Tama;
    public GameObject Player;
    public GameObject Elliot;

    //Chests
    public GameObject chestByCave;

    //Story NPCs
    public GameObject HoodedNPCQuestGiver;

    //Enemies
    public GameObject FirstEnemy;

    //Quests
    public List<GameObject> EndOfFloorQuestMarkers;

    void Awake() {
        if ((currentStory != this) && (currentStory != null))
            Destroy(gameObject);
        else
            currentStory = this;
    }	
  
    public void startingDialogueScene() {
        //Starting dialogue Scene

        DialogueSystem.Main.displayScriptedDialogueLines("Prologue_Scene_1");


        //create go to quest for story quests
        List<Item> noReward = new List<Item>();
        QuestsInterface storyQuest1 = new ReachLocationQuest("Story", "Start of a Wishful Journey", "Akira's Journey Starts Here. He and this close friends Elliot, Neru and Tama make their way to the Ancient Towers entrance", EndOfFloorQuestMarkers, noReward);
        EndOfFloorQuestMarkers[0].GetComponent<ReachQuestLocationMarker>().setActionWhenLocationReached(moveToNextFloor);
        Player.GetComponent<QuestsManager>().addQuest(storyQuest1);
        //setting up dialogue for the chest opening
        Neru.GetComponent<SubCharactersBase>().setPlayerEvent(Dialogue2);
    }

    //2nd dialogue scene
    //waiting for Akira at the initial Starting position
    public void Dialogue2() {
        //Dialogue for chest opening

        if (chestByCave.GetComponent<ItemsHeld>().isOpened()) {
            DialogueSystem.Main.displayScriptedDialogueLines("Prologue_Optional_ChestOpened");
        } else {
            DialogueSystem.Main.displayScriptedDialogueLines("Prologue_Optional_ChestClosed");
        }

        DialogueSystem.Main.setActionAfterDialogueQueueFinished(move1);
    }

    //Moving to next scene, next to the Forest Keeper quest NPC and new dialogue event talking to the NPC
    public void move1() {
        //move characters to new position and prompt new event
        Neru.GetComponent<SubCharactersBase>().moveSubCharacterToLocation(new Vector3(36.19f, 1.06f, 70.27f));
        Tama.GetComponent<SubCharactersBase>().moveSubCharacterToLocation(new Vector3(33.72f, 1.06f, 66.29f));
        Elliot.GetComponent<SubCharactersBase>().moveSubCharacterToLocation(new Vector3(26.74f, 0.39f, 69.44f));
        Elliot.GetComponent<SubCharactersBase>().setPlayerEvent(Dialogue3);
    }

    //waiting for the player to talk and accept the NPCs quest, then the group moves to the bridge.
    //third dialogue scene
    public void Dialogue3() {
        DialogueSystem.Main.displayScriptedDialogueLines("Prologue_Scene_2");
        HoodedNPCQuestGiver.GetComponent<NPC_Interface>().setStoryTalkToDependentAction(move2, true);
    }

    //Moves Subcharacter to the bridge and wait for the player
    public void move2() {
        Neru.GetComponent<SubCharactersBase>().moveSubCharacterToLocation(new Vector3(31.32f, 0.47f, 96f), NerufaceFirstEnemy);
        Tama.GetComponent<SubCharactersBase>().moveSubCharacterToLocation(new Vector3(34.42f, 0.47f, 96f), TamafaceFirstEnemy);
        Elliot.GetComponent<SubCharactersBase>().moveSubCharacterToLocation(new Vector3(29.12f, 0.39f, 98.32f), ElliotfaceFirstEnemy);
        Elliot.GetComponent<SubCharactersBase>().setPlayerEvent(Dialogue4);
    }

    //has neru face the first enemy
    public void NerufaceFirstEnemy() {
        Neru.transform.parent.LookAt(FirstEnemy.transform);
    }

    //has tama face the first enemy
    public void TamafaceFirstEnemy() {
        Tama.transform.parent.LookAt(FirstEnemy.transform);

    }

    //has elliot face the first enemy
    public void ElliotfaceFirstEnemy() {
        Elliot.transform.parent.LookAt(FirstEnemy.transform);

    }

    //Dialogue screen about the first spirit enemy and a event will be available on defeating the enemy.
    public void Dialogue4() {
        DialogueSystem.Main.displayScriptedDialogueLines("Prologue_Scene_3");
        FirstEnemy.GetComponent<EnemyBaseClass>().setActionToPerformOnDefeat(afterEnemyDefeat);
    }

    //action to be triggered after the enemy has been defeated
    public void afterEnemyDefeat() {
        Elliot.GetComponent<SubCharactersBase>().setPlayerEvent(Dialogue5);

    }

    //Dialogue to be played after the enemy has been defeated and at the bridge.
    public void Dialogue5() {
        DialogueSystem.Main.displayScriptedDialogueLines("Prologue_Scene_4");
        DialogueSystem.Main.setActionAfterDialogueQueueFinished(move3);

    }

    //Moves Subcharacters to one of the Azur Crystals(Blue-ish light crystals) and sets up an event on Tama
    public void move3() {
        Neru.GetComponent<SubCharactersBase>().moveSubCharacterToLocation(new Vector3(58.24f, 0.01f, 146.28f), NerufaceAzurCrystal);
        Tama.GetComponent<SubCharactersBase>().moveSubCharacterToLocation(new Vector3(56.36f, -0.02f, 150.94f), TamafaceAzurCrystal);
        Elliot.GetComponent<SubCharactersBase>().moveSubCharacterToLocation(new Vector3(58.12f, -0.05f, 155.48f), ElliotfaceAzurCrystal);
        Tama.GetComponent<SubCharactersBase>().setPlayerEvent(Dialogue6);

    }
    
    //Dialogue scene about the crystals
    public void Dialogue6() {
        DialogueSystem.Main.displayScriptedDialogueLines("Prologue_Scene_5");
        DialogueSystem.Main.setActionAfterDialogueQueueFinished(moveTowardsEntrance);
    }

    //Used to make the characters look at the crystals
    public void NerufaceAzurCrystal() {
        Neru.transform.parent.LookAt(new Vector3(-17.66f, 11.10939f, 109.15f));
    }

    public void ElliotfaceAzurCrystal() {
        Elliot.transform.parent.LookAt(new Vector3(-17.66f, 11.10939f, 109.15f));
    }

    public void TamafaceAzurCrystal() {
        Tama.transform.parent.LookAt(new Vector3(-17.66f, 11.10939f, 109.15f));
    }

    //moves characters towards the Towers entrance and setups up a dialogue scene
    public void moveTowardsEntrance() {
        Neru.GetComponent<SubCharactersBase>().moveSubCharacterToLocation(new Vector3(43.5f, 0.43f, 172.87f), NerufaceEntrance);
        Tama.GetComponent<SubCharactersBase>().moveSubCharacterToLocation(new Vector3(47.88f, 0.47f, 172.83f), TamafaceEntrance);
        Elliot.GetComponent<SubCharactersBase>().moveSubCharacterToLocation(new Vector3(45.86f, 0.39f, 171.16f), ElliotfaceEntrance);
        Elliot.GetComponent<SubCharactersBase>().setPlayerEvent(Dialogue7);
    }

    //used to make the characters face the entrance of the tower
    public void NerufaceEntrance() {
        Neru.transform.parent.LookAt(new Vector3(47.11f, 1.99f, 187.31f));
    }

    public void ElliotfaceEntrance() {
        Elliot.transform.parent.LookAt(new Vector3(47.11f, 1.99f, 187.31f));
    }

    public void TamafaceEntrance() {
        Tama.transform.parent.LookAt(new Vector3(47.11f, 1.99f, 187.31f));
    }

    //final prologue dialogue scene before entering the tower
    public void Dialogue7() {
        DialogueSystem.Main.displayScriptedDialogueLines("Prologue_Scene_6");

    }

    //end of floor and has the game move the characters to the next floor
    public void moveToNextFloor() {
        Debug.Log("Move to floor 2 Cave");
        Player.GetComponent<PlayerObjectSave>().LoadSceneAfterSaveOption(1);
        Player.GetComponent<PlayerObjectSave>().savePlayerDataToFile();
    }

}
