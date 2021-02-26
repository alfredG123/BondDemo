public class BasicAttackMove : AttackMove
{
    public static BasicAttackMove Smash = new BasicAttackMove(1, "Smash", "Deal damage that equals 100% of the attack. Gain 5.", 1, TypeAttribute.Normal, TypeTargetSelection.SingleTarget, .4f, 1, 10, false);
    public static BasicAttackMove Tackle = new BasicAttackMove(0, "Tackcle", "Deal damage that equals 40% of the attack. Gain 10.", 1, TypeAttribute.Normal, TypeTargetSelection.SingleTarget, .4f, 1, 10, true, next_level_move: BasicAttackMove.Smash, upgrade_cost:1);

    public BasicAttackMove(int value, string name, string description, int priority, TypeAttribute attribute, TypeTargetSelection target_selection_type, float power, float accuracy, int energy_gain, bool is_upgradeable, bool has_secondary_effect = false, TypeFieldEffect field_effect_type = TypeFieldEffect.None, TypeLastingStatusEffect lasting_status_effect_type = TypeLastingStatusEffect.None, TypeTemporaryStatusEffect temporary_status_effect_type = TypeTemporaryStatusEffect.None, float secondary_effect_chance = 0, BasicAttackMove next_level_move = null, int upgrade_cost = 0)
        : base(value, name, description, TypeMove.BasicAttack, priority, attribute, target_selection_type, power, accuracy, is_upgradeable, has_secondary_effect, field_effect_type, lasting_status_effect_type, temporary_status_effect_type, secondary_effect_chance, upgrade_cost)
    {
        EnergyGain = energy_gain;
        NextLevelMove = next_level_move;
    }

    public int EnergyGain { get; private set; }
    public BasicAttackMove NextLevelMove { get; private set; }
}