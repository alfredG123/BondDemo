public class AttackMove : BaseMove
{
    public AttackMove(int value, string name, TypeMove move_type, int priority, TypeAttribute attribute, TypeTargetSelection target_selection_type, int power, int accuracy, bool has_secondary_effect = false, TypeFieldEffect field_effect_type = TypeFieldEffect.None, TypeLastingStatusEffect lasting_status_effect_type = TypeLastingStatusEffect.None, TypeTemporaryStatusEffect temporary_status_effect_type = TypeTemporaryStatusEffect.None, int secondary_effect_chance = 0)
        : base(value, name, move_type, priority)
    {
        Attribute = attribute;
        TargetSelectionType = target_selection_type;
        Power = power;
        Accuracy = accuracy;
        HasSecondaryEffect = has_secondary_effect;
        FieldEffectType = field_effect_type;
        LastingStatusEffectType = lasting_status_effect_type;
        TemporaryStatusEffectType = temporary_status_effect_type;
        SecondaryEffectChance = secondary_effect_chance;
    }

    public TypeAttribute Attribute { get; private set; }
    public TypeTargetSelection TargetSelectionType { get; private set; }
    public int Power { get; private set; }
    public int Accuracy { get; private set; }
    public bool HasSecondaryEffect { get; private set; }
    public TypeFieldEffect FieldEffectType { get; private set; }
    public TypeLastingStatusEffect LastingStatusEffectType { get; private set; }
    public TypeTemporaryStatusEffect TemporaryStatusEffectType { get; private set; }
    public int SecondaryEffectChance { get; private set; }
}