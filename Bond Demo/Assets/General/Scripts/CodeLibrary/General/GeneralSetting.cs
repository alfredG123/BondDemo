﻿public static class GeneralSetting
{
    /// <summary>
    /// Enumeration for indicating the play mode
    /// Testing - Run the game in a way that is easier for the developers to test codes
    /// Play - Run the game normally
    /// </summary>
    public enum Mode
    {
        Play,
        Testing
    }

    /// <summary>
    /// Return the current play mode
    /// </summary>
    public static Mode CurrentMode { get; private set; } = Mode.Play;
}
