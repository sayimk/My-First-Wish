using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatClass {

    //this class will be used to represent a single stat in the characterstats, this has methods for the stat information as well as calculating the final modified stat val

    public string statName { get; set; }
    public string statDescription { get; set; }
    public int statBaseValue { get; set; }
    private List<StatModifierClass> statModifiers = new List<StatModifierClass>();
    private int modifiedStatValue;

    //statClass constructor
    public StatClass(string StatName, string StatDescription, int StatBaseValue) {
        statName = StatName;
        statDescription = StatDescription;
        statBaseValue = StatBaseValue;
    }
    
    public List<StatModifierClass> getModifierList() {
        return statModifiers;
    }

    //method for adding a modifier to the active modifiers list
    public void addModifier(StatModifierClass statModifier) {
        statModifiers.Add(statModifier);
    }

    //method for removing the modifier from the list of active stat modifiers
    public void removeModifier(StatModifierClass statModifier) {
        int index = -1;
        //change so that it searches for correct statbonus name using if and if it doesn't exist, do nothing

        for (int i = 0; i<statModifiers.Count; i++) {
            if ((statModifiers[i].getModifierName() == statModifier.getModifierName())&&(statModifiers[i].getModifierValue()==statModifier.getModifierValue())) {
                index = i;
            }
        }

        if (index!= -1) {
            statModifiers.RemoveAt(index);
        }
    }

    //method for returning the calculated stat value
    private void calculateModifiedStatValue() {
        modifiedStatValue = statBaseValue;
        foreach (var modifier in statModifiers) {
            modifiedStatValue = modifiedStatValue + modifier.getModifierValue();
        }
    }

    //method for returning the modified stat value
    public int getModifiedStatValue() {
        calculateModifiedStatValue();
        return modifiedStatValue;
    }
}
