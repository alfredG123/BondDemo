using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Watress", menuName = "Monsters/Watress")]
public class WatressData : MonsterData
{
    private void Awake()
    {
        name = "Watress";

        health = 10;
        attack = 1;
        defense = 1;
        speed = 3;

        weakness = new List<Attribute>();
        weakness.Add(Attribute.Plant);

        talent = new BlessingAura();
    }
}
