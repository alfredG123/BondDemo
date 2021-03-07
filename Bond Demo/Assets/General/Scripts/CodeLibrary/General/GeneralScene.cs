using UnityEngine.SceneManagement;

public static class GeneralScene
{
    /// <summary>
    /// Load the specified scene
    /// </summary>
    /// <param name="scene_to_load"></param>
    public static void LoadScene(Scene scene_to_load)
    {
        SceneManager.LoadScene((int)scene_to_load);
    }
}
