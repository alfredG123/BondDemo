using System;

public static class GeneralRandom
{
    private const uint BIT_NOISE1 = 0x68E31DA4;
    private const uint BIT_NOISE2 = 0xB5297A4D;
    private const uint BIT_NOISE3 = 0x1B56C4E9;

    private static Random _RNG;

    /// <summary>
    /// Return the current seed
    /// </summary>
    public static int Seed { get; private set; } = 97;

    /// <summary>
    /// Initialize the global random number generator
    /// </summary>
    public static void SetSeed()
    {
        DateTime current_time;
        int current_hour;
        int current_second;

        // Get the current time information to be used as a seed
        current_time = DateTime.Now;
        current_hour = current_time.Hour;
        current_second = current_time.Second;

        // Save the seed in the global for the retrieve
        Seed = (int)GetRandomSeed(current_hour, current_second);

        // Override the current random number generator
        _RNG = new Random(Seed);
    }

    /// <summary>
    /// Get a random number between min(inclusive) and max(exclusive)
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static int GetRandomNumberInRange(int min, int max)
    {
        int result;

        GeneralError.CheckIfLess(max, min, "GetRandomNumberInRange");

        SetInitialSeed();

        result = _RNG.Next(min, max);

        return (result);
    }

    /// <summary>
    /// Get a random number between 0 and 1, then compare it with the success rate
    /// </summary>
    /// <param name="success_rate"></param>
    /// <returns></returns>
    public static bool RollDiceAndCheckIfSuccess(float success_rate)
    {
        bool success;
        float min_value = 0f;
        float max_value = 1f;

        // If the current play mode is testing, check the parameters
        if (GeneralSetting.CurrentMode == GeneralSetting.Mode.Testing)
        {
            GeneralError.CheckIfLess(success_rate, min_value, "GetRandomChance");
            GeneralError.CheckIfGreater(success_rate, max_value, "GetRandomChance");
        }

        SetInitialSeed();

        success = success_rate > _RNG.NextDouble();

        return (success);
    }

    /// <summary>
    /// Set the seed if it is not already
    /// </summary>
    private static void SetInitialSeed()
    {
        // Create an instance of the random number generator if not already
        if (_RNG == null)
        {
            // If the current is testing, use the fixed seed
            if (GeneralSetting.CurrentMode == GeneralSetting.Mode.Testing)
            {
                _RNG = new Random(Seed);
            }
            else
            {
                SetSeed();
            }
        }
    }

    /// <summary>
    /// Generate a random seed (FROM Math for Game Programmers)
    /// </summary>
    /// <param name="index"></param>
    /// <param name="pre_seed"></param>
    /// <returns></returns>
    private static uint GetRandomSeed(int index, int pre_seed)
    {
        uint mangle_bits = (uint) index;
        uint unsigned_seed = (uint) pre_seed;
        int shift = 8;

        mangle_bits *= BIT_NOISE1;
        mangle_bits += unsigned_seed;
        mangle_bits ^= (mangle_bits >> shift);
        mangle_bits += BIT_NOISE2;
        mangle_bits ^= (mangle_bits << shift);
        mangle_bits *= BIT_NOISE3;
        mangle_bits ^= (mangle_bits >> shift);

        return (mangle_bits);
    }
}
