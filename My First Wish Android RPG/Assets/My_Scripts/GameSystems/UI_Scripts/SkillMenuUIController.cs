using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//this class is used to control the UI for the skills menu
public class SkillMenuUIController : MonoBehaviour {

    private int currentListOffset = 0;
    private int currentButton = 0;
    SkillsManager playersManager = null;
    private Image tempButtonImage;


    public void loadInitialSkills() {

        for (int i = 0; i < 4; i++) {

            if (i < playersManager.getTotalSkillCount()) {
                if (!playersManager.getSkillInfoAt(i + currentListOffset).isSkillLocked()) {
                    gameObject.transform.Find("SkillsList").Find("Slot" + i.ToString()).Find("Text").gameObject.GetComponent<Text>().text = playersManager.getSkillInfoAt(i + currentListOffset).getSkillName();

                    gameObject.transform.Find("SkillsList").Find("SkillIcon"+i.ToString()).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Skill_Icons/"+playersManager.getSkillInfoAt(i + currentListOffset).getIconSpriteName());
                } else {
                    gameObject.transform.Find("SkillsList").Find("Slot" + i.ToString()).Find("Text").gameObject.GetComponent<Text>().text = "Locked";
                    gameObject.transform.Find("SkillsList").Find("SkillIcon" + i.ToString()).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Skill_Icons/Blank_Icon");

                }

            } else {
                gameObject.transform.Find("SkillsList").Find("Slot" + i.ToString()).Find("Text").gameObject.GetComponent<Text>().text = "---";
                gameObject.transform.Find("SkillsList").Find("SkillIcon" + i.ToString()).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Skill_Icons/Blank_Icon");

            }

        }

        gameObject.transform.Find("SetSkill1Button").gameObject.GetComponent<Button>().interactable = false;
        gameObject.transform.Find("SetSkill2Button").gameObject.GetComponent<Button>().interactable = false;
    }

    public void assignSkillManager(SkillsManager player) {
        playersManager = player;
    }

    //opening the skills menu
    public void openSkillsMenu() {
        gameObject.GetComponent<Canvas>().enabled = true;

        loadInitialSkills();
    }

    public void closeSkillMenu() {
        Debug.Log("Closing menu");
        gameObject.GetComponent<Canvas>().enabled = false;
    }




    public void scrollSkillsList(float val) {

        float multiplier = 20f;

        float output = val * multiplier;
        int listOffset = (int)Math.Truncate(output);

        Debug.Log(listOffset);

        for (int i = 0; i < 4; i++) {

            if (i + listOffset < playersManager.getTotalSkillCount()) {
                currentListOffset = listOffset;

                if (!playersManager.getSkillInfoAt(i + currentListOffset).isSkillLocked()) {
                    gameObject.transform.Find("SkillsList").Find("Slot" + i.ToString()).Find("Text").gameObject.GetComponent<Text>().text = playersManager.getSkillInfoAt(i + currentListOffset).getSkillName();
                    gameObject.transform.Find("SkillsList").Find("SkillIcon" + i.ToString()).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Skill_Icons/" + playersManager.getSkillInfoAt(i + currentListOffset).getIconSpriteName());

                } else {
                    gameObject.transform.Find("SkillsList").Find("Slot" + i.ToString()).Find("Text").gameObject.GetComponent<Text>().text = "Locked";
                    gameObject.transform.Find("SkillsList").Find("SkillIcon" + i.ToString()).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Skill_Icons/Blank_Icon");

                }

            } else {
                gameObject.transform.Find("SkillsList").Find("Slot" + i.ToString()).Find("Text").gameObject.GetComponent<Text>().text = "---";
                gameObject.transform.Find("SkillsList").Find("SkillIcon" + i.ToString()).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Skill_Icons/Blank_Icon");

            }
        }

    }


    public void displaySkillInfo(int buttonId) {

        if ((buttonId + currentListOffset) < playersManager.getTotalSkillCount()) {

            currentButton = buttonId;

            gameObject.transform.Find("SetSkill1Button").gameObject.GetComponent<Button>().interactable = false;
            gameObject.transform.Find("SetSkill2Button").gameObject.GetComponent<Button>().interactable = false;


            if ((!playersManager.getSlot1Skill().getSkillName().Equals(playersManager.getSkillInfoAt((buttonId + currentListOffset)).getSkillName())) &&
                (!playersManager.getSlot2Skill().getSkillName().Equals(playersManager.getSkillInfoAt((buttonId + currentListOffset)).getSkillName()))
                && (!playersManager.getSkillInfoAt((buttonId + currentListOffset)).isSkillLocked())) {

                gameObject.transform.Find("SetSkill1Button").gameObject.GetComponent<Button>().interactable = true;
                gameObject.transform.Find("SetSkill2Button").gameObject.GetComponent<Button>().interactable = true;

           } else {
                gameObject.transform.Find("SetSkill1Button").gameObject.GetComponent<Button>().interactable = false;
                gameObject.transform.Find("SetSkill2Button").gameObject.GetComponent<Button>().interactable = false;

                
            }

            //if the skill is still locked then enable the lock image
            if (playersManager.getSkillInfoAt((buttonId + currentListOffset)).isSkillLocked())
                gameObject.transform.Find("SkillLock").gameObject.GetComponent<Image>().enabled = true;
            else
                gameObject.transform.Find("SkillLock").gameObject.GetComponent<Image>().enabled = false;


            //setting skillName
            gameObject.transform.Find("SkillInfo").Find("SkillName").gameObject.GetComponent<Text>().text =
                playersManager.getSkillInfoAt((buttonId + currentListOffset)).getSkillName();

            //setting skillDescription
            gameObject.transform.Find("SkillInfo").Find("SkillDescription").gameObject.GetComponent<Text>().text =
                playersManager.getSkillInfoAt((buttonId + currentListOffset)).getSkillDescription();

            //setting accuracy
            gameObject.transform.Find("SkillInfo").Find("AccuracyTitle").gameObject.GetComponent<Text>().enabled = true;
            gameObject.transform.Find("SkillInfo").Find("AccuracyValue").gameObject.GetComponent<Text>().text =
                playersManager.getSkillInfoAt((buttonId + currentListOffset)).getAccuracy().ToString();

            //setting cooldown
            gameObject.transform.Find("SkillInfo").Find("CooldownTitle").gameObject.GetComponent<Text>().enabled = true;
            gameObject.transform.Find("SkillInfo").Find("CooldownValue").gameObject.GetComponent<Text>().text =
                playersManager.getSkillInfoAt((buttonId + currentListOffset)).getRequiredCooldownTime().ToString() + "s";


            //if there is a multiplier then setting it, otherwise blanking it
            if (playersManager.getSkillInfoAt((buttonId + currentListOffset)).getMultiplierValue() == 0.0f) {

                gameObject.transform.Find("SkillInfo").Find("MultiplierTitle").gameObject.GetComponent<Text>().enabled = false;
                gameObject.transform.Find("SkillInfo").Find("MultiplierValue").gameObject.GetComponent<Text>().text = "";

            } else {
                gameObject.transform.Find("SkillInfo").Find("MultiplierTitle").gameObject.GetComponent<Text>().enabled = true;
                gameObject.transform.Find("SkillInfo").Find("MultiplierValue").gameObject.GetComponent<Text>().text =
                    playersManager.getSkillInfoAt((buttonId + currentListOffset)).getMultiplierValue().ToString() + "x";

            }
        } else {

            //blanking fields
            gameObject.transform.Find("SkillInfo").Find("SkillName").gameObject.GetComponent<Text>().text = "";
            gameObject.transform.Find("SkillInfo").Find("SkillDescription").gameObject.GetComponent<Text>().text = "";

            gameObject.transform.Find("SkillInfo").Find("AccuracyTitle").gameObject.GetComponent<Text>().enabled = false;
            gameObject.transform.Find("SkillInfo").Find("AccuracyValue").gameObject.GetComponent<Text>().text ="";

            gameObject.transform.Find("SkillInfo").Find("CooldownTitle").gameObject.GetComponent<Text>().enabled = false;
            gameObject.transform.Find("SkillInfo").Find("CooldownValue").gameObject.GetComponent<Text>().text ="";

            gameObject.transform.Find("SkillInfo").Find("MultiplierTitle").gameObject.GetComponent<Text>().enabled = false;
            gameObject.transform.Find("SkillInfo").Find("MultiplierValue").gameObject.GetComponent<Text>().text = "";

            gameObject.transform.Find("SkillInfo").Find("BaseValueTitle").gameObject.GetComponent<Text>().enabled = false;
            gameObject.transform.Find("SkillInfo").Find("BaseValue").gameObject.GetComponent<Text>().text = "";

            gameObject.transform.Find("SetSkill1Button").gameObject.GetComponent<Button>().interactable = false;
            gameObject.transform.Find("SetSkill2Button").gameObject.GetComponent<Button>().interactable = false;

        }
    }

    public void setSkill(int i) {
        Debug.Log("Setting skill " + i.ToString());
        gameObject.transform.Find("SetSkill1Button").gameObject.GetComponent<Button>().interactable = false;
        gameObject.transform.Find("SetSkill2Button").gameObject.GetComponent<Button>().interactable = false;

        if (i == 1) {
            playersManager.setSkill1(playersManager.getSkillInfoAt(currentButton + currentListOffset).getSkillName());
            Debug.Log("Set skill 1 to " + (currentButton + currentListOffset));
        } else if (i == 2) {
            playersManager.setSkill2(playersManager.getSkillInfoAt(currentButton + currentListOffset).getSkillName());
            Debug.Log("Set skill 2 to " + (currentButton + currentListOffset));
        }

        gameObject.transform.parent.Find("SkillDisplay").Find("ActiveSkillIcons").Find("ActiveSkill"+i.ToString()).gameObject.GetComponent<Image>().sprite 
            = Resources.Load<Sprite>("Skill_Icons/" + playersManager.getSkillInfoAt(currentButton + currentListOffset).getIconSpriteName());

    }

}
