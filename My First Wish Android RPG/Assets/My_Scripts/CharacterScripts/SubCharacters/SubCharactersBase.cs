using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//this class is for where the character specific classes will inherit from
public class SubCharactersBase : MonoBehaviour {

    public string walkAnimationTrigger;
    public string stopWalkingTrigger;
    protected bool moving = false;
    public int minDistance = 2;
    protected Vector3 currentDest;
    protected bool actionAvaliable = false;
    protected Action playerEvent;
    protected List<Action> eventQueue = new List<Action>();
    protected Action controlActionOnArrival;
    public GameObject TellSystemUI;
    public GameObject mainCamera;
    public GameObject player;


    public SubCharactersBase() {

    }
    public void updatePositionOfTellSystemUI() {
        if (TellSystemUI.GetComponent<Canvas>().enabled) {
            TellSystemUI.transform.LookAt(new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z));
        }
    }

    // call this method everyframe using monobehaviour Update() and it will auto check if the sub characters have reached their destination.
    public void checkIfArrivedAtDestination() {

        if (moving && (Vector3.Distance(gameObject.transform.parent.position, currentDest) < ((float)minDistance))) {
            gameObject.GetComponent<Animator>().SetTrigger(stopWalkingTrigger);
            gameObject.GetComponent<SphereCollider>().isTrigger = true;
            moving = false;
            if (controlActionOnArrival != null) {
                controlActionOnArrival.DynamicInvoke();
                controlActionOnArrival = null;
            }
        }

    }

    public void characterInit() {
        //TellSystemUI.GetComponent<Canvas>().enabled = false ;

    }

    //call this method to have the characters move to a new 
    public void moveSubCharacterToLocation(Vector3 newLocation) {
        gameObject.transform.parent.GetComponent<NavMeshAgent>().SetDestination(newLocation);
        gameObject.GetComponent<Animator>().SetTrigger(walkAnimationTrigger);
        currentDest = newLocation;
        gameObject.GetComponent<SphereCollider>().isTrigger = false;
        moving = true;

    }

    //move character to new location and execute a action on arrival
    public void moveSubCharacterToLocation(Vector3 newLocation, Action ActionOnArrivalAtDestination) {
        gameObject.transform.parent.GetComponent<NavMeshAgent>().SetDestination(newLocation);
        gameObject.GetComponent<Animator>().SetTrigger(walkAnimationTrigger);
        currentDest = newLocation;
        gameObject.GetComponent<SphereCollider>().isTrigger = false;
        moving = true;
        controlActionOnArrival = ActionOnArrivalAtDestination;
    }

    //set up a player event that the player can interact with
    public void setPlayerEvent(Action playerEvent) {
        Debug.Log("Event set to" + playerEvent.Method.Name);
        eventQueue.Add(playerEvent);

        TellSystemUI.GetComponent<Canvas>().enabled = true;
        Debug.Log(TellSystemUI.GetComponent<Canvas>().enabled);
    }

    //Event handler to start the playerEvent
    public void activatePlayerStoryEvent() {

        Debug.Log(eventQueue.Count);

        if (!moving&&(eventQueue.Count != 0)) {
            Debug.Log("Executing event");

            gameObject.transform.parent.LookAt(player.transform.parent);

            if (playerEvent == null&&(eventQueue.Count!=0)) {
                Debug.Log("Fetching next event from Queue");
                playerEvent = eventQueue[0];
                playerEvent.DynamicInvoke();
            } else if (playerEvent == eventQueue[0]) {

                Debug.Log("Current event has been played, resetting");
                playerEvent = null;
                eventQueue.RemoveAt(0);

                if (eventQueue.Count > 0) {
                    Debug.Log("Replacing old event with new one");
                    playerEvent = eventQueue[0];
                    playerEvent.DynamicInvoke();
                }
            }


            if (eventQueue.Count < 2) {
                Debug.Log("Less than 2 events, disabling icon");
                TellSystemUI.GetComponent<Canvas>().enabled = false;
            }


        }

        

    }

}
