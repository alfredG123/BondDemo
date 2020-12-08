using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonsterObject : MonoBehaviour
{
    protected BaseMonsterInfo monster_info;

    private void Awake()
    {
        monster_info = new BaseMonsterInfo();
    }

    private void Start()
    {
        Animator animator = GetComponent<Animator>();
        animator.speed = monster_info.Speed;
    }
}
