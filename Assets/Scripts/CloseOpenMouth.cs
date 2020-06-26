using System.Collections;
using System.Collections.Generic;
using Shapes2D;
using UnityEngine;

public class CloseOpenMouth : MonoBehaviour
{
    private Shape shape;
    void Start()
    {
        shape = GetComponent<Shape>();
        CloseOpen();
    }


    private void CloseOpen()
    {
        var targetValue = shape.settings.startAngle == 0 ? 30 : 0;
        LeanTween.value(gameObject, shape.settings.startAngle, targetValue, 0.2f)
        .setOnUpdate((float val) =>
            {
                shape.settings.startAngle = val;
                shape.settings.endAngle = 330 + targetValue - val;

            }
        )
        .setOnComplete(() => CloseOpen());
    }
}
