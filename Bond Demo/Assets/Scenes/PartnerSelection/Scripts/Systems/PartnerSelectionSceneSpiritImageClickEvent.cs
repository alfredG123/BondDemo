﻿using UnityEngine;
using UnityEngine.EventSystems;

public class PartnerSelectionSceneSpiritImageClickEvent : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private PartnerSelectionSceneDisplayHandler _DisplayHandler = null;
    [SerializeField] private int _SpiritIndex = 0;

    /// <summary>
    /// Call display class to show the detail information about the selected spirit
    /// </summary>
    /// <param name="event_data"></param>
    public void OnPointerClick(PointerEventData event_data)
    {
        _DisplayHandler.DisplaySelectedSpirit(_SpiritIndex);
    }
}