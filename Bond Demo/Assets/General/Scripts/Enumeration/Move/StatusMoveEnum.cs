public class StatusMove : BaseMove
{
    public static StatusMove DamageUpSmall = new StatusMove(0, nameof(DamageUpSmall), "Gain a damage boost buff for the user. Cost 10.", 1, TypeTargetSelection.MultipleAlliesIncludeSelf, 1f, 10, false, temporary_status_effect_type: TypeTemporaryStatusEffect.DamageUpSmall);
    public static StatusMove Posion = new StatusMove(1, nameof(Posion), "Posion the target. Cost 10.", 1, TypeTargetSelection.SingleTarget, 1f, 10, false, lasting_status_effect_type: TypeLastingStatusEffect.Posioned);
    public static StatusMove Burn = new StatusMove(2, nameof(Burn), "Burn the target. Cost 10.", 1, TypeTargetSelection.SingleTarget, 1f, 10, false, lasting_status_effect_type: TypeLastingStatusEffect.Burn);
    public static StatusMove HarmfulField = new StatusMove(3, nameof(HarmfulField), "Cause the field to be harmful. Cost 10.", 1, TypeTargetSelection.SelfTarget, 1f, 10, false, field_effect_type: TypeFieldEffect.Field, field_type: TypeField.Harmful);

    public StatusMove(int value, string name, string description, int priority, TypeTargetSelection target_selection_type, float accuracy, int energy_cost, bool is_upgradeable, TypeFieldEffect field_effect_type = TypeFieldEffect.None, TypeField field_type = TypeField.None, TypeLastingStatusEffect lasting_status_effect_type = TypeLastingStatusEffect.None, TypeTemporaryStatusEffect temporary_status_effect_type = TypeTemporaryStatusEffect.None)
        : base(value, name, description, TypeMove.StatusMove, priority, target_selection_type, is_upgradeable)
    {
        FieldEffectType = field_effect_type;
        FieldType = field_type;
        LastingStatusEffectType = lasting_status_effect_type;
        TemporaryStatusEffectType = temporary_status_effect_type;
        Accuracy = accuracy;
        EnergyCost = energy_cost;
    }

    public TypeFieldEffect FieldEffectType { get; private set; }
    public TypeField FieldType { get; private set; }
    public TypeLastingStatusEffect LastingStatusEffectType { get; private set; }
    public TypeTemporaryStatusEffect TemporaryStatusEffectType { get; private set; }
    public float Accuracy { get; private set; }
    public int EnergyCost { get; private set; }
}