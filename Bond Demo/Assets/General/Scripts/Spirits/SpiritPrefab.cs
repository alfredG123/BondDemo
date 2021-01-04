using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiritPrefab : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private Image spirit_image;
    [SerializeField] private Slider health_bar_slider;
    [SerializeField] private Slider stamina_bar_slider;
#pragma warning restore 0649

    private Spirit _spirit;
    private float _max_health;
    private float _max_stamina;
    private float _current_health;
    private float _current_stamina;
    private GameObject _move_buttons;

    public Spirit Spirit
    {
        get => (_spirit);
    }

    public void SetSpirit(Spirit spirit_to_set)
    {
        _spirit = spirit_to_set;

        SetStatus(spirit_to_set);

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

        SetStatus(spirit_to_set);

        GetComponent<Animator>().SetBool("IsAlly", is_ally);
    }

    private void SetStatus(Spirit spirit_to_set)
    {
        spirit_image.sprite = Spirit.SpiritSprite;

        _current_health = spirit_to_set.CurrentHealth;
        _current_stamina = spirit_to_set.Stamina;

        _max_health = spirit_to_set.MaxHealth;
        _max_stamina = spirit_to_set.Stamina;
    }

    public void CalculateStatus()
    {

    }

    public void SetSkills(GameObject move_buttons)
    {
        _move_buttons = move_buttons;

        for (int i = 0; i < 4; i++)
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
        _current_stamina -= skill.StaminaCost;

        stamina_bar_slider.value = (float)_current_stamina / (float)Spirit.Stamina;
    }

    public void TakeSkill(SpiritSkill skill)
    {
        _current_health -= skill.SkillPower;

        health_bar_slider.value = (float)_current_health / (float)Spirit.MaxHealth;
    }

    public void PlayAttackAnimation()
    {
        GetComponent<Animator>().SetTrigger("Attack");
    }
}
