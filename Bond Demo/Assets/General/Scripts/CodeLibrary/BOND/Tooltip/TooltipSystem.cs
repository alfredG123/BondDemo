using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    [SerializeField] private Tooltip _Tooltip = null;

    private static TooltipSystem _Current;

    private void Awake()
    {
        _Current = this;
    }

    public static void ShowTooltip(string header_text, string content_text)
    {
        _Current._Tooltip.SetText(header_text, content_text);

        _Current._Tooltip.gameObject.SetActive(true);
    }

    public static void HideTooltip()
    {
        _Current._Tooltip.gameObject.SetActive(false);
    }
}
