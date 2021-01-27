public class BaseMove : BaseEnumeration
{
    public static BaseMove Tackle = new BaseMove(0, "Tackcle", 40, 1f, 1);

    public BaseMove(int id, string name, int power, float accuracy, int priority)
        : base(id, name)
    {
        Power = power;
        Accuracy = accuracy;
        Priority = priority;
    }

    public int Power { get; private set; }

    public float Accuracy { get; private set; }

    public int Priority { get; private set; }
}
