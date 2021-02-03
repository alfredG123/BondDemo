using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private string _HeaderText = "";
    private string _ContentText = "";

    public void SetTooltipText(string header_text, string content_text)
    {
        _HeaderText = header_text;

        _ContentText = content_text;
    }

    public void OnPointerEnter(PointerEventData event_data)
    {
        StartCoroutine(nameof(ShowTooltip));
    }

    public void OnPointerExit(PointerEventData event_data)
    {
        StopCoroutine(nameof(ShowTooltip));

        TooltipSystem.HideTooltip();
    }

    private IEnumerator ShowTooltip()
    {
        yield return new WaitForSeconds(.5f);


        TooltipSystem.ShowTooltip(_HeaderText, _ContentText);
    }
}
