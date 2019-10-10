using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Allows use of Unity's JSON Converter
[System.Serializable]

//this class is for storing the dialogue for specific quest npc dialogue
//it allows them to be converted to and from JSON plain text with can be written/read from txt files

public class QuestNPCDialogue : ScriptableObject{

    public string NPCName;

    //holds dialogue for all of the different quests the npc way hold
    public QuestLineContainer[] NPCsQuestsDialogue;



    public QuestNPCDialogue() {
        NPCName = "";
        NPCsQuestsDialogue = new QuestLineContainer[] { };
    }


    public bool areAllQuestsComplete() {

        bool finished = true;

        for (int i = 0; i < NPCsQuestsDialogue.Length; i++) {

            if (NPCsQuestsDialogue[i].isQuestComplete == false)
                finished = false;

        }

        return finished;
    }

    public void addNewQuestDialogue(QuestLineContainer questLine) {

        QuestLineContainer[] temp = new QuestLineContainer[(NPCsQuestsDialogue.Length + 1)];

        for (int i = 0; i < NPCsQuestsDialogue.Length; i++) {

            temp[i] = NPCsQuestsDialogue[i];

        }

        temp[temp.Length - 1] = questLine;

        NPCsQuestsDialogue = temp;
    }

    public QuestLineContainer GetQuestLineContainer(int location) {
        return NPCsQuestsDialogue[location];
    }

    public void setNPCName(string name) {
        NPCName = name;
    }

    public string getNPCName() {
        return NPCName;
    }

    public QuestLineContainer searchByQuestTitleAndNotCompleted(string questTitle) {

        for (int i = 0; i < NPCsQuestsDialogue.Length; i++) {
            if ((NPCsQuestsDialogue[i].isQuestComplete==false)&&(NPCsQuestsDialogue[i].questLineTitle.Equals(questTitle))) {
                return NPCsQuestsDialogue[i];
            }
        }

        return null;
    }
}
