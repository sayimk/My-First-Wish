using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Allows use of Unity's JSON Converter
//vars are publics for use with Unity's JSON Converter
[System.Serializable]

//this class represents individual text paragraphs that a person may speak
public class TextLine{

    public int lineID;
    public string speaker;
    public string text;


    public TextLine(int lineID, string speaker, string text) {
        this.lineID = lineID;
        this.text = text;
        this.speaker = speaker;
    }
}
