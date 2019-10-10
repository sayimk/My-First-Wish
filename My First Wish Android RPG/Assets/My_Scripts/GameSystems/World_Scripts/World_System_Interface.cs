using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World_System_Interface : MonoBehaviour {

    //my static instance of this class used by other classes to manage WorldSystems
    public static World_System_Interface main { get; set; }

    public GameObject AmbientAudio;
    public GameObject BattleMusic;

    //Add other WorldSystem Objects here when required (excluding Dialogue System)

   void Awake() {
        main = this;
        playAmbientAudio();
    }

    public bool playAmbientAudio() {

        if (main.AmbientAudio.GetComponent<AudioSource>().isPlaying) {
            return false;
        } else {
            main.AmbientAudio.GetComponent<AudioSource>().Play();
            return true;
        }
    }

    public bool stopAmbientAudio() {

        if (main.AmbientAudio.GetComponent<AudioSource>().isPlaying) {
            main.AmbientAudio.GetComponent<AudioSource>().Stop();
            return true;
        }else return false;

    }

    public bool playBattleAudio() {

        if (main.BattleMusic.GetComponent<AudioSource>().isPlaying) {
            return false;
        } else {
            main.BattleMusic.GetComponent<AudioSource>().Play();
            return true;
        }
    }

    public bool stopBattleAudio() {

        if (main.BattleMusic.GetComponent<AudioSource>().isPlaying) {
            main.BattleMusic.GetComponent<AudioSource>().Stop();
            return true;
        } else return false;

    }

    public bool stopBattleAudioWithFadeOut() {

        if (main.BattleMusic.GetComponent<AudioSource>().isPlaying) {
            gameObject.GetComponent<Animator>().SetTrigger("FadeOut");
            return true;
        } else return false;
    }
}
