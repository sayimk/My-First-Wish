using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is a script for item chest and hold string names of items that are within the chest
public class ItemsHeld : MonoBehaviour {

    public List<string> items = new List<string>();
    public List<string> itemsDescription = new List<string>();
    public List<int> itemsAmount = new List<int>();
    public List<string> itemUse_Or_Equip_Type_Set = new List<string>();
    private List<Item> itemObjects = new List<Item>();
    public GameObject Player;
    private bool opened = false;

    public void Start() {

        Item itemTemp;

        for(int i=0; i<items.Count; i++) {
            itemTemp = new Item(items[i], itemsDescription[i], itemsAmount[i], itemUse_Or_Equip_Type_Set[i]);
            itemObjects.Add(itemTemp);
        }
    }

    //returns the list of items stored in this chest
    public List<string> getItemsNames() {
        return items;
    }

    //this is a method that is called then the user clicks on the chest, it will call addItems method from  the inventory class on the player
    public void AddToPlayerInventory() {
        if (!opened) {
            gameObject.GetComponent<Animator>().SetTrigger("open");
            Player.GetComponent<Player_Inventory>().addItems(itemObjects);
            opened = true;
            //Add PopUp message to DialogueSystem to Notify Player of obtained or already opened, switch model
            DialogueSystem.Main.displayNotification("You Have Obtained " + itemsString());
        } else {
            DialogueSystem.Main.displayNotification("You have already opened this chest");
        }
    }

    public string itemsString() {
        string itemsString = "";

        foreach (var item in items) {
            itemsString =itemsString + item;
            if (!item.Equals(items[(items.Count - 1)])) {
                itemsString = itemsString + ", ";
            }
        }

        return itemsString;
    }

    public bool isOpened() {
        return opened;
    }
}
