using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteSelector : MonoBehaviour
{
    [SerializeField] private List<Sprite> _SpriteList = new List<Sprite>();

    /// <summary>
    /// Set the sprite for the object
    /// </summary>
    /// <param name="enemy_count"></param>
    public void SetSprite(int enemy_count)
    {
        int sprite_index = 0;

        if (enemy_count == 1)
        {
            sprite_index = 0;
        }
        else if (enemy_count == 2)
        {
            sprite_index = 1;
        }
        else if (enemy_count == 3)
        {
            sprite_index = 2;
        }

        this.gameObject.GetComponent<SpriteRenderer>().sprite = _SpriteList[sprite_index];
    }
}
