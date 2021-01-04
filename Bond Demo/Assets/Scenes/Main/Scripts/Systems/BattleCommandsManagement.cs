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
    [SerializeField] private GameObject EnemySelection;
#pragma warning restore 0649

    int _current_decision_index;
    List<GameObject> _target_list;
    List<int> _move_index_list;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start()
    {
        // Initialize variables
        _current_decision_index = 0;
        _target_list = new List<GameObject>();
        _move_index_list = new List<int>();
    }

    /// <summary>
    /// Reset variables, and display actions UI
    /// </summary>
    public void SetUpForFirstDecision()
    {
        // Reset variables
        _current_decision_index = 0;
        _target_list.Clear();
        _move_index_list.Clear();

        SetUpEnemySelection();

        // Show the UI for player to select actions
        ActionButtons.SetActive(true);
    }

    /// <summary>
    /// Display actions UI, or preform the selected actions
    /// </summary>
    public void PerformNextStep()
    {

        // Hide the UI for selecting moves
        MoveButtons.SetActive(false);

        // Check if all actions are decided
        if (_current_decision_index < 3)
        {
            // Show the UI for player to select actions
            ActionButtons.SetActive(true);
        }
        else
        {

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

    public void BackToPreviousSpirit()
    {
        //moves.SetActive(false);

        //actions.SetActive(true);
    }

    public void SpiritMove1()
    {
        
    }

    public void SpiritMove2()
    {

    }

    public void SpiritMove3()
    {

    }

    public void SpiritMove4()
    {

    }

    public void SpiritDefend()
    {

    }

    public void SelectEnemy1()
    {

    }

    public void SelectEnemy2()
    {

    }

    public void SelectEnemy3()
    {

    }

    public void BackToMain()
    {
        //moves.SetActive(false);

        //actions.SetActive(true);
    }

    private void SetUpEnemySelection()
    {
        for (int i = 0; i < 3; i++)
        {
            EnemySelection.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = EnemySpiritObjects.transform.GetChild(i).gameObject.GetComponent<SpiritPrefab>().Spirit.SpiritName;
        }
    }
}
