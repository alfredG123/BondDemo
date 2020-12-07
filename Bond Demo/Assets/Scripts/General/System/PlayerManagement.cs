using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    private List<BaseMonster> team;

    private void Awake()
    {
        team = new List<BaseMonster>();
    }

    public List<BaseMonster> Team
    {
        get { return (team); }
    }

    public void SetLinkedMonster(BaseMonster _chosen_monster)
    {
        _chosen_monster.IsLinkedMonster = true;
        team.Add(_chosen_monster);
    }
}
