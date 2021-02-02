public class BasicAttackMove : AttackMove
{
    public static BasicAttackMove Tackle = new BasicAttackMove(0, "Tackcle", 1, TypeAttribute.Normal, TypeTargetSelection.SingleTarget, 40, 100, 10);

    public BasicAttackMove(int value, string name, int priority, TypeAttribute attribute, TypeTargetSelection target_selection_type, int power, int accuracy, int energy_gain, bool has_secondary_effect = false, TypeFieldEffect field_effect_type = TypeFieldEffect.None, TypeLastingStatusEffect lasting_status_effect_type = TypeLastingStatusEffect.None, TypeTemporaryStatusEffect temporary_status_effect_type = TypeTemporaryStatusEffect.None, int secondary_effect_chance = 0)
        : base(value, name, TypeMove.BasicAttack, priority, attribute, target_selection_type, power, accuracy, has_secondary_effect, field_effect_type, lasting_status_effect_type, temporary_status_effect_type, secondary_effect_chance)
    {
        EnergyGain = energy_gain;
    }

    public int EnergyGain { get; private set; }
}