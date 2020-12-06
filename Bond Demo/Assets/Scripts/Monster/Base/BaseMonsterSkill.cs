using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonsterSkill
{
    protected string name;
    protected string description;
    protected Attribute attribute;

    public BaseMonsterSkill()
    {
        name = "Origin Pulse";
        description = "Heal all party members to full health";
        attribute = Attribute.Genesis;
    }
}
