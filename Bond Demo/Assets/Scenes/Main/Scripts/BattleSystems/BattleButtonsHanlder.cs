using UnityEngine;

public class BattleButtonsHanlder : MonoBehaviour
{
    [SerializeField] private BattleDisplayHandler _BattleDisplayHanlder = null;
    [SerializeField] private BattleProgressionManagement _BattleProgressionManagement = null;

    private SpiritPrefab _Spirit = null;

    /// <summary>
    /// Reset variables, and display actions UI
    /// </summary>
    public void SetUpForFirstDecision(SpiritPrefab spirit)
    {
        _Spirit = spirit;

        // Show the buttons for the player to select actions
        _BattleDisplayHanlder.DisplayBattleButtons(TypePlanningPhrase.SelectingAction, spirit.Spirit, false);
    }

    /// <summary>
    /// Switch UI from selecting action to selecting moves for the spirit
    /// </summary>
    public void PlayerOrderAction()
    {
        _BattleDisplayHanlder.DisplayBattleButtons(TypePlanningPhrase.SelectingMove, _Spirit.Spirit, true);
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

        //GetComponent<BattleProgressionManagement>().SpiritMoveOrderList.SetPriorityForDefense(PlayerSpirit.gameObject);
    }

    /// <summary>
    /// Record the action in the list, and modify UI
    /// </summary>
    /// <param name="move_type"></param>
    private void SelectAction(TypeSelectedMove move_type)
    {
        _Spirit.SetMove(move_type);

        if (move_type != TypeSelectedMove.Defend)
        {
            _BattleDisplayHanlder.DisplayBattleButtons(TypePlanningPhrase.SelectingTarget, _Spirit.Spirit, true);
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
        _BattleDisplayHanlder.DisplayBattleButtons(TypePlanningPhrase.SelectingAction, _Spirit.Spirit, false);
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
        //PlayerSpirit.SetTarget(EnemyParty.transform.GetChild(target_object_index).gameObject);

        // Determin what to do next
        PerformNextStep();
    }

    /// <summary>
    /// Replace the UI for selecting a tager with selecting a move 
    /// </summary>
    public void BackToSelectingMoves()
    {
        _BattleDisplayHanlder.DisplayBattleButtons(TypePlanningPhrase.SelectingMove, _Spirit.Spirit, true);
    }

    /// <summary>
    /// Display actions UI, or preform the selected actions
    /// </summary>
    public void PerformNextStep()
    {
        // Hide all buttons
        _BattleDisplayHanlder.DisplayBattleButtons(TypePlanningPhrase.None, _Spirit.Spirit, false);

        _BattleProgressionManagement.StartBattle();
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
