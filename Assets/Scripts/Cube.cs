using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] Collider _triggerColider;
    private Renderer _renderer;

    private float _delayInSeconds;
    private float _minDelay = 2;
    private float _maxDelay = 5;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter()
    {
        ColorChanger();
        Destroy();
    }

    private void ColorChanger()
    {
        Color color = Random.ColorHSV();
        _renderer.material.color = color;
    }

    private void Destroy()
    {
        _delayInSeconds = Random.Range(_minDelay, _maxDelay);
        Destroy(gameObject, _delayInSeconds);
    }
}
