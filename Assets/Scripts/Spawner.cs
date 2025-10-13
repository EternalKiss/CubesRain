using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrebaf;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 10;
    [SerializeField] private float _randomPositionSpawnX;
    [SerializeField] private float _randomPositionSpawnY;
    [SerializeField] private float _positionSpawnZ = 5f;

    private ObjectPool<GameObject> _cubesPool;

    private void Awake()
    {
        _cubesPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_cubePrebaf),
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: (cube) => cube.SetActive(false),
            actionOnDestroy: (cube) => Destroy(cube),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void ActionOnGet(GameObject cube)
    {
        cube.transform.position = GetSpawnPosition();
        cube.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        cube.SetActive(true);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0.0f, _repeatRate);
    }

    private void GetCube()
    {
        _cubesPool.Get();
    }

    private void OnTriggerEnter(Collider other)
    {
        _cubesPool.Release(other.gameObject);
    }

    private Vector3 GetSpawnPosition()
    {
        float positionX = Random.Range(0, _randomPositionSpawnX);
        float positionY = Random.Range(0, _randomPositionSpawnY);
        return new Vector3(positionX, _positionSpawnZ, positionY);
    }
}
