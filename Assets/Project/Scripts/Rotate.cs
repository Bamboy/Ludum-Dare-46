using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed = 1f;

    private void Update()
    {
        transform.Rotate(0f, speed * Time.deltaTime, 0f);
    }
}
