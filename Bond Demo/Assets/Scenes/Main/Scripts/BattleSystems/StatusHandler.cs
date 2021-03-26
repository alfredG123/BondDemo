using UnityEngine;
using UnityEngine.UI;

public class StatusHandler : MonoBehaviour
{
    private Slider _HealthObject = null;
    private Slider _EnergyObject = null;

    private float _MaxHealth = 0;
    private float _MaxEnergy = 0;

    public void InitializeStatus(Spirit spirit)
    {
        _MaxHealth = spirit.MaxHealth;
        _MaxEnergy = spirit.MaxEnergy;

        _HealthObject = transform.GetChild(0).gameObject.GetComponent<Slider>();
        SetHealth(spirit.CurrentHealth);

        _EnergyObject = transform.GetChild(1).gameObject.GetComponent<Slider>();
        SetEnergy(spirit.CurrentEnergy);

        gameObject.Activate();
    }

    public void HideStatus()
    {
        gameObject.Deactivate();
    }

    public void SetHealth(float current_health)
    {
        _HealthObject.value = current_health / _MaxHealth;

        if (_HealthObject.value <= 0)
        {
            HideStatus();
        }
    }

    public void SetEnergy(float current_energy)
    {
        _EnergyObject.value = current_energy / _MaxEnergy;
    }
}
