using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour {

    //UI
    public GameObject gameControlUI;
    public GameObject titleUI;
    public GameObject titleCamera;
    public GameObject mainCamera;
    public GameObject FloorScriptObject;

    public void startButton() {

        titleUI.GetComponent<Canvas>().enabled = false;
        titleCamera.GetComponent<Animator>().SetTrigger("PanOut");
    }

    public void panOutComplete() {

        //disabling TitleScreen Camera
        titleCamera.GetComponent<Camera>().enabled = false;
        titleCamera.GetComponent<AudioListener>().enabled = false;
        titleCamera.GetComponent<FlareLayer>().enabled = false;
        titleCamera.GetComponent<Animator>().enabled = false;


        //enabling MainCamera
        mainCamera.GetComponent<AudioListener>().enabled = true;
        mainCamera.GetComponent<FlareLayer>().enabled = true;
        gameControlUI.transform.Find("MobileJoystick").gameObject.GetComponent<Image>().enabled = true;
        gameControlUI.transform.Find("MenuOpen").gameObject.GetComponent<Image>().enabled = true;
        gameControlUI.transform.Find("HeavyAttack").gameObject.GetComponent<Image>().enabled = true;
        gameControlUI.transform.Find("Quick Attack").gameObject.GetComponent<Image>().enabled = true;
        gameControlUI.transform.Find("StatusBar").gameObject.GetComponent<Image>().enabled = true;
        gameControlUI.transform.Find("HPGauge").gameObject.GetComponent<Canvas>().enabled = true;
        gameControlUI.transform.Find("Button").gameObject.GetComponent<Image>().enabled = true;



        //starting prologue floor event
        FloorScriptObject.GetComponent<Floor_1_Prologue_Story_Events>().startingDialogueScene();
    }
}
