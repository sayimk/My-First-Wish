using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterClass : MonoBehaviour {

    public virtual void action(){
    }

    public virtual void MoveCharacter(){
    }

    public virtual void enterCombat() {
    }

    public virtual void leaveCombat() {

    }

    public virtual void standardAttack() {

    }


    public virtual void quickAttack() {

    }

    public virtual void heavyAttack() {

    }

    public virtual void setCurrentWeapon(GameObject weapon) {
    }
}
