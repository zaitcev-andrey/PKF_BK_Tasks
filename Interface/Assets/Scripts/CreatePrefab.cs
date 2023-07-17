using UnityEngine;

public class CreatePrefab : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _spawnZone;
    [SerializeField] private float _radiusOfSpawnZone;
    private InterfaceManager _interfaceManager;

    private void Start()
    {
        _interfaceManager = GameObject.FindObjectOfType<InterfaceManager>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            GameObject obj = Instantiate(_prefab, _spawnZone.position + new Vector3( Random.Range(-_radiusOfSpawnZone, _radiusOfSpawnZone), 5f,
                Random.Range(-_radiusOfSpawnZone, _radiusOfSpawnZone)), Quaternion.identity);

            _interfaceManager.AddNewObjectToList(obj);
        }
    }
}
