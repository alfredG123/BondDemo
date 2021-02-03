public class BaseMove : BaseEnumeration
{
    public BaseMove(int value, string name, string description, TypeMove move_type, int priority)
        : base(value, name)
    {
        Description = description;
        MoveType = move_type;
        Priority = priority;
    }

    public string Description { get; private set; }
    public TypeMove MoveType { get; private set; }
    public int Priority { get; private set; }
}
