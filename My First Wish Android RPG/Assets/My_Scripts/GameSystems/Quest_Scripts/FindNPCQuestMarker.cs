using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class is a FindNPCQuestMarker, it is used to mark specific NPCs that need to be talked to 
public class FindNPCQuestMarker : MonoBehaviour {

    private bool isMarkerActive = false;
    private Action action = null;
    public bool hasSpokenWith = false;

    public FindNPCQuestMarker() {

    }

    //used to set the action to perform when the player has spoken with the NPC
    public void setActionWhenNPCSpokenWith(Action action) {
        this.action = action;
    }

    //triggers the marker and shows that the npc has been spoken to
    public void talkToQuestTargetNPC() {

        if ((!hasSpokenWith) && (isMarkerActive)) {
            hasSpokenWith = true;
            if (action != null)
                action.DynamicInvoke();
        }
    }

    //used to return markers status
    public bool hasTargetNPCBeenFoundAndSpokenTo() {
        return hasSpokenWith;
    }

    //activating the marker when the quest has been issued
    public void activateFindNPCMarker() {
        isMarkerActive = true;
    }

    //deactivating the marker
    public void deactivateFindNPCMarker() {
        isMarkerActive = false;
    }
}
