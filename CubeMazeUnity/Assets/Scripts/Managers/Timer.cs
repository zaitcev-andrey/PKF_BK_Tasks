using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float _time;
    void Start()
    {
        _time = 0f;
    }

    void Update()
    {
        _time += Time.deltaTime;
    }
}
