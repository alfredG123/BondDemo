public class Item : BaseEnumeration
{
    public static Item Cystal = new Item(1, "Cystal");

    public Item(int value, string name)
    : base(value, name)
    {
    }
}
