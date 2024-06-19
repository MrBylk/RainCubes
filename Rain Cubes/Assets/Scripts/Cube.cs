using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour
{
    private Renderer _renderer;
    private bool _isCollided = false;
    private Color _defaultColor;

    public event UnityAction<Cube> DeactivatingCube;

    private void Awake()
    {
        _defaultColor = Color.blue;
        _renderer = GetComponent<Renderer>();
        _renderer.material.color = _defaultColor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Platform>())
        {
            StartLifeTimer();
        }
    }

    public void ResetState()
    {
        _isCollided = false;
        _renderer.material.color = _defaultColor;
    }

    private void StartLifeTimer()
    {
        if (_isCollided == false)
        {
            _renderer.material.color = Random.ColorHSV();
            StartCoroutine(nameof(TimeToDestroyCube));
            _isCollided = true;
        }
    }

    private IEnumerator TimeToDestroyCube()
    {
        int timer;
        var wait = new WaitForSeconds(1);
        int lifeTime = GetRandomLifeTime();

        for (int i = 0; i < lifeTime; i++)
        {
            timer = i;
            yield return wait;
        }

        DeactivatingCube?.Invoke(this);
    }

    private int GetRandomLifeTime()
    {
        int minValue = 2;
        int maxValue = 5;
        int lifeTime;

        lifeTime = Random.Range(minValue, maxValue + 1);
        return lifeTime;
    }
}