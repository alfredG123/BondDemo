using UnityEngine;
using UnityEngine.UI;

public class SpiritPrefab : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private GameObject status;
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

    private SpiritMove move_to_perform;
    private GameObject target_to_aim;

    public Spirit Spirit
    {
        get => _spirit;
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

        status.SetActive(true);
    }

    public int CalculateDamage(SpiritMove move)
    {
        int damage;

        damage = Mathf.CeilToInt(((float)move.MovePower / 100) * Spirit.Attack);

        return (damage);
    }

    public void SetTarget(GameObject target)
    {
        target_to_aim = target;
    }

    public GameObject GetTarget()
    {
        if (!Spirit.IsAlly)
        {
            target_to_aim = GetComponent<EnemyBattleLogic>().GetTarget();
        }

        return (target_to_aim);
    }

    public void SetMove(TypeSelectedMove move_type)
    {
        if (move_type == TypeSelectedMove.Move1)
        {
            move_to_perform = Spirit.MoveSet[0];
        }
        else if (move_type == TypeSelectedMove.Move2)
        {
            move_to_perform = Spirit.MoveSet[1];
        }
        else if (move_type == TypeSelectedMove.Move3)
        {
            move_to_perform = Spirit.MoveSet[2];
        }
        else if (move_type == TypeSelectedMove.Defend)
        {

        }
    }

    public SpiritMove GetMove()
    {
        if (!Spirit.IsAlly)
        {
            move_to_perform = GetComponent<EnemyBattleLogic>().GetMove();
        }

        return (move_to_perform);
    }

    public bool PerformMove(SpiritMove move)
    {
        bool move_is_perform = false;

        if (_current_stamina >= move.MoveStaminaCost)
        {
            _current_stamina -= move.MoveStaminaCost;

            stamina_bar_slider.value = _current_stamina / _max_stamina;

            stamina_bar_text.text = _current_stamina + "/" + _max_stamina;

            move_is_perform = true;

            PlayAttackAnimation();
        }

        return (move_is_perform);
    }

    public bool TakeMove(int move_damage)
    {
        bool spirit_faint = false;

        _current_health -= move_damage;

        PlayHitAnimation();

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

    public void PlayHitAnimation()
    {
        GetComponent<Animator>().SetTrigger("GetHit");
    }
}
