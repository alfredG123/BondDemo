
using UnityEngine;

public class LoseButtonHandlers : MonoBehaviour
{
    public void ReturnToTitle()
    {
        GeneralScene.LoadScene(Scene.Title);
    }

    public void ReturnToSelection()
    {
        GeneralScene.LoadScene(Scene.PartnerSelection);
    }
}
