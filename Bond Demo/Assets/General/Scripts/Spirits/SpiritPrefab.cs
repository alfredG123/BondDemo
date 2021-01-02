using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiritPrefab : MonoBehaviour
{
    private Spirit _spirit;
    private int _current_health;
    private int _current_stamina;
    private GameObject _health_bar;
    private GameObject _stamina_bar;
    private GameObject _move_buttons;

    public Spirit Spirit
    {
        get => (_spirit);
    }

    public void SetSpirit(Spirit spirit_to_set)
    {
        _spirit = spirit_to_set;

        _current_health = spirit_to_set.Health;
        _current_stamina = spirit_to_set.Stamina;

        GetComponent<SpriteRenderer>().sprite = spirit_to_set.SpiritSprite;
    }

    public void SetSpirit(Spirit spirit_to_set, bool is_ally)
    {
        _spirit = spirit_to_set;

        // This code is temporary
        if (is_ally)
        {
            spirit_to_set.JoinParty();
        }

        GetComponent<SpriteRenderer>().sprite = spirit_to_set.SpiritSprite;

        _current_health = spirit_to_set.Health;
        _current_stamina = spirit_to_set.Stamina;

        GetComponent<Animator>().SetBool("IsAlly", is_ally);
    }

    public void SetStatus(GameObject status_object)
    {
        _health_bar = status_object.transform.GetChild(1).gameObject;
        _stamina_bar = status_object.transform.GetChild(2).gameObject;
    }

    public void SetSkills(GameObject move_buttons)
    {
        _move_buttons = move_buttons;

        for (int i = 0; i < move_buttons.transform.childCount - 1; i++)
        {
            if (i < Spirit.Skills.Count)
            {
                move_buttons.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = Spirit.Skills[i].SkillName;
            }
            else
            {
                move_buttons.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void PerformSkill(SpiritSkill skill)
    {
        _current_health -= skill.HealthCost;
        _current_stamina -= skill.StaminaCost;

        _health_bar.GetComponent<Slider>().value = (float)_current_health / (float)Spirit.Health;
        _stamina_bar.GetComponent<Slider>().value = (float)_current_stamina / (float)Spirit.Stamina;

        Debug.Log(_health_bar.GetComponent<Slider>().value);
    }

    public void TakeSkill(SpiritSkill skill)
    {
        _current_health -= skill.HealthCost;
        _current_stamina -= skill.StaminaCost;

        _health_bar.GetComponent<Slider>().value = (float)_current_health / (float)Spirit.Health;
        _stamina_bar.GetComponent<Slider>().value = (float)_current_stamina / (float)Spirit.Stamina;
    }

    public void PlayAttackAnimation()
    {
        GetComponent<Animator>().SetTrigger("Attack");
    }
}
