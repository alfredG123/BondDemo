public class LoadObjectEnum : BaseEnumeration
{
    public static LoadObjectEnum Text = new LoadObjectEnum(1, nameof(Text), "Prefab/Text");
    public static LoadObjectEnum Map = new LoadObjectEnum(2, nameof(Map), "Prefab/Map");
    public static LoadObjectEnum SpiritImage = new LoadObjectEnum(3, nameof(SpiritImage), "Image/Spirit");

    public LoadObjectEnum(int value, string name, string path)
    : base(value, name)
    {
        Path = path;
    }

    public string Path { get; private set; }
}
