using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    public List<Spirit> Party { get; private set; } = new List<Spirit>();

    public void AddSpiritToParty(BaseSpirit spirit_to_set, string name)
    {
        Party.Add(new Spirit(spirit_to_set, name, true));
    }

    public void SetUpTemporaryParty()
    {
        Party.Add(new Spirit(BaseSpirit.C1, "Max", true));
        Party.Add(new Spirit(BaseSpirit.D1, "Lax", true));
        Party.Add(new Spirit(BaseSpirit.A1, "Rax", true));
    }
}
