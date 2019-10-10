using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BioProfile {

    private string fullName = "";
    private string firstName = "";
    private int currentViewIndex;
    private List<BioFile> bios = new List<BioFile>();

    public BioProfile() {

    }

    public BioProfile(string firstName, string fullName) {
        this.fullName = fullName;
        this.firstName = firstName;
        currentViewIndex = 0;
    }

    public string getFirstName() {
        return firstName;
    }

    public void setFirstName(string firstName) {
        this.firstName = firstName;
    }

    public string getFullName() {
        return fullName;
    }

    public void setFullName(string fullName) {
        this.fullName = fullName;
    }

    public List<BioFile> getAllBios() {
        return bios;
    }

    public void addBio(string bioTitle, string bioContent, bool locked) {
        BioFile temp = new BioFile(bioTitle, bioContent, locked);
        bios.Add(temp);
    }

    public BioFile getSpecificBio(int listId) {
        return bios[listId];
    }

    public BioFile searchViaTitle(string title) {

        for(int i =0; i < bios.Count; i++) {
            if (bios[i].Equals(title)) {
                return bios[i];
            }
        }

        return null;
    }

    public int getCurrentViewIndex() {
        return currentViewIndex;
    }

    public void incrementCurrentViewIndex() {

        if (currentViewIndex + 1 < bios.Count) {
            currentViewIndex = currentViewIndex + 1;
        }

    }

    public void decrementCurrentViewIndex() {
        if (currentViewIndex - 1 >= 0) {
            currentViewIndex = currentViewIndex - 1;
        }
    }

    public void unlockNextBio() {

        bool completed = false;
        int i = 0;

        while (!completed&&(i<bios.Count)) {

            if (bios[i].isLocked()) {
                bios[i].setLock(false);
                completed = true;
            }

            i++;
        }
    }

    public void changeViewIndexToUnlockedBioIndex() {

        int index = 0;

        for (int i = 0; i < bios.Count; i++) {
            if (!bios[i].isLocked())
                index = i;
        }

        currentViewIndex = index;
    }

    //format must be title:contents
    public void addBioFromResources(string pathFromResources, bool locked) {
        TextAsset fileContents = Resources.Load(pathFromResources) as TextAsset;

        if (!fileContents.text.Contains(":")) {
            throw new System.Exception("Invalid External Bio Format -" + pathFromResources);
        }

        char[] fileChars = fileContents.text.ToCharArray();

        string titleBuffer = "";
        string contentsBuffer = "";
        bool seenDivider = false;

        for (int i = 0; i < fileChars.Length; i++) {
            if (!seenDivider) {

                if (fileChars[i] != ':')
                    titleBuffer = titleBuffer + fileChars[i];
                else seenDivider = true;

            } else {
                contentsBuffer = contentsBuffer + fileChars[i];
            }
        }

        addBio(titleBuffer, contentsBuffer, locked);
    }

}
