public class BaseMove : BaseEnumeration
{
    public BaseMove(int value, string name, string description, TypeMove move_type, int priority, TypeTargetSelection target_selection_type)
        : base(value, name)
    {
        Description = description;
        MoveType = move_type;
        Priority = priority;
        TargetSelectionType = target_selection_type;
    }

    public string Description { get; private set; }
    public TypeMove MoveType { get; private set; }
    public int Priority { get; private set; }
    public TypeTargetSelection TargetSelectionType { get; private set; }
}
