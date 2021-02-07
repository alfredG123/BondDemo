using UnityEngine;

public class AssetsLoader : MonoBehaviour
{
    [SerializeField] private Transform _TextPopUpObject = null;

    public static AssetsLoader Assets { get; private set; }
    public Transform TextPopUpObject { get => _TextPopUpObject; }

    private void Awake()
    {
        Assets = this;
    }
}
