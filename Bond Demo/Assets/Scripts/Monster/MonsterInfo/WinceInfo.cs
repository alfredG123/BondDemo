using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinceInfo : BaseMonsterInfo
{
    public WinceInfo()
    {
        name = "Wince";
        entry_number = 5;

        health = 5;
        attack = 2;
        defense = 0;
        speed = 5;

        weakness = new List<Attribute>();
        weakness.Add(Attribute.Fire);

        talent = new WindSupport();
    }
}
