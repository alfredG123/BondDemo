using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    private readonly List<GameObject> monsters_in_party = new List<GameObject>();

    public void AddMonsterToParty(GameObject _chosen_monster)
    {
        Monster monster = _chosen_monster.GetComponent<Monster>();

        if (monster != null)
        {
            // Carry the game object to next
            DontDestroyOnLoad(_chosen_monster);

            monster.JoinParty();
            monsters_in_party.Add(_chosen_monster);
        }
        else
        {
            GeneralScripts.ReturnToStarterScene("PlayerManagement");
            return;
        }
    }

    public GameObject GetMonsterFromParty(int _party_index)
    {
        return (monsters_in_party[_party_index]);
    }
}
