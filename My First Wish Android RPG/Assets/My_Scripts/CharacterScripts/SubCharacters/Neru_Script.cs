using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neru_Script : SubCharactersBase {

    void Start() {
        characterInit();
    }

    void Update() {

        checkIfArrivedAtDestination();
        updatePositionOfTellSystemUI();
    }

}
