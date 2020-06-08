using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField]
    public float cameraFollowSpeed = 3f;

    [HideInInspector]
    public GameObject player;

    private Vector3 cameraPositionToFollow;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        cameraPositionToFollow = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 25);
        transform.position = Vector3.Lerp(transform.position, cameraPositionToFollow, cameraFollowSpeed * Time.deltaTime);
    }
}