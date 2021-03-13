﻿using UnityEngine;

public class TreasurePanelDisplayHandlers : MonoBehaviour
{
    [SerializeField] private GameObject _MessageText = null;

    public void DisplayTreasure()
    {
        int random_number_of_cystal = Random.Range(1, 101);

        PlayerInformation.AddItemToBag(Item.Cystal, random_number_of_cystal);

        GeneralComponent.SetText(_MessageText, "Gain Cystal x" + random_number_of_cystal);
    }
}
