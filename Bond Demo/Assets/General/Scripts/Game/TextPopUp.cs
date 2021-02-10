using TMPro;
using UnityEngine;

public class TextPopUp : MonoBehaviour
{
    private TextMeshPro _TextMesh;
    private float _MoveSpeedInY = 5f;
    private float _TimerFoFadingBegin = .5f;
    private float _FadingSpeed = 3f;
    private Color _TextColor;
    public static TextPopUp CreateTextPopUp(string text_to_pop, Vector2 position, Color text_color)
    {
        Transform text_pop_up_transform = Instantiate(AssetsLoader.Assets.TextPopUpObject, position, Quaternion.identity);

        TextPopUp text_pop_up = text_pop_up_transform.GetComponent<TextPopUp>();

        text_pop_up.SetText(text_to_pop, text_color);

        return (text_pop_up);
    }

    private void Awake()
    {
        _TextMesh = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        Vector2 position = transform.position + new Vector3(0, _MoveSpeedInY) * Time.deltaTime;

        float pivot_x = position.x / Screen.width;
        float pivot_y = position.y / Screen.height;

        GetComponent<RectTransform>().pivot = new Vector2(pivot_x, pivot_y);

        transform.position = position;

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
        _TextMesh.SetText(text_to_set);
        _TextMesh.color = text_color;
        _TextColor = _TextMesh.color;
    }
}
