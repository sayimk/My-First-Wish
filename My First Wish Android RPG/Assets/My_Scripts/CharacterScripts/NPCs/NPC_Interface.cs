using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface NPC_Interface{
    void interact();

    void setStoryTalkToDependentAction(Action DependentAction, bool needAcceptQuest);

    }
