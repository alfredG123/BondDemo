
using UnityEngine;

public class LoseButtonHandlers : MonoBehaviour
{
    public void ReturnToTitle()
    {
        General.LoadScene(TypeScene.Title);
    }

    public void ReturnToSelection()
    {
        General.LoadScene(TypeScene.PartnerSelection);
    }
}
