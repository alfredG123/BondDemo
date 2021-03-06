﻿using UnityEngine;
using System.Collections.Generic;

public class SpiritPrefab : MonoBehaviour
{
    [SerializeField] private StatusHandler _StatusBar = null;

    private TypeLastingStatusEffect lasting_status = TypeLastingStatusEffect.None;

    private readonly List<TypeTemporaryStatusEffect> temporary_status = new List<TypeTemporaryStatusEffect>();

    public Spirit Spirit { get; set; }
    public BaseMove MoveToPerform { get; private set; }
    public GameObject TargetToAim { get; private set; }

    public void InitializeSpirit()
    {
        Spirit.CurrentEnergy = 0;
    }

    public void SetMove(TypeSelectedMove move_type)
    {
        if (move_type == TypeSelectedMove.Move1)
        {
            MoveToPerform = Spirit.MoveSet[0];
        }
        else if (move_type == TypeSelectedMove.Move2)
        {
            MoveToPerform = Spirit.MoveSet[1];
        }
        else if (move_type == TypeSelectedMove.Move3)
        {
            MoveToPerform = Spirit.MoveSet[2];
        }
        else if (move_type == TypeSelectedMove.Attack)
        {
            MoveToPerform = Spirit.BasicAttack;
        }
        else if (move_type == TypeSelectedMove.Defend)
        {
            MoveToPerform = Spirit.BasicDefend;
        }
    }

    public void SetTargetToAim(GameObject target)
    {
        TargetToAim = target;
    }

    public void PerformMove(BaseMove move)
    {
        if (move is BasicAttackMove basic_attack_move_to_perform)
        {
           Spirit.CurrentEnergy  += basic_attack_move_to_perform.EnergyGain;
        
            if (Spirit.CurrentEnergy > Spirit.MaxEnergy)
            {
                Spirit.CurrentEnergy = Spirit.MaxEnergy;
            }
        }
        if (move is EnergyAttackMove energy_attack_to_perform)
        {
            Spirit.CurrentEnergy -= energy_attack_to_perform.EnergyCost;
        }
        else if (move is StatusMove status_move_to_perform)
        {
            Spirit.CurrentEnergy -= status_move_to_perform.EnergyCost;
        }

        UpdateEnergy(Spirit.CurrentEnergy);

        PopMove(move);
    }

    public bool TakeMove(Spirit attacker, BaseMove move_to_take)
    {
        bool is_spirit_faint = false;
        float damage = 0;
        int  final_damage;
        float random;
        bool is_critical_hit = false;
        TypeAttribute move_attribute = TypeAttribute.Normal;
        TypeEffectiveness effectiveness_type = TypeEffectiveness.Effective;

        if ((move_to_take is StatusMove) && (MoveToPerform is BasicDefendMove))
        {
            TextObjectPopUp.CreateTextPopUp(Spirit.Name + " protects itself from the move", transform.GetChild(1).transform.position, Color.white);

            return (is_spirit_faint);
        }

        if (!CheckAttackHit(attacker, move_to_take))
        {
            TextObjectPopUp.CreateTextPopUp(move_to_take.Name + " miss!", transform.GetChild(1).transform.position, Color.white);

            return (is_spirit_faint);
        }

        if (move_to_take is StatusMove status_move)
        {
            if (status_move.TemporaryStatusEffectType != TypeTemporaryStatusEffect.None)
            {
                temporary_status.Add(status_move.TemporaryStatusEffectType);

                if (status_move.TemporaryStatusEffectType == TypeTemporaryStatusEffect.DamageDownSmall)
                {
                    Spirit.Attack *= .8f;
                }
            }

            if (status_move.LastingStatusEffectType != TypeLastingStatusEffect.None)
            {
                if (lasting_status == TypeLastingStatusEffect.None)
                {
                    lasting_status = status_move.LastingStatusEffectType;
                }
                else
                {
                    TextObjectPopUp.CreateTextPopUp(status_move.Name + " fails", transform.GetChild(1).transform.position, Color.white);
                }
            }

            return (is_spirit_faint);
        }

        if (move_to_take is BasicAttackMove basic_attack_move_to_perform)
        {
            damage = basic_attack_move_to_perform.Power * attacker.Attack;

            move_attribute = basic_attack_move_to_perform.Attribute;
        }
        if (move_to_take is EnergyAttackMove energy_attack_to_perform)
        {
            damage = energy_attack_to_perform.Power * attacker.Attack;

            move_attribute = energy_attack_to_perform.Attribute;
        }

        if (Spirit.Weakness.Contains(move_attribute))
        {
            effectiveness_type = TypeEffectiveness.SuperEffective;

            damage *= 2;
        }
        else if (Spirit.Resistance.Contains(move_attribute))
        {
            effectiveness_type = TypeEffectiveness.NotEffective;

            damage /= 2;
        }
        else if (Spirit.Negation.Contains(move_attribute))
        {
            PopDamage(0, effectiveness_type, false);

            return (is_spirit_faint);
        }

        random = Random.Range(0f, 1f);
        if (random < attacker.CriticalChance)
        {
            is_critical_hit = true;

            damage *= attacker.CriticalModifier;
        }

        if (MoveToPerform is BasicDefendMove basic_defend_move)
        {
            damage *= (1 - basic_defend_move.DamageReducation);
        }

        random = Random.Range(.8f, 1.2f);

        damage *= random;

        final_damage = Mathf.CeilToInt(damage);

        PopDamage(final_damage, effectiveness_type, is_critical_hit);

        Spirit.CurrentHealth -= final_damage;

        if (Spirit.CurrentHealth <= 0)
        {
            Spirit.CurrentHealth = 0;

            is_spirit_faint = true;
        }

        UpdateHealth(Spirit.CurrentHealth);

        return (is_spirit_faint);
    }

    public void TakeBuff(BaseMove move_to_take)
    {
        if (move_to_take is StatusMove status_move)
        {
            if (status_move.TemporaryStatusEffectType != TypeTemporaryStatusEffect.None)
            {
                temporary_status.Add(status_move.TemporaryStatusEffectType);
            
                if (status_move.TemporaryStatusEffectType == TypeTemporaryStatusEffect.DamageUpSmall)
                {
                    Spirit.Attack *= 1.2f;
                }
            }

            if (status_move.LastingStatusEffectType != TypeLastingStatusEffect.None)
            {
                lasting_status = status_move.LastingStatusEffectType;
            }
        }
        else
        {
            //GeneralError.CheckIfWrongType(move_to_take, StatusMove, "TakeBuff");
        }
    }

    public bool CheckAttackHit(Spirit attacker, BaseMove move_to_take)
    {
        bool is_attack_hit = false;
        float random_number;
        float hit_probability;
        float move_probability = 0f;

        if (move_to_take is BasicAttackMove basic_attack_move_to_perform)
        {
            move_probability = basic_attack_move_to_perform.Accuracy;
        }
        else if (move_to_take is EnergyAttackMove energy_attack_to_perform)
        {
            move_probability = energy_attack_to_perform.Accuracy;
        }
        else if (move_to_take is StatusMove status_move_to_perform)
        {
            move_probability = status_move_to_perform.Accuracy;
        }

        hit_probability = move_probability * attacker.Accuracy * (1 - Spirit.Evasion);

        random_number = Random.Range(0f, 1f);

        if (random_number < hit_probability)
        {
            is_attack_hit = true;
        }

        return (is_attack_hit);
    }

    public void UpdateHealth(float current_health)
    {
        _StatusBar.SetHealth(current_health);
    }

    public void UpdateEnergy(float current_energy)
    {
        _StatusBar.SetEnergy(current_energy);
    }

    public void InitializeStatus()
    {
        _StatusBar.InitializeStatus(Spirit);
    }

    private void PopMove(BaseMove move)
    {
        string text_to_set = move.Name;
        Color text_color = Color.cyan;

        TextObjectPopUp.CreateTextPopUp(text_to_set, transform.GetChild(0).transform.position, text_color);
    }

    private void PopDamage(int damage, TypeEffectiveness effectiveness, bool is_critical_hit)
    {
        string text_to_set = damage.ToString();
        Color text_color = Color.cyan;

        if (effectiveness == TypeEffectiveness.SuperEffective)
        {
            text_color = Color.red;
        }
        else if (effectiveness == TypeEffectiveness.NotEffective)
        {
            text_color = Color.white;
        }
        else if (effectiveness == TypeEffectiveness.NoEffect)
        {
            text_to_set = "No Effect";

            text_color = Color.white;
        }

        if ((is_critical_hit) && (effectiveness != TypeEffectiveness.NoEffect))
        {
            text_to_set += "(CRITICAL!!!)";

            text_color = Color.yellow;
        }

        TextObjectPopUp.CreateTextPopUp(text_to_set, transform.GetChild(0).transform.position, text_color);
    }

    public void HideStatus()
    {
        _StatusBar.HideStatus();
    }

    private void PopDamage(int damage, TypeLastingStatusEffect status)
    {
        Color text_color = Color.white;

        if (status == TypeLastingStatusEffect.Posioned)
        {
            text_color = new Color(0.6320754f, 0f, 0.5557545f);
        }
        else if (status == TypeLastingStatusEffect.Burn)
        {
            text_color = Color.red;
        }

        TextObjectPopUp.CreateTextPopUp(damage.ToString(), transform.GetChild(1).transform.position, text_color);
    }

    public bool CheckEnergy(TypeSelectedMove move_type)
    {
        bool sufficient = true;
        BaseMove move;

        SetMove(move_type);

        move = MoveToPerform;

        if (move is EnergyAttackMove energy_attack_to_perform)
        {
            if (energy_attack_to_perform.EnergyCost > Spirit.CurrentEnergy)
            {
                sufficient = false;
            }
        }
        else if (move is StatusMove status_move_to_perform)
        {
            if (status_move_to_perform.EnergyCost > Spirit.CurrentEnergy)
            {
                sufficient = false;
            }
        }

        return (sufficient);
    }

    public void ClearTemporaryStatus()
    {
        foreach (TypeTemporaryStatusEffect temporary_effect in temporary_status)
        {
            if (temporary_effect == TypeTemporaryStatusEffect.DamageUpSmall)
            {
                Spirit.Attack /= 1.2f;
            }
            else if (temporary_effect == TypeTemporaryStatusEffect.DamageDownSmall)
            {
                Spirit.Attack /= .8f;
            }
        }
    }

    public bool TakeStatusDamage()
    {
        int final_damage = 0;
        bool is_spirit_faint = false;

        if (lasting_status != TypeLastingStatusEffect.None)
        {
            if (lasting_status == TypeLastingStatusEffect.Posioned)
            {
                final_damage = Mathf.CeilToInt(Spirit.MaxHealth * .2f);
            }
            else if (lasting_status == TypeLastingStatusEffect.Burn)
            {
                final_damage = Mathf.CeilToInt(Spirit.MaxHealth * .1f);
            }

            PopDamage(final_damage, lasting_status);

            Spirit.CurrentHealth -= final_damage;

            if (Spirit.CurrentHealth <= 0)
            {
                Spirit.CurrentHealth = 0;

                is_spirit_faint = true;
            }

            UpdateHealth(Spirit.CurrentHealth);
        }

        return (is_spirit_faint);
    }

    public bool TakeEnvironmentDamage(TypeField field_type)
    {
        int final_damage;
        bool is_spirit_faint = false;

        if (field_type == TypeField.Harmful)
        {
            final_damage = Mathf.CeilToInt(Spirit.MaxHealth * .1f);

            PopDamage(final_damage, TypeLastingStatusEffect.None);

            Spirit.CurrentHealth -= final_damage;

            if (Spirit.CurrentHealth <= 0)
            {
                Spirit.CurrentHealth = 0;

                is_spirit_faint = true;
            }

            UpdateHealth(Spirit.CurrentHealth);
        }

        return (is_spirit_faint);
    }

    //private void PopHeal(int heal_amount)
    //{
    //    TextPopUp.CreateTextPopUp(heal_amount.ToString(), transform.GetChild(1).transform.position, Color.green);
    //}

    /*
    private Spirit _spirit;
    private readonly float _max_health;
    private readonly float _max_energy;
    private float _current_health;
    private float _current_energy;

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

        if (_current_energy >= move.MoveEnergyCost)
        {
            _current_energy -= move.MoveEnergyCost;

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

    public void RestoreEnergy()
    {
        if (!this.gameObject.activeSelf)
        {
            return;
        }

        if (_current_energy < _max_energy)
        {
            _current_energy += 10;
        }

        if (_current_energy > _max_energy)
        {
            _current_energy = _max_energy;
        }
    }
    */
}
