using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCommandsManagement : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private GameObject ActionButtons;
    [SerializeField] private GameObject MoveButtons;

    [SerializeField] private GameObject PlayerSpiritObjects;
    [SerializeField] private GameObject EnemySpiritObjects;
    [SerializeField] private GameObject TargetSelection;
#pragma warning restore 0649

    int _current_decision_index;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        // Initialize variables
        _current_decision_index = 0;
    }

    /// <summary>
    /// Reset variables, and display actions UI
    /// </summary>
    public void SetUpForFirstDecision()
    {
        // Reset variables
        _current_decision_index = 0;

        // Modified the text for each target button
        SetUpTargetSelection();

        // Show the UI for player to select actions
        ActionButtons.SetActive(true);
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
            // Show the UI for player to select actions
            ActionButtons.SetActive(true);
        }
        else
        {
            gameObject.GetComponent<BattleProgressionManagement>().PerformBattle();
        }
    }

    /// <summary>
    /// Switch UI from selecting action to selecting moves for the spirit
    /// </summary>
    public void PlayerOrderAction()
    {
        PlayerSpiritObjects.transform.GetChild(_current_decision_index).gameObject.GetComponent<SpiritPrefab>().SetSkills(MoveButtons);

        ActionButtons.SetActive(false);

        MoveButtons.SetActive(true);
    }

    public void PlayerSwitchAction()
    {

    }

    public void PlayerItemAction()
    {

    }

    public void PlayerSkillAction()
    {

    }

    public void PlayerEvoluteAction()
    {

    }

    /// <summary>
    /// Decrement the decision list to replace the older action
    /// </summary>
    public void BackToPreviousSpirit()
    {
        if (_current_decision_index > 0)
        {
            _current_decision_index--;
        }
    }

    /// <summary>
    /// Button handler for selecting first move
    /// </summary>
    public void SpiritMove1()
    {
        SelectAction(TypeAction.Move1);
    }

    /// <summary>
    /// Button handler for selecting second move
    /// </summary>
    public void SpiritMove2()
    {
        SelectAction(TypeAction.Move2);
    }

    /// <summary>
    /// Button handler for selecting third move
    /// </summary>
    public void SpiritMove3()
    {
        SelectAction(TypeAction.Move3);
    }

    /// <summary>
    /// Button handler for selecting fourth move
    /// </summary>
    public void SpiritMove4()
    {
        SelectAction(TypeAction.Move4);
    }

    /// <summary>
    /// Button handler for selecting defend
    /// </summary>
    public void SpiritDefend()
    {
        SelectAction(TypeAction.Defend);
    }

    /// <summary>
    /// Record the action in the list, and modify UI
    /// </summary>
    /// <param name="action_type"></param>
    private void SelectAction(TypeAction action_type)
    {
        GeneralScripts.GetSpiritPrefabScript(PlayerSpiritObjects.transform.GetChild(_current_decision_index).gameObject).SetAction(action_type);

        // Hide the move buttons
        MoveButtons.SetActive(false);
        
        // Show the UI for selecting target
        TargetSelection.SetActive(true);
    }

    /// <summary>
    /// Replace the UI for selecting a move with selecting an action
    /// </summary>
    public void BackToMain()
    {
        MoveButtons.SetActive(false);

        ActionButtons.SetActive(true);
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
        GeneralScripts.GetSpiritPrefabScript(PlayerSpiritObjects.transform.GetChild(_current_decision_index).gameObject).SetTarget(EnemySpiritObjects.transform.GetChild(target_object_index).gameObject);

        // Show the UI for selecting target
        TargetSelection.SetActive(false);

        // Determin what to do next
        PerformNextStep();
    }

    /// <summary>
    /// Replace the UI for selecting a tager with selecting a move 
    /// </summary>
    public void BackToSelectingMoves()
    {
        TargetSelection.SetActive(false);

        MoveButtons.SetActive(true);
    }

    /// <summary>
    /// Modified the text for each target selection button
    /// </summary>
    private void SetUpTargetSelection()
    {
        for (int i = 0; i < 3; i++)
        {
            TargetSelection.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = EnemySpiritObjects.transform.GetChild(i).gameObject.GetComponent<SpiritPrefab>().Spirit.SpiritName;
        }
    }
}
