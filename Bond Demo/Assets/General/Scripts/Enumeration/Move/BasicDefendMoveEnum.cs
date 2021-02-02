public class BasicDefendMove : BaseMove
{
    public static BasicDefendMove Protect = new BasicDefendMove(1, "Protect", 5, 40);

    public BasicDefendMove(int value, string name, int priority, int damage_reduction, bool has_secondary_effect = false, TypeLastingStatusEffect lasting_status_effect_type = TypeLastingStatusEffect.None, TypeTemporaryStatusEffect temporary_status_effect_type = TypeTemporaryStatusEffect.None, int secondary_effect_accuracy = 0)
        : base(value, name, TypeMove.BasicDefend, priority)
    {
        DamageReducation = damage_reduction;
        HasSecondaryEffect = has_secondary_effect;
        LastingStatusEffectType = lasting_status_effect_type;
        TemporaryStatusEffectType = temporary_status_effect_type;
        SecondaryEffectAccuracy = secondary_effect_accuracy;
    }

    public int DamageReducation { get; private set; }
    public bool HasSecondaryEffect { get; private set; }
    public TypeLastingStatusEffect LastingStatusEffectType { get; private set; }
    public TypeTemporaryStatusEffect TemporaryStatusEffectType { get; private set; }
    public int SecondaryEffectAccuracy { get; private set; }
}