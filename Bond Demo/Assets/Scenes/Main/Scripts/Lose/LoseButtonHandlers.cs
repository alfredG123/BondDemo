
using UnityEngine;

public class LoseButtonHandlers : MonoBehaviour
{
    public void ReturnToTitle()
    {
        GeneralScene.LoadScene(GeneralScene.Scene.Title);
    }

    public void ReturnToSelection()
    {
        GeneralScene.LoadScene(GeneralScene.Scene.PartnerSelection);
    }
}
