using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasualNPC : MonoBehaviour, NPC_Interface {

    public string NPCName;
    private Action PendingTalkToStoryAction;
    private CasualNPCDialogue casualDialogue;


    public void interact() {

        if (PendingTalkToStoryAction != null) {
            PendingTalkToStoryAction.DynamicInvoke();
            PendingTalkToStoryAction = null;
        }

        if (casualDialogue == null) {
            casualDialogue = DialogueSystem.Main.getCasualDialogueForSpecificNPC(NPCName);
        }

        System.Random rnd = new System.Random();

        int casualPos = rnd.Next(0, casualDialogue.getTotalSizeOfDialogueLines());

        DialogueSystem.Main.DisplayText(casualDialogue.getDialogueLine(casualPos).speaker, casualDialogue.getDialogueLine(casualPos).text);

    }

    public void setStoryTalkToDependentAction(Action DependentAction, bool needAcceptQuest) {
        PendingTalkToStoryAction = DependentAction;
    }
}
