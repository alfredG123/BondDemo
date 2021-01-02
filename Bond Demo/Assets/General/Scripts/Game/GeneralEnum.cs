using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Scene
// This enum needs to match build setting
public enum TypeScene
{
    Title = 0,
    StartersSelection = 1,
    Main = 2
}
#endregion

#region Spirit
public enum TypeAttribute
{
    Neutral,
    Plant,
    Fire,
    Water,
    Earth,
    Wind,
    Electric
}

public enum TypeSkill
{
    Attack,
    Status
}
#endregion

#region Maze
public enum TypeDoor
{
    TopDoor,
    BottomDoor,
    LeftDoor,
    RightDoor
}

public enum TypeRoom
{
    Entry,
    Normal,
    NextLevel
}
#endregion