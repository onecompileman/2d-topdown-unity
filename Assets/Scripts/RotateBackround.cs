using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes2D;

public class RotateBackround : MonoBehaviour
{
    [SerializeField]
    public Shape shape;

    [SerializeField]
    public float rotateSpeed;

    void Update()
    {
        shape.settings.fillRotation += rotateSpeed;
    }
}
