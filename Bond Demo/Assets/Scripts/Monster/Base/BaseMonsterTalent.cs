using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonsterTalent
{
    protected string name;
    protected string description;

    public BaseMonsterTalent()
    {
        name = "Origin";
        description = "All attacks are nullified.";
    }

    public string TalentName
    {
        get { return (name); }
    }

    public string TalentDescription
    {
        get { return (description); }
    }
}
