using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private int _poolCapacity = 3;
    [SerializeField] private int _poolMaxSize = 3;
    [SerializeField] private float _randomPositionSpawnX;
    [SerializeField] private float _randomPositionSpawnY;
    [SerializeField] private float _positionSpawnZ = 5f;

    private ObjectPool<Cube> _cubesPool;
    private Coroutine _spawnCoroutine;

    private void Awake()
    {
        _cubesPool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_cubePrefab),
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: (cube) => OnRelease(cube),
            actionOnDestroy: (cube) => Destroy(cube.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void ActionOnGet(Cube cube)
    {
        cube.transform.position = GetSpawnPosition();
        cube.gameObject.SetActive(true);
        cube.Hitted += ReleaseCube;
    }

    private void OnRelease(Cube cube)
    {
        cube.gameObject.SetActive(false);
    }

    private IEnumerator SpawnCubesPerCooldown()
    {
        var wait = new WaitForSeconds(_repeatRate);

        while (enabled)
        {
            yield return wait;

            if (_cubesPool.CountActive < _poolMaxSize)
            {
                _cubesPool.Get();
            }
        }
    }

    private void ReleaseCube(Cube cube)
    {
        cube.Hitted -= ReleaseCube;
        _cubesPool.Release(cube);
        cube.ResetCube();
    }

    private Vector3 GetSpawnPosition()
    {
        float positionX = Random.Range(0, _randomPositionSpawnX);
        float positionY = Random.Range(0, _randomPositionSpawnY);
        return new Vector3(positionX, _positionSpawnZ, positionY);
    }

    private void OnEnable()
    {
        _spawnCoroutine = StartCoroutine(SpawnCubesPerCooldown());
    }

    private void OnDisable()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }

        _cubesPool?.Clear();
    }
}
