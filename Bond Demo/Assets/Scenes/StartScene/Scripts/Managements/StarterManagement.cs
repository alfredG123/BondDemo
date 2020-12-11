using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StarterManagement : MonoBehaviour
{
    [SerializeField] private GameObject notice_box = null;
    [SerializeField] private GameObject stats_box = null;
    [SerializeField] private GameObject confirmation_box = null;
    [SerializeField] private GameObject data_storage = null;
    [SerializeField] private GameObject canvas_background = null;

    private int chosen_index = 0;
    private Vector2 previous_position = Vector2.zero;

    private Collider2D game_object_detector;
    private Monster monster_data_detector;

    // Make a enum for this?
    private bool is_deciding = true;

    #region Choosing A Starter

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && is_deciding)
        {
            game_object_detector = Physics2D.OverlapCircle(GeneralScripts.GetMousePositionInWorldSpace(), 0.1f);

            if (game_object_detector != null)
            {
                monster_data_detector = game_object_detector.gameObject.GetComponent<Monster>();

                if (monster_data_detector != null)
                {
                    DisplayMonsterInfo(monster_data_detector.monster_data);

                    // 
                    canvas_background.SetActive(true);

                    // Show the monster's stats
                    stats_box.SetActive(true);

                    // Ask the player to confirm the choice
                    confirmation_box.SetActive(true);

                    is_deciding = false;
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
        canvas_background.SetActive(false);

        //
        is_deciding = true;
    }

    // Show the notice box, and hide everything else
    public void ConfirmChoice()
    {
        notice_box.SetActive(true);

        stats_box.SetActive(false);
        confirmation_box.SetActive(false);
    }

    #endregion

    private void DisplayMonsterInfo(MonsterData chosen_monster)
    {
        Text weakness_text;

        // Modified the text objects to show stats
        stats_box.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Name: " + chosen_monster.monster_name;
        stats_box.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Health: " + chosen_monster.health.ToString();
        stats_box.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Attack: " + chosen_monster.attack.ToString();
        stats_box.transform.GetChild(3).gameObject.GetComponent<Text>().text = "Defense: " + chosen_monster.defense.ToString();
        stats_box.transform.GetChild(4).gameObject.GetComponent<Text>().text = "Speed: " + chosen_monster.speed.ToString();

        weakness_text = stats_box.transform.GetChild(5).gameObject.GetComponent<Text>();
        weakness_text.text = "Weakness:";

        foreach (Attribute weakness in chosen_monster.weakness)
        {
            weakness_text.text = weakness_text.text + "\r\n" + weakness.ToString();
        }

        //stats_box.transform.GetChild(6).gameObject.GetComponent<Text>().text = "Talent:\r\n" + chosen_monster.Talent.TalentName + "\r\n(" + chosen_monster.Talent.TalentDescription + ")";
    }

    #endregion

    #region Entering battle

    public void GameBegin()
    {
        //data_storage.GetComponent<PlayerManagement>().SetLinkedMonster(final_chosen_monster);
        SceneManager.LoadScene("Battle");
    }

    #endregion
}