using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiressObject : BaseMonsterObject
{
    private void Awake()
    {
        monster_info = new FiressInfo();
        SetAnimator();
        Debug.Log(3);
    }
}
