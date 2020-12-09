using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassyObject : BaseMonsterObject
{
    private void Awake()
    {
        monster_info = new GrassyInfo();
        SetAnimator();
    }
}
