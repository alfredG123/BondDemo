using UnityEngine;

public class BattleButtonsHanlder : MonoBehaviour
{
    [SerializeField] private BattleDisplayHandler _BattleDisplayHanlder = null;
    [SerializeField] private BattleProgressionManagement _BattleProgressionManagement = null;

    [SerializeField] private GameObject _PlayerParty = null;
    [SerializeField] private GameObject _EnemyParty = null;

    [SerializeField] private GameObject _CurrentSpiritText = null;

    private SpiritPrefab _CurrentSpirit = null;
    private int _CurrentSpiritIndex = 0;

    /// <summary>
    /// Reset variables, and display actions UI
    /// </summary>
    public void SetUpForFirstDecision()
    {
        _CurrentSpiritIndex = 0;

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

    }

    public void BackToPreviousSpirit()
    {
        _CurrentSpiritIndex--;

        SetCurrentSpiritPrefab();

        _BattleDisplayHanlder.DisplayBattleButtons(TypePlanningPhrase.SelectingAction, _CurrentSpirit.Spirit, (_CurrentSpiritIndex > 0));
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

            TextPopUp.CreateTextPopUp(text_to_set, General.GetMousePositionInWorldSpace(), text_color);

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

            if (child_count == 1)
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
        _BattleDisplayHanlder.DisplayBattleButtons(TypePlanningPhrase.SelectingAction, _CurrentSpirit.Spirit, (_CurrentSpiritIndex > 0));
    }

    /// <summary>
    /// Button handler for selecting first target
    /// </summary>
    public void SelectTarget1()
    {
        SelectTarget(target_object_index: 0);
    }

    /// <summary>
    /// Button handler for selecting second target
    /// </summary>
    public void SelectTarget2()
    {
        SelectTarget(target_object_index: 1);
    }

    /// <summary>
    /// Button handler for selecting third target
    /// </summary>
    public void SelectTarget3()
    {
        SelectTarget(target_object_index: 2);
    }

    /// <summary>
    /// Add target game object to the list
    /// </summary>
    /// <param name="target_object_index"></param>
    private void SelectTarget(int target_object_index)
    {
        _CurrentSpirit.SetTargetToAim(GetEnenmySpiritPrefabByIndex(target_object_index).gameObject);

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
            _BattleDisplayHanlder.DisplayBattleButtons(TypePlanningPhrase.SelectingAction, _CurrentSpirit.Spirit, (_CurrentSpiritIndex > 0));
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

        General.SetText(_CurrentSpiritText, _CurrentSpirit.Spirit.Name);

        _CurrentSpiritText.SetActive(true);
    }

    public SpiritPrefab GetEnenmySpiritPrefabByIndex(int index)
    {
        SpiritPrefab spirit = _EnemyParty.transform.GetChild(index).gameObject.GetComponent<SpiritPrefab>();

        return (spirit);
    }

    /*
    [SerializeField] private SpiritPrefab PlayerSpirit = null;
    [SerializeField] private GameObject EnemyParty = null;

    private BattleDisplayHandler _battle_display_hanlder;
    private Spirit _current_spirit;

    /// <summary>
    /// Initialize global variables
    /// </summary>
    private void Awake()
    {
        if ((PlayerSpirit == null) || (EnemyParty == null))
        {
            General.ReturnToTitleSceneForErrors("BattleButtonsHandler.Awake", "The global variables are not set");
        }

        _battle_display_hanlder = GetComponent<BattleDisplayHandler>();
    }

    /// <summary>
    /// Reset variables, and display actions UI
    /// </summary>
    public void SetUpForFirstDecision()
    {
        _current_spirit = PlayerSpirit.Spirit;

        // Show the buttons for the player to select actions
        _battle_display_hanlder.DisplayBattleButtons(TypePlanningPhrase.SelectingAction, _current_spirit, false);
    }

    /// <summary>
    /// Display actions UI, or preform the selected actions
    /// </summary>
    public void PerformNextStep()
    {
        // Hide all buttons
        _battle_display_hanlder.DisplayBattleButtons(TypePlanningPhrase.None, _current_spirit, false);

        gameObject.GetComponent<BattleProgressionManagement>().StartBattle();
    }

    /// <summary>
    /// Switch UI from selecting action to selecting moves for the spirit
    /// </summary>
    public void PlayerOrderAction()
    {
        _battle_display_hanlder.DisplayBattleButtons(TypePlanningPhrase.SelectingMove, _current_spirit, true);
    }

    public void PlayerSkillAction()
    {

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

    /// <summary>
    /// Button handler for selecting defend
    /// </summary>
    public void SpiritDefend()
    {
        SelectAction(TypeSelectedMove.Defend);

        GetComponent<BattleProgressionManagement>().SpiritMoveOrderList.SetPriorityForDefense(PlayerSpirit.gameObject);
    }

    /// <summary>
    /// Record the action in the list, and modify UI
    /// </summary>
    /// <param name="move_type"></param>
    private void SelectAction(TypeSelectedMove move_type)
    {
        PlayerSpirit.SetMove(move_type);

        if (move_type != TypeSelectedMove.Defend)
        {
            _battle_display_hanlder.DisplayBattleButtons(TypePlanningPhrase.SelectingTarget, _current_spirit, true);
        }
        else
        {
            PerformNextStep();
        }
    }

    /// <summary>
    /// Replace the UI for selecting a move with selecting an action
    /// </summary>
    public void BackToMain()
    {
        _battle_display_hanlder.DisplayBattleButtons(TypePlanningPhrase.SelectingAction, _current_spirit, false);
    }

    /// <summary>
    /// Button handler for selecting first target
    /// </summary>
    public void SelectTarget1()
    {
        SelectTarget(target_object_index: 0);
    }

    /// <summary>
    /// Button handler for selecting second target
    /// </summary>
    public void SelectTarget2()
    {
        SelectTarget(target_object_index: 1);
    }

    /// <summary>
    /// Button handler for selecting third target
    /// </summary>
    public void SelectTarget3()
    {
        SelectTarget(target_object_index: 2);
    }

    /// <summary>
    /// Add target game object to the list
    /// </summary>
    /// <param name="target_object_index"></param>
    private void SelectTarget(int target_object_index)
    {
        PlayerSpirit.SetTarget(EnemyParty.transform.GetChild(target_object_index).gameObject);

        // Determin what to do next
        PerformNextStep();
    }

    /// <summary>
    /// Replace the UI for selecting a tager with selecting a move 
    /// </summary>
    public void BackToSelectingMoves()
    {
        _battle_display_hanlder.DisplayBattleButtons(TypePlanningPhrase.SelectingMove, _current_spirit, true);
    }
    */
}
