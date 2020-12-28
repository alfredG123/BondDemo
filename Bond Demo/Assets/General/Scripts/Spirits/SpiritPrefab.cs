using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritPrefab : MonoBehaviour
{
    private Spirit _spirit;

    public Spirit Spirit
    {
        get => (_spirit);
    }

    public void SetSpirit(Spirit spirit_to_set)
    {
        _spirit = spirit_to_set;

        GetComponent<SpriteRenderer>().sprite = spirit_to_set.SpiritSprite;
    }

    public void SetSpirit(Spirit spirit_to_set, bool is_ally)
    {
        _spirit = spirit_to_set;

        GetComponent<SpriteRenderer>().sprite = spirit_to_set.SpiritSprite;

        GetComponent<Animator>().SetBool("IsAlly", is_ally);
    }

    public void PlayAttackAnimation()
    {
        GetComponent<Animator>().SetTrigger("Attack");
    }
}
