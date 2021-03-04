public class Skill : BaseEnumeration
{
    public static Skill Switch = new Skill(1, "Switch");

    public Skill(int value, string name)
    : base(value, name)
    {
    }
}
