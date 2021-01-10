using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDisplayHandler : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private GameObject BattleField;

    [SerializeField] private GameObject PlayerIcon;
    [SerializeField] private GameObject CurrentSpiritNote;

    [SerializeField] private GameObject ActionButtons;
    [SerializeField] private GameObject MoveButtons;
    [SerializeField] private GameObject TargetButtons;

    [SerializeField] private GameObject PlayerParty;
    [SerializeField] private GameObject EnemyParty;
#pragma warning restore 0649

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start()
    {
        // Move the main camera to the battle field
        MoveCameraToBattleField();

        // Activate battle field UI
        DisplayBattleField(is_active: true);
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

    /// <summary>
    /// Display different buttons based on the phrase
    /// </summary>
    /// <param name="phrase"></param>
    /// <param name="show_back_button"></param>
    public void DisplayBattleButtons(TypePlanningPhrase phrase, Spirit current_spirit, bool show_back_button)
    {
        // Hide all buttons by default
        ActionButtons.SetActive(false);
        MoveButtons.SetActive(false);
        TargetButtons.SetActive(false);

        if (phrase == TypePlanningPhrase.SelectingAction)
        {
            ActionButtons.SetActive(true);

            ActionButtons.transform.GetChild(2).gameObject.SetActive(show_back_button);
        }
        else if (phrase == TypePlanningPhrase.SelectingMove)
        {
            MoveButtons.SetActive(true);

            // Hide the faint spirit
            for (int i = 0; i < MoveButtons.transform.childCount; i++)
            {
                if (i < current_spirit.Skills.Count)
                {
                    MoveButtons.transform.GetChild(i).gameObject.SetActive(true);

                    General.SetText(MoveButtons.transform.GetChild(i).transform.GetChild(0).gameObject, current_spirit.Skills[i].SkillName);
                }
                else
                {
                    MoveButtons.transform.GetChild(i).gameObject.SetActive(false);
                }
            }

            MoveButtons.transform.GetChild(4).gameObject.SetActive(show_back_button);
        }
        else if(phrase == TypePlanningPhrase.SelectingTarget)
        {
            TargetButtons.SetActive(true);

            // Hide the faint spirit
            for (int i = 0; i < EnemyParty.transform.childCount; i++)
            {
                if (EnemyParty.transform.GetChild(i).gameObject.activeSelf)
                {
                    TargetButtons.transform.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    TargetButtons.transform.GetChild(i).gameObject.SetActive(false);
                }
            }

            TargetButtons.transform.GetChild(3).gameObject.SetActive(show_back_button);
        }
    }

    /// <summary>
    /// Display name of the current spirit, if it is needed
    /// </summary>
    /// <param name="current_spirit_index"></param>
    /// <param name="is_displaying"></param>
    public void DisplayCurrentSpiritForNote(int current_spirit_index, bool is_displaying)
    {
        CurrentSpiritNote.SetActive(false);

        if (is_displaying)
        {
            General.SetText(CurrentSpiritNote, "Current: " + General.GetSpiritPrefabComponent(PlayerParty.transform.GetChild(current_spirit_index).gameObject).Spirit.SpiritName);

            CurrentSpiritNote.SetActive(true);
        }
    }
}
