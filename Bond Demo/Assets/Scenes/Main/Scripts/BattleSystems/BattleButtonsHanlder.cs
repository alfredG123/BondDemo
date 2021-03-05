using UnityEngine;

public class BattleButtonsHanlder : MonoBehaviour
{
    [SerializeField] private BattleDisplayHandler _BattleDisplayHanlder = null;
    [SerializeField] private BattleProgressionManagement _BattleProgressionManagement = null;

    [SerializeField] private GameObject _PlayerParty = null;
    [SerializeField] private GameObject _EnemyParty = null;

    [SerializeField] private GameObject _CurrentSpiritText = null;

    [SerializeField] private Canvas _MessageCanvas = null;

    private SpiritPrefab _CurrentSpirit = null;
    private int _FirstSpiritIndex = 0;
    private int _CurrentSpiritIndex = 0;
    private bool _SwitchSpirit = false;

    /// <summary>
    /// Reset variables, and display actions UI
    /// </summary>
    public void SetUpForFirstDecision()
    {
        _CurrentSpiritIndex = 0;

        SetFirstSpiritPrefab();

        while (_CurrentSpiritIndex < _PlayerParty.transform.childCount)
        {
            if ((_CurrentSpiritIndex < _PlayerParty.transform.childCount) && (_PlayerParty.transform.GetChild(_CurrentSpiritIndex).gameObject.activeSelf))
            {
                break;
            }

            _CurrentSpiritIndex++;
        }

        SetCurrentSpiritPrefab();

        // Show the buttons for the player to select actions
        _BattleDisplayHanlder.DisplayBattleButtons(TypePlanningPhrase.SelectingAction, _CurrentSpirit.Spirit, false);
    }

    /// <summary>
    /// Switch UI from selecting action to selecting moves for the spirit
    /// </summary>
    public void PlayerOrderAction()
    {
        _BattleDisplayHanlder.DisplayBattleButtons(TypePlanningPhrase.SelectingMove, _CurrentSpirit.Spirit, true);
    }

    public void PlayerSkillAction()
    {
        _BattleDisplayHanlder.DisplayBattleButtons(TypePlanningPhrase.SelectingSkill, _CurrentSpirit.Spirit, true);
    }

    public void BackToPreviousSpirit()
    {
        _CurrentSpiritIndex--;

        SetCurrentSpiritPrefab();

        _BattleDisplayHanlder.DisplayBattleButtons(TypePlanningPhrase.SelectingAction, _CurrentSpirit.Spirit, (_CurrentSpiritIndex > _FirstSpiritIndex));
    }

    /// <summary>
    /// Button handler for selecting first move
    /// </summary>
    public void SpiritMove1()
    {
        SelectAction(TypeSelectedMove.Move1);
    }

    /// <summary>
    /// Button handler for selecting second move
    /// </summary>
    public void SpiritMove2()
    {
        SelectAction(TypeSelectedMove.Move2);
    }

    /// <summary>
    /// Button handler for selecting third move
    /// </summary>
    public void SpiritMove3()
    {
        SelectAction(TypeSelectedMove.Move3);
    }

    public void SpiritAttack()
    {
        SelectAction(TypeSelectedMove.Attack);
    }

    /// <summary>
    /// Button handler for selecting defend
    /// </summary>
    public void SpiritDefend()
    {
        SelectAction(TypeSelectedMove.Defend);
    }

    /// <summary>
    /// Record the action in the list, and modify UI
    /// </summary>
    /// <param name="move_type"></param>
    private void SelectAction(TypeSelectedMove move_type)
    {
        int child_count = 0;

        if (!_CurrentSpirit.CheckEnergy(move_type))
        {
            string text_to_set = "Insufficient energy to use " + _CurrentSpirit.MoveToPerform.Name;
            Color text_color = Color.red;

            TextUIPopUp.CreateTextPopUp(text_to_set, GeneralInput.GetMousePositionInWorldSpace(), text_color, _MessageCanvas);

            return;
        }

        _CurrentSpirit.SetMove(move_type);

        if (move_type == TypeSelectedMove.Defend)
        {
            PerformNextStep();
        }
        else
        {
            foreach (Transform child in _EnemyParty.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    _CurrentSpirit.SetTargetToAim(child.gameObject);

                    child_count++;
                }
            }

            foreach (Transform child in _PlayerParty.transform)
            {
                if (child.gameObject.GetComponent<SpiritPrefab>() != _CurrentSpirit)
                {
                    if (child.gameObject.activeSelf)
                    {
                        _CurrentSpirit.SetTargetToAim(child.gameObject);

                        child_count++;
                    }
                }
            }

            if (child_count == 1)
            {
                PerformNextStep();
            }
            else if((_CurrentSpirit.MoveToPerform.TargetSelectionType == TypeTargetSelection.MultipleTarget) || (_CurrentSpirit.MoveToPerform.TargetSelectionType == TypeTargetSelection.MultipleAlliesIncludeSelf) || (_CurrentSpirit.MoveToPerform.TargetSelectionType == TypeTargetSelection.SelfTarget))
            {
                PerformNextStep();
            }
            else
            {
                _BattleDisplayHanlder.DisplayBattleButtons(TypePlanningPhrase.SelectingTarget, _CurrentSpirit.Spirit, true);
            }
        }
    }

    /// <summary>
    /// Replace the UI for selecting a move with selecting an action
    /// </summary>
    public void BackToMain()
    {
        _BattleDisplayHanlder.DisplayBattleButtons(TypePlanningPhrase.SelectingAction, _CurrentSpirit.Spirit, (_CurrentSpiritIndex > _FirstSpiritIndex));
    }

    /// <summary>
    /// Button handler for selecting first target
    /// </summary>
    public void SelectTarget1()
    {
        SelectTarget(target_object_index: 0, is_ally: false);
    }

    /// <summary>
    /// Button handler for selecting second target
    /// </summary>
    public void SelectTarget2()
    {
        SelectTarget(target_object_index: 1, is_ally: false);
    }

    /// <summary>
    /// Button handler for selecting third target
    /// </summary>
    public void SelectTarget3()
    {
        SelectTarget(target_object_index: 2, is_ally: false);
    }

    public void SelectAlly1()
    {
        SelectTarget(target_object_index: 0, is_ally: true);
    }

    public void SelectAlly2()
    {
        SelectTarget(target_object_index: 1, is_ally: true);
    }

    /// <summary>
    /// Add target game object to the list
    /// </summary>
    /// <param name="target_object_index"></param>
    private void SelectTarget(int target_object_index, bool is_ally)
    {
        _CurrentSpirit.SetTargetToAim(GetSpiritPrefabByIndex(target_object_index, is_ally).gameObject);

        // Determine what to do next
        PerformNextStep();
    }

    /// <summary>
    /// Replace the UI for selecting a tager with selecting a move 
    /// </summary>
    public void BackToSelectingMoves()
    {
        _BattleDisplayHanlder.DisplayBattleButtons(TypePlanningPhrase.SelectingMove, _CurrentSpirit.Spirit, true);
    }

    /// <summary>
    /// Display actions UI, or preform the selected actions
    /// </summary>
    public void PerformNextStep()
    {
        while (_CurrentSpiritIndex < _PlayerParty.transform.childCount)
        {
            _CurrentSpiritIndex++;

            if ((_CurrentSpiritIndex < _PlayerParty.transform.childCount) && (_PlayerParty.transform.GetChild(_CurrentSpiritIndex).gameObject.activeSelf))
            {
                break;
            }
        }

        if(_CurrentSpiritIndex >= _PlayerParty.transform.childCount)
        {
            // Hide all buttons
            _BattleDisplayHanlder.HideAllButtons();

            _CurrentSpiritText.SetActive(false);

            _BattleProgressionManagement.StartBattle();
        }
        else
        {
            SetCurrentSpiritPrefab();

            // Show the buttons for the player to select actions
            _BattleDisplayHanlder.DisplayBattleButtons(TypePlanningPhrase.SelectingAction, _CurrentSpirit.Spirit, (_CurrentSpiritIndex > _FirstSpiritIndex));
        }
    }

    /// <summary>
    /// Get the spirit prefab component from the game object
    /// </summary>
    /// <param name="game_object"></param>
    /// <returns></returns>
    public void SetCurrentSpiritPrefab()
    {
        _CurrentSpirit = _PlayerParty.transform.GetChild(_CurrentSpiritIndex).gameObject.GetComponent<SpiritPrefab>();

        GeneralComponent.SetText(_CurrentSpiritText, _CurrentSpirit.Spirit.Name);

        _CurrentSpiritText.SetActive(true);
    }

    public void SetFirstSpiritPrefab()
    {
        _FirstSpiritIndex = 0;

        foreach (Transform child in _PlayerParty.transform)
        {
            if (child.gameObject.activeSelf)
            {
                break;
            }
            else
            {
                _FirstSpiritIndex++;
            }
        }
    }

    public SpiritPrefab GetSpiritPrefabByIndex(int index, bool is_ally)
    {
        SpiritPrefab spirit = null;
        int count = 0;
        
        if (is_ally)
        {
            foreach (Transform child in _PlayerParty.transform)
            {
                if (child.gameObject.GetComponent<SpiritPrefab>() != _CurrentSpirit)
                {
                    if (index == count)
                    {
                        _CurrentSpirit.SetTargetToAim(child.gameObject);

                        spirit = child.gameObject.GetComponent<SpiritPrefab>();

                        break;
                    }

                    count++;
                }
            }
        }
        else
        {
            spirit = _EnemyParty.transform.GetChild(index).gameObject.GetComponent<SpiritPrefab>();
        }

        return (spirit);
    }

    public void CollectRewards()
    {
        _BattleDisplayHanlder.HideReward();

        _BattleDisplayHanlder.DisableBattle();
    }

    public void SwitchSpirit1()
    {
        SwitchSpirit(0);
    }

    public void SwitchSpirit2()
    {
        SwitchSpirit(1);
    }

    public void SwitchSpirit3()
    {
        SwitchSpirit(2);
    }

    private void SwitchSpirit(int index)
    {
        _BattleProgressionManagement.SwitchSpirit(index);
    }

    public void SetUpReplacingFaintedSpirit()
    {
        _SwitchSpirit = false;
    }

    public void SelectSpirit(int spirit_index, int button_index)
    {
        _BattleProgressionManagement.SelectSpirit(PlayerManagement.GetPartyMember(spirit_index), button_index, _SwitchSpirit);
    }

    public void SelectSkill1()
    {
        SelectSkill(0);
    }

    public void SelectSkill2()
    {
        SelectSkill(1);
    }
    public void SelectSkill3()
    {
        SelectSkill(2);
    }

    private void SelectSkill(int index)
    {
        Skill skill = PlayerManagement.GetActiveSkill(index);

        if (skill == Skill.Switch)
        {
            _SwitchSpirit = true;

            _BattleDisplayHanlder.DisplaySelectSpirit();
        }
    }
}
