using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatModifierClass {

    //this class is for holding and representing individual statModifiers and hold methods relating to individual modifiers

    private string modifierName;
    private string modifierType;

    //for now modifierValues are int, use floats if decimal values are needed
    //classes that need to be altered are statClass and StatModiferClass
    private int modifierValue;

    public StatModifierClass() {
    }

    //Stat Modifier types can be HP, Attack, Defence, Agility
    public StatModifierClass(string ModifierName,  string modifierType, int ModifierValue){
        modifierName = ModifierName;
        modifierValue = ModifierValue;
        this.modifierType = modifierType;
    }

    public string getModifierName() {
        return modifierName;
    }

    public string getModifierType() {
        return modifierType;
    }

    public void setModifierType(string type) {
        modifierType = type;
    }

    public void setModifierName(string ModifierName) {
        modifierName = ModifierName;
    }

    public int getModifierValue() {
        return modifierValue;

    }

    public void setModifierValue(int ModifierValue) {
        modifierValue = ModifierValue;
    }
}
