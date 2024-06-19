using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class CubesSpawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private int _maxSize = 20;
    [SerializeField] private float _spawnDelay = 0.3f;

    private ObjectPool<Cube> _pool;
    private bool _isWork = true;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: OnGet,
            actionOnRelease: OnRelease,
            actionOnDestroy: Destroy,
            collectionCheck: true,
            maxSize: _maxSize);
    }

    private void Start()
    {
        StartCoroutine(Spawn(_spawnDelay));
    }

    public void DeactivateCube(Cube cube)
    {
        _pool.Release(cube);
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

    private void OnGet(Cube obj)
    {
        obj.transform.position = GetRandomSpawnPosition();

        if (obj.TryGetComponent(out Cube cube))
        {
            cube.ResetState();
        }

        obj.gameObject.SetActive(true);
    }

    private void OnRelease(Cube cube)
    {
        cube.gameObject.SetActive(false);
        cube.DeactivatingCube -= DeactivateCube;
    }

    private void GetCube()
    {
        _pool.Get(out Cube cube);
        cube.DeactivatingCube += DeactivateCube;
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