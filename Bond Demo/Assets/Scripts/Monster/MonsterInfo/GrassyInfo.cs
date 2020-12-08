using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassyInfo : BaseMonsterInfo
{
    public GrassyInfo()
    {
        name = "Grassy";
        entry_number = 1;

        health = 10;
        attack = 1;
        defense = 0;
        speed = 2;

        weakness = new List<Attribute>();
        weakness.Add(Attribute.Fire);
        weakness.Add(Attribute.Wind);

        talent = new Regeneration();
    }
}
