using UnityEngine;

public class BattleButtonsHanlder : MonoBehaviour
{
    [SerializeField] GameObject PlayerParty = null;
    [SerializeField] GameObject EnemyParty = null;

    int _current_decision_index;
    BattleDisplayHandler _battle_display_hanlder;
    Spirit _current_spirit;

    /// <summary>
    /// Initialize global variables
    /// </summary>
    private void Awake()
    {
        if ((PlayerParty == null) || (EnemyParty == null))
        {
            General.ReturnToTitleSceneForErrors("BattleButtonsHandler.Awake", "The global variables are not set");
        }

        _current_decision_index = 0;

        _battle_display_hanlder = GetComponent<BattleDisplayHandler>();
    }

    /// <summary>
    /// Reset variables, and display actions UI
    /// </summary>
    public void SetUpForFirstDecision()
    {
        // Reset variables
        _current_decision_index = 0;

        while (!PlayerParty.transform.GetChild(_current_decision_index).gameObject.activeSelf)
        {
            _current_decision_index++;
        }

        GetCurrentSpirit();

        // Display the name of current spirit
        _battle_display_hanlder.DisplayCurrentSpiritForNote(_current_decision_index, true);

        // Show the buttons for the player to select actions
        _battle_display_hanlder.DisplayBattleButtons(TypePlanningPhrase.SelectingAction, _current_spirit, false);
    }

    /// <summary>
    /// Display actions UI, or preform the selected actions
    /// </summary>
    public void PerformNextStep()
    {
        _current_decision_index++;

        // Check if all actions are decided
        if (_current_decision_index < 3)
        {
            while (!PlayerParty.transform.GetChild(_current_decision_index).gameObject.activeSelf)
            {
                _current_decision_index++;
            }

            GetCurrentSpirit();

            // Display the name of current spirit
            _battle_display_hanlder.DisplayCurrentSpiritForNote(_current_decision_index, true);

            // Show the buttons for player to select actions
            _battle_display_hanlder.DisplayBattleButtons(TypePlanningPhrase.SelectingAction, _current_spirit, true);
        }
        else
        {   
            // Hide the name of current spirit
            _battle_display_hanlder.DisplayCurrentSpiritForNote(_current_decision_index, false);

            // Hide all buttons
            _battle_display_hanlder.DisplayBattleButtons(TypePlanningPhrase.None, _current_spirit, false);

            gameObject.GetComponent<BattleProgressionManagement>().StartBattle();
        }
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
    /// Decrement the decision list to replace the older action
    /// </summary>
    public void BackToPreviousSpirit()
    {
        _current_decision_index--;

        if (_current_decision_index < 0)
        {
            General.ReturnToTitleSceneForErrors("BattleButtonsHandler.BackToPreviousSpirit", "_current_decision_index is out of bound");
        }

        GetCurrentSpirit();

        // Display the name of current spirit
        _battle_display_hanlder.DisplayCurrentSpiritForNote(_current_decision_index, true);
        

        _battle_display_hanlder.DisplayBattleButtons(TypePlanningPhrase.SelectingAction, _current_spirit,!(_current_decision_index == 0));
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

        GetComponent<BattleProgressionManagement>().SpiritMoveOrderList.SetPriorityForDefense(PlayerParty.transform.GetChild(_current_decision_index - 1).gameObject);
    }

    /// <summary>
    /// Record the action in the list, and modify UI
    /// </summary>
    /// <param name="move_type"></param>
    private void SelectAction(TypeSelectedMove move_type)
    {
        General.GetSpiritPrefabComponent(PlayerParty.transform.GetChild(_current_decision_index).gameObject).SetMove(move_type);

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
        _battle_display_hanlder.DisplayBattleButtons(TypePlanningPhrase.SelectingAction, _current_spirit, !(_current_decision_index == 0));
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
        General.GetSpiritPrefabComponent(PlayerParty.transform.GetChild(_current_decision_index).gameObject).SetTarget(EnemyParty.transform.GetChild(target_object_index).gameObject);

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

    private void GetCurrentSpirit()
    {
        _current_spirit = General.GetSpiritPrefabComponent(PlayerParty.transform.GetChild(_current_decision_index).gameObject).Spirit;
    }
}
