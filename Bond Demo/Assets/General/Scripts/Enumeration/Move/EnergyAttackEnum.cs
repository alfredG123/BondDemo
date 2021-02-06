public class EnergyAttackMove : AttackMove
{
    public static EnergyAttackMove Ember = new EnergyAttackMove(0, "Ember", "Burn the target with 50% of attack as the damage. Cost 10.", 1, TypeAttribute.Fire, TypeTargetSelection.SingleTarget, .5f, 1, 10, has_secondary_effect: true, lasting_status_effect_type: TypeLastingStatusEffect.Burn, secondary_effect_chance: .1f);

    public EnergyAttackMove(int value, string name, string description, int priority, TypeAttribute attribute, TypeTargetSelection target_selection_type, float power, float accuracy, int energy_cost, bool has_secondary_effect = false, TypeFieldEffect field_effect_type = TypeFieldEffect.None, TypeLastingStatusEffect lasting_status_effect_type = TypeLastingStatusEffect.None, TypeTemporaryStatusEffect temporary_status_effect_type = TypeTemporaryStatusEffect.None, float secondary_effect_chance = 0)
        : base(value, name, description, TypeMove.EnergyAttackMove, priority, attribute, target_selection_type, power, accuracy, has_secondary_effect, field_effect_type, lasting_status_effect_type, temporary_status_effect_type, secondary_effect_chance)
    {
        EnergyCost = energy_cost;
    }

    public int EnergyCost { get; private set; }
}