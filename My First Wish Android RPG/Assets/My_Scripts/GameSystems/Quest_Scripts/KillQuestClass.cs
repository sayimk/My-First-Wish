using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class is for single target kill quests
public class KillQuestClass : QuestsInterface {

    public string questGiver { set; get; }
    public string questName { set; get; }
    public string questType { set; get; }
    public string questDescription { set; get; }
    public string targetName { set; get; }
    public List<GameObject> targets { get; set; }
    public List<Item> questRewardList { set; get; }
    private bool questComplete = false;
    private int requiredDefeatCount;
    private int currentDefeatedCount;
    public int countAtAddedTime { get; set; }

    public KillQuestClass(string questGiver, string questName, string questDescription,
        List<GameObject> targets, List<Item> rewardsList, int requiredEnemyDefeat) {

        this.questGiver = questGiver;
        this.questName = questName;
        this.questDescription = questDescription;
        this.targets = targets;
        questRewardList = rewardsList;
        requiredDefeatCount = requiredEnemyDefeat;
        currentDefeatedCount = 0;
        questType = "Kill Quest";
        targetName = targets[0].name;

    }

    //updates the status of the quest
    public void updateQuestCompleteStatus(Player_Log player_Log) {

        currentDefeatedCount = player_Log.getTotalDefeatedViaEnemyType(targetName)- countAtAddedTime;

        if (currentDefeatedCount == requiredDefeatCount) {
            questComplete = true;

            //fire a notification of quest completion
            DialogueSystem.Main.displayNotification("You Have Completed "+questName+", go to "+questGiver+" to finish and get your reward");
            Debug.Log(questComplete);
        }

        List<GameObject> temp = new List<GameObject>();

        for (int i = 0; i < targets.Count; i++) {

            if (targets[i]!=null) {
                temp.Add(targets[i]);
            }
        }

        targets = temp;

    }

    //checks if the quest is complete
    public bool getQuestCompleteStatus() {
        return questComplete;
    }

    public void intializeQuest() {
        
    }
}
