using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BioFile {

    private string bioTitle = "";
    private string bioContent = "";
    private bool locked = true;

    public BioFile() {

    }

    public BioFile(string bioTitle, string bioContent, bool locked) {
        this.bioTitle = bioTitle;
        this.bioContent = bioContent;
        this.locked = locked;
    }

    public string getBioTitle() {
        return bioTitle;
    }

    public string getBioContents() {
        return bioContent;
    }

    public bool isLocked() {
        return locked;
    }

    public void setLock(bool locked) {
        this.locked = locked;
    }
}
