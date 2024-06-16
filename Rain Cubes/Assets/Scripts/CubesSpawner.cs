using UnityEngine;
using UnityEngine.Pool;

public class CubesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _maxSize = 20;
    [SerializeField] private float _spawnSpeed = 0.3f;

    private ObjectPool<GameObject> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            maxSize: _maxSize);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0.0f, _spawnSpeed);
    }

    public void DeactivateCube(GameObject cube)
    {
        _pool.Release(cube);
    }

    private void ActionOnGet(GameObject obj)
    {
        obj.transform.position = GetRandomSpawnPosition();
        obj.GetComponent<Cube>().ResetState();
        obj.SetActive(true);
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float xPosition;
        float yPosition = 10;
        float zPosition;
        float minPosition = 0;
        float maxPosition = 10;
        Vector3 position;

        xPosition = Random.Range(minPosition, maxPosition + 1);
        zPosition = Random.Range(minPosition, maxPosition + 1);
        position = new Vector3(xPosition, yPosition, zPosition);
        return position;
    }
}