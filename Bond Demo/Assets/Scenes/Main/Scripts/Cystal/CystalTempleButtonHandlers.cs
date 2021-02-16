using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CystalTempleButtonHandlers : MonoBehaviour
{
    [SerializeField] private MainManagement _MainManagement = null;
    [SerializeField] GameObject _CystalTemple = null;

    public void BackToMap()
    {
        _CystalTemple.SetActive(false);

        _MainManagement.ShowMap();
    }
}
