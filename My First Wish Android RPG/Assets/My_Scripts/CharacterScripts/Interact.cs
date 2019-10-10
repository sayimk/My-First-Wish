using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Interact : MonoBehaviour {

    public GameObject MC;

    public bool WorldInteractsActive = true;

    // Update is called once per frame
    void Update () {

		if ((Input.touchCount == 1)&&(!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(0)) && (Input.GetTouch(0).phase==TouchPhase.Began) && (WorldInteractsActive == true)){
            MC.GetComponent<MC_Script>().MoveCharacter();
            Debug.Log("Moving");
        }

    }		
}
