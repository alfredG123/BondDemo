﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleProgressionManagement : MonoBehaviour
{
    [SerializeField] private GameObject _PlayerSpiritPrefabGroup = null;
    [SerializeField] private GameObject _EnemySpiritPrefabGroup = null;

    [SerializeField] private BattleButtonsHanlder _BattleButtonsHanlder = null;
    [SerializeField] private BattleDisplayHandler _BattleDisplayHanlder = null;

    [SerializeField] private MainManagement _MainManagement = null;

    private readonly SpiritMoveOrderManagement _SpiritMoveOrderManagement = new SpiritMoveOrderManagement();
    private int _EnemyCount = 3;
    private TypeField _FieldType = TypeField.None;
    private Spirit _SwitchingSpirit = null;

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
        PlayerInformation.ResetActiveMember();

        if (PlayerInformation.ActivePartyMemberCount() == 0)
        {
            PlayerInformation.SetUpTemporaryPlayer();
        }

        foreach (Transform child in _PlayerSpiritPrefabGroup.transform)
        {
            child.gameObject.SetActive(false);
        }

        for (int i = 0; i < PlayerInformation.ActivePartyMemberCount(); i++)
        {
            SpawnSpirit(PlayerInformation.GetPartyMember(i), _PlayerSpiritPrefabGroup, i);
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

        prefab.GetComponent<SpiritPrefab>().InitializeSpirit();

        prefab.GetComponent<SpiritPrefab>().InitializeStatus();

        prefab.SetActive(true);

        prefab.GetComponent<SpriteRenderer>().sprite = AssetsLoader.Assets.LoadSprite(spirit_to_spawn.ImageName, LoadObjectEnum.SpiritImage);
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
        bool is_ally_faint = false;
        int current_active_spirit = 0;

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
                    if (!prefab.TargetToAim.activeSelf)
                    {
                        prefab.SetTargetToAim(GetAliveTarget(spirit_to_move));
                    }

                    is_spirit_faint = TakeAction(spirit_to_move, prefab.TargetToAim, prefab.MoveToPerform);
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

                                PlayerInformation.RemoveFaintSpirit(child.gameObject.GetComponent<SpiritPrefab>().Spirit);
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

               PlayerInformation.AddItemToBag(Item.Cystal, _EnemyCount);
            }
            else
            {
                _MainManagement.Lose();
            }

            foreach (Transform child in _PlayerSpiritPrefabGroup.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    child.gameObject.GetComponent<SpiritPrefab>().HideStatus();
                }
            }
        }
        else
        {
            foreach (Transform child in _PlayerSpiritPrefabGroup.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    current_active_spirit++;
                }
                else
                {
                    is_ally_faint = true;
                }
            }

            if ((is_ally_faint) && (current_active_spirit < PlayerInformation.PartyMemberCount()))
            {
                _BattleButtonsHanlder.SetUpReplacingFaintedSpirit();

                _BattleDisplayHanlder.DisplaySelectSpirit();
            }
            else
            {
                GetComponent<BattleButtonsHanlder>().SetUpForFirstDecision();
            }
        }
    }

    private void PerformAction(GameObject spirit_to_move)
    {
        SpiritPrefab spirit_prefab;

        spirit_prefab = spirit_to_move.GetComponent<SpiritPrefab>();

        spirit_prefab.PerformMove(spirit_prefab.MoveToPerform);
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

    private GameObject GetAliveTarget(GameObject spirit_to_move)
    {
        GameObject target = null;

        if (spirit_to_move.GetComponent<SpiritPrefab>().Spirit.IsAlly)
        {
            foreach (Transform child in _EnemySpiritPrefabGroup.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    target = child.gameObject;
                    break;
                }
            }
        }
        else
        {
            foreach (Transform child in _PlayerSpiritPrefabGroup.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    target = child.gameObject;
                    break;
                }
            }
        }

        return (target);
    }

    public void SelectSpirit(Spirit spirit, int button_index, bool switching_spirit)
    {
        int available_position = 0;
        bool need_more = false;

        if (switching_spirit)
        {
            _SwitchingSpirit = spirit;

            _BattleDisplayHanlder.DisplayActiveSelectSpirit();
        }
        else
        {
            foreach (Transform child in _PlayerSpiritPrefabGroup.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    available_position++;
                }
                else
                {
                    break;
                }
            }

            SpawnSpirit(spirit, _PlayerSpiritPrefabGroup, available_position);

            PlayerInformation.SetSpiritActive(spirit);

            _BattleDisplayHanlder.DestorySelectionButton(button_index);

            foreach (Transform child in _PlayerSpiritPrefabGroup.transform)
            {
                if (!child.gameObject.activeSelf)
                {
                    need_more = true;

                    break;
                }
            }

            if (need_more)
            {
                if (PlayerInformation.ActivePartyMemberCount() >= PlayerInformation.PartyMemberCount())
                {
                    need_more = false;
                }
            }
            
            if (!need_more)
            {
                _BattleDisplayHanlder.FinishSwitching();

                GetComponent<BattleButtonsHanlder>().SetUpForFirstDecision();
            }
        }
    }

    public void SwitchSpirit(int spirit_index)
    {
        PlayerInformation.SwitchSpirit(_SwitchingSpirit, _PlayerSpiritPrefabGroup.transform.GetChild(spirit_index).GetComponent<SpiritPrefab>().Spirit);

        SpawnSpirit(_SwitchingSpirit, _PlayerSpiritPrefabGroup, spirit_index);

        _BattleDisplayHanlder.FinishSwitching();
    }
    #endregion
}
