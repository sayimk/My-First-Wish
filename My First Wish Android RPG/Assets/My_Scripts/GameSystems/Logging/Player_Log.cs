using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class holds a log of what the player does, such as total amount of enemies defeated
public class Player_Log : MonoBehaviour {


    //list of Enemies
    int totalSpirits = 0; //"Spirit"


    //Combat Data
    private List<string> attackLog = new List<string>();

    //Attack Types
    public const string QuickAttack = "QuickAttack";
    public const string HeavyAttack = "HeavyAttack";
    public const string StandardAttack = "StandardAttack";

    //Enemy Types
    public const string SpiritEnemy = "Spirit";

    //Methods for Player_Log
    public void loadData(PlayerObjectData playerObjectData) {
        attackLog.Clear();
        attackLog.AddRange(playerObjectData.attackLog);

        //Loading individual counts
        totalSpirits = playerObjectData.totalSpirits;

        Debug.Log("Player Log Data Loaded");
    }


    public bool incrementTotalEnemyDefeated(string Enemy_Name) {

        switch (Enemy_Name) {

            case "Spirit": {

                    //increments logs count
                    totalSpirits = totalSpirits + 1;

                    //refreshes the quests and checks log
                    gameObject.GetComponent<QuestsManager>().updateAllQuests();
                    Debug.Log("Spirit Kill Count " + totalSpirits);
                    return false;
            }

            default: {
                    return false;
            }
        }
    }

    public int getTotalDefeatedViaEnemyType(string Enemy_Name) {

        switch (Enemy_Name) {

            case "Spirit": {
                    return totalSpirits;
                }

            default: {
                    return 0;
                }
        }

    }

    public void logPlayersAttack(string playersAttack) {

        bool added = false;

        //logging HeavyAttack
        if (playersAttack.Equals(HeavyAttack)) {
            attackLog.Add(HeavyAttack);
            added = true;
        }

        //logging QuickAttack
        if (playersAttack.Equals(QuickAttack)) {
            attackLog.Add(QuickAttack);
            added = true;
        }

        //logging StandardAttack
        if (playersAttack.Equals(StandardAttack)) {
            attackLog.Add(StandardAttack);
            added = true;
        }

        if (!added)
            throw new System.ArgumentException("Invalid Attack name added, use the attacks from Players_Log");
    }

    public int getTotalLoggedAttacks() {

        return attackLog.Count;
    }

    public int getLogDataForSpecificAttack(string attackName) {

        int counter = 0;

        for (int i=0; i< attackLog.Count; i++) {

            if (attackLog[i].Equals(attackName))
                counter = counter + 1;
        }

        return counter;
    }

    public string getLastAttack() {

        if (attackLog.Count == 0)
            return "";

        return attackLog[(attackLog.Count - 1)];
    }

    public string get2ndLastAttack() {

        if (attackLog.Count < 2)
            return "";

        return attackLog[(attackLog.Count - 2)];
    }


    //this method will look for a pattern in which attacks were used after the specified attack and an overall occurance amount
    public Dictionary<string,int> getFollowingAttackAmountData(string AttackName) {

        Dictionary<string, int> dataStorage = new Dictionary<string, int>();
        int totalOccurances = 0;

        for (int i = 0; i < (attackLog.Count-1); i++) {


            if (attackLog[i].Equals(AttackName)) {

                totalOccurances = totalOccurances + 1;

                string followingKey = attackLog[(i + 1)];

                dataStorage[followingKey] = dataStorage[followingKey] + 1;

            }

        }
        return dataStorage;
    }

    public List<string> getAttackLog() {
        List<string> temp = attackLog;
        return temp;
    }

}
