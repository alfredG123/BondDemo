using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _Header = null;
    [SerializeField] private TextMeshProUGUI _Content = null;
    [SerializeField] private LayoutElement _LayoutElement = null;

    private readonly int _CharacterLimitPerLine = 50;

    private RectTransform _RectTransform;

    private void Awake()
    {
        _RectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector2 position = Input.mousePosition;

        float pivot_x = position.x / Screen.width;
        float pivot_y = position.y / Screen.height;

        _RectTransform.pivot = new Vector2(pivot_x, pivot_y);

        transform.position = position;
    }

    public void SetText(string header_text, string content_text)
    {
        int header_length;
        int content_length;

        if (string.IsNullOrEmpty(header_text))
        {
            _Header.gameObject.SetActive(false);
        }
        else
        {
            _Header.text = header_text;
            _Header.gameObject.SetActive(true);
        }

        _Content.text = content_text;

        header_length = _Header.text.Length;
        content_length = _Content.text.Length;

        _LayoutElement.enabled = ((header_length > _CharacterLimitPerLine) || (content_length > _CharacterLimitPerLine));
    }
}
