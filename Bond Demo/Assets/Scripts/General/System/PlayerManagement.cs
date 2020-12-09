using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    private List<BaseMonsterInfo> team;

    private void Awake()
    {
        team = new List<BaseMonsterInfo>();
    }

    public List<BaseMonsterInfo> Team
    {
        get { return (team); }
    }

    public void SetLinkedMonster(BaseMonsterInfo _chosen_monster)
    {
        _chosen_monster.IsLinkedMonster = true;
        _chosen_monster.IsAlly = true;
        team.Add(_chosen_monster);
    }
}
