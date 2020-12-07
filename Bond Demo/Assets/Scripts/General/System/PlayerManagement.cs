using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    private BaseMonster linked_monster;

    public BaseMonster LinkedMonster
    {
        get { return (linked_monster); }
    }

    public void SetLinkedMonster(BaseMonster _chosen_monster)
    {
        linked_monster = _chosen_monster;
    }
}
