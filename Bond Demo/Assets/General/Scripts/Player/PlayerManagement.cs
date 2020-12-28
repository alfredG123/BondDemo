using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    private readonly List<Spirit> _spirits_in_party = new List<Spirit>();

    public void AddSpiritToParty(Spirit spirit_to_add)
    {
        spirit_to_add.JoinParty();
        _spirits_in_party.Add(spirit_to_add);
    }

    public Spirit GetSpiritFromParty(int _party_index)
    {
        return (_spirits_in_party[_party_index]);
    }
}
