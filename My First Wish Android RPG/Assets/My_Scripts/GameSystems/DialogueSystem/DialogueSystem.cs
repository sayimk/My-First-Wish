using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour {


    public static DialogueSystem Main { get; set; }
    public GameObject Player;
    private List<string> textList = new List<string>();
    private List<string> dialogueQueue = new List<string>();
    private List<string> speakerQueue = new List<string>();
    private Action endOfDialogueAction;
    private int textIndex = 0;
    private int textBoundary = 0;
    public GameObject DialogueSystemUI;
    public GameObject DialogueButton;
    public GameObject WorldInteractObject;
    public GameObject NotificationUI;
    public GameObject EXPOutput;
    public GameObject menu;
    private bool dialogueSystemInUse = false;
    private bool textListFinished = false;
    private int bufferIndex = 0;

    //choice System VaRS
    public GameObject Choice2UI;
    public GameObject Choice3UI;
    private GameObject currentChoiceUIInUse = null;
    private int choiceReturned = -1;
    Dictionary<int, List<TextLine>> Responses = new Dictionary<int, List<TextLine>>();
    Dictionary<int, string> choices = new Dictionary<int, string>();

    //npc reference, used for holding a reference to the NPC asking for quest confirmation
    GameObject NPCQuestGiver = null;
    string npcName="";
    string rejectionStatement = "";
    bool questConfirmationPostponed = false;

    // Use this for initialization
    //if Main exists and isn't this class then destroy and make a new one
    void Awake(){
        if ((Main != this) && (Main != null))
            Destroy(gameObject);
        else
            Main = this;
    }
    // Update is called once per frame
    void Update () {
		
	}

    public bool IsDialogueSystemInUse(){
        return dialogueSystemInUse;
    }

    //this method displays text onto the screen and allows dialogue to be queued and displayed one after another
    public void DisplayText(string Speaker, string DialogueText){

        if ((!dialogueSystemInUse)) {
            dialogueSystemInUse = true;
            textListFinished = false;
            DialogueSystemUI.GetComponent<Canvas>().enabled = true;
            WorldInteractObject.GetComponent<Interact>().WorldInteractsActive = false;
            Player.GetComponent<SphereCollider>().enabled = false;
            menu.GetComponent<Menu_UI>().MenuDisable();

            if (!Speaker.Equals("#Choice")) {
                DialogueSystemUI.transform.Find("DialogueSpeakerName").Find("Text").gameObject.GetComponent<Text>().text = Speaker;

                bufferIndex = 0;
                textList.Clear();

                string buffer = "";
                for (int i = 0; i < DialogueText.Length; i++) {
                    if (DialogueText[i].Equals('.') || (i == (DialogueText.Length - 1))) {
                        buffer = buffer + DialogueText[i];
                        textList.Add(buffer);
                        buffer = "";
                    } else {
                        buffer = buffer + DialogueText[i];
                    }
                }

                DialogueSystemUI.transform.Find("DialogueText").Find("Text").gameObject.GetComponent<Text>().text = textList[bufferIndex];

            } else {
                speakerQueue.Add(Speaker);
                dialogueQueue.Add(DialogueText);
            }

        } else {
            speakerQueue.Add(Speaker);
            dialogueQueue.Add(DialogueText);
        }
    }

    //Method used for clearing up the dialogue system, ready for a new set of dialogue
    private void DialogueSystemCleanUp(){
        dialogueSystemInUse = false;
        WorldInteractObject.GetComponent<Interact>().WorldInteractsActive = true;
        DialogueSystemUI.GetComponent<Canvas>().enabled = false;
        DialogueSystemUI.transform.Find("DialogueText").Find("Text").gameObject.GetComponent<Text>().text = "";
        DialogueSystemUI.transform.Find("DialogueSpeakerName").Find("Text").gameObject.GetComponent<Text>().text = "";
        Player.GetComponent<SphereCollider>().enabled = true;

        //Executin
    }

    //add a check for end of text and change button text and action.
    public void OnClick(){
        bufferIndex = bufferIndex + 1;

        //set the next textline in the queue for current dialogue
        if (textList.Count > bufferIndex) {
            DialogueSystemUI.transform.Find("DialogueText").Find("Text").gameObject.GetComponent<Text>().text = textList[bufferIndex];

        } else if (speakerQueue.Count > 0) {

            //if the #Choice keyword then start choice system
            if (speakerQueue[0].Equals("#Choice")) {

                string fileName = dialogueQueue[0];
                speakerQueue.RemoveAt(0);
                dialogueQueue.RemoveAt(0);
                displayChoices(fileName);

            } else {
                //else start next speakers dialogue
                DialogueSystemCleanUp();
                DisplayText(speakerQueue[0], dialogueQueue[0]);
                speakerQueue.RemoveAt(0);
                dialogueQueue.RemoveAt(0);
            }
        } else if (questConfirmationPostponed) {
            DialogueSystemCleanUp();
            playerConfirmQuest(npcName, NPCQuestGiver, rejectionStatement);

        } else {

            //if no more dialogue then clean up and end
            DialogueSystemCleanUp();
            if (endOfDialogueAction != null) {
                endOfDialogueAction.DynamicInvoke();
                endOfDialogueAction = null;
            }
        }

    }

    public void setActionAfterDialogueQueueFinished(Action MethodToExecute) {
        endOfDialogueAction = MethodToExecute;
    }

    //This is the Method for the notification System (Slides down from the top of the screen)
    public bool displayNotification(string NotificationMessage) {

        try {

            NotificationUI.transform.Find("Banner").Find("Text").
                gameObject.GetComponent<Text>().text = NotificationMessage;

            NotificationUI.GetComponent<Animator>().SetTrigger("display");


        } catch (System.Exception e) {
            Debug.Log(e.Message);
            return false;
        }
        return true;
    }

    //this method is used for outputting EXP gains from defeating enemies
    public bool displayEXP(int EXPGained) {

        try {

            EXPOutput.transform.Find("Text").
                gameObject.GetComponent<Text>().text = "Gained "+EXPGained.ToString()+" EXP";

            EXPOutput.GetComponent<Animator>().SetTrigger("display");


        } catch (System.Exception e) {
            Debug.Log(e.Message);
            return false;
        }
        return true;
    }

    //used for fetching casual dialogue for an NPC  
    public CasualNPCDialogue getCasualDialogueForSpecificNPC(string NPCName) {

        CasualNPCDialogue output = (CasualNPCDialogue)ScriptableObject.CreateInstance("CasualNPCDialogue");

        TextAsset jsonTextFile = Resources.Load("DialogueLines/CasualDialogue/"+NPCName) as TextAsset;
        JsonUtility.FromJsonOverwrite(jsonTextFile.text, output);
        return output;
    }

    //used for fetching Quest dialogue for an NPC
    public QuestNPCDialogue getQuestDialogueForSpecificNPC(string NPCName) {

        QuestNPCDialogue output = (QuestNPCDialogue)ScriptableObject.CreateInstance("QuestNPCDialogue");

        TextAsset jsonTextFile = Resources.Load("DialogueLines/QuestDialogue/" + NPCName) as TextAsset;
        JsonUtility.FromJsonOverwrite(jsonTextFile.text, output);
        return output;
    }

    //saving quest dialogue to a file
    public void outputJSONFileFromQuestNPCDialogue(QuestNPCDialogue dialogue, string NPCNameWithCap) {

     File.WriteAllText(Application.dataPath + "/Resources/DialogueLines/QuestDialogue/"+NPCNameWithCap+ ".txt", 
         JsonUtility.ToJson(dialogue));

    }

    //saving casual dialogue to a file
    public void outputJSONFileFromCasualNPCDialogue(CasualNPCDialogue dialogue, string NPCNameWithCap) {

        File.WriteAllText(Application.dataPath + "/Resources/DialogueLines/CasualDialogue/"+ NPCNameWithCap+".txt",
            JsonUtility.ToJson(dialogue));

    }

    //this will get the script and parse it into speaker and there lines for the dialogue system
    public void displayScriptedDialogueLines(string textFileName) {

        TextAsset TextFile = Resources.Load("DialogueLines/ScriptedDialogue/" + textFileName) as TextAsset;

        string dialogue = TextFile.text;

        List<TextLine> output = new List<TextLine>();

        string speaker = "";
        string line = "";
        bool finished;
        int counter = 0;
        Char[] letters;

        //splitting the paragraph into array of text lines
        string[] lines = dialogue.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        //going through each line and seperating them into speaker parts and line part with the seperator : e.g. Akira:Hmmm
        for (int i = 0; i < lines.Length; i++) {

            speaker = "";
            line = "";
           letters = lines[i].ToCharArray();
           finished = false;

           for (int j = 0; j < letters.Length; j++) {

                if (letters[j] == '#') {
                    Debug.Log("Found Choice");
                    TextLine temp = new TextLine(counter, "#Choice", lines[i].Substring(1));
                    finished = true;
                    counter = counter + 1;
                    output.Add(temp);
                }

                if (!finished) {
                    if (!letters[j].Equals(':')) {
                        speaker = speaker + letters[j];

                    }else {
                        //saving the speaker and line to the TextLine Array
                        line = lines[i].Substring((j), (letters.Length - j));
                        output.Add(new TextLine(counter, speaker, line.Substring(1)));
                        counter = counter + 1;
                        finished = true;
                    }
                }
           }
        }

        //this will auto display the dialogue on the screen
        for (int i = 0; i < output.Count; i++) {

            DisplayText(output[i].speaker, output[i].text);
        }

    }


    //Choice System --------------------------------------------
    public void resetChoiceUI() {

        for (int i = 0; i < 2; i++) {
            Choice2UI.transform.Find(("Choice" + i.ToString())).Find("Text").gameObject.GetComponent<Text>().text = "";
        }

        for (int i = 0; i < 3; i++) {
            Choice3UI.transform.Find(("Choice" + i.ToString())).Find("Text").gameObject.GetComponent<Text>().text = "";
        }
    }

    public void displayChoices(string fileName) {


        //preparing for choices
        DialogueButton.GetComponent<Button>().interactable = false;
        WorldInteractObject.GetComponent<Interact>().WorldInteractsActive = false;
        choices.Clear();
        Responses.Clear();
        resetChoiceUI();


        //parsing choice file
        TextAsset ChoiceFile = Resources.Load("DialogueLines/DialogueChoices/" + fileName) as TextAsset;
        int totalChoices = -1;
        int currentChoiceIndex = 0;
        int newChoiceIndex = 1;

        string buffer = "";
        string[] textSplitted = ChoiceFile.text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        //easier working with list
        List<string> textList = new List<string>();

        for (int i = 0; i < textSplitted.Length; i++) {
            textList.Add(textSplitted[i]);
        }

        //checking options
        totalChoices = Convert.ToInt32(textList[0]);
        textList.RemoveAt(0);

        //going through the text from the choices file and assigning choices and responses
        for (int i = 0; i < textList.Count; i++) {

            buffer = "";
            char[] lineChars = textList[i].ToCharArray();
            bool finishedLine = false;
            int counter = 0;
            int internalLineCounter = 0 ;

            while ((!finishedLine)&&(counter<lineChars.Length)) {


                if (lineChars[counter].Equals(':')) {

                    //checking for choice Keyword and adding a new choice and response
                    if ((buffer.Equals("CHOICE"))||(buffer.Equals("choice"))) {
                        List<TextLine> responses = new List<TextLine>();
                        string choice = textList[i].Substring(counter + 1);
                        Responses.Add(newChoiceIndex, responses);
                        choices.Add(newChoiceIndex, choice);
                        currentChoiceIndex = currentChoiceIndex + 1;
                        newChoiceIndex = newChoiceIndex+1;
                        internalLineCounter = 0;
                        finishedLine = true;

                    } else {
                        //adding a response to the latest choice
                        if (currentChoiceIndex == 0)
                            throw new Exception("Invalid Choice Data File");

                        TextLine temp = new TextLine(internalLineCounter, buffer, textList[i].Substring(counter + 1));
                        Responses[currentChoiceIndex].Add(temp);
                        finishedLine = true;
                    }

                } else {
                    //incrementing buffer
                    buffer = buffer + lineChars[counter];
                }

                counter = counter + 1;
            }


        }

        //exception if invalid size
        if ((totalChoices > 4) || totalChoices < 1)
            throw new Exception("Min or Max choice amount invalid, check ChoiceFile " + fileName);

        //enabling correct choice display
        if (totalChoices == 2) {
            currentChoiceUIInUse = Choice2UI;
            Debug.Log("currentChoice System is 2");
            Debug.Log(currentChoiceUIInUse);
        } else if (totalChoices == 3) {
            currentChoiceUIInUse = Choice3UI;
            Debug.Log("currentChoice System is 3");

        }

        //enabling the canvas
        currentChoiceUIInUse.GetComponent<Canvas>().enabled = true;

        //setting choices
        for (int i = 0; i < totalChoices; i++) {
            currentChoiceUIInUse.transform.Find(("Choice" + i.ToString())).Find("Text").gameObject.GetComponent<Text>().text = choices[(i+1)];
        }

    }

    //2nd half of choices method, called by buttons and handling the response from the player
    public void receiveChoice(int returned) {

        resetChoiceUI();

        if (NPCQuestGiver == null) {
            currentChoiceUIInUse.GetComponent<Canvas>().enabled = false;
            currentChoiceUIInUse = null;
            choiceReturned = returned;

            //adding responses for choice to the dialogue queue
            for (int i = 0; i < Responses[choiceReturned].Count; i++) {
                speakerQueue.Insert(i, Responses[choiceReturned][i].speaker);
                dialogueQueue.Insert(i, Responses[choiceReturned][i].text);
            }

            //proceeding with dialogue and re-enabling button
            DialogueButton.GetComponent<Button>().interactable = true;
            OnClick();
        } else {

            Choice2UI.GetComponent<Canvas>().enabled = false;
            DialogueButton.GetComponent<Button>().interactable = true;
            questConfirmationPostponed = false;

            if (returned == 1) {

                NPCQuestGiver.GetComponent<QuestNPC>().playerConfirmationResponse(true);
                OnClick();

            } else {
                DialogueSystemUI.transform.Find("DialogueSpeakerName").Find("Text").gameObject.GetComponent<Text>().text = npcName;
                DialogueSystemUI.transform.Find("DialogueText").Find("Text").gameObject.GetComponent<Text>().text = rejectionStatement;
                NPCQuestGiver.GetComponent<QuestNPC>().playerConfirmationResponse(false);

            }

            NPCQuestGiver = null;

        }

        //DialogueSystemCleanUp();

    }

    //quest confirmation dialogue
    public void playerConfirmQuest(string npcName, GameObject NPCReference, string questRejectMessage) {
        NPCQuestGiver = NPCReference;
        rejectionStatement = questRejectMessage;
        this.npcName = npcName;

        if (!dialogueSystemInUse) {

            DialogueSystemUI.GetComponent<Canvas>().enabled = true;
            WorldInteractObject.GetComponent<Interact>().WorldInteractsActive = false;
            Player.GetComponent<SphereCollider>().enabled = false;
            menu.GetComponent<Menu_UI>().MenuDisable();

            DialogueSystemUI.transform.Find("DialogueText").Find("Text").gameObject.GetComponent<Text>().text = "Will you Accept " + npcName + "'s Request";

            Choice2UI.GetComponent<Canvas>().enabled = true;

            Choice2UI.transform.Find("Choice0").Find("Text").gameObject.GetComponent<Text>().text = "Accept";
            Choice2UI.transform.Find("Choice1").Find("Text").gameObject.GetComponent<Text>().text = "Decline";

            DialogueButton.GetComponent<Button>().interactable = false;

        } else {
            questConfirmationPostponed = true;
        }
    }

}
