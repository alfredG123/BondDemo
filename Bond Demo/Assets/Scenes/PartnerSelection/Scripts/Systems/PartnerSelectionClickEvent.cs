using UnityEngine;
using UnityEngine.EventSystems;

public class PartnerSelectionClickEvent : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private PartnerSelectionUIDisplay _UIDisplay = null;
    [SerializeField] private int _spiritIndex = 0;

    /// <summary>
    /// Trigger click event to show the information about the selected spirit
    /// </summary>
    /// <param name="event_data"></param>
    public void OnPointerClick(PointerEventData event_data)
    {
        _UIDisplay.DisplaySelectedSpiritInfo(_spiritIndex);
    }
}
