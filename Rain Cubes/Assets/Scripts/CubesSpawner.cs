using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class CubesSpawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private int _maxSize = 20;
    [SerializeField] private float _spawnDelay = 0.3f;

    private ObjectPool<GameObject> _pool;
    private bool _isWork = true;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_prefab.gameObject),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            maxSize: _maxSize);
    }

    private void Start()
    {
        StartCoroutine(Spawn(_spawnDelay));
    }

    private void OnEnable()
    {
        Cube.DeactivatingCube += DeactivateCube;
    }

    private void OnDisable()
    {
        Cube.DeactivatingCube -= DeactivateCube;
    }

    public void DeactivateCube(Cube cube)
    {
        _pool.Release(cube.gameObject);
    }

    private IEnumerator Spawn(float delay)
    {
        var wait = new WaitForSeconds(delay);

        while (_isWork)
        {
            GetCube();
            yield return wait;
        }
    }

    private void ActionOnGet(GameObject obj)
    {
        obj.transform.position = GetRandomSpawnPosition();

        if (obj.TryGetComponent(out Cube cube))
        {
            cube.ResetState();
        }

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