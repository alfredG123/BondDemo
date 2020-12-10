using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPrefab : MonoBehaviour
{
    [SerializeField] public MonsterData monster_data = null; // need to be put back to private
    Animator animator;

    private void Awake()
    {
        SetAnimator();
    }

    protected void SetAnimator()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsAlly", monster_data.fight_with_player);
    }
}
