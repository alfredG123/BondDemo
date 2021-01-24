using UnityEngine;
using UnityEngine.UI;

public class SpiritPrefab : MonoBehaviour
{
    /*
    private Spirit _spirit;
    private readonly float _max_health;
    private readonly float _max_stamina;
    private float _current_health;
    private float _current_stamina;

    private bool _in_defense_state = false;

    private SpiritMove move_to_perform;
    private GameObject target_to_aim;

    public Spirit Spirit
    {
        get => _spirit;
    }

    public bool InDefenseState
    {
        get => _in_defense_state;
    }

    public void SetSpirit(Spirit spirit_to_set)
    {
        _spirit = spirit_to_set;

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
        _in_defense_state = false;

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
            _in_defense_state = true;
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

            move_is_perform = true;
        }

        return (move_is_perform);
    }

    public bool TakeMove(Spirit attacker, SpiritMove move_to_take, BattleDisplayHandler battle_display_handler)
    {
        bool spirit_faint = false;

        int damage = Mathf.CeilToInt(((float)move_to_take.MovePower / 100) * attacker.Attack);

        int random = Random.Range(0, 100);
        TypeEffectiveness effectiveness_type = TypeEffectiveness.Effective;
        bool critical_hit = false;

        if (Spirit.Weakness.Contains(move_to_take.MoveAttributeType))
        {
            damage *= 2;

            effectiveness_type = TypeEffectiveness.SuperEffective;
        }
        else if (Spirit.Resistance.Contains(move_to_take.MoveAttributeType))
        {
            damage /= 2;

            effectiveness_type = TypeEffectiveness.NotEffective;
        }

        if (random < 10)
        {
            damage *= 2;

            critical_hit = true;
        }

        if (_in_defense_state)
        {
            damage /= 4;
        }

        battle_display_handler.DisplayBattleNarrativeFoEffectiveness(critical_hit, effectiveness_type);

        _current_health -= damage;

        if (_current_health <= 0)
        {
            _current_health = 0;

            spirit_faint = true;
        }

        return (spirit_faint);
    }

    public void RestoreStamina()
    {
        if (!this.gameObject.activeSelf)
        {
            return;
        }

        if (_current_stamina < _max_stamina)
        {
            _current_stamina += 10;
        }

        if (_current_stamina > _max_stamina)
        {
            _current_stamina = _max_stamina;
        }
    }
    */
}
