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
    Normal,
    Plant,
    Fire,
    Water,
    Earth,
    Wind,
    Electric
}

public enum TypeLastingStatusEffect
{
    None,
    Burn,
    Posioned,
    Paralysis,
}

public enum TypeFieldEffect
{
    None,
    Weather,
    Field,
    Aura,
    Background
}

public enum TypeField
{
    None,
    Harmful
}

public enum TypeTemporaryStatusEffect
{
    None,
    DamageDownSmall,
    DamageDownMiddle,
    DamageDownLarge,
    DamageUpSmall,
    DamageUpMiddle,
    DamageUpLarge,
}

public enum TypeMove
{
    None,
    BasicAttack,
    BasicDefend,
    EnergyAttackMove,
    StatusMove,
}

public enum TypeTargetSelection
{
    SelfTarget,
    MultipleAlliesIncludeSelf,
    SingleTarget,
    MultipleTarget,
}
#endregion

#region MAZE
public enum TypeGridMapCell
{
    Wall,
    Enemy,
    Normal,
    Treasure,
    RestPlace,
    CystalTemple,
    WormHole,
    SurvivedSpirit,
    Final
}
#endregion

#region BATTLE_INPUT
public enum TypeSelectedMove
{
    None,
    Attack,
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