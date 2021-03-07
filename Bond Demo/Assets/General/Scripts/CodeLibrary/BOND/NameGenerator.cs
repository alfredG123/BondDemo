public static class NameGenerator
{
    private static readonly string[] _RandomNickname = { 
        "Bella", "Charlie", "Luna", "Lucy", "Max"
    , "Bailey", "Daisy", "Cooper", "Molly", "Lola"
    , "Oliver", "Leo", "Milo", "Lily", "Simba"
    };

    /// <summary>
    /// Pick a name from a pool of nicknames
    /// </summary>
    /// <returns></returns>
    public static string GetName()
    {
        int random_value;
        int first_nickname_index = 0;

        random_value = GeneralRandom.GetRandomNumberInRange(first_nickname_index, _RandomNickname.Length);

        return (_RandomNickname[random_value]);
    }
}
