using UnityEngine.SceneManagement;

public static class GeneralScene
{
    /// <summary>
    /// Enumeration for defining each scene in the game
    /// This enum needs to match build setting
    /// </summary>
    public enum Scene
    {
        Title = 0,
        PartnerSelection = 1,
        Main = 2
    }

    /// <summary>
    /// Return the current scene
    /// </summary>
    /// <returns></returns>
    public static Scene GetCurrentScene()
    {
        return ((Scene)SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Load the specified scene
    /// </summary>
    /// <param name="scene_to_load"></param>
    public static void LoadScene(Scene scene_to_load)
    {
        SceneManager.LoadScene((int)scene_to_load);
    }
}
