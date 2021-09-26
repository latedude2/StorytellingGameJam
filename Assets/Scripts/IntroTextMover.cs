using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroTextMover : MonoBehaviour
{
    public float textSpeed = 1f;

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * textSpeed);
    }
}
