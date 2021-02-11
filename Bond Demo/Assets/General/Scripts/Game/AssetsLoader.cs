using UnityEngine;

public class AssetsLoader : MonoBehaviour
{
    [SerializeField] private Transform _TextPopUpObject = null;
    [SerializeField] private Transform _TextPopUpUI = null;

    [SerializeField] private GameObject _PlayerPrefab = null;
    [SerializeField] private GameObject _EnemeyPrefab = null;
    [SerializeField] private GameObject _TreasurePrefab = null;
    [SerializeField] private GameObject _RestPlacePrefab = null;
    [SerializeField] private GameObject _CystalTempleOnPrefab = null;

    public static AssetsLoader Assets { get; private set; }
    public Transform TextPopUpObject { get => _TextPopUpObject; }
    public Transform TextPopUpUI { get => _TextPopUpUI; }
    public GameObject PlayerPrefab { get => _PlayerPrefab; }
    public GameObject EnemeyPrefab { get => _EnemeyPrefab; }
    public GameObject TreasurePrefab { get => _TreasurePrefab; }
    public GameObject RestPlacePrefab { get => _RestPlacePrefab; }
    public GameObject CystalTempleOnPrefab { get => _CystalTempleOnPrefab; }

    private void Awake()
    {
        Assets = this;
    }
}
