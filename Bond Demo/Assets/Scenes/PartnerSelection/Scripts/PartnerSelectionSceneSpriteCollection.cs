using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartnerSelectionSceneSpriteCollection : MonoBehaviour
{
    [SerializeField] private List<Sprite> _SpiritSprites = new List<Sprite>();

    public Sprite GetSpiritSpriteByImageName(string image_name)
    {
        return (_SpiritSprites.Find(spirit => spirit.name == image_name));
    }
}
