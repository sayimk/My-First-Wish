using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is the player inventory class, it will manage an hold all of the players items that they obtain from the gameworld via Chests
public class Player_Inventory : MonoBehaviour {

    List<Item> InventoryItems = new List<Item>();
    
    
    //Method for adding an item to the Inventory
    public void addItems(List<Item> itemsList) {

        bool found = false;
        List<Item> ItemsToBeAdded = new List<Item>();

        //checking each item to be added to check if they exist and then Incrementing existing amount, if it doesnt exist, then adding it later via another list
        for (int i=0; i < itemsList.Count; i++) {
            for (int j = 0; j < InventoryItems.Count; j++) {

                if (itemsList[i].itemName.Equals(InventoryItems[j].itemName)) {
                    found = true;
                    InventoryItems[j].itemAmount = (InventoryItems[j].itemAmount + itemsList[i].itemAmount);
                }
            }

            //checking if the item was found in the Inventory, if it wasn't add it later
            if (!found) {
                ItemsToBeAdded.Add(itemsList[i]);
            }
       
            found = false;
        }

        //adding items that were not found in the inventory
        InventoryItems.AddRange(ItemsToBeAdded);
    }

    //method for getting inventory amount
    public int getInventoryItemAmount(int location) {
        return InventoryItems[location].itemAmount;
    }

    //method for fetching item names
    public string getItemNameFromList(int location) {
        if (location >= InventoryItems.Count)
            return "";
        return InventoryItems[location].itemName;
    }

    //method for getting total inventory amount
    public int getTotalInventoryItemAmount() {
        return InventoryItems.Count;
    }

    //for fetching item description
    public string getItemDescription(int location) {
        return InventoryItems[location].itemDescription;
    }

    public int searchForItemName(string itemName) {

        int index = -1;

        for (int i = 0; i < InventoryItems.Count; i++) {
            if (InventoryItems[i].itemName.Equals(itemName)) {
                index = i;
            }
        }

        return index;
    }

    //for getting item types
    public string getItem_Use_or_Equip_Type(int location) {
        return InventoryItems[location].Use_Or_Equip_Item_Type;
    }

    public bool useItem(int location) {

        if ((InventoryItems[location].itemAmount - 1 >= 0)) {
            InventoryItems[location].itemAmount = InventoryItems[location].itemAmount - 1;
            return true;
        } else {
            return false;
        }
    }

    public void cleanUpEmptyItems() {

        List<Item> nonEmptyItems = new List<Item>();


        for (int i = 0; i < InventoryItems.Count; i++) {

            if (InventoryItems[i].itemAmount > 0) {
                nonEmptyItems.Add(InventoryItems[i]);
            }

        }

        InventoryItems.Clear();
        InventoryItems.AddRange(nonEmptyItems);
    }

    public List<Item> getInventory() {
        List<Item> temp = InventoryItems;
        return temp;
    }

    //loads saved inventory data
    public void loadData(PlayerObjectData playerObjectData) {
        InventoryItems.Clear();
        InventoryItems.AddRange(playerObjectData.InventoryItems);

        Debug.Log("Inventory Data Loaded");
    }


}
