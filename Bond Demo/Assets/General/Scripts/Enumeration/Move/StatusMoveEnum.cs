public class StatusMove : BaseMove
{
    public StatusMove(int value, string name, int priority, TypeLastingStatusEffect lasting_status_effect_type, TypeTemporaryStatusEffect temporary_status_effect_type, int accuracy, int energy_cost)
        : base(value, name, TypeMove.StatusMove, priority)
    {
        LastingStatusEffectType = lasting_status_effect_type;
        TemporaryStatusEffectType = temporary_status_effect_type;
        Accuracy = accuracy;
        EnergyCost = energy_cost;
    }

    public TypeLastingStatusEffect LastingStatusEffectType { get; private set; }
    public TypeTemporaryStatusEffect TemporaryStatusEffectType { get; private set; }
    public int Accuracy { get; private set; }
    public int EnergyCost { get; private set; }
}