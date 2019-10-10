using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachQuestLocationMarker : MonoBehaviour {

    //whether or not the marker has been reached by the player
    public bool reachedMarker = false;
    private bool isMarkerActive = false;
    public GameObject Player;

    //an optional property where an action can be passed by the set method and
    // the optional action can be executed when the player reaches the location
    private Action locationAction = null;

    public ReachQuestLocationMarker() {

    }

    //used for setting an action to perform when the location has been reached
    public void setActionWhenLocationReached(Action action) {
        locationAction = action;
    }

    //when the player enters the colliders range, set the reached target to true
    public void OnTriggerEnter(Collider collider) {

        if ((collider.tag.Equals("Player")&&isMarkerActive&&!reachedMarker)) {
            reachedMarker = true;

            Player.GetComponent<QuestsManager>().updateAllQuests();

            if (locationAction != null)
                locationAction.DynamicInvoke();
        }

    }

    //used for fetching if the marker has been triggered
    public bool checkIfMarkerIsReached() {
        return reachedMarker;
    }

    //activating the marker when the quest has been issued 
    public void activeLocationMarker() {
        isMarkerActive = true;
    }

    //deactivating the marker
    public void deactiveLocationMarker() {
        isMarkerActive = false;
    }
}
