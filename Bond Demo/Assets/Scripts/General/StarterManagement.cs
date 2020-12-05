using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StarterManagement : MonoBehaviour
{
    [SerializeField] private GameObject starters = null;
    [SerializeField] private GameObject confirmation_box = null;
    [SerializeField] private GameObject stats_box = null;
    [SerializeField] private GameObject note_box = null;
    [SerializeField] private GameObject game_data = null;
    private int chosen_index = 0;
    private Vector2 previous_position = Vector2.zero;

    private void Start()
    {
        Button starter_button;
        starter_button = starters.transform.GetChild(0).gameObject.GetComponent<Button>();
        starter_button.onClick.AddListener(() => ChooseAMonster(0));
        starter_button = starters.transform.GetChild(1).gameObject.GetComponent<Button>();
        starter_button.onClick.AddListener(() => ChooseAMonster(1));
        starter_button = starters.transform.GetChild(2).gameObject.GetComponent<Button>();
        starter_button.onClick.AddListener(() => ChooseAMonster(2));
        starter_button = starters.transform.GetChild(3).gameObject.GetComponent<Button>();
        starter_button.onClick.AddListener(() => ChooseAMonster(3));
        starter_button = starters.transform.GetChild(4).gameObject.GetComponent<Button>();
        starter_button.onClick.AddListener(() => ChooseAMonster(4));
    }

    // Close other options, and inform the player about the losing condition.
    public void ChooseAMonster(int index)
    {
        chosen_index = index;

        previous_position = starters.transform.GetChild(index).gameObject.GetComponent<RectTransform>().localPosition;

        for(int i = 0; i < starters.transform.childCount; i++)
        {
            if (i != index)
            {
                starters.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        starters.transform.GetChild(index).gameObject.GetComponent<RectTransform>().localPosition = new Vector2(-50f, 0f);

        starters.transform.GetChild(index).gameObject.GetComponent<Button>().enabled = false;

        BaseMonster chosen_monster = game_data.GetComponent<GameData>().GetMonsterInfo(chosen_index + 1);

        stats_box.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Name: " + chosen_monster.MonsterName;
        stats_box.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Attribute: " + chosen_monster.Attribute;
        stats_box.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Health: " + chosen_monster.Health.ToString();
        stats_box.transform.GetChild(3).gameObject.GetComponent<Text>().text = "Attack: " + chosen_monster.Attack.ToString();
        stats_box.transform.GetChild(4).gameObject.GetComponent<Text>().text = "Defense: " + chosen_monster.Defense.ToString();
        stats_box.transform.GetChild(5).gameObject.GetComponent<Text>().text = "Speed: " + chosen_monster.Speed.ToString();
        
        stats_box.SetActive(true);
        confirmation_box.SetActive(true);
    }

    public void RegretChoice()
    {
        starters.transform.GetChild(chosen_index).gameObject.GetComponent<RectTransform>().localPosition = previous_position;

        starters.transform.GetChild(chosen_index).gameObject.GetComponent<Button>().enabled = true;

        for (int i = 0; i < starters.transform.childCount; i++)
        {
            starters.transform.GetChild(i).gameObject.SetActive(true);
        }

        stats_box.SetActive(false);
        confirmation_box.SetActive(false);
    }

    public void ConfirmChoice()
    {
        note_box.SetActive(true);
        starters.SetActive(false);
        stats_box.SetActive(false);
        confirmation_box.SetActive(false);
    }

    public void GameBegin()
    {
        SceneManager.LoadScene("Battle");
    }
}
