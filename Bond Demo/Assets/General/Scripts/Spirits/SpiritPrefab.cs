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
    [SerializeField] private Text health_bar_text;
    [SerializeField] private Text stamina_bar_text;
#pragma warning restore 0649

    private Spirit _spirit;
    private float _max_health;
    private float _max_stamina;
    private float _current_health;
    private float _current_stamina;
    private GameObject _move_buttons;
    private TypeAction action_type;
    private SpiritSkill skill_to_perform;
    private GameObject target_to_aim;

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

        health_bar_text.text = _current_health + "/" + _max_health;
        stamina_bar_text.text = _current_stamina + "/" + _max_stamina;
    }

    public int CalculateDamage(SpiritSkill skill)
    {
        int damage;

        damage = Mathf.CeilToInt(((float)skill.SkillPower / 100) * Spirit.Attack);

        return (damage);
    }

    public void SetTarget(GameObject target)
    {
        target_to_aim = target;
    }

    public GameObject GetTarget()
    {
        return (target_to_aim);
    }

    public void SetAction(TypeAction action_type)
    {
        if (action_type == TypeAction.Move1)
        {
            skill_to_perform = Spirit.Skills[0];
        }
        else if (action_type == TypeAction.Move2)
        {
            skill_to_perform = Spirit.Skills[1];
        }
        else if (action_type == TypeAction.Move3)
        {
            skill_to_perform = Spirit.Skills[2];
        }
        else if (action_type == TypeAction.Move4)
        {
            skill_to_perform = Spirit.Skills[3];
        }
        else if (action_type == TypeAction.Defend)
        {

        }
    }

    public SpiritSkill GetSkill()
    {
        if (skill_to_perform == null)
        {
            skill_to_perform = Spirit.Skills[0];
        }

        return (skill_to_perform);
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

    public bool PerformSkill(SpiritSkill skill)
    {
        bool skill_is_perform = false;

        if (_current_stamina >= skill.StaminaCost)
        {
            _current_stamina -= skill.StaminaCost;

            stamina_bar_slider.value = _current_stamina / _max_stamina;

            stamina_bar_text.text = _current_stamina + "/" + _max_stamina;

            skill_is_perform = true;
        }

        return (skill_is_perform);
    }

    public bool TakeSkill(int skill_damage)
    {
        bool spirit_faint = false;

        _current_health -= skill_damage;

        if (_current_health > 0)
        {
            health_bar_slider.value = _current_health / _max_health;

            health_bar_text.text = _current_health + "/" + _max_health;
        }
        else
        {
            health_bar_slider.value = 0;

            spirit_faint = true;
        }

        return (spirit_faint);
    }

    public void PlayAttackAnimation()
    {
        GetComponent<Animator>().SetTrigger("Attack");
    }
}
