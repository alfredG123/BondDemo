using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StarterManagement : MonoBehaviour
{
    [SerializeField] private GameObject data_storage = null;
    [SerializeField] private GameObject canvas_background = null;
    [SerializeField] private GameObject starter_image = null;
    [SerializeField] private GameObject stats_box = null;
    [SerializeField] private GameObject confirmation_box = null;

    private Collider2D game_object_detector;
    private MonsterPrefab monster_script_detector;
    private GameObject chosen_monster;
    private bool is_deciding = true;

    #region Choosing A Starter

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && is_deciding)
        {
            game_object_detector = Physics2D.OverlapCircle(GeneralScripts.GetMousePositionInWorldSpace(), 0.1f);

            if (game_object_detector != null)
            {
                monster_script_detector = game_object_detector.gameObject.GetComponent<MonsterPrefab>();

                if (monster_script_detector != null)
                {
                    DisplayMonsterInfo(monster_script_detector.Monster);

                    canvas_background.SetActive(true);

                    // Show the monster's stats
                    stats_box.SetActive(true);

                    // Ask the player to confirm the choice
                    confirmation_box.SetActive(true);

                    starter_image.SetActive(true);

                    is_deciding = false;

                    chosen_monster = game_object_detector.gameObject;
                }
            }
        }
    }

    #region Button Handlers

    // Reset all game objects
    public void RegretChoice()
    {
        // Show the monster's stats
        stats_box.SetActive(false);

        // Hide the question box
        confirmation_box.SetActive(false);

        //
        starter_image.SetActive(false);

        //
        canvas_background.SetActive(false);

        //
        is_deciding = true;
    }

    // Show the notice box, and hide everything else
    public void ConfirmChoice()
    {
        stats_box.SetActive(false);
        confirmation_box.SetActive(false);

        GameBegin();
    }

    #endregion

    private void DisplayMonsterInfo(Monster _chosen_monster)
    {
        starter_image.GetComponent<Image>().sprite = _chosen_monster.MonsterSprite;

        // Modified the text objects to show stats
        stats_box.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Name: " + _chosen_monster.MonsterName;
        stats_box.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Health: " + _chosen_monster.HealthText;
        stats_box.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Attack: " + _chosen_monster.AttackText;
        stats_box.transform.GetChild(3).gameObject.GetComponent<Text>().text = "Defense: " + _chosen_monster.DefenseText;
        stats_box.transform.GetChild(4).gameObject.GetComponent<Text>().text = "Speed: " + _chosen_monster.SpeedText;

        //display skill
    }

    #endregion

    #region Entering battle

    public void GameBegin()
    {
        data_storage.GetComponent<PlayerManagement>().AddMonsterToParty(chosen_monster.GetComponent<MonsterPrefab>().Monster);
        SceneManager.LoadScene("Battle");
    }

    #endregion
}