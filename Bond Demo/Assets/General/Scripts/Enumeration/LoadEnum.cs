public class LoadEnum : BaseEnumeration
{
    public static LoadEnum Text = new LoadEnum(1, nameof(Text), "Prefab/Text");
    public static LoadEnum Map = new LoadEnum(2, nameof(Map), "Prefab/Map");
    public static LoadEnum SpiritImage = new LoadEnum(3, nameof(SpiritImage), "Image/Spirit");

    public LoadEnum(int value, string name, string path)
    : base(value, name)
    {
        Path = path;
    }

    public string Path { get; private set; }
}
