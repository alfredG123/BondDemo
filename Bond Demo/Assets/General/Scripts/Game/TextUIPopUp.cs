using TMPro;
using UnityEngine;


public class TextUIPopUp : MonoBehaviour
{
    private TextMeshProUGUI _TextMesh;
    private readonly float _MoveSpeedInY = 30f;
    private float _TimerFoFadingBegin = .5f;
    private readonly float _FadingSpeed = 3f;
    private Color _TextColor;

    public static TextUIPopUp CreateTextPopUp(string text_to_pop, Vector2 position, Color text_color, Canvas canvas)
    {
        Transform pop_up_text = AssetsLoader.Assets.LoadTransform("PopupTextUI", LoadEnum.Text);

        Transform text_pop_up_transform = Instantiate(pop_up_text, GeneralInput.ConvertWorldToScreenPosition(position), Quaternion.identity);

        text_pop_up_transform.SetParent(canvas.transform);

        float pivot_x = text_pop_up_transform.position.x / Screen.width;
        float pivot_y = text_pop_up_transform.position.y / Screen.height;

        text_pop_up_transform.gameObject.GetComponent<RectTransform>().pivot = new Vector2(pivot_x, pivot_y);

        TextUIPopUp text_pop_up = text_pop_up_transform.GetComponent<TextUIPopUp>();

        text_pop_up.SetText(text_to_pop, text_color);

        return (text_pop_up);
    }

    private void Awake()
    {
        _TextMesh = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        transform.position += new Vector3(0, _MoveSpeedInY) * Time.deltaTime;

        float pivot_x = transform.position.x / Screen.width;
        float pivot_y = transform.position.y / Screen.height;

        GetComponent<RectTransform>().pivot = new Vector2(pivot_x, pivot_y);

        _TimerFoFadingBegin -= Time.deltaTime;
        if (_TimerFoFadingBegin < 0)
        {
            _TextColor.a -= _FadingSpeed * Time.deltaTime;
            _TextMesh.color = _TextColor;

            if (_TextColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void SetText(string text_to_set, Color text_color)
    {
        _TextMesh.text = text_to_set;
        _TextMesh.color = text_color;
        _TextColor = _TextMesh.color;
    }
}
