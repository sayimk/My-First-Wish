using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is the item object class, it hold details amount items
[System.Serializable]
public class Item{

    //Vars
    public string itemName;
    public int itemAmount;
    public string itemDescription;

    //this is a string value to identify the Interaction type of the item, Use or Equip
    public string Use_Or_Equip_Item_Type;

    //Item Constructor
    public Item() {
        itemName ="";
        itemAmount = 0;
        itemDescription = "";
        Use_Or_Equip_Item_Type = "";
    }

    public Item(string itemName, string itemDescription, int itemAmount, string Use_Or_Equip_Item_Type){
        this.itemName = itemName;
        this.itemAmount = itemAmount;
        this.itemDescription = itemDescription;
        this.Use_Or_Equip_Item_Type = Use_Or_Equip_Item_Type;
    }

 
}
