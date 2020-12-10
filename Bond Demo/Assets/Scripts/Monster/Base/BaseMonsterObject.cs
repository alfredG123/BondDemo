using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonsterObject : MonoBehaviour
{
    protected BaseMonsterInfo monster_info;
    protected BattleState battle_state;
    Animator animator;

    public BaseMonsterInfo MonsterInfo
    {
        get { return (monster_info); }
    }

    private void Awake()
    {
        monster_info = new BaseMonsterInfo();
        SetAnimator();
        Debug.Log(1);
    }

    protected void SetAnimator()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsAlly", monster_info.IsAlly);
    }

    // Passive
    public void ReadyToMove()
    {
        battle_state = BattleState.Ready;
    }

    public bool PlanMove()
    {
        battle_state = BattleState.PlanMove;
        
        if (monster_info.IsAlly)
        {
            return (false);
        }
        else
        {
            return (true);
        }
    }

    public void PlayerMakeMove(int move_index)
    {
        battle_state = BattleState.Battle;

        if (move_index == 0)
        {
            PerformNormalAttack();
        }
    }

    public void EnemyMakeMove()
    {
        battle_state = BattleState.Battle;

        PerformNormalAttack();

        StartCoroutine("Wait");
    }

    public void PostMoveSet()
    {
        battle_state = BattleState.Postbattle;

        //GameObject.Find("BattleManager").GetComponent<BattleManagement>().StartBattle();
    }

    private void PerformNormalAttack()
    {
        animator.SetTrigger("AttackTrigger");
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        PostMoveSet();
    }
}
