using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour
{
    private Renderer _renderer;
    private Color _defaultColor = Color.white;

    private bool _isHitted;
    private int _minLifeTime = 2;
    private int _maxLifeTime = 6;

    public event Action<Cube> Hitted;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private IEnumerator DetermineLifetime()
    {
        var wait = new WaitForSeconds(Random.Range(_minLifeTime, _maxLifeTime));

        yield return wait;

        Hitted?.Invoke(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isHitted == false && collision.gameObject.TryGetComponent<Terrain>(out _))
        {
            ChangeColor();
            _isHitted = true;
            StartCoroutine(DetermineLifetime());
        }
    }

    private void ChangeColor()
    {
        _renderer.material.color = Random.ColorHSV();
    }

    public void ResetCube()
    {
        _renderer.material.color = _defaultColor;
        _isHitted = false;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }
}
