public class StatusMove : BaseMove
{
    public static StatusMove Howl = new StatusMove(0, "Howl", 1, TypeTargetSelection.SelfTarget, 100, 10, temporary_status_effect_type: TypeTemporaryStatusEffect.DamageUpSmall);

    public StatusMove(int value, string name, int priority, TypeTargetSelection target_selection_type, int accuracy, int energy_cost, TypeFieldEffect field_effect_type = TypeFieldEffect.None, TypeLastingStatusEffect lasting_status_effect_type = TypeLastingStatusEffect.None, TypeTemporaryStatusEffect temporary_status_effect_type = TypeTemporaryStatusEffect.None)
        : base(value, name, TypeMove.StatusMove, priority)
    {
        TargetSelectionType = target_selection_type;
        FieldEffectType = field_effect_type;
        LastingStatusEffectType = lasting_status_effect_type;
        TemporaryStatusEffectType = temporary_status_effect_type;
        Accuracy = accuracy;
        EnergyCost = energy_cost;
    }

    public TypeTargetSelection TargetSelectionType { get; private set; }
    public TypeFieldEffect FieldEffectType { get; private set; }
    public TypeLastingStatusEffect LastingStatusEffectType { get; private set; }
    public TypeTemporaryStatusEffect TemporaryStatusEffectType { get; private set; }
    public int Accuracy { get; private set; }
    public int EnergyCost { get; private set; }
}