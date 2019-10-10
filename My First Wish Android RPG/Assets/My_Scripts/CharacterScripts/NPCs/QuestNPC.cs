using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Serializable wrapper class for list of lists in inspector
[System.Serializable]
public class rewardsString{
    public List<string> list;
}

//Serializable wrapper class for list of lists in inspector
[System.Serializable]
public class rewardsInt {
    public List<int> list;
}

//Serializable wrapper class for list of lists in inspector
[System.Serializable]
public class rewardsGameObject {
    public List<GameObject> list;
}

public class QuestNPC : MonoBehaviour, NPC_Interface {

    public GameObject player;
    public string NPCName;
    public string questRejectMessage = "";
    private List<QuestsInterface> avaliableQuests = new List<QuestsInterface>();
    private QuestsInterface activeQuest;
    private QuestLineContainer questDialogue;
    private int fillerPosition = 0;
    private CasualNPCDialogue casualDialogue;


    //Story Specific Actions and events
    private Action PendingTalkToStoryAction;
    private bool RequiresQuestToBeAccepted = false;

    //Kill quests 
    public List<string> killQuestNames;
    public List<string> killQuestDescriptions;
    public List<rewardsGameObject> targets;
    public List<rewardsString> KillRewardNames;
    public List<rewardsString> killRewardDescriptions;
    public List<rewardsInt> killRewardAmounts;
    public List<rewardsString> killRewardItemType_Use_or_Equip;
    public List<int> RequiredKills;
    //Add Additional Quests below

    //Use this method for creating quest objects
    //and saving to an avaliable Quest list
    void Start() {

        //compiling all quests
        //kill quests
        for (int i = 0; i < killQuestNames.Count; i++) {

            List<Item> rewards = new List<Item>();

            for (int j=0; j < KillRewardNames[i].list.Count; j++) {

                Item item = new Item(KillRewardNames[i].list[j],killRewardDescriptions[i].list[j], killRewardAmounts[i].list[j], killRewardItemType_Use_or_Equip[i].list[j]);
                rewards.Add(item);
            }

            KillQuestClass quest = new KillQuestClass(NPCName, killQuestNames[i], killQuestDescriptions[i], targets[i].list, rewards, RequiredKills[i]);
            avaliableQuests.Add(quest);

        }

        //Handle Additional Quest Types here



        Debug.Log("Finished Compiling quests for " + NPCName + ", has " + avaliableQuests.Count.ToString() + " Quest Avaliable");

    }

    public void interact() {

        if (activeQuest == null) {

            bool questAvaliable = false;
            gameObject.GetComponent<Animator>().SetTrigger("startTalking");

            while ((!questAvaliable) && (avaliableQuests.Count != 0)) {

                if (player.GetComponent<QuestsManager>().checkIfQuestAlreadyCompleted(avaliableQuests[0].questName, avaliableQuests[0].questGiver)) {

                    avaliableQuests.RemoveAt(0);

                } else {
                    questAvaliable = true;
                }
            }

            if ((avaliableQuests.Count>0)||questAvaliable) {

                activeQuest = avaliableQuests[0];
                questDialogue = DialogueSystem.Main.getQuestDialogueForSpecificNPC(NPCName).searchByQuestTitleAndNotCompleted(activeQuest.questName);

                for (int i = 0; i < questDialogue.getTotalSizeOfQuestIntroDialogue(); i++) {
                    DialogueSystem.Main.DisplayText(questDialogue.getQuestIntroDialogue(i).speaker, questDialogue.getQuestIntroDialogue(i).text);
                }

                DialogueSystem.Main.setActionAfterDialogueQueueFinished(endTalkingAnimation);

                //player acceptance of quest
                DialogueSystem.Main.playerConfirmQuest(NPCName, gameObject, questRejectMessage);



            } else {

                if(casualDialogue == null) {
                    casualDialogue = DialogueSystem.Main.getCasualDialogueForSpecificNPC(NPCName);
                }

                System.Random rnd = new System.Random();

                int casualPos = rnd.Next(0, casualDialogue.getTotalSizeOfDialogueLines());

                DialogueSystem.Main.DisplayText(casualDialogue.getDialogueLine(casualPos).speaker, casualDialogue.getDialogueLine(casualPos).text);
                DialogueSystem.Main.setActionAfterDialogueQueueFinished(endTalkingAnimation);

            }

        } else {

            if(player.GetComponent<QuestsManager>().checkQuestStatus(activeQuest.questGiver, activeQuest.questName)) {

                Debug.Log("Quest Completed");

                for (int i = 0; i < questDialogue.getTotalSizeOfQuestCompletionDialogue(); i++) {
                    DialogueSystem.Main.DisplayText(questDialogue.getQuestCompletionDialogue(i).speaker, questDialogue.getQuestCompletionDialogue(i).text);
                }
                DialogueSystem.Main.setActionAfterDialogueQueueFinished(endTalkingAnimation);
                player.GetComponent<QuestsManager>().npcConfirmedAndFinishedQuest(activeQuest.questName, activeQuest.questGiver);
                activeQuest = null;
                questDialogue = null;

            } else {

                Debug.Log("Quest Not Completed Yet");

                if (fillerPosition >= questDialogue.getTotalSizeOfQuestFillerDialogue())
                    fillerPosition = 0;

                DialogueSystem.Main.DisplayText(questDialogue.getQuestFillerDialogue(fillerPosition).speaker, questDialogue.getQuestFillerDialogue(fillerPosition).text);
                fillerPosition = fillerPosition + 1;
                DialogueSystem.Main.setActionAfterDialogueQueueFinished(endTalkingAnimation);


            }
        }

        if (!RequiresQuestToBeAccepted  && PendingTalkToStoryAction != null) {
            PendingTalkToStoryAction.DynamicInvoke();
            PendingTalkToStoryAction = null;
        }

    }

    public void playerConfirmationResponse(bool response) {

        if (response) {
            player.GetComponent<QuestsManager>().addQuest(avaliableQuests[0]);

            Debug.Log("Request Accepted");

            if (PendingTalkToStoryAction == null)
                Debug.Log("Null");

            if (PendingTalkToStoryAction != null) {
                Debug.Log("Performing action");
                PendingTalkToStoryAction.DynamicInvoke();
                PendingTalkToStoryAction = null;
                RequiresQuestToBeAccepted = false;
            }

        } else {
            activeQuest = null;

            if ((PendingTalkToStoryAction != null)&& !RequiresQuestToBeAccepted) {
                PendingTalkToStoryAction.DynamicInvoke();
                PendingTalkToStoryAction = null;
                RequiresQuestToBeAccepted = false;
            }
        }
    }

    public void endTalkingAnimation() {
        gameObject.GetComponent<Animator>().SetTrigger("stopTalking");
    }

    public void setStoryTalkToDependentAction(Action DependentAction, bool needAcceptQuest) {
        Debug.Log("Action Set");
        PendingTalkToStoryAction = DependentAction;
        RequiresQuestToBeAccepted = needAcceptQuest;
    }
}
