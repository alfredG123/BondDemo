using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spirit Skill", menuName = "BOND/SpiritSkill")]
public class SpiritSkill : ScriptableObject
{
#pragma warning disable 0649
    [SerializeField] private TypeSkill skill_type;
    [SerializeField] private string skill_name;
    [SerializeField] private string skill_description;
    [SerializeField] private TypeAttribute skill_attribute;
    [SerializeField] private int skill_power;
    [SerializeField] private int skill_accuracy;
    [SerializeField] private int health_cost;
    [SerializeField] private int stamina_cost;
#pragma warning restore 0649

    #region Properties
    public TypeSkill SkillType
    {
        get => (skill_type);
    }

    public string SkillName
    {
        get => (skill_name);
    }

    public string SkillDescription
    {
        get => (skill_description);
    }

    public TypeAttribute SkillAttribute
    {
        get => (skill_attribute);
    }

    public int SkillPower
    {
        get => (skill_power);
    }

    public int SkillAccuracy
    {
        get => (skill_accuracy);
    }

    public int HealthCost
    {
        get => (health_cost);
    }

    public int StaminaCost
    {
        get => (stamina_cost);
    }
    #endregion
}
