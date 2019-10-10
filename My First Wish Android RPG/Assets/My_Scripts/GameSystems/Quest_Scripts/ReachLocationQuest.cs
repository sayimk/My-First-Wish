using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachLocationQuest : QuestsInterface {

    public string questGiver { get; set; }
    public string questName { get; set; }
    public string questType { get; set; }
    public string questDescription { get; set; }
    public List<GameObject> targets { get; set; }
    public List<Item> questRewardList { get; set; }
    public int countAtAddedTime { get; set; }
    public string targetName { get; set; }
    private bool questComplete = false;

    //this quest will require the player to reach a particular location (ReachQuestLocationMarker) for the 
    //quest to be completed, multiple target points can be made because targets is a reference
    public ReachLocationQuest(string questGiver, string questName, string questDescription,
        List<GameObject> targets, List<Item> rewardsList) {

        this.questGiver = questGiver;
        this.questName = questName;
        questType = "Reach Location Quest";
        this.questDescription = questDescription;
        this.targets = targets;
        questRewardList = rewardsList;


    }

    //fetching is quest is complete
    public bool getQuestCompleteStatus() {
        return questComplete;
    }

    //called by quest manager to update quest status
    public void updateQuestCompleteStatus(Player_Log player_Log) {

        bool reached = true;

        //checking the status of the markers
        for (int i =0; i <targets.Count; i++) {
            if (!targets[i].GetComponent<ReachQuestLocationMarker>().checkIfMarkerIsReached()) {
                reached = false;
            }
        }

        if (reached)
            questComplete = true;
    }

    public void intializeQuest() {
        for (int i = 0; i < targets.Count; i++) {
            targets[i].GetComponent<ReachQuestLocationMarker>().activeLocationMarker();
        }
    }
}
