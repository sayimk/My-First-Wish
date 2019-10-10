using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Floor_2_Cave_Script : MonoBehaviour {

    public GameObject player;
    public GameObject Neru;
    public GameObject Tama;
    public GameObject Elliot;
    public GameObject menuBG;
    public GameObject MenuBiosSection;

    public Floor_2_Cave_Script currentStory { get; set; }

    void Awake() {
        if ((currentStory != this) && (currentStory != null))
            Destroy(gameObject);
        else
            currentStory = this;


    }

    void Start() {
        loadPlayerDataFromPrologue();
        menuBG.GetComponent<Image>().sprite = Resources.Load<Sprite>("Menu_Elements/bgs/Phone_bg_Cave");
        CaveScene1();

    }

    public void loadPlayerDataFromPrologue() {
        player.GetComponent<PlayerObjectSave>().loadPlayerDataFromFile();
    }

    //Initial scene when the group arrive at the cave an hear an amnious voice warn them
    public void CaveScene1() {
        DialogueSystem.Main.displayScriptedDialogueLines("CaveScene1");
        DialogueSystem.Main.setActionAfterDialogueQueueFinished(move1);
    }

    //move to the first path split
    public void move1() {

        Neru.GetComponent<SubCharactersBase>().moveSubCharacterToLocation(new Vector3(305.13f, 0f, 294.6f));
        Tama.GetComponent<SubCharactersBase>().moveSubCharacterToLocation(new Vector3(312.87f, 0f, 305.76f));
        Elliot.GetComponent<SubCharactersBase>().moveSubCharacterToLocation(new Vector3(300.18f, 0f, 300.4f));
        Neru.GetComponent<SubCharactersBase>().setPlayerEvent(CaveScene2);
    }

    public void CaveScene2() {

        DialogueSystem.Main.displayScriptedDialogueLines("CaveScene2");
        DialogueSystem.Main.setActionAfterDialogueQueueFinished(move2);
    }

    public void move2() {

        Neru.GetComponent<SubCharactersBase>().moveSubCharacterToLocation(new Vector3(158f, 0f, 363.99f));
        Tama.GetComponent<SubCharactersBase>().moveSubCharacterToLocation(new Vector3(174.71f, 0f, 362.63f));
        Elliot.GetComponent<SubCharactersBase>().moveSubCharacterToLocation(new Vector3(165.92f, 0f, 361.89f));
        Elliot.GetComponent<SubCharactersBase>().setPlayerEvent(CaveScene3);

    }

    public void CaveScene3() {
        DialogueSystem.Main.displayScriptedDialogueLines("CaveScene3");
        DialogueSystem.Main.setActionAfterDialogueQueueFinished(move2_5);
    }

    public void move2_5() {
        Neru.GetComponent<SubCharactersBase>().moveSubCharacterToLocation(new Vector3(267.57f, 0f, 420.26f));
        Tama.GetComponent<SubCharactersBase>().moveSubCharacterToLocation(new Vector3(273.5f, 0f, 413.5f));
        Elliot.GetComponent<SubCharactersBase>().moveSubCharacterToLocation(new Vector3(272.74f, 0f, 418.93f));
        Elliot.GetComponent<SubCharactersBase>().setPlayerEvent(intermissionEvent);

    }

    public void intermissionEvent() {
        DialogueSystem.Main.displayScriptedDialogueLines("CaveSceneIntermission");
        DialogueSystem.Main.setActionAfterDialogueQueueFinished(move3);
    }

    public void move3() {
        Neru.GetComponent<SubCharactersBase>().moveSubCharacterToLocation(new Vector3(278.26f, 0f, 536.36f));
        Tama.GetComponent<SubCharactersBase>().moveSubCharacterToLocation(new Vector3(310.44f, 0f, 519.13f));
        Elliot.GetComponent<SubCharactersBase>().moveSubCharacterToLocation(new Vector3(279f, 0f, 492.5f));

        //setting personal events
        Neru.GetComponent<SubCharactersBase>().setPlayerEvent(NeruPersonal1CaveVillage);
        Tama.GetComponent<SubCharactersBase>().setPlayerEvent(TamaPersonal1CaveVillage);
        Elliot.GetComponent<SubCharactersBase>().setPlayerEvent(ElliotPersonal1CaveVillage);
    }

    //personal events
    public void NeruPersonal1CaveVillage() {
        DialogueSystem.Main.displayScriptedDialogueLines("NeruPersonal1CaveVillage");
        DialogueSystem.Main.setActionAfterDialogueQueueFinished(moveNeruToSquare);
        MenuBiosSection.GetComponent<BioManager>().UnlockNextBio(BioManager.CharacterNeru);
    }

    public void ElliotPersonal1CaveVillage() {
        DialogueSystem.Main.displayScriptedDialogueLines("ElliotPersonal1CaveVillage");
        DialogueSystem.Main.setActionAfterDialogueQueueFinished(moveElliotToSquare);
        MenuBiosSection.GetComponent<BioManager>().UnlockNextBio(BioManager.CharacterElliot);
    }

    public void TamaPersonal1CaveVillage() {
        DialogueSystem.Main.displayScriptedDialogueLines("TamaPersonal1CaveVillage");
        DialogueSystem.Main.setActionAfterDialogueQueueFinished(moveTamaToSquare);
        MenuBiosSection.GetComponent<BioManager>().UnlockNextBio(BioManager.CharacterTama);
    }

    public void moveTamaToSquare() {
        Tama.GetComponent<SubCharactersBase>().moveSubCharacterToLocation(new Vector3(281.47f, 0f, 487.86f));
    }

    public void moveNeruToSquare() {
        Neru.GetComponent<SubCharactersBase>().moveSubCharacterToLocation(new Vector3(277.06f, 0f, 487.65f));
    }

    public void moveElliotToSquare() {
        Elliot.GetComponent<SubCharactersBase>().moveSubCharacterToLocation(new Vector3(279f, 0f, 491.11f));

    }

}
