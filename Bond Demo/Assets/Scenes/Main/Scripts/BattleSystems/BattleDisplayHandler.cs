using System.Collections;
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

    [SerializeField] private GameObject PlayerParty;
    [SerializeField] private GameObject EnemyParty;

    [SerializeField] private GameObject _MapManagement;

    [SerializeField] private MainManagement _MainManagement = null;

    [SerializeField] private GameObject _Reward = null;

    [SerializeField] private GameObject _SelectionPanel = null;
    [SerializeField] private GameObject _SelectionButton = null;
    [SerializeField] private GameObject _Spirits = null;
    [SerializeField] private GameObject _SpiritSelectionGroup = null;
    [SerializeField] private GameObject _ActiveSpiritSelectionGroup = null;
    [SerializeField] private BattleButtonsHanlder _BattleButtonsHanlder = null;
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
        General.SetMainCameraPositionXYOnly(BattleField.transform.position);
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
        General.SetMainCameraPositionXYOnly(PlayerIcon.transform.position);
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
                    General.SetText(EnergyMoveButtons.transform.GetChild(i).transform.GetChild(0).gameObject, current_spirit.MoveSet[i].Name);

                    EnergyMoveButtons.transform.GetChild(i).gameObject.GetComponent<TooltipTrigger>().SetTooltipText(current_spirit.MoveSet[i].Name, current_spirit.MoveSet[i].Description);

                    EnergyMoveButtons.transform.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    EnergyMoveButtons.transform.GetChild(i).gameObject.SetActive(false);
                }
            }

            BasicMoveButtons.transform.GetChild(0).gameObject.GetComponent<TooltipTrigger>().SetTooltipText(current_spirit.BasicAttack.Name, current_spirit.BasicAttack.Description);
            General.SetText(BasicMoveButtons.transform.GetChild(0).transform.GetChild(0).gameObject, current_spirit.BasicAttack.Name);

            BasicMoveButtons.transform.GetChild(1).gameObject.GetComponent<TooltipTrigger>().SetTooltipText(current_spirit.BasicDefend.Name, current_spirit.BasicDefend.Description);
            General.SetText(BasicMoveButtons.transform.GetChild(1).transform.GetChild(0).gameObject, current_spirit.BasicDefend.Name);
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

                    General.SetText(TargetButtons.transform.GetChild(i).transform.GetChild(0).gameObject, EnemyParty.transform.GetChild(i).gameObject.GetComponent<SpiritPrefab>().Spirit.Name);
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

                        General.SetText(AllyButtons.transform.GetChild(index).transform.GetChild(0).gameObject, PlayerParty.transform.GetChild(i).gameObject.GetComponent<SpiritPrefab>().Spirit.Name);
                    }
                    else
                    {
                        AllyButtons.transform.GetChild(index).gameObject.SetActive(false);
                    }

                    index++;
                }
            }
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
        General.SetText(_Reward.transform.GetChild(1).gameObject, item.Name + " x" + quantity);

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

        if (PlayerManagement.PartyMemberCount() == PlayerManagement.ActivePartyMemberCount())
        {
            return;
        }

        for (int i = 0; i < PlayerManagement.PartyMemberCount(); i++)
        {
            if (PlayerManagement.CheckIfActive(PlayerManagement.GetPartyMember(i)))
            {
                continue;
            }

            int spirit_index = i;
            int button_index = _Spirits.transform.childCount - 1;

            spirit_button = GameObject.Instantiate(_SelectionButton, _Spirits.transform);
            General.SetText(spirit_button.transform.GetChild(0).gameObject, PlayerManagement.GetPartyMember(i).Name);
            General.ActivateObject(spirit_button);

            button = spirit_button.GetComponent<Button>();

            // spirit_index is passed by reference
            button.onClick.AddListener(() => { _BattleButtonsHanlder.SelectSpirit(spirit_index, button_index); });
        }

        General.ActivateObject(_SelectionPanel);
        General.ActivateObject(_SpiritSelectionGroup);
    }

    public void DisplayActiveSelectSpirit()
    {
        for (int i = 0; i < 3; i++)
        {
            if (PlayerManagement.GetActivePartyMember(i) != null)
            {
                General.ActivateObject(_ActiveSpiritSelectionGroup.transform.GetChild(i).gameObject);
            }
            else
            {
                General.DeactivateObject(_ActiveSpiritSelectionGroup.transform.GetChild(i).gameObject);
            }
        }
    }

    public void FinishSwitching()
    {
        General.DeactivateObject(_SelectionPanel);
        General.DeactivateObject(_SpiritSelectionGroup);
        General.DeactivateObject(_ActiveSpiritSelectionGroup);

        foreach (Transform child in _Spirits.transform)
        {
            if (child.gameObject.activeSelf)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void DestorySelectionButton(int index)
    {
        Destroy(_Spirits.transform.GetChild(index).gameObject);
    }
}
