using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    public List<Spirit> Party { get; private set; } = new List<Spirit>();

    public void AddSpiritToParty(BaseSpirit spirit_to_set, string name)
    {
        Party.Add(new Spirit(spirit_to_set, name));
    }

    public void SetUpTemporaryParty()
    {
        Party.Add(new Spirit(BaseSpirit.C1, "Max"));
        Party.Add(new Spirit(BaseSpirit.D1, "Lax"));
        Party.Add(new Spirit(BaseSpirit.E1, "Rax"));
    }
}
