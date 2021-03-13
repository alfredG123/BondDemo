using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurvivedSpiritPanelDisplayHandlers : MonoBehaviour
{
    [SerializeField] private Image _SurvivedSpiritImage = null;
    [SerializeField] private GameObject _Result = null;
    [SerializeField] private GameObject _HelpButtonText = null;
    [SerializeField] private GameObject _InvitationButtonText = null;

    private readonly List<BaseSpirit> _SurvivedSpiritList = new List<BaseSpirit>();
    private BaseSpirit _CurrentSpirit = null;
    float _CurrentSuccessRate = 0f;
    int _CurrentHelpSupplyAmount = 0;

    private void Awake()
    {
        _SurvivedSpiritList.Add(BaseSpirit.A1);
        _SurvivedSpiritList.Add(BaseSpirit.B1);
        _SurvivedSpiritList.Add(BaseSpirit.C1);
        _SurvivedSpiritList.Add(BaseSpirit.D1);
        _SurvivedSpiritList.Add(BaseSpirit.E1);
    }

    private void SetHelpButton()
    {
        _CurrentHelpSupplyAmount = Random.Range(0, 5);

        GeneralComponent.SetText(_HelpButtonText, "Ask For Help (" + _CurrentHelpSupplyAmount + ")");
    }

    private void SetInvitationButton()
    {
        //_CurrentSuccessRate = Random.Range(0f, 1f);
        _CurrentSuccessRate = 1f;

        GeneralComponent.SetText(_InvitationButtonText, "Invite To Party (" + (_CurrentSuccessRate * 100).ToString("0.00") + "%)");
    }

    public void DisplaySurvivedSpirit()
    {
        int random_spirit_index = Random.Range(0, _SurvivedSpiritList.Count);

        _CurrentSpirit = _SurvivedSpiritList[random_spirit_index];

        _SurvivedSpiritImage.sprite = AssetsLoader.Assets.LoadSprite(_CurrentSpirit.ImageName, LoadObjectEnum.SpiritImage);

        SetHelpButton();

        SetInvitationButton();
    
        if (PlayerInformation.PartyMemberCount() == 0)
        {
            PlayerInformation.SetUpTemporaryPlayer();
        }
    }

    public void DisplayHelpSupply()
    {
        PlayerInformation.AddItemToBag(Item.Cystal, _CurrentHelpSupplyAmount);

        GeneralComponent.SetText(_Result.transform.GetChild(1).gameObject, Item.Cystal.Name + " x" + _CurrentHelpSupplyAmount);

        GeneralGameObject.ActivateObject(_Result);
    }

    public void DisplayInvitationResult()
    {
        float random_success_rate = Random.Range(0f, 1f);
        
        if (random_success_rate < _CurrentSuccessRate)
        {
            PlayerInformation.AddSpiritToParty(_CurrentSpirit, _CurrentSpirit.Name + ": "+ Random.Range(0f, 1f));

            GeneralComponent.SetText(_Result.transform.GetChild(1).gameObject, _CurrentSpirit.Name + " join your party.");
        }
        else
        {
            GeneralComponent.SetText(_Result.transform.GetChild(1).gameObject, _CurrentSpirit.Name + " reject to join your party");
        }

        GeneralGameObject.ActivateObject(_Result);
    }

    public void HideResult()
    {
        GeneralGameObject.DeactivateObject(_Result);
    }
}
