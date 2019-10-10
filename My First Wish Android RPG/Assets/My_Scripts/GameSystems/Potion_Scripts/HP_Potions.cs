using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_Potions : MonoBehaviour, Potions_Interface {

    //this is named like this to make is easily understanded in Inspector
    public string HP_Attack_Defence_Agility_Modified_By_Potion;

    public int modifiedAmount;


    public string getStatModified() {
        return HP_Attack_Defence_Agility_Modified_By_Potion;
    }

    public int getPotionModiferAmount() {
        return modifiedAmount;
    }
}
