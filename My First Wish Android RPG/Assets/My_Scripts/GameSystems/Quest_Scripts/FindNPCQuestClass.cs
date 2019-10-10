using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class has is used for the quest where the player has to find and talk to a specific NPC
public class FindNPCQuestClass : QuestsInterface {
    public string questGiver { get; set; }
    public string questName { get; set; }
    public string questType { get; set; }
    public string questDescription { get; set; }
    public List<GameObject> targets { get; set; }
    public List<Item> questRewardList { get; set; }
    public int countAtAddedTime { get; set; }
    public string targetName { get; set; }
    private bool questComplete = false;

    public bool getQuestCompleteStatus() {
        return questComplete;
    }

    //checking if the NPC Markers have been triggered
    public void updateQuestCompleteStatus(Player_Log player_Log) {

        bool spokenWithNPCs = true;

        for (int i = 0; i <targets.Count; i++) {
            if (!targets[i].GetComponent<FindNPCQuestMarker>().hasTargetNPCBeenFoundAndSpokenTo()) {
                spokenWithNPCs = false;
            }
        }

        if (spokenWithNPCs)
            questComplete = true;
    }

    //used for initializing the quest, activating all NPC Markers
    public void intializeQuest() {
        for (int i = 0; i< targets.Count; i++) {
            targets[i].GetComponent<FindNPCQuestMarker>().activateFindNPCMarker();
        }
    }
}
