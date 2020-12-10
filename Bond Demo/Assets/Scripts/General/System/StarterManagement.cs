using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StarterManagement : MonoBehaviour
{
    [SerializeField] private GameObject starters = null;
    [SerializeField] private GameObject notice_box = null;
    [SerializeField] private GameObject stats_box = null;
    [SerializeField] private GameObject confirmation_box = null;
    [SerializeField] private GameObject data_storage = null;

    private int chosen_index = 0;
    private Vector2 previous_position = Vector2.zero;
    private List<BaseMonsterInfo> start_monster_list = null;
    private BaseMonsterInfo final_chosen_monster = null;

    private void Start()
    {
        Button starter_monster_button;

        // Set a listener for each monster choice.
        for(int i = 0; i < starters.transform.childCount; i++)
        {
            // AddListener will take the reference.
            // If i is passed, all listeners will refer to i.
            // Declear a new variable for each listener.
            int index = i;

            starter_monster_button = starters.transform.GetChild(i).gameObject.GetComponent<Button>();
            starter_monster_button.onClick.AddListener(() => ChooseAMonster(index));
        }

        start_monster_list = new List<BaseMonsterInfo>();
    }

    // Close other options, and inform the player about the losing condition.
    public void ChooseAMonster(int _index)
    {
        chosen_index = _index;

        previous_position = starters.transform.GetChild(_index).gameObject.GetComponent<RectTransform>().localPosition;

        // Hide other monsters that are not chosen
        for(int i = 0; i < starters.transform.childCount; i++)
        {
            if (i != _index)
            {
                starters.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        // Move the chosen monster to the center
        starters.transform.GetChild(_index).gameObject.GetComponent<RectTransform>().localPosition = new Vector2(-50f, 0f);

        starters.transform.GetChild(_index).gameObject.GetComponent<Button>().enabled = false;

        DisplayMonsterInfo(_index + 1);

        // Show the monster's stats
        stats_box.SetActive(true);
        
        // Ask the player to confirm the choice
        confirmation_box.SetActive(true);
    }

    private void DisplayMonsterInfo(int _chosen_monster_index)
    {
        Text weakness_text;

        // Get information about the monster
        BaseMonsterInfo chosen_monster = ChooseMonster(_chosen_monster_index);

        // Modified the text objects to show stats
        stats_box.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Name: " + chosen_monster.MonsterName;
        stats_box.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Health: " + chosen_monster.Health.ToString();
        stats_box.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Attack: " + chosen_monster.Attack.ToString();
        stats_box.transform.GetChild(3).gameObject.GetComponent<Text>().text = "Defense: " + chosen_monster.Defense.ToString();
        stats_box.transform.GetChild(4).gameObject.GetComponent<Text>().text = "Speed: " + chosen_monster.Speed.ToString();

        weakness_text = stats_box.transform.GetChild(5).gameObject.GetComponent<Text>();
        weakness_text.text = "Weakness:";

        foreach (Attribute weakness in chosen_monster.MonsterWeakness)
        {
            weakness_text.text = weakness_text.text + "\r\n" + weakness.ToString();
        }

        stats_box.transform.GetChild(6).gameObject.GetComponent<Text>().text = "Talent:\r\n" + chosen_monster.Talent.TalentName + "\r\n(" + chosen_monster.Talent.TalentDescription + ")";
    }

    // Create default information for starting monsters
    private BaseMonsterInfo ChooseMonster(int _chosen_monster_index)
    {
        BaseMonsterInfo chosen_monster = start_monster_list.Find(monster => monster.EntryNumber == _chosen_monster_index);

        if (chosen_monster == null)
        {
            // The number need to match the ordering in the game object, starters
            if (_chosen_monster_index == 1)
            {
                chosen_monster = new GrassyInfo();
            }
            else if(_chosen_monster_index == 2)
            {
                chosen_monster = new FiressInfo();
            }
            else if (_chosen_monster_index == 3)
            {
                chosen_monster = new WatressInfo();
            }
            else if (_chosen_monster_index == 4)
            {
                chosen_monster = new EarthyInfo();
            }
            else if (_chosen_monster_index == 5)
            {
                chosen_monster = new WinceInfo();
            }
            else
            {
                chosen_monster = new BaseMonsterInfo();
            }

            start_monster_list.Add(chosen_monster);
        }

        final_chosen_monster = chosen_monster;

        return (chosen_monster);
    }

    // Reset all game objects
    public void RegretChoice()
    {
        // Put the chosen monster to the original position
        starters.transform.GetChild(chosen_index).gameObject.GetComponent<RectTransform>().localPosition = previous_position;

        starters.transform.GetChild(chosen_index).gameObject.GetComponent<Button>().enabled = true;

        // Show all the monsters again
        for (int i = 0; i < starters.transform.childCount; i++)
        {
            starters.transform.GetChild(i).gameObject.SetActive(true);
        }

        // Show the monster's stats
        stats_box.SetActive(false);

        // Hide the question box
        confirmation_box.SetActive(false);
    }

    // Show the notice box, and hide everything else
    public void ConfirmChoice()
    {
        notice_box.SetActive(true);

        starters.SetActive(false);
        stats_box.SetActive(false);
        confirmation_box.SetActive(false);
    }

    public void GameBegin()
    {
        //data_storage.GetComponent<PlayerManagement>().SetLinkedMonster(final_chosen_monster);
        SceneManager.LoadScene("Battle");
    }
}
