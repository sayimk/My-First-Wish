using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Allows use of Unity's JSON Converter
//vars are publics for use with Unity's JSON Converter
[System.Serializable]
public class CasualNPCDialogue : ScriptableObject {

    //NPCs name
    public string NPC_Name { get; set; }

    //all of the things that the NPC can say
    public TextLine[] dialogueLines;

    //constructing a new instance of it
    public CasualNPCDialogue() {
        dialogueLines = new TextLine[] {};
        NPC_Name = "";
    }

    //resizing the array and adding the new dialogue line to the array
    public void addNewDialogueLine(TextLine dialogueLine) {

        int newSize = dialogueLines.Length + 1;

        TextLine[] temp = new TextLine[newSize];


        for (int i = 0; i < dialogueLines.Length; i++) {
            temp[i] = dialogueLines[i];
        }

        temp[(temp.Length - 1)] = dialogueLine;

        dialogueLines = temp;

    }

    //fetching dialogueline via location
    public TextLine getDialogueLine(int location) {

        return dialogueLines[location];
    }

    //returning max size
    public int getTotalSizeOfDialogueLines() {
        return dialogueLines.Length;
    }

}
