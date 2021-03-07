using UnityEngine;

public class SceneInitialization : MonoBehaviour
{
    /// <summary>
    /// Create a game information object when the scene is loaded
    /// </summary>
    private void Start()
    {
        GameInformation.CreateInstance();
    }
}
