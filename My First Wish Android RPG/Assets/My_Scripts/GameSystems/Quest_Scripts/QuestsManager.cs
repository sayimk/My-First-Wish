using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    //this class will manage and monitor the status of the quests that the player receives
public class QuestsManager : MonoBehaviour {

    private List<QuestsInterface> questList = new List<QuestsInterface>();

    private List<QuestsInterface> completedQuestList = new List<QuestsInterface>();

    //this is used to get the compass methods as the compass is part of the mainCamera (set via inspector)
    public GameObject mainCamera;

    private int activeQuestInt = -1;


    //Kill Quest Methods
    public int getActiveQuestPosition() {

        return activeQuestInt;
    }

    public void addQuest(QuestsInterface quest) {

        if((quest.questType.Equals("Kill Quest"))) {
            quest.countAtAddedTime = gameObject.GetComponent<Player_Log>().getTotalDefeatedViaEnemyType(quest.targetName);
        }
        QuestsInterface temp = quest;
        temp.intializeQuest();
        questList.Add(temp);
        DialogueSystem.Main.displayNotification("You have started " + temp.questName+", View Quests on Phone for details");
    }

    public void setQuestAsActive(int position) {
        Debug.Log(position);
        if (position >= 0) {
            if ((questList[position].targets.Count > 0))
                mainCamera.GetComponent<Camera_Control>().setNewTargetCompass(questList[position].targets[0].transform.position.x, questList[position].targets[0].transform.position.y, questList[position].targets[0].transform.position.z);
            else
                mainCamera.GetComponent<Camera_Control>().endTargetCompass();
        }
        activeQuestInt = position;

    }

    //this method will be used by the NPC to check the status of the quest that was given to the player
    public bool checkQuestStatus(string questGiver , string questName) {

        for (int i = 0; i<questList.Count; i++) {

            if ((questList[i].questName.Equals(questName)) && (questList[i].questGiver.Equals(questGiver))) {
                return questList[i].getQuestCompleteStatus();
            }
        }

        return false;
    }

    public void updateAllQuests() {

        foreach (var quest in questList) {
            quest.updateQuestCompleteStatus(gameObject.GetComponent<Player_Log>());
        }

        //setQuestAsActive(activeQuestInt);

        if (activeQuestInt!=-1)
            if (questList[activeQuestInt].getQuestCompleteStatus()) {
                mainCamera.GetComponent<Camera_Control>().endTargetCompass();
                activeQuestInt = -1;
            }
    }

    public void npcConfirmedAndFinishedQuest(string questName, string questGiver) {

        bool foundQuest = false;

        for (int i = 0; i < questList.Count; i++) {

            if ((!foundQuest) && (questList[i].questName.Equals(questName)) && (questList[i].questGiver.Equals(questGiver))) {

                foundQuest = true;

                //adding rewards to inventory
                gameObject.GetComponent<Player_Inventory>().addItems(questList[i].questRewardList);

                completedQuestList.Add(questList[i]);
                questList.RemoveAt(i);

                DialogueSystem.Main.displayNotification("You have finished and gained rewards from " + questName);
            }
        }
    }

    public bool checkIfQuestAlreadyCompleted(string questName, string questGiver) {

        for (int i = 0; i < completedQuestList.Count; i++) {

            if ((completedQuestList[i].questName.Equals(questName)) && (completedQuestList[i].questGiver.Equals(questGiver))) {

                return true;

            }
        }
        return false;
    }

    public int getTotalOpenQuests() {

        return questList.Count;
    }

    public string getQuestTitleViaPosition(int listPos) {

        if (listPos < questList.Count)
            return questList[listPos].questName;
        else return "---";
    }

    public QuestsInterface getQuestViaPosition(int position) {

        return questList[position];

    }
}
