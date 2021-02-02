public class AttackMove : BaseMove
{
    public AttackMove(int value, string name, int priority, TypeTargetSelection target_selection_type, int power, int accuracy, int energy_cost, bool has_secondary_effect = false, TypeLastingStatusEffect lasting_status_effect_type = TypeLastingStatusEffect.None, TypeTemporaryStatusEffect temporary_status_effect_type = TypeTemporaryStatusEffect.None, int secondary_effect_accuracy = 0)
        : base(value, name, TypeMove.AttackMove, priority)
    {
        TargetSelectionType = target_selection_type;
        Power = power;
        Accuracy = accuracy;
        EnergyCost = energy_cost;
        HasSecondaryEffect = has_secondary_effect;
        LastingStatusEffectType = lasting_status_effect_type;
        TemporaryStatusEffectType = temporary_status_effect_type;
        SecondaryEffectAccuracy = secondary_effect_accuracy;
    }

    public TypeTargetSelection TargetSelectionType { get; private set; }
    public int Power { get; private set; }
    public int Accuracy { get; private set; }
    public int EnergyCost { get; private set; }
    public bool HasSecondaryEffect { get; private set; }
    public TypeLastingStatusEffect LastingStatusEffectType { get; private set; }
    public TypeTemporaryStatusEffect TemporaryStatusEffectType { get; private set; }
    public int SecondaryEffectAccuracy { get; private set; }
}