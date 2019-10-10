using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//for a quest type to be accepted by the player's quest Monitor system, it must implment this interface
public interface QuestsInterface {

    void updateQuestCompleteStatus(Player_Log player_Log);

    bool getQuestCompleteStatus();

    void intializeQuest();

    string questGiver { set; get; }
    string questName { set; get; }
    string questType { set; get; }
    string questDescription { set; get; }
    List<GameObject> targets { set; get; }
    List<Item> questRewardList { set; get; }
    int countAtAddedTime { get; set; }
    string targetName { set; get; }

}
