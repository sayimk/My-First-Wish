using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest_UI_Manager : MonoBehaviour {

    public GameObject QuestListSlots;

    public GameObject QuestInfoScreen;

    public QuestsManager playersQuests;

    private int currentListOffset =0;

    private int currentlySelected;


    public void scrollQuestList(float val) {

        float output = val * 10;
        int listOffset = (int) Math.Truncate(output);


        for (int i = 0; i < 8; i++) {

            if (i + listOffset < playersQuests.getTotalOpenQuests()) { 
                QuestListSlots.transform.Find("Slot" + i.ToString()).Find("Text").gameObject.GetComponent<Text>().text = playersQuests.getQuestTitleViaPosition((i + listOffset));
                currentListOffset = listOffset;

        }else{
            QuestListSlots.transform.Find("Slot" + i.ToString()).Find("Text").gameObject.GetComponent<Text>().text = "---";
        }
        }

    }

    public void initializeQuestInfo(QuestsManager questManager) {

        playersQuests = questManager;

        QuestInfoScreen.transform.Find("SetAsActive").gameObject.GetComponent<Button>().interactable = false;

        QuestInfoScreen.transform.Find("Title").gameObject.GetComponent<Text>().text = "";
        QuestInfoScreen.transform.Find("QuestGiver").gameObject.GetComponent<Text>().text = "---";
        QuestInfoScreen.transform.Find("QuestType").gameObject.GetComponent<Text>().text = "---";
        QuestInfoScreen.transform.Find("Description").gameObject.GetComponent<Text>().text = "";


        for (int i = 0; i < 8; i++) {
            QuestListSlots.transform.Find("Slot" + i.ToString()).Find("Text").gameObject.GetComponent<Text>().text = "---";

        }

        for (int i=0; (i < playersQuests.getTotalOpenQuests()) && (i<8); i++) {
            QuestListSlots.transform.Find("Slot" + i.ToString()).Find("Text").gameObject.GetComponent<Text>().text = playersQuests.getQuestTitleViaPosition(i);

        }


    }

    public void fetchAndSetDetails(int location) {
        Debug.Log("Quest List location is " + (location + currentListOffset));
        currentlySelected = (location + currentListOffset);

        if (currentlySelected < playersQuests.getTotalOpenQuests()) {

            if (!playersQuests.getQuestViaPosition(currentlySelected).getQuestCompleteStatus()) {

                if (playersQuests.getActiveQuestPosition() != currentlySelected) { 

                    QuestInfoScreen.transform.Find("SetAsActive").gameObject.GetComponent<Button>().interactable = true;
                    QuestInfoScreen.transform.Find("SetAsActive").Find("Text").GetComponent<Text>().text = "Set as Active";
                } else {
                    QuestInfoScreen.transform.Find("SetAsActive").gameObject.GetComponent<Button>().interactable = false;
                    QuestInfoScreen.transform.Find("SetAsActive").Find("Text").GetComponent<Text>().text = "Active";
                }
                

            }else {
                QuestInfoScreen.transform.Find("SetAsActive").gameObject.GetComponent<Button>().interactable = false;
                QuestInfoScreen.transform.Find("SetAsActive").Find("Text").GetComponent<Text>().text = "Completed";
            }


            QuestInfoScreen.transform.Find("Title").gameObject.GetComponent<Text>().text = 
                playersQuests.getQuestViaPosition(currentlySelected).questName;

            QuestInfoScreen.transform.Find("QuestGiver").gameObject.GetComponent<Text>().text =
                 playersQuests.getQuestViaPosition(currentlySelected).questGiver;

            QuestInfoScreen.transform.Find("Description").gameObject.GetComponent<Text>().text =
                 playersQuests.getQuestViaPosition(currentlySelected).questDescription;

            QuestInfoScreen.transform.Find("QuestType").gameObject.GetComponent<Text>().text =
                 playersQuests.getQuestViaPosition(currentlySelected).questType;

        } else {
            QuestInfoScreen.transform.Find("SetAsActive").gameObject.GetComponent<Button>().interactable = false;
            QuestInfoScreen.transform.Find("Title").gameObject.GetComponent<Text>().text = "";
            QuestInfoScreen.transform.Find("QuestGiver").gameObject.GetComponent<Text>().text = "---";
            QuestInfoScreen.transform.Find("QuestType").gameObject.GetComponent<Text>().text = "---";
            QuestInfoScreen.transform.Find("Description").gameObject.GetComponent<Text>().text = "";
        }

    }

    public void setAsActive() {
        Debug.Log(currentlySelected + " Set As Active");
        playersQuests.setQuestAsActive(currentlySelected);
        QuestInfoScreen.transform.Find("SetAsActive").gameObject.GetComponent<Button>().interactable = false;
        QuestInfoScreen.transform.Find("SetAsActive").Find("Text").GetComponent<Text>().text = "Active";
    }
}
