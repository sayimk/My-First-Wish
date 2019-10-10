using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BioManager : MonoBehaviour {

    //ProfileConstants
    public const string CharacterNeru = "Neru";
    public const string CharacterTama = "Tama";
    public const string CharacterElliot = "Elliot";
    public const string CharacterAkira = "Akira";

    //Profile Instances
    BioProfile Tama = new BioProfile("Tama", "Tama Claudius");
    BioProfile Neru = new BioProfile("Neru", "Neru Claudius");
    BioProfile Akira = new BioProfile("Akira", "Akira Crawford");
    BioProfile Elliot = new BioProfile("Elliot", "Elliot Crawford");

    //UI Elements
    GameObject textArea;
    GameObject nameTag;

    //Tracking parameters
    BioProfile currentlyViewBio = null;
    public BioManager() {

    }

    public void Awake() {
        //creating shortcut variables
        textArea = gameObject.transform.Find("TextArea").gameObject;
        nameTag = gameObject.transform.Find("NameTag").gameObject;
    }

    //using the method for setting up the bio profiles
    public void Start() {

        //Loading Bio1s and locking
        Tama.addBioFromResources("BioFiles/TamaBio1", true);
        Neru.addBioFromResources("BioFiles/NeruBio1", true);
        Elliot.addBioFromResources("BioFiles/ElliotBio1", true);
        Akira.addBioFromResources("BioFiles/AkiraBio1", true);

    }

    //event Handlers
    //for going to the next bio page
    public void forwardButton() {
        currentlyViewBio.incrementCurrentViewIndex();
        displayBioInformation(currentlyViewBio);
    }

    //for going to the previous bio page
    public void backButtons() {
        currentlyViewBio.decrementCurrentViewIndex();
        displayBioInformation(currentlyViewBio);
    }

    //for loading Akira's Bio
    public void AkiraBio() {
        currentlyViewBio = Akira;
        nameTag.transform.Find("Text").gameObject.GetComponent<Text>().text = Akira.getFullName();
        displayBioInformation(Akira);
    }

    //for loading Elliot's Bio
    public void ElliotBio() {
        currentlyViewBio = Elliot;
        nameTag.transform.Find("Text").gameObject.GetComponent<Text>().text = Elliot.getFullName();
        displayBioInformation(Elliot);
    }

    //for loading Neru's Bio
    public void NeruBio() {
        currentlyViewBio = Neru;
        nameTag.transform.Find("Text").gameObject.GetComponent<Text>().text = Neru.getFullName();
        displayBioInformation(Neru);

    }

    //for loading Tama's Bio
    public void TamaBio() {
        Debug.Log("Clicked Tama");
        currentlyViewBio = Tama;
        nameTag.transform.Find("Text").gameObject.GetComponent<Text>().text = Tama.getFullName();
        displayBioInformation(Tama);

    }

    //used for unlocking the next bio that is locked
    public void UnlockNextBio(string CharacterName) {

        switch (CharacterName) {
            default:
                break;

            case "Akira": {
                Akira.unlockNextBio();
                Akira.changeViewIndexToUnlockedBioIndex();
            }

                break;

            case "Neru": {
                Neru.unlockNextBio();
                Neru.changeViewIndexToUnlockedBioIndex();
            }

                break;

            case "Tama": {
                  Tama.unlockNextBio();
                  Tama.changeViewIndexToUnlockedBioIndex();
            }

                break;

            case "Elliot": {
                   Elliot.unlockNextBio();
                   Elliot.changeViewIndexToUnlockedBioIndex();
            }

                break;
        }
    }

    public void setTextAreaToLockedPlaceHolder() {
        textArea.transform.Find("Title").gameObject.GetComponent<Text>().text = "Locked";
        textArea.transform.Find("Content").gameObject.GetComponent<Text>().text = "---";
    }

    public void setTextArea(string title, string contents) {
        textArea.transform.Find("Title").gameObject.GetComponent<Text>().text = title;
        textArea.transform.Find("Content").gameObject.GetComponent<Text>().text = contents;
    }

    public void displayBioInformation(BioProfile sourceBio) {
        BioFile temp = sourceBio.getSpecificBio(sourceBio.getCurrentViewIndex());

        if (temp.isLocked()) {
            setTextAreaToLockedPlaceHolder();
        } else {
            setTextArea(temp.getBioTitle(), temp.getBioContents());
        }
    }
}
