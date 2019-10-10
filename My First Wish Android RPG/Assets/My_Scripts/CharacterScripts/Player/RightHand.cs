using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHand : MonoBehaviour {

    public void OnTriggerEnter(Collider col) {

        gameObject.transform.GetChild(0).gameObject.GetComponent<WeaponsInterface>().attackEnemyViaCollider(col);
        Debug.Log("Weapon hit enemy");
    }

}
