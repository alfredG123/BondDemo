using UnityEngine;

public class BattleManagement : MonoBehaviour
{
    [SerializeField] private GameObject ally_positions;
    [SerializeField] private GameObject emeny_positions;
    [SerializeField] private GameObject monster_prefab;

    private GameObject game_manager = null;

    private void Start()
    {
        game_manager = GameObject.Find("GeneralGameManager");

        // For debug purpose
        if (game_manager == null)
        {
            GeneralScripts.CreateDefaultGameManager();
        }

        SpawnMonstersForPlayer();
        SpawnMonstersForEnemy();
    }

    private void SpawnMonstersForPlayer()
    {

    }

    private void SpawnMonstersForEnemy()
    {

    }
}
