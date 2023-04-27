using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheelscript : MonoBehaviour
{
    public float zRotate;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(0, 0, zRotate, Space.Self);
    }
}
