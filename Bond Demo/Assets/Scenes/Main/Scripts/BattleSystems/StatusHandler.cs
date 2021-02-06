using UnityEngine;

public class StatusHandler : MonoBehaviour
{
    private GameObject _HealthObject = null;
    private GameObject _EnergyObject = null;

    private float _MaxHealth = 0;
    private float _MaxEnergy = 0;

    public void InitializeStatus(Spirit spirit)
    {
        _MaxHealth = spirit.MaxHealth;
        _MaxEnergy = spirit.MaxEnergy;

        _HealthObject = transform.GetChild(0).GetChild(0).gameObject;
        SetHealth(spirit.CurrentHealth);

        _EnergyObject = transform.GetChild(1).GetChild(0).gameObject;
        SetEnergy(spirit.CurrentEnergy);
    }

    public void SetHealth(float current_health)
    {
        _HealthObject.transform.localScale = new Vector2(current_health / _MaxHealth, 1);
    }

    public void SetEnergy(float current_energy)
    {
        _EnergyObject.transform.localScale = new Vector2(current_energy / _MaxEnergy, 1);
    }
}
