public class BaseMove : BaseEnumeration
{
    public BaseMove(int value, string name, TypeMove move_type, int priority)
        : base(value, name)
    {
        MoveType = move_type;
        Priority = priority;
    }

    public TypeMove MoveType { get; private set; }
    public int Priority { get; private set; }
}
