using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManagement : MonoBehaviour
{
    [SerializeField] private GameObject sample_text = null;
    private GameObject data_storage = null;

    private void Start()
    {
        data_storage = GameObject.Find("DataStorage");

        // For debug purpose
        if (data_storage == null)
        {
            data_storage = new GameObject("DataStorage");
            data_storage.AddComponent<GameData>();
            data_storage.GetComponent<GameData>().SetUpPlayer(new BaseMonster());
        }
        
        sample_text.GetComponent<Text>().text = "You choose " + data_storage.GetComponent<GameData>().Player.LinkedMonster.MonsterName + " as your linked monster.";
    }
}
