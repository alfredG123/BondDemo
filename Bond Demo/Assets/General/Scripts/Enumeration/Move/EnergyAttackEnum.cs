public class EnergyAttackMove : AttackMove
{
    public static EnergyAttackMove FlameThrower = new EnergyAttackMove(2, "FlameThrower", "Burn the target with 100% of attack as the damage. Cost 20.", 1, TypeAttribute.Fire, TypeTargetSelection.SingleTarget, .5f, 1f, 10, false, has_secondary_effect: true, lasting_status_effect_type: TypeLastingStatusEffect.Burn, secondary_effect_chance: .1f, upgrade_cost: 1);
    public static EnergyAttackMove Ember = new EnergyAttackMove(0, "Ember", "Burn the target with 50% of attack as the damage. Cost 10.", 1, TypeAttribute.Fire, TypeTargetSelection.SingleTarget, .5f, 1f, 10, true, has_secondary_effect: true, lasting_status_effect_type: TypeLastingStatusEffect.Burn, secondary_effect_chance: .1f, next_level_move: EnergyAttackMove.FlameThrower, upgrade_cost: 10);
    public static EnergyAttackMove FireBlast = new EnergyAttackMove(1, "Fire Blast", "Burn all targets with 10% of attack as the damage. Cost 15.", 1, TypeAttribute.Fire, TypeTargetSelection.MultipleTarget, .1f, 1f, 15, true);

    public EnergyAttackMove(int value, string name, string description, int priority, TypeAttribute attribute, TypeTargetSelection target_selection_type, float power, float accuracy, int energy_cost, bool is_upgradeable, bool has_secondary_effect = false, TypeFieldEffect field_effect_type = TypeFieldEffect.None, TypeLastingStatusEffect lasting_status_effect_type = TypeLastingStatusEffect.None, TypeTemporaryStatusEffect temporary_status_effect_type = TypeTemporaryStatusEffect.None, float secondary_effect_chance = 0, EnergyAttackMove next_level_move = null, int upgrade_cost = 0)
        : base(value, name, description, TypeMove.EnergyAttackMove, priority, attribute, target_selection_type, power, accuracy, is_upgradeable, has_secondary_effect, field_effect_type, lasting_status_effect_type, temporary_status_effect_type, secondary_effect_chance, upgrade_cost)
    {
        EnergyCost = energy_cost;

        NextLevelMove = next_level_move;
    }

    public int EnergyCost { get; private set; }
    public EnergyAttackMove NextLevelMove { get; private set; }
}