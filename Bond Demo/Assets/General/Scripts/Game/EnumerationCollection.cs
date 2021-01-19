using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region SCENE
// This enum needs to match build setting
public enum TypeScene
{
    Title = 0,
    PartnerSelection = 1,
    Main = 2
}
#endregion

#region SPIRIT
public enum TypeAttribute
{
    Plant,
    Fire,
    Water,
    Earth,
    Wind,
    Electric
}

public enum TypeMove
{
    SingleTargetAttack,
    AOEAttack,
    Status
}

public enum TypeMoveStatus
{
    Burn,
    Posion,
    Paralysis,
    AccuracyDownSmall
}
#endregion

#region MAZE
public enum TypeDoor
{
    TopDoor,
    BottomDoor,
    LeftDoor,
    RightDoor
}

public enum TypeRoom
{
    Wall,
    Entry,
    Normal,
    NextLevel,
    Final
}
#endregion

#region BATTLE_INPUT
public enum TypeSelectedMove
{
    None,
    Move1,
    Move2,
    Move3,
    Defend
}

public enum TypePlanningPhrase
{
    None,
    SelectingAction,
    SelectingMove,
    SelectingTarget
}

public enum TypeEffectiveness
{
    Effective,
    NotEffective,
    SuperEffective,
    NoEffect
}
#endregion