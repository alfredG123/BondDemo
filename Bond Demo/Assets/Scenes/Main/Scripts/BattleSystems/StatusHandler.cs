using UnityEngine;
using UnityEngine.UI;

public class StatusHandler : MonoBehaviour
{
    private GameObject _NameObject = null;
    private GameObject _LevelObject = null;
    private GameObject _HealthObject = null;
    private GameObject _StaminaObject = null;

    public void InitializeStatus(Spirit spirit)
    {
        _NameObject = transform.GetChild(0).gameObject;
        _NameObject.GetComponent<Text>().text = spirit.Name;

        _LevelObject = transform.GetChild(1).gameObject;
        _LevelObject.GetComponent<Text>().text = "Lv." + spirit.Level.ToString();

        _HealthObject = transform.GetChild(2).gameObject;
        _HealthObject.GetComponent<Slider>().value = 1;

        _StaminaObject = transform.GetChild(3).gameObject;
        _StaminaObject.GetComponent<Slider>().value = 1;

        gameObject.SetActive(true);
    }
}
