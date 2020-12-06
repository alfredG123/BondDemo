using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlessingAura : BaseMonsterTalent
{
    public BlessingAura()
    {
        name = "Blessing Aura";
        description = "At end of the turn, the monster gain a shield that is equal to 1/8 of base health. If the shield is already existed, refresh the shield.";
    }
}
