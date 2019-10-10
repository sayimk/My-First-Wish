using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneHandedSword : MonoBehaviour, WeaponsInterface {

    //Must Match item class name
    public string ItemName;
    public List<string> Stat_Bonus_Names;
    public List<string> Stat_Bonus_type_HP_Attack_Defence_Agility;
    public List<int> Stat_Bonus_Values;
    public List<StatModifierClass> StatModifiers = new List<StatModifierClass>();
    private Animator animator;

    //Player ModifiedStats, used for Damage Calculations
    int PlayerHP;
    int PlayerAttack;
    int PlayerDefence;
    int PlayerAgility;

    //used for checking and setting attacks
    private int CurrentDamageTotal =0;
    private string currentAttack = "StandardAttack";
    private bool skill = false;
    private int skillAccuracy = 0;
    GameObject enemy;

    // Use this for initialization
    void Awake() {

        for(int i=0; i <Stat_Bonus_Names.Count; i++) {
            StatModifierClass temp = new StatModifierClass(Stat_Bonus_Names[i], Stat_Bonus_type_HP_Attack_Defence_Agility[i], Stat_Bonus_Values[i]);
            StatModifiers.Add(temp);
        }

}



public string getWeaponName() {
        return ItemName;
    }

    public void enterCombat() {
        Debug.Log("Entering Combat");
        //currentAttack = Player_Log.StandardAttack;

    }

    public void performQuickAttack(Player_Log playersLog) {

        //calulate damage from quick attack then set the amount
        float multiplier = 1.1f;
        CurrentDamageTotal = (int)calculateReducedDamage(PlayerAttack, enemy.GetComponent<EnemyBaseClass>().getDefenceStat(), multiplier);
        currentAttack = Player_Log.QuickAttack;

        animator.SetTrigger("quickAttack");

    }

    public void performHeavyAttack(Player_Log playersLog) {

        //calulate damage from heavy attack then set the amount
        float multiplier = 1.4f;

        if(enemy!=null)
            CurrentDamageTotal = (int)calculateReducedDamage(PlayerAttack, enemy.GetComponent<EnemyBaseClass>().getDefenceStat(), multiplier);

        currentAttack = Player_Log.HeavyAttack;

        animator.SetTrigger("heavyAttack");

    }

    //Update to new formula
    public void performSkillAttack(string skillName, int totalDamage, int accuracy) {

        currentAttack = skillName;
        CurrentDamageTotal = totalDamage;
        skill = true;
        skillAccuracy = accuracy;

    }

    public List<StatModifierClass> getWeaponStatBonus() {
        return StatModifiers;
    }

    public void attackEnemyViaCollider(Collider col) {

        if (col.tag == "Enemy") {
            enemy = col.gameObject;
            if (CurrentDamageTotal == 0)
                CurrentDamageTotal = (int)calculateReducedDamage(PlayerAttack, enemy.GetComponent<EnemyBaseClass>().getDefenceStat(), 1.0f);



            col.gameObject.GetComponent<EnemyBaseClass>().addEnemyDamage(CurrentDamageTotal, currentAttack, skill, skillAccuracy);
        }
    }

    //this allows me to know when the attack animation has finished and when it finishes, i can reset damage to normal
    public void attackMotionFinishedCallback(string animationName) {

        if (!animationName.Equals("standardAttack")&&(enemy!=null)) {
            //reset Damage Amount to standard amount
            CurrentDamageTotal = (int)calculateReducedDamage(PlayerAttack, enemy.GetComponent<EnemyBaseClass>().getDefenceStat(), 1.0f);
            currentAttack = Player_Log.StandardAttack;
            skill = false;
            skillAccuracy = 0;
        }
    }

    //this method sets the players currents stats on weapon equip, these values let the damage be calculated in the weapon class
    public void setCharacterStatsValues(GameObject player) {

        PlayerHP = player.GetComponent<CharacterStats>().getModifiedStatValue("HP");
        PlayerAttack = player.GetComponent<CharacterStats>().getModifiedStatValue("Attack");
        PlayerDefence = player.GetComponent<CharacterStats>().getModifiedStatValue("Defence");
        PlayerAgility = player.GetComponent<CharacterStats>().getModifiedStatValue("Agility");
        animator = player.GetComponent<Animator>();
        animator.SetTrigger("armed");
    }

    public float calculateReducedDamage(int attackerAttackStat, int defenderDefenceStat, float multiplier1IsDefault) {

        float totalDA = attackerAttackStat + defenderDefenceStat;
        float pAttack = attackerAttackStat;
        float fDamage = (pAttack * (pAttack / totalDA))*multiplier1IsDefault;

        return fDamage;
    }
}
