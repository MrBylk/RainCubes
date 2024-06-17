using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour
{
    public static event UnityAction<Cube> DeactivatingCube;
    private Renderer _renderer;
    private bool _isChangedColor = false;
    private Color _defaultColor;

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
        _isChangedColor = false;
        _renderer.material.color = _defaultColor;
    }

    private void StartLifeTimer()
    {
        if (_isChangedColor == false)
        {
            GetComponent<Renderer>().material.color = Random.ColorHSV();
            _isChangedColor = true;
        }

        StartCoroutine(nameof(TimeToDestroyCube));
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