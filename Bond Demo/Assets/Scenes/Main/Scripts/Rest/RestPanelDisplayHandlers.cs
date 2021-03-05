using UnityEngine;

public class RestPanelDisplayHandlers : MonoBehaviour
{
    [SerializeField] private GameObject _MessageText = null;

    public void DisplayRest()
    {
        int party_member_count = PlayerManagement.PartyMemberCount();

        if (party_member_count == 0)
        {
            GeneralComponent.SetText(_MessageText, "All party members have a good rest!");
        }
        else
        {
            for (int i = 0; i < PlayerManagement.PartyMemberCount(); i++)
            {
                PlayerManagement.GetPartyMember(i).CurrentHealth = PlayerManagement.GetPartyMember(i).MaxHealth;
            }

            GeneralComponent.SetText(_MessageText, "All party members are healed!");
        }
    }
}
