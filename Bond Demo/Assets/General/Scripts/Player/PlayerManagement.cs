using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    public Spirit ParnterSpirit { get; private set; } = null;

    public void SetSpiritAsPartner(BaseSpirit spirit_to_set, string name)
    {
        ParnterSpirit = new Spirit(spirit_to_set, name);
    }
}
