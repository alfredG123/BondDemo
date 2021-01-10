using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Level", menuName ="BOND/Level/Spirits")]
public class SpiritsInLevel : ScriptableObject
{
#pragma warning disable 0649
    [SerializeField] private int GameLevel;
    [SerializeField] private List<BaseSpiritData> SpiritList = null;
#pragma warning restore 0649

    /// <summary>
    /// Get number of spirits in the current level
    /// </summary>
    public int NumberOfSpirits
    {
        get => (SpiritList.Count);
    }

    /// <summary>
    /// Get the spirit at the specific index in the list
    /// </summary>
    /// <param name="spirit_index"></param>
    /// <returns></returns>
    public BaseSpiritData GetSpiritData(int spirit_index)
    {
        return (SpiritList[spirit_index]);
    }

    /// <summary>
    /// Get the spirit at a random index in the list
    /// </summary>
    /// <returns></returns>
    public BaseSpiritData GetRandomSpiritData()
    {
        int random_index = Random.Range(0, SpiritList.Count);

        return (SpiritList[random_index]);
    }
}
