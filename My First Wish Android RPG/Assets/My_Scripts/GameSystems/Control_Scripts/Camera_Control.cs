using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Camera_Control : MonoBehaviour {
    //camera Variables
	float x;
	float z;
	public float speed = 10;
    GameObject compass;
    //compass Variables
    private bool display = false;
    private float targetX;
    private float targetY;
    private float targetZ;


	// Use this for initialization
	void Start () {
        Screen.SetResolution(1280,720,true);
		x = Camera.main.transform.position.x;
		z = Camera.main.transform.position.z;
        compass = gameObject.transform.Find("Compass").gameObject;
        compass.SetActive(false);
    }

	// Update is called once per frame
	void Update () {


		z = z + (CrossPlatformInputManager.GetAxis ("Horizontal")/speed);
		x = x + (-CrossPlatformInputManager.GetAxis ("Vertical")/speed);
        

		Camera.main.transform.position = new Vector3 (x, Camera.main.transform.position.y,z);

        //for updating the roation of the compass
        if (display) {
            compass.transform.LookAt(new Vector3(targetX, targetY, targetZ));
        }
	}

    public void setNewTargetCompass(float targetX, float targetY, float targetZ) {

        this.targetX = targetX;
        this.targetY = targetY;
        this.targetZ = targetZ;

        compass.SetActive(true);
        display = true;
    }

    public void endTargetCompass() {

        display = false;
        compass.SetActive(false);
    }

}
