using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wince", menuName = "Monsters/Wince")]
public class WinceData : MonsterData
{
    private void Awake()
    {
        name = "Wince";

        health = 5;
        attack = 2;
        defense = 0;
        speed = 5;

        weakness = new List<Attribute>();
        weakness.Add(Attribute.Fire);

        talent = new WindSupport();
    }
}
