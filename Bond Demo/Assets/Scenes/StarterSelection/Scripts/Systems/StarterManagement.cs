using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StarterManagement : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private GameObject _game_management;
    [SerializeField] private GameObject _instruction_text;
    [SerializeField] private GameObject _stats_details;
    [SerializeField] private GameObject _confirmation_dialogue;

    [SerializeField] private SpiritInLevel _starter_spirits;
    [SerializeField] private GameObject _start_spirit1;
    [SerializeField] private GameObject _start_spirit2;
    [SerializeField] private GameObject _start_spirit3;
#pragma warning restore 0649

    private GameObject _starter_spirit_object;
    private bool _is_deciding = true;
    private int _chosen_spirit_index = 0;

    private void Start()
    {
        SetUpStarterSpiritList();
    }

    #region Choosing A Starter

    private void Update()
    {
        if (_is_deciding)
        {
            _starter_spirit_object = GeneralScripts.GetGameObjectByMouse();
                    
            if (_starter_spirit_object != null)
            {
                if (_starter_spirit_object.GetComponent<SpiritPrefab>() == null)
                {
                    GeneralScripts.ReturnToTitleSceneForErrors("StarterManagement.Update", "starter_spirit does not have SpiritPrefab script.");
                }

                ChooseSpirit();
            }

            //DisplaySpiritInfo();

                    //_stats_details.SetActive(true);

                    //// Ask the player to confirm the choice
                    //_confirmation_dialogue.SetActive(true);

                    //is_deciding = false;
        }
    }

    #region Button Handlers

    // Reset all game objects
    public void RegretChoice()
    {
        _stats_details.SetActive(false);

        // Hide the question box
        _confirmation_dialogue.SetActive(false);

        //
        _is_deciding = true;
    }

    // Show the notice box, and hide everything else
    public void ConfirmChoice()
    {
        _stats_details.SetActive(false);
        _confirmation_dialogue.SetActive(false);

        GameBegin();
    }

    #endregion

    private void SetUpStarterSpiritList()
    {
        int rand = 0;
        int starter_index = 0;
        List<int> temp_index_list = new List<int>();
        List<int> starter_spirits_index_list = new List<int>();

        for (int i = 0; i < _starter_spirits.NumberOfSpirits; i++)
        {
            temp_index_list.Add(i);
        }

        while (starter_spirits_index_list.Count < 3)
        {
            rand = Random.Range(0, temp_index_list.Count);

            starter_index = temp_index_list[rand];

            starter_spirits_index_list.Add(starter_index);

            temp_index_list.RemoveAt(rand);
        }

        Spirit spirit = new Spirit(_starter_spirits.GetSpiritData(starter_spirits_index_list[0]));
        _start_spirit1.GetComponent<SpiritPrefab>().SetSpirit(spirit);

        spirit = new Spirit(_starter_spirits.GetSpiritData(starter_spirits_index_list[1]));
        _start_spirit2.GetComponent<SpiritPrefab>().SetSpirit(spirit);

        spirit = new Spirit(_starter_spirits.GetSpiritData(starter_spirits_index_list[2]));
        _start_spirit3.GetComponent<SpiritPrefab>().SetSpirit(spirit);
    }

    private void ChooseSpirit()
    {

    }

    private void DisplaySpiritInfo()
    {
        _stats_details.SetActive(true);

        Spirit starter_spirit = _starter_spirit_object.GetComponent<SpiritPrefab>().Spirit;

        // Modified the text objects to show stats
        _stats_details.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Name: " + starter_spirit.SpiritName;
        _stats_details.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Health: " + starter_spirit.HealthText;
        _stats_details.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Attack: " + starter_spirit.AttackText;
        _stats_details.transform.GetChild(3).gameObject.GetComponent<Text>().text = "Defense: " + starter_spirit.DefenseText;
        _stats_details.transform.GetChild(4).gameObject.GetComponent<Text>().text = "Speed: " + starter_spirit.SpeedText;

        //display skill
    }

    #endregion

    #region Entering battle

    public void GameBegin()
    {
        _game_management.GetComponent<PlayerManagement>().AddSpiritToParty(_starter_spirit_object.GetComponent<SpiritPrefab>().Spirit);
        SceneManager.LoadScene("Battle");
    }

    #endregion
}