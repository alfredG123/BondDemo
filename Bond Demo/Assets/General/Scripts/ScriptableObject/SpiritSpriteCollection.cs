using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spirit Sprite Collection", menuName = "BOND/SpiritSpriteCollection")]
public class SpiritSpriteCollection : ScriptableObject
{
    [SerializeField] private List<Sprite> _SpiritSprites = null;

    public Sprite GetSpiritSpriteByImageName(string image_name)
    {
        return (_SpiritSprites.Find(spirit => spirit.name == image_name));
    }
}
