﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleLogic : MonoBehaviour
{
    [SerializeField] GameObject PlayerParty = null;

    private void Start()
    {
        if (PlayerParty == null)
        {
            General.ReturnToTitleSceneForErrors("EnemyBattleLogic.Start", "The global variable is not set");
        }
    }

    public SpiritSkill GetSkill()
    {
        Spirit spirit = General.GetSpiritPrefabComponent(gameObject).Spirit;

        return (spirit.Skills[0]);
    }

    public GameObject GetTarget()
    {
        int current_health;
        int min_health = int.MaxValue;
        GameObject spirit_to_check;
        GameObject target_to_set = null;

        // Pick the spirit that has the lowest health
        for (int i = 0; i < PlayerParty.transform.childCount; i++)
        {
            spirit_to_check = PlayerParty.transform.GetChild(i).gameObject;

            if (spirit_to_check.activeSelf)
            {
                current_health = General.GetSpiritPrefabComponent(spirit_to_check).Spirit.CurrentHealth;

                if (current_health < min_health)
                {
                    target_to_set = spirit_to_check;

                    min_health = current_health;
                }
            }
        }

        return (target_to_set);
    }
}
