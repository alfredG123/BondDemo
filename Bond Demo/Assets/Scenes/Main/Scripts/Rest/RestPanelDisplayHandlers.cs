using UnityEngine;

public class RestPanelDisplayHandlers : MonoBehaviour
{
    [SerializeField] private GameObject _MessageText = null;

    public void DisplayRest()
    {
        int party_member_count = PlayerInformation.PartyMemberCount();

        if (party_member_count == 0)
        {
            _MessageText.SetText("All party members have a good rest!");
        }
        else
        {
            for (int i = 0; i < PlayerInformation.PartyMemberCount(); i++)
            {
                PlayerInformation.GetPartyMember(i).CurrentHealth = PlayerInformation.GetPartyMember(i).MaxHealth;
            }

            _MessageText.SetText("All party members are healed!");
        }
    }
}
