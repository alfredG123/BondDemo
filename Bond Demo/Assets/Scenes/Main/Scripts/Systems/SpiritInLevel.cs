﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Level", menuName ="BOND/Level/Spirits")]
public class SpiritInLevel : ScriptableObject
{
    [SerializeField] private int game_level;
    [SerializeField] private List<BaseSpiritData> spirits_in_level = null;

    public List<BaseSpiritData> SpiritsInLevel
    {
        get => (spirits_in_level);
    }

    public int NumberOfSpirits
    {
        get => (spirits_in_level.Count);
    }

    public BaseSpiritData GetSpiritData(int spirit_index)
    {
        return (spirits_in_level[spirit_index]);
    }
}