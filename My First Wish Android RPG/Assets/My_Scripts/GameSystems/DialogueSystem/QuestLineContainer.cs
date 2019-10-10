using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Allows use of Unity's JSON Converter
[System.Serializable]

public class QuestLineContainer{

    public string questLineTitle;

    public bool isQuestComplete;

    //set of Dialogue for introducing the quest to the player
    public TextLine[] questIntroDialogue;

    //set of Dialogue lines for waiting for the player to complete the quest or for quest content
    public TextLine[] questFillerDialogue;

    //set of dialogue lines for when the player completes the questline
    public TextLine[] questCompletionDialogue;


    public QuestLineContainer() {
        questLineTitle = ""; 
        questCompletionDialogue = new TextLine[] { };
        questFillerDialogue = new TextLine[] { };
        questIntroDialogue = new TextLine[] { };
        isQuestComplete = false;
    }

    //questIntroDialogue Methods

    public void addNewIntroDialogueLine(TextLine dialogueLine) {

        int newSize = questIntroDialogue.Length + 1;

        TextLine[] temp = new TextLine[newSize];


        for (int i = 0; i < questIntroDialogue.Length; i++) {
            temp[i] = questIntroDialogue[i];
        }

        temp[(temp.Length - 1)] = dialogueLine;

        questIntroDialogue = temp;

    }

    public TextLine getQuestIntroDialogue(int location) {

        return questIntroDialogue[location];
    }

    public int getTotalSizeOfQuestIntroDialogue() {
        return questIntroDialogue.Length;
    }


    //questFillerDialogue methods
    public void addNewquestFillerDialogue(TextLine dialogueLine) {

        int newSize = questFillerDialogue.Length + 1;

        TextLine[] temp = new TextLine[newSize];


        for (int i = 0; i < questFillerDialogue.Length; i++) {
            temp[i] = questFillerDialogue[i];
        }

        temp[(temp.Length - 1)] = dialogueLine;

        questFillerDialogue = temp;

    }

    public TextLine getQuestFillerDialogue(int location) {

        return questFillerDialogue[location];
    }

    public int getTotalSizeOfQuestFillerDialogue() {
        return questFillerDialogue.Length;
    }

    //QuestCompletionDialogue methods

    public void addNewQuestCompletionDialogue(TextLine dialogueLine) {

        int newSize = questCompletionDialogue.Length + 1;

        TextLine[] temp = new TextLine[newSize];


        for (int i = 0; i < questCompletionDialogue.Length; i++) {
            temp[i] = questCompletionDialogue[i];
        }

        temp[(temp.Length - 1)] = dialogueLine;

        questCompletionDialogue = temp;

    }

    public TextLine getQuestCompletionDialogue(int location) {

        return questCompletionDialogue[location];
    }

    public int getTotalSizeOfQuestCompletionDialogue() {
        return questCompletionDialogue.Length;
    }
}
