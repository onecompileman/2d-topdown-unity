using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinMovement : MonoBehaviour
{

    [SerializeField]
    public GameObject room;

    [SerializeField]
    public float timeLerp;

    [SerializeField]
    public bool isVertical;

    private float counter = 0;
    void Start()
    {
        MoveLerp();
    }

    private void MoveLerp()
    {
        if (isVertical)
        {
            LeanTween.move(
                gameObject,
                new Vector3(transform.position.x, Random.Range(room.transform.position.y - 30, room.transform.position.y + 30), transform.position.z),
                 timeLerp
            )
            .setOnComplete(() =>
            {
                MoveLerp();
            });
        }
        else
        {
            LeanTween.move(
                gameObject,
                new Vector3(Random.Range(room.transform.position.x - 30, room.transform.position.x + 30), transform.position.y, transform.position.z),
                 timeLerp
            )
            .setOnComplete(() =>
            {
                MoveLerp();
            });
        }
    }
}
