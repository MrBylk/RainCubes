using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private Platform _platform;
    private CubesSpawner _spawner;
    private Renderer _renderer;
    private bool _isChangedColor = false;
    private Color _defaultColor;

    private void Awake()
    {
        _defaultColor = Color.blue;
        _renderer = GetComponent<Renderer>();
        _renderer.material.color = _defaultColor;
        _platform = GameObject.Find("Platform").GetComponent<Platform>();
        _spawner = GameObject.Find("CubesSpawner").GetComponent<CubesSpawner>();
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

        _spawner.DeactivateCube(gameObject);
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