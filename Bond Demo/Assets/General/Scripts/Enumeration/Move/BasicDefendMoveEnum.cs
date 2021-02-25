public class BasicDefendMove : BaseMove
{
    public static BasicDefendMove Protect = new BasicDefendMove(1, "Protect", "Reduce 40% of the damage.", 5, .4f, true);

    public BasicDefendMove(int value, string name, string description, int priority, float damage_reduction,bool is_upgradeable, bool has_secondary_effect = false, TypeLastingStatusEffect lasting_status_effect_type = TypeLastingStatusEffect.None, TypeTemporaryStatusEffect temporary_status_effect_type = TypeTemporaryStatusEffect.None, float secondary_effect_accuracy = 0)
        : base(value, name, description, TypeMove.BasicDefend, priority, TypeTargetSelection.SelfTarget, is_upgradeable)
    {
        DamageReducation = damage_reduction;
        HasSecondaryEffect = has_secondary_effect;
        LastingStatusEffectType = lasting_status_effect_type;
        TemporaryStatusEffectType = temporary_status_effect_type;
        SecondaryEffectAccuracy = secondary_effect_accuracy;
    }

    public float DamageReducation { get; private set; }
    public bool HasSecondaryEffect { get; private set; }
    public TypeLastingStatusEffect LastingStatusEffectType { get; private set; }
    public TypeTemporaryStatusEffect TemporaryStatusEffectType { get; private set; }
    public float SecondaryEffectAccuracy { get; private set; }
}