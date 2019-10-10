using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class spiritEnemy : EnemyBaseClass
{
    public override void Start() {

        //Setting stats for specific Enemy based on Level
        MaxHP = Convert.ToInt32((Level * 70) / 1.4);
        AttackStat = Convert.ToInt32(((Level * 60) / 8.7));
        DefenceStat = Convert.ToInt32((Level * 60) / 9.5);
        AgilityStat = Convert.ToInt32(((Level * 60) / 3.8));
        currentHP = MaxHP;

        base.Start();
    }
}
