using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenCameraEventHandler : MonoBehaviour {

    public GameObject TitleScreenUI;


    public void callEventAfterPanOutAnimation() {
        TitleScreenUI.GetComponent<TitleScreen>().panOutComplete();
    }
}
