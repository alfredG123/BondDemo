using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StarterManagement : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private GameObject game_management;
    [SerializeField] private GameObject instruction_text;
    [SerializeField] private GameObject stats_details;
    [SerializeField] private RectTransform stats_details_rect_transform;
    [SerializeField] private GameObject confirmation_dialogue;

    [SerializeField] private SpiritsInLevel starter_spirits;
    [SerializeField] private GameObject start_spirit1;
    [SerializeField] private GameObject start_spirit2;
    [SerializeField] private GameObject start_spirit3;
#pragma warning restore 0649

    private GameObject _starter_spirit_object;
    private bool _is_spirit_choose = true;

    private void Start()
    {
        // Generate three random starting spirits
        SetUpStarterSpiritList();
    }

    #region Choosing A Starter

    private void Update()
    {
        // Listen the mouse click, if no spirit is selected
        if (_is_spirit_choose)
        {

            // Check if the left mouse is clicked
            if (Input.GetMouseButtonDown(0))
            {
                // Check collision at the mouse position
                _starter_spirit_object = General.GetGameObjectAtMousePosition();

                if (_starter_spirit_object != null)
                {
                    // Error handling (There should only be spirit objects in the scene)
                    if (_starter_spirit_object.GetComponent<SpiritPrefab>() == null)
                    {
                        General.ReturnToTitleSceneForErrors("StarterManagement.Update", "starter_spirit does not have SpiritPrefab script.");
                    }

                    // Display the relation information and confirmation
                    ChooseSpirit();
                }
            }

            // Check if the number key is clicked
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // Set the left spirit as the starter
                _starter_spirit_object = start_spirit1;

                // Display the relation information and confirmation
                ChooseSpirit();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                // Set the center spirit as the starter
                _starter_spirit_object = start_spirit2;

                // Display the relation information and confirmation
                ChooseSpirit();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                // Set the right spirit as the starter
                _starter_spirit_object = start_spirit3;

                // Display the relation information and confirmation
                ChooseSpirit();
            }
        }
        else
        {
            // Check if the key is clicked
            if (Input.GetKeyDown(KeyCode.Return))
            {
                // Load the next scene
                ConfirmChoice();
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                // Show the UI and reset global
                RegretChoice();
            }
        }
    }


    // Handle the onclick event for reconsidering
    public void RegretChoice()
    {
        // Hide confirmation related UI
        HideStatsAndConfirmation();

        // Reset the global flag for enbling the selection
        _is_spirit_choose = true;
    }

    // Handle the onclick event for confirming
    public void ConfirmChoice()
    {
        // Add the chosen spirit to the party
        game_management.GetComponent<PlayerManagement>().AddSpiritToParty(_starter_spirit_object.GetComponent<SpiritPrefab>().Spirit);

        // Load the main scene 
        General.LoadScene(TypeScene.Main);
    }

    // Randomly pick three starters
    private void SetUpStarterSpiritList()
    {
        int rand;
        int starter_index;
        List<int> temp_index_list = new List<int>();
        List<int> starter_spirits_index_list = new List<int>();

        // Initialize a temporary list to hold the spirit index
        for (int i = 0; i < starter_spirits.NumberOfSpirits; i++)
        {
            temp_index_list.Add(i);
        }

        // Pick three indexes from the temporary list
        while (starter_spirits_index_list.Count < 3)
        {
            rand = Random.Range(0, temp_index_list.Count);

            starter_index = temp_index_list[rand];

            starter_spirits_index_list.Add(starter_index);

            temp_index_list.RemoveAt(rand);
        }


        // initialize a spirit object for each prefab


        Spirit spirit = new Spirit(starter_spirits.GetSpiritData(starter_spirits_index_list[0]));
        start_spirit1.GetComponent<SpiritPrefab>().SetSpirit(spirit);

        spirit = new Spirit(starter_spirits.GetSpiritData(starter_spirits_index_list[1]));
        start_spirit2.GetComponent<SpiritPrefab>().SetSpirit(spirit);

        spirit = new Spirit(starter_spirits.GetSpiritData(starter_spirits_index_list[2]));
        start_spirit3.GetComponent<SpiritPrefab>().SetSpirit(spirit);
    }

    // Display UI for selecting a spirit
    private void ChooseSpirit()
    {
        // Show the UI that contains information about the chosen spirit
        DisplaySpiritInfo();

        // Show buttons for confirming or reconsidering
        DisplayConfirmation();

        _is_spirit_choose = false;
    }

    // Display UI for showing related information about the chosen spirit
    private void DisplaySpiritInfo()
    {
        // Place the UI at the pre-defined location that is specified by the first child of the spirit prefab object
        Vector3 position = General.ConvertWorldToScreenPosition(_starter_spirit_object.transform.GetChild(0).transform.position);

        stats_details.transform.position = position;

        // Calculate the pivot to fit the UI in the screen
        float pivot_x = position.x / Screen.width;
        float pivot_y = position.y / Screen.height;

        stats_details_rect_transform.pivot = new Vector2(pivot_x, pivot_y);


        // Set the text in the UI
        
        
        Spirit starter_spirit = _starter_spirit_object.GetComponent<SpiritPrefab>().Spirit;

        // Modified the text objects to show stats
        stats_details.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Name: " + starter_spirit.SpiritName;
        stats_details.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Health: " + starter_spirit.HealthText;
        stats_details.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Attack: " + starter_spirit.AttackText;
        stats_details.transform.GetChild(3).gameObject.GetComponent<Text>().text = "Defense: " + starter_spirit.DefenseText;
        stats_details.transform.GetChild(4).gameObject.GetComponent<Text>().text = "Speed: " + starter_spirit.SpeedText;

        //display skill

        // Display the UI
        stats_details.SetActive(true);
    }

    // Activate UI that are related to the confirmation
    private void DisplayConfirmation()
    {
        instruction_text.SetActive(false);

        confirmation_dialogue.SetActive(true);
    }

    // Deactivate UI that are related to the confirmation
    private void HideStatsAndConfirmation()
    {
        stats_details.SetActive(false);

        confirmation_dialogue.SetActive(false);

        instruction_text.SetActive(true);
    }

    #endregion
}