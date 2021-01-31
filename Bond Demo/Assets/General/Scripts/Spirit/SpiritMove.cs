using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spirit Move", menuName = "BOND/SpiritMove")]
public class SpiritMove : ScriptableObject
{
#pragma warning disable 0649
    [SerializeField] private TypeMove SpiritMoveType;
    [SerializeField] private string SpiritMoveName;
    [SerializeField] private string SpiritMoveDescription;
    [SerializeField] private TypeAttribute SpiritMoveAttributeType;
    [SerializeField] private int SpiritMovePower;
    [SerializeField] private int SpiritMoveEnergyCost;
    [SerializeField] private int SpiritMoveAccuracy;
    [SerializeField] private bool SpiritMoveAlwaysHit;
    [SerializeField] private int SpiritMoveCriticalChance;
    [SerializeField] private bool SpiritMoveHasSideEffect;
    [SerializeField] private TypeMoveStatus SpiritMoveStatusType;
    [SerializeField] private int SpiritMoveSideEffectAccuracy;
#pragma warning restore 0649

    #region Properties
    public TypeMove MoveType
    {
        get => SpiritMoveType;
    }

    public string MoveName
    {
        get => SpiritMoveName;
    }

    public string MoveDescription
    {
        get => SpiritMoveDescription;
    }

    public TypeAttribute MoveAttributeType
    {
        get => SpiritMoveAttributeType;
    }

    public int MovePower
    {
        get => SpiritMovePower;
    }

    public int MoveEnergyCost
    {
        get => SpiritMoveEnergyCost;
    }

    public int MoveAccuracy
    {
        get => SpiritMoveAccuracy;
    }

    public bool MoveAlwaysHit
    {
        get => SpiritMoveAlwaysHit;
    }

    public int MoveCriticalChance
    {
        get => SpiritMoveCriticalChance;
    }

    public bool HasSideEffect
    {
        get => SpiritMoveHasSideEffect;
    }

    public TypeMoveStatus MoveStatusType
    {
        get => SpiritMoveStatusType;
    }

    public int SideEffectAccuracy
    {
        get => SpiritMoveSideEffectAccuracy;
    }
    #endregion
}
