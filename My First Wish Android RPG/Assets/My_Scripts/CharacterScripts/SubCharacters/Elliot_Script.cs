using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elliot_Script : SubCharactersBase {

    void Start() {
        characterInit();
    }

    void Update() {
        checkIfArrivedAtDestination();
        updatePositionOfTellSystemUI();
    }

}
