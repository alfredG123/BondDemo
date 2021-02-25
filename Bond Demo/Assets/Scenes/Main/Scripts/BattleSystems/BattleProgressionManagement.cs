﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleProgressionManagement : MonoBehaviour
{
    [SerializeField] private GameObject _PlayerSpiritPrefabGroup = null;
    [SerializeField] private GameObject _EnemySpiritPrefabGroup = null;

    [SerializeField] private BattleButtonsHanlder _BattleButtonsHanlder = null;
    [SerializeField] private BattleDisplayHandler _BattleDisplayHanlder = null;

    private readonly SpiritMoveOrderManagement _SpiritMoveOrderManagement = new SpiritMoveOrderManagement();
    private int _EnemyCount = 3;
    private TypeField _FieldType = TypeField.None;

    public void TriggerEncounter(int enemy_count)
    {
        _EnemyCount = enemy_count;

        _BattleDisplayHanlder.SetUpBattleUI();

        SetUpPrefab();

        _BattleButtonsHanlder.SetUpForFirstDecision();
    }

    #region CREATE_BATTLE_FIELD
    /// <summary>
    /// Set up the spirits on the battle field
    /// </summary>
    public void SetUpPrefab()
    {
        SpawnSpiritForPlayer();

        SpawnSpiritForEnemy();
    }

    /// <summary>
    /// Get spirits from the player, and set it up
    /// </summary>
    private void SpawnSpiritForPlayer()
    {
        if(PlayerManagement.PartyMemberCount() == 0)
        {
            PlayerManagement.SetUpTemporaryParty();
        }

        foreach (Transform child in _PlayerSpiritPrefabGroup.transform)
        {
            child.gameObject.SetActive(false);
        }

        for (int i = 0; i < PlayerManagement.PartyMemberCount(); i++)
        {
            SpawnSpirit(PlayerManagement.GetPartyMember(i), _PlayerSpiritPrefabGroup, i);
        }
    }

    /// <summary>
    /// Get spirits from the scriptable object level
    /// </summary>
    private void SpawnSpiritForEnemy()
    {
        Spirit spirit;

        foreach (Transform child in _EnemySpiritPrefabGroup.transform)
        {
            child.gameObject.SetActive(false);
        }

        for (int i = 0; i < _EnemyCount; i++)
        {
            spirit = new Spirit(BaseSpirit.E1, false);

            SpawnSpirit(spirit, _EnemySpiritPrefabGroup, i);
        }
    }

    /// <summary>
    /// Set up the prefab, and add it to the move order list
    /// </summary>
    /// <param name="spirit_to_spawn"></param>
    /// <param name="spirit_prefab_objects"></param>
    /// <param name="spirit_position_index"></param>
    /// <param name="is_ally"></param>
    private void SpawnSpirit(Spirit spirit_to_spawn, GameObject spirit_prefab_objects, int spirit_position_index)
    {
        GameObject prefab = spirit_prefab_objects.transform.GetChild(spirit_position_index).gameObject;

        prefab.GetComponent<SpiritPrefab>().Spirit = spirit_to_spawn;

        prefab.SetActive(true);

        prefab.GetComponent<SpriteRenderer>().sprite = AssetsLoader.Assets.LoadSprite(spirit_to_spawn.ImageName, LoadEnum.SpiritImage);

        prefab.transform.GetChild(0).GetComponent<StatusHandler>().InitializeStatus(spirit_to_spawn);
    }

    #endregion

    #region PLAY_ACTIONS
    public void StartBattle()
    {
        SetUpMoveOrder();

        StartCoroutine(nameof(PerformBattle));
    }

    private void SetUpMoveOrder()
    {
        for (int i = 0; i < _PlayerSpiritPrefabGroup.transform.childCount; i++)
        {
            if (_PlayerSpiritPrefabGroup.transform.GetChild(i).gameObject.activeSelf)
            {
                _SpiritMoveOrderManagement.AddSpiritObjectToList(_PlayerSpiritPrefabGroup.transform.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < _EnemySpiritPrefabGroup.transform.childCount; i++)
        {
            if (_EnemySpiritPrefabGroup.transform.GetChild(i).gameObject.activeSelf)
            {
                _EnemySpiritPrefabGroup.transform.GetChild(i).gameObject.GetComponent<EnemyBattleLogic>().SetSpiritBattleInfo(_PlayerSpiritPrefabGroup);

                _SpiritMoveOrderManagement.AddSpiritObjectToList(_EnemySpiritPrefabGroup.transform.GetChild(i).gameObject);
            }
        }
    }

    private IEnumerator PerformBattle()
    {
        GameObject spirit_to_move;
        SpiritPrefab prefab;
        bool is_spirit_faint = false;

        while (_SpiritMoveOrderManagement.HasSpiritToMove())
        {
            spirit_to_move = _SpiritMoveOrderManagement.GetSpiritToMove();

            if (spirit_to_move.activeSelf)
            {
                prefab = spirit_to_move.GetComponent<SpiritPrefab>();

                PerformAction(spirit_to_move);

                yield return new WaitForSeconds(.5f);

                if (prefab.MoveToPerform.TargetSelectionType == TypeTargetSelection.MultipleTarget)
                {
                    foreach (Transform child in _EnemySpiritPrefabGroup.transform)
                    {
                        if (child.gameObject.activeSelf)
                        {
                            is_spirit_faint = TakeAction(spirit_to_move, child.gameObject, prefab.MoveToPerform);
                        }
                    }
                }
                else if (prefab.MoveToPerform.TargetSelectionType == TypeTargetSelection.SelfTarget)
                {
                    if (prefab.MoveToPerform is StatusMove status_move)
                    {
                        if (status_move.FieldEffectType == TypeFieldEffect.None)
                        {
                            TakeBuff(spirit_to_move, prefab.MoveToPerform);
                        }
                        else if (status_move.FieldEffectType == TypeFieldEffect.Field)
                        {
                            _FieldType = status_move.FieldType;
                        }
                    }
                }
                else if (prefab.MoveToPerform.TargetSelectionType == TypeTargetSelection.MultipleAlliesIncludeSelf)
                {
                    if (prefab.MoveToPerform is StatusMove)
                    {
                        foreach (Transform child in _PlayerSpiritPrefabGroup.transform)
                        {
                            if (child.gameObject.activeSelf)
                            {
                                TakeBuff(child.gameObject, prefab.MoveToPerform);
                            }
                        }
                    }
                }
                else if (prefab.MoveToPerform.TargetSelectionType == TypeTargetSelection.SingleTarget)
                {
                    if (prefab.TargetToAim.activeSelf)
                    {
                        is_spirit_faint = TakeAction(spirit_to_move, prefab.TargetToAim, prefab.MoveToPerform);
                    }
                    else
                    {
                        Debug.Log("The target has fainted!");
                    }
                }

                yield return new WaitForSeconds(.5f);

                if (is_spirit_faint)
                {
                    foreach (Transform child in _PlayerSpiritPrefabGroup.transform)
                    {
                        if (child.gameObject.activeSelf)
                        {
                            if (child.gameObject.GetComponent<SpiritPrefab>().Spirit.CurrentHealth == 0)
                            {
                                child.gameObject.SetActive(false);

                                PlayerManagement.RemoveFaintSpirit(child.gameObject.GetComponent<SpiritPrefab>().Spirit);
                            }
                        }
                    }

                    foreach (Transform child in _EnemySpiritPrefabGroup.transform)
                    {
                        if (child.gameObject.activeSelf)
                        {
                            if (child.gameObject.GetComponent<SpiritPrefab>().Spirit.CurrentHealth == 0)
                            {
                                child.gameObject.SetActive(false);
                            }
                        }
                    }


                    if (CheckBattleResult(out _))
                    {
                        break;
                    }
                }
            }
        }

        TakeStatusDamage();

        TakeEnvironmentDamage();

        if (CheckBattleResult(out bool is_player_winning))
        {
            _SpiritMoveOrderManagement.ClearMoveOrder();

            if (is_player_winning)
            {
                ClearTemporaryStatus();

                _BattleDisplayHanlder.DisplayReward(Item.Cystal, _EnemyCount);

               PlayerManagement.AddItemToBag(Item.Cystal, _EnemyCount);
            }
            else
            {
                General.ReturnToTitleSceneForErrors("PerformBattle", "Player lose");
            }
        }
        else
        {
            GetComponent<BattleButtonsHanlder>().SetUpForFirstDecision();
        }
    }

    private void PerformAction(GameObject spirit_to_move)
    {
        SpiritPrefab spirit_prefab;

        spirit_prefab = spirit_to_move.GetComponent<SpiritPrefab>();

        if (spirit_prefab.MoveToPerform.TargetSelectionType == TypeTargetSelection.SelfTarget)
        {
            //Debug.Log(spirit_prefab.Spirit.Name + " uses " + spirit_prefab.MoveToPerform.Name + "!");

            spirit_prefab.PerformMove(spirit_prefab.MoveToPerform);
        }
        else
        {
            //Debug.Log(spirit_prefab.Spirit.Name + " uses " + spirit_prefab.MoveToPerform.Name + " at " + spirit_prefab.TargetToAim.GetComponent<SpiritPrefab>().Spirit.Name + "!");

            spirit_prefab.PerformMove(spirit_prefab.MoveToPerform);
        }
    }

    private bool TakeAction(GameObject spirit_to_move, GameObject target, BaseMove move)
    {
        SpiritPrefab spirit_prefab;
        SpiritPrefab target_prefab;
        bool target_faint;

        spirit_prefab = spirit_to_move.GetComponent<SpiritPrefab>();

        target_prefab = target.GetComponent<SpiritPrefab>();

        target_faint = target_prefab.TakeMove(spirit_prefab.Spirit, move);

        return (target_faint);
    }

    private void TakeBuff(GameObject target, BaseMove move)
    {
        SpiritPrefab target_prefab;

        target_prefab = target.GetComponent<SpiritPrefab>();

        target_prefab.TakeBuff(move);
    }

    private bool CheckBattleResult(out bool player_win)
    {
        bool is_battle_over = false;
        player_win = false;
        int fainted_ally_count = _PlayerSpiritPrefabGroup.transform.childCount;
        int fainted_enemy_count = _EnemySpiritPrefabGroup.transform.childCount;

        if (!is_battle_over)
        {
            for (int i = 0; i < _PlayerSpiritPrefabGroup.transform.childCount; i++)
            {
                if (!_PlayerSpiritPrefabGroup.transform.GetChild(i).gameObject.activeSelf)
                {
                    fainted_ally_count--;
                }
            }

            if (fainted_ally_count == 0)
            {
                is_battle_over = true;
            }
        }

        if (!is_battle_over)
        {
            for (int i = 0; i < _EnemySpiritPrefabGroup.transform.childCount; i++)
            {
                if (!_EnemySpiritPrefabGroup.transform.GetChild(i).gameObject.activeSelf)
                {
                    fainted_enemy_count--;
                }
            }

            if (fainted_enemy_count == 0)
            {
                is_battle_over = true;
                player_win = true;
            }
        }

        return (is_battle_over);
    }

    private void ClearTemporaryStatus()
    {
        foreach (Transform child in _PlayerSpiritPrefabGroup.transform)
        {
            if (child.gameObject.activeSelf)
            {
                child.gameObject.GetComponent<SpiritPrefab>().ClearTemporaryStatus();
            }
        }
    }

    private void TakeStatusDamage()
    {
        GameObject spirit_to_check;
        bool is_spirit_faint = false;
        SetUpMoveOrder();

        while (_SpiritMoveOrderManagement.HasSpiritToMove())
        {
            spirit_to_check = _SpiritMoveOrderManagement.GetSpiritToMove();

            spirit_to_check.GetComponent<SpiritPrefab>().TakeStatusDamage();

            if (is_spirit_faint)
            {
                spirit_to_check.SetActive(false);
            }
        }

        _SpiritMoveOrderManagement.ClearMoveOrder();
    }

    private void TakeEnvironmentDamage()
    {
        GameObject spirit_to_check;
        bool is_spirit_faint = false;
        
        SetUpMoveOrder();

        if (_FieldType != TypeField.None)
        {
            while (_SpiritMoveOrderManagement.HasSpiritToMove())
            {
                spirit_to_check = _SpiritMoveOrderManagement.GetSpiritToMove();

                spirit_to_check.GetComponent<SpiritPrefab>().TakeEnvironmentDamage(_FieldType);

                if (is_spirit_faint)
                {
                    spirit_to_check.SetActive(false);
                }
            }
        }

        _SpiritMoveOrderManagement.ClearMoveOrder();
    }

    #endregion
}
