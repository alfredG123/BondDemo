﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDisplayHandler : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private GameObject BattleField;

    [SerializeField] private GameObject PlayerIcon;

    [SerializeField] private GameObject ActionButtons;
    [SerializeField] private GameObject BasicMoveButtons;
    [SerializeField] private GameObject EnergyMoveButtons;
    [SerializeField] private GameObject TargetButtons;
    [SerializeField] private GameObject AllyButtons;
    [SerializeField] private GameObject SkillButtons;

    [SerializeField] private GameObject PlayerParty;
    [SerializeField] private GameObject EnemyParty;

    [SerializeField] private GameObject _MapManagement;

    [SerializeField] private MainManagement _MainManagement = null;

    [SerializeField] private GameObject _Reward = null;

    [SerializeField] private GameObject _SelectionPanel = null;
    [SerializeField] private GameObject _SelectionButton = null;
    [SerializeField] private GameObject _SpiritSelectionGroup = null;
    [SerializeField] private GameObject _ActiveSpiritSelectionGroup = null;
    [SerializeField] private BattleButtonsHanlder _BattleButtonsHanlder = null;

    [SerializeField] private Canvas _MessageCanvas = null;

    [SerializeField] private CameraMovement _Camera = null;
#pragma warning restore 0649

    public void SetUpBattleUI()
    {
        // Move the main camera to the battle field
        MoveCameraToBattleField();

        // Activate battle field UI
        DisplayBattleField(is_active: true);
    }

    public void DisableBattle()
    {
        // Deactivate battle field UI
        DisplayBattleField(is_active: false);

        _MainManagement.ShowMap();
    }

    /// <summary>
    /// Move the main camera to the battle field
    /// </summary>
    private void MoveCameraToBattleField()
    {
        _Camera.SetMainCameraPositionXYOnly(BattleField.transform.position);
    }

    /// <summary>
    /// Activate or deactivate battle field UI
    /// </summary>
    /// <param name="is_active"></param>
    public void DisplayBattleField(bool is_active)
    {
        PlayerIcon.SetActive(is_active);
    }

    public void MoveCameraToPlayer()
    {
        _Camera.SetMainCameraPositionXYOnly(PlayerIcon.transform.position);
    }

    /// <summary>
    /// Display different buttons based on the phrase
    /// </summary>
    /// <param name="phrase"></param>
    /// <param name="show_back_button"></param>
    public void DisplayBattleButtons(TypePlanningPhrase phrase, Spirit current_spirit, bool show_back_button)
    {
        int index = 0;

        // Hide all buttons by default
        ActionButtons.SetActive(false);
        BasicMoveButtons.SetActive(false);
        EnergyMoveButtons.SetActive(false);
        TargetButtons.SetActive(false);
        AllyButtons.SetActive(false);
        SkillButtons.SetActive(false);

        if (phrase == TypePlanningPhrase.SelectingAction)
        {
            ActionButtons.SetActive(true);

            ActionButtons.transform.GetChild(3).gameObject.SetActive(show_back_button);
        }
        else if (phrase == TypePlanningPhrase.SelectingMove)
        {
            BasicMoveButtons.SetActive(true);
            EnergyMoveButtons.SetActive(true);

            for (int i = 0; i < 3; i++)
            {
                if (i < current_spirit.MoveSet.Count)
                {
                    EnergyMoveButtons.transform.GetChild(i).transform.GetChild(0).gameObject.SetText(current_spirit.MoveSet[i].Name);

                    EnergyMoveButtons.transform.GetChild(i).gameObject.GetComponent<TooltipTrigger>().SetTooltipText(current_spirit.MoveSet[i].Name, current_spirit.MoveSet[i].Description);

                    EnergyMoveButtons.transform.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    EnergyMoveButtons.transform.GetChild(i).gameObject.SetActive(false);
                }
            }

            BasicMoveButtons.transform.GetChild(0).gameObject.GetComponent<TooltipTrigger>().SetTooltipText(current_spirit.BasicAttack.Name, current_spirit.BasicAttack.Description);
            BasicMoveButtons.transform.GetChild(0).transform.GetChild(0).gameObject.SetText(current_spirit.BasicAttack.Name);

            BasicMoveButtons.transform.GetChild(1).gameObject.GetComponent<TooltipTrigger>().SetTooltipText(current_spirit.BasicDefend.Name, current_spirit.BasicDefend.Description);
            BasicMoveButtons.transform.GetChild(1).transform.GetChild(0).gameObject.SetText(current_spirit.BasicDefend.Name);
        }
        else if(phrase == TypePlanningPhrase.SelectingTarget)
        {
            TargetButtons.SetActive(true);
            AllyButtons.SetActive(true);

            // Hide the faint spirit
            for (int i = 0; i < EnemyParty.transform.childCount; i++)
            {
                if (EnemyParty.transform.GetChild(i).gameObject.activeSelf)
                {
                    TargetButtons.transform.GetChild(i).gameObject.SetActive(true);

                    TargetButtons.transform.GetChild(i).transform.GetChild(0).gameObject.SetText(EnemyParty.transform.GetChild(i).gameObject.GetComponent<SpiritPrefab>().Spirit.Name);
                }
                else
                {
                    TargetButtons.transform.GetChild(i).gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < PlayerParty.transform.childCount; i++)
            {
                if (PlayerParty.transform.GetChild(i).gameObject.GetComponent<SpiritPrefab>().Spirit != current_spirit)
                {
                    if (PlayerParty.transform.GetChild(i).gameObject.activeSelf)
                    {
                        AllyButtons.transform.GetChild(index).gameObject.SetActive(true);

                        AllyButtons.transform.GetChild(index).transform.GetChild(0).gameObject.SetText(PlayerParty.transform.GetChild(i).gameObject.GetComponent<SpiritPrefab>().Spirit.Name);
                    }
                    else
                    {
                        AllyButtons.transform.GetChild(index).gameObject.SetActive(false);
                    }

                    index++;
                }
            }
        }
        else if (phrase == TypePlanningPhrase.SelectingSkill)
        {
            SkillButtons.Activate();

            foreach (Transform child in SkillButtons.transform)
            {
                child.gameObject.Deactivate();
            }

            // Hide the faint spirit
            for (int i = 0; i < PlayerInformation.ActiveSkillCount(); i++)
            {
                SkillButtons.transform.GetChild(i).GetChild(0).gameObject.SetText(PlayerInformation.GetActiveSkill(i).Name);

                SkillButtons.transform.GetChild(i).gameObject.Activate();
            }

            SkillButtons.transform.GetChild(3).gameObject.SetActive(show_back_button);
        }
    }

    public void HideAllButtons()
    {
        // Hide all buttons by default
        ActionButtons.SetActive(false);
        BasicMoveButtons.SetActive(false);
        EnergyMoveButtons.SetActive(false);
        TargetButtons.SetActive(false);
        AllyButtons.SetActive(false);
    }

    public void DisplayReward(Item item, int quantity)
    {
        _Reward.transform.GetChild(1).gameObject.SetText(item.Name + " x" + quantity);

        _Reward.SetActive(true);
    }

    public void HideReward()
    {
        _Reward.SetActive(false);
    }

    public void DisplaySelectSpirit()
    {
        GameObject spirit_button;
        Button button;

        if (PlayerInformation.PartyMemberCount() == PlayerInformation.ActivePartyMemberCount())
        {
            TextUIPopUp.CreateTextPopUp("All spirits are on the battle field.", GeneralInput.GetMousePositionInWorldSpace(), Color.red, _MessageCanvas);

            return;
        }

        for (int i = 0; i < PlayerInformation.PartyMemberCount(); i++)
        {
            if (PlayerInformation.CheckIfActive(PlayerInformation.GetPartyMember(i)))
            {
                continue;
            }

            int spirit_index = i;
            int button_index = _SpiritSelectionGroup.transform.childCount - 1;

            spirit_button = GameObject.Instantiate(_SelectionButton, _SpiritSelectionGroup.transform);
            spirit_button.transform.GetChild(0).gameObject.SetText(PlayerInformation.GetPartyMember(i).Name);
            spirit_button.Activate();

            button = spirit_button.GetComponent<Button>();

            // spirit_index is passed by reference
            button.onClick.AddListener(() => { _BattleButtonsHanlder.SelectSpirit(spirit_index, button_index); });
        }

        _SelectionPanel.Activate();
        _SpiritSelectionGroup.Activate();
    }

    public void DisplayActiveSelectSpirit()
    {
        _SpiritSelectionGroup.Deactivate();

        for (int i = 0; i < 3; i++)
        {
            if (PlayerInformation.GetActivePartyMember(i) != null)
            {
                _ActiveSpiritSelectionGroup.transform.GetChild(i).GetChild(0).gameObject.SetText(PlayerInformation.GetActivePartyMember(i).Name);

                _ActiveSpiritSelectionGroup.transform.GetChild(i).gameObject.Activate();
            }
            else
            {
                _ActiveSpiritSelectionGroup.transform.GetChild(i).gameObject.Deactivate();
            }
        }

        _ActiveSpiritSelectionGroup.Activate();
    }

    public void FinishSwitching()
    {
        _SelectionPanel.Deactivate();
        _SpiritSelectionGroup.Deactivate();
        _ActiveSpiritSelectionGroup.Deactivate();

        foreach (Transform child in _SpiritSelectionGroup.transform)
        {
            if (child.gameObject.activeSelf)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void DestorySelectionButton(int index)
    {
        Destroy(_SpiritSelectionGroup.transform.GetChild(index).gameObject);
    }
}
