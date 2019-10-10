using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//interface that every weapon is based on, for a weapon to be used by the player, it must implement this interface
public interface WeaponsInterface{

    string getWeaponName();

    void enterCombat();

    void performQuickAttack(Player_Log playersLog);

    void performHeavyAttack(Player_Log playerLog);

    void attackMotionFinishedCallback(string animationName);

    void setCharacterStatsValues(GameObject player);

   List<StatModifierClass> getWeaponStatBonus();

    void performSkillAttack(string skillName, int totalDamage, int accurarcy);

    void attackEnemyViaCollider(Collider col);

    float calculateReducedDamage(int attackerAttackStat, int defenderDefenceStat, float multiplier1IsDefault);

    }
