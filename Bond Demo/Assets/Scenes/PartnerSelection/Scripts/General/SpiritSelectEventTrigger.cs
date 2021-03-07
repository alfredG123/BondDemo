using UnityEngine;
using UnityEngine.EventSystems;

public class SpiritSelectEventTrigger : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private PartnerSelectionDisplay _DisplayHandler = null;
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
