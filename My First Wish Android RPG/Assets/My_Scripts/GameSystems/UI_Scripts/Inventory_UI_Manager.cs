using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//this script is for easily managing all of the inventory slot and handle the button events, the Menu UI will call a method from here to activate the Inventory grid,
// this will then fetch the information from the Inventory 
public class Inventory_UI_Manager : MonoBehaviour {

    //this property is a reference from the Menu UI
    public Player_Inventory playerInventoryReference;

    //these are the Inventory Slot References for setting slot items, and the information grid for accessing panel information
    public GameObject inventorySlots;
    public GameObject informationGrid;
    public GameObject InventoryViewer;
    public GameObject playerRightHand;
    public GameObject playerLeftHand;
    public GameObject PlayerCharacter;
    public GameObject currentlyEquipped;
    public GameObject currentWeaponHolder;
    public GameObject currentlyViewed;
    public GameObject CurrentlyEquippedUIDisplay;

    private string currentlyViewedItemName;
    private List<StatModifierClass> currentWeaponBonuses;

    //this method is for rendering/filling the inventory grid
    public void RenderInventoryUIGrid(Player_Inventory playerInventoryScript) {
        playerInventoryReference = playerInventoryScript;
        string slotName = "";

        for(int i=0; ((i < playerInventoryReference.getTotalInventoryItemAmount()) && (i < 15)); i++) {
            slotName = ("Slot" + i.ToString());
            inventorySlots.transform.Find(slotName).gameObject.GetComponent<Image>().enabled = true;
            inventorySlots.transform.Find(slotName).gameObject.GetComponent<Button>().enabled = true;
            inventorySlots.transform.Find(slotName).Find("Text").gameObject.GetComponent<Text>().enabled = true;
            inventorySlots.transform.Find(slotName).Find("Text").gameObject.GetComponent<Text>().text = playerInventoryReference.getItemNameFromList(i);
        }
    }

    //this method will close and reset the inventory item grid
    public void CloseAndResetInventoryUIGrid() {
        string slotName = "";

        for (int i = 0; (i<15); i++) {
            slotName = ("Slot" + i.ToString());
            inventorySlots.transform.Find(slotName).gameObject.GetComponent<Image>().enabled = false;
            inventorySlots.transform.Find(slotName).gameObject.GetComponent<Button>().enabled = false;
            inventorySlots.transform.Find(slotName).Find("Text").gameObject.GetComponent<Text>().enabled = false;
        }

        //cleaning up information Grid Contents
        informationGrid.transform.Find("ItemName").gameObject.GetComponent<Text>().text = "";
        informationGrid.transform.Find("ItemAmount").gameObject.GetComponent<Text>().text = "";
        informationGrid.transform.Find("ItemDescription").gameObject.GetComponent<Text>().text = "";
        informationGrid.transform.Find("ItemDescription").gameObject.GetComponent<Text>().text = "";
        informationGrid.transform.Find("AmountHeader").gameObject.GetComponent<Text>().enabled = false;

        //Disabling view Camera
        informationGrid.transform.Find("ItemView").gameObject.GetComponent<RawImage>().enabled = false;

        if (currentlyViewed != null) {
            Destroy(currentlyViewed);
        }

        if(playerInventoryReference!=null)
            playerInventoryReference.cleanUpEmptyItems();


    }

    //this will handle the UI event when an inventory item is selected, the item information will be displayed in the inventory grid, also sets the use or equip button for item interaction
    public void InventoryButtonEventHandler(int InventoryLocation) {
        informationGrid.transform.Find("ItemName").gameObject.GetComponent<Text>().text = playerInventoryReference.getItemNameFromList(InventoryLocation);
        informationGrid.transform.Find("ItemAmount").gameObject.GetComponent<Text>().text = playerInventoryReference.getInventoryItemAmount(InventoryLocation).ToString();
        informationGrid.transform.Find("ItemDescription").gameObject.GetComponent<Text>().text = playerInventoryReference.getItemDescription(InventoryLocation);
        informationGrid.transform.Find("ItemDescription").gameObject.GetComponent<Text>().text = playerInventoryReference.getItemDescription(InventoryLocation);
        informationGrid.transform.Find("AmountHeader").gameObject.GetComponent<Text>().enabled = true;
        currentlyViewedItemName = playerInventoryReference.getItemNameFromList(InventoryLocation);

        //setting the currentlyViewed prefab and enabling live preview

        if (currentlyViewed != null)
            Destroy(currentlyViewed);

        informationGrid.transform.Find("ItemView").gameObject.GetComponent<RawImage>().enabled = true;

        currentlyViewed = (GameObject)Instantiate(Resources.Load<GameObject>("InventoryItemSprites/"+ playerInventoryReference.getItemNameFromList(InventoryLocation)+"View"), InventoryViewer.transform);

        //set child of InventoryView to the prefab View version of the Item in storage
        if (((currentlyEquipped==null)||!currentlyEquipped.GetComponent<WeaponsInterface>().getWeaponName().Equals(playerInventoryReference.getItemNameFromList(InventoryLocation)))) {
            if (playerInventoryReference.getItem_Use_or_Equip_Type(InventoryLocation).Equals("Equip")) {
                DisplayEquipUIButton();
            } else {
                DisplayUseUIButton();
            }
        }

    }

    //this will handle the UI event for equipping an Item
    public void EquipButtonEventHandler() {
            
        //Removing any existing weapons
        if (currentlyEquipped != null) {
            Destroy(currentlyEquipped);
        }

        //equipping weapon
         currentlyEquipped = (GameObject)Instantiate(Resources.Load<GameObject>("WeaponObjects/" + currentlyViewedItemName), playerRightHand.transform);
         currentWeaponHolder = (GameObject)Instantiate(Resources.Load<GameObject>("WeaponObjects/" + currentlyViewedItemName+"Holder"), playerLeftHand.transform);
        //removing old stat Bonuses
        if (currentWeaponBonuses != null) {
            PlayerCharacter.GetComponent<CharacterStats>().removeBonusModifiers(currentWeaponBonuses);
            currentWeaponBonuses = null;
        }

        //Applying Stat Boosts from the weapon onto the players character
        currentWeaponBonuses = currentlyEquipped.GetComponent<WeaponsInterface>().getWeaponStatBonus();
        PlayerCharacter.GetComponent<CharacterStats>().addBonusModifier(currentWeaponBonuses);
        CurrentlyEquippedUIDisplay.GetComponent<Text>().text= currentlyEquipped.GetComponent<WeaponsInterface>().getWeaponName();
        DisableUIInventoryButtons();

        //setting weapon onto baseCharacterClass
        PlayerCharacter.GetComponent<MC_Script>().setCurrentWeapon(currentlyEquipped);
        PlayerCharacter.GetComponent<MC_Script>().setCurrentWeaponHolder(currentWeaponHolder);

        //sending Character Stats to the child Weapon for Damage Calculations
        currentlyEquipped.GetComponent<WeaponsInterface>().setCharacterStatsValues(PlayerCharacter);

        //updating Game UI
        PlayerCharacter.GetComponent<CharacterStats>().updateUIHPGauge();

        //puts weapon away
        PlayerCharacter.GetComponent<MC_Script>().putSwordinHolder();
    }

    //this will handle the UI event for using an item
    public void UseButtonEventHandler() {
        Debug.Log("Item Used");

        Debug.Log(currentlyViewedItemName);
        playerInventoryReference.useItem(playerInventoryReference.searchForItemName(currentlyViewedItemName));

        //Clause for a healing potion
        if (currentlyViewed.GetComponent<Potions_Interface>().getStatModified().Equals("HP")) {
            PlayerCharacter.GetComponent<CharacterStats>().healPlayerHP(currentlyViewed.GetComponent<Potions_Interface>().getPotionModiferAmount());
        }
        //add different types of potions here





        Debug.Log("Changing inventory amount");
        informationGrid.transform.Find("ItemAmount").gameObject.GetComponent<Text>().text = playerInventoryReference.getInventoryItemAmount(playerInventoryReference.searchForItemName(currentlyViewedItemName)).ToString();
        PlayerCharacter.GetComponent<CharacterStats>().updateUIHPGauge();

        if (playerInventoryReference.getInventoryItemAmount(playerInventoryReference.searchForItemName(currentlyViewedItemName))==0) {
            informationGrid.transform.Find("UseButton").gameObject.GetComponent<Button>().interactable = false;
        } else {
            informationGrid.transform.Find("UseButton").gameObject.GetComponent<Button>().interactable = true;
        }

    }

    //this method will toggle the Use Button on and the Equip Button off
    public void DisplayUseUIButton() {
        informationGrid.transform.Find("EquipButton").gameObject.GetComponent<Button>().enabled = false;
        informationGrid.transform.Find("EquipButton").gameObject.GetComponent<Image>().enabled = false;
        informationGrid.transform.Find("EquipButton").Find("Text").gameObject.GetComponent<Text>().enabled = false;

        informationGrid.transform.Find("UseButton").gameObject.GetComponent<Button>().enabled = true;
        informationGrid.transform.Find("UseButton").gameObject.GetComponent<Image>().enabled = true;
        informationGrid.transform.Find("UseButton").Find("Text").gameObject.GetComponent<Text>().enabled = true;

        if (playerInventoryReference.getInventoryItemAmount(playerInventoryReference.searchForItemName(currentlyViewedItemName)) == 0) {
            informationGrid.transform.Find("UseButton").gameObject.GetComponent<Button>().interactable = false;
        } else {
            informationGrid.transform.Find("UseButton").gameObject.GetComponent<Button>().interactable = true;
        }

    }

    //this method will toggle the Equip Button on and the use button off
    public void DisplayEquipUIButton() {
        informationGrid.transform.Find("EquipButton").gameObject.GetComponent<Button>().enabled = true;
        informationGrid.transform.Find("EquipButton").gameObject.GetComponent<Image>().enabled = true;
        informationGrid.transform.Find("EquipButton").Find("Text").gameObject.GetComponent<Text>().enabled = true;

        informationGrid.transform.Find("UseButton").gameObject.GetComponent<Button>().enabled = false;
        informationGrid.transform.Find("UseButton").gameObject.GetComponent<Image>().enabled = false;
        informationGrid.transform.Find("UseButton").Find("Text").gameObject.GetComponent<Text>().enabled = false;
    }

    public void DisableUIInventoryButtons() {
        informationGrid.transform.Find("EquipButton").gameObject.GetComponent<Button>().enabled = false;
        informationGrid.transform.Find("EquipButton").gameObject.GetComponent<Image>().enabled = false;
        informationGrid.transform.Find("EquipButton").Find("Text").gameObject.GetComponent<Text>().enabled = false;

        informationGrid.transform.Find("UseButton").gameObject.GetComponent<Button>().enabled = false;
        informationGrid.transform.Find("UseButton").gameObject.GetComponent<Image>().enabled = false;
        informationGrid.transform.Find("UseButton").Find("Text").gameObject.GetComponent<Text>().enabled = false;
    }
}
