using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthyObject : BaseMonsterObject
{
    private void Awake()
    {
        monster_info = new EarthyInfo();
        SetAnimator();
        Debug.Log(2);
    }
}
