using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatressInfo : BaseMonsterInfo
{
    public WatressInfo()
    {
        name = "Watress";
        entry_number = 3;

        health = 10;
        attack = 1;
        defense = 1;
        speed = 3;

        weakness = new List<Attribute>();
        weakness.Add(Attribute.Plant);

        talent = new BlessingAura();
    }
}
