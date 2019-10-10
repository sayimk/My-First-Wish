using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tama_Script : SubCharactersBase {

    void Start() {
        characterInit();
    }

    void Update() {
       checkIfArrivedAtDestination();
       updatePositionOfTellSystemUI();
    }
}
