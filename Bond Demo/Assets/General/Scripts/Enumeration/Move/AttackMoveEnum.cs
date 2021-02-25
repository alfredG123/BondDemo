public class AttackMove : BaseMove
{
    public AttackMove(int value, string name, string description, TypeMove move_type, int priority, TypeAttribute attribute, TypeTargetSelection target_selection_type, float power, float accuracy, bool is_upgradeable, bool has_secondary_effect = false, TypeFieldEffect field_effect_type = TypeFieldEffect.None, TypeLastingStatusEffect lasting_status_effect_type = TypeLastingStatusEffect.None, TypeTemporaryStatusEffect temporary_status_effect_type = TypeTemporaryStatusEffect.None, float secondary_effect_chance = 0)
        : base(value, name, description, move_type, priority, target_selection_type, is_upgradeable)
    {
        Attribute = attribute;
        Power = power;
        Accuracy = accuracy;
        HasSecondaryEffect = has_secondary_effect;
        FieldEffectType = field_effect_type;
        LastingStatusEffectType = lasting_status_effect_type;
        TemporaryStatusEffectType = temporary_status_effect_type;
        SecondaryEffectChance = secondary_effect_chance;
    }

    public TypeAttribute Attribute { get; private set; }
    public float Power { get; private set; }
    public float Accuracy { get; private set; }
    public bool HasSecondaryEffect { get; private set; }
    public TypeFieldEffect FieldEffectType { get; private set; }
    public TypeLastingStatusEffect LastingStatusEffectType { get; private set; }
    public TypeTemporaryStatusEffect TemporaryStatusEffectType { get; private set; }
    public float SecondaryEffectChance { get; private set; }
}