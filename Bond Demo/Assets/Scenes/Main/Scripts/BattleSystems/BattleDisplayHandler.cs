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

    [SerializeField] private GameObject BattleNarrativeText;

    [SerializeField] private GameObject maze;

    [SerializeField] private MainManagement _MainManagement = null;

    [SerializeField] private GameObject _Reward = null;
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
        BattleNarrativeText.SetActive(false);

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

    public void DisplayBattleNarrativeForUsingMove()
    {
        //if (spirit.IsAlly)
        //{
        //    General.SetText(BattleNarrativeText, spirit.SpiritName + " used " + move.MoveName + " on " + target.SpiritName + "!");
        //}
        //else
        //{
        //    General.SetText(BattleNarrativeText, spirit.SpiritName + " used " + move.MoveName + "!");
        //}

        BattleNarrativeText.SetActive(true);
    }

    public void DisplayBattleNarrativeFoEffectiveness(bool critical_hit, TypeEffectiveness effectiveness)
    {
        string hit_effect;

        hit_effect = "Effectiveness: ";

        if (effectiveness == TypeEffectiveness.Effective)
        {
            hit_effect = "The move is effective.";
        }
        else if(effectiveness == TypeEffectiveness.NotEffective)
        {
            hit_effect = "The move is not effective.";
        }
        else if(effectiveness == TypeEffectiveness.SuperEffective)
        {
            hit_effect = "The move is super effective!";
        }

        if (critical_hit)
        {
            hit_effect += "\r\n";

            hit_effect += "CRITICAL HIT!";
        }

        General.SetText(BattleNarrativeText, hit_effect);

        BattleNarrativeText.SetActive(true);
    }

    public void DisplayBattleNarrativeForMissingTarget()
    {
        General.SetText(BattleNarrativeText, "The move miss!");

        BattleNarrativeText.SetActive(true);
    }

    public void DisplayBattleNarrativeForDefense(Spirit spirit)
    {
        General.SetText(BattleNarrativeText,  spirit.Name + " defend yourself!");

        BattleNarrativeText.SetActive(true);
    }

    public void DisableBattleNarrative()
    {
        BattleNarrativeText.SetActive(false);
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
}
