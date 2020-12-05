using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    BaseMonsterData monster_library;

    private void Awake()
    {
        monster_library = new BaseMonsterData();

        DontDestroyOnLoad(this.gameObject);
    }

    public BaseMonster GetMonsterInfo(int entry_number)
    {
        return(monster_library.GetMonster(entry_number));
    }
}
