using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleLogic : MonoBehaviour
{
    public void SetSpiritBattleInfo(GameObject target_party)
    {
        float random = Random.Range(0, 1f);
        GameObject target;

        if (random < .5f)
        {
            target = target_party.transform.GetChild(0).gameObject;

            if (!target.activeSelf)
            {
                target = target_party.transform.GetChild(1).gameObject;
            }

            if (!target.activeSelf)
            {
                target = target_party.transform.GetChild(2).gameObject;
            }

            GetComponent<SpiritPrefab>().SetMove(TypeSelectedMove.Attack);
            GetComponent<SpiritPrefab>().SetTargetToAim(target);
        }
        else
        {
            GetComponent<SpiritPrefab>().SetMove(TypeSelectedMove.Defend);
        }
    }
}
