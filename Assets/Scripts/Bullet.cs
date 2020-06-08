using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    public float damage;

    [SerializeField]
    public float speed;

    [SerializeField]
    public float maxTravelDistance;

    [SerializeField]
    public GameObject collideEffects;

    [SerializeField]
    public bool isRotating;

    [SerializeField]
    public float rotationSpeed;

    [HideInInspector]
    public Vector3 velocity;

    private float distanceTravelled = 0;

    void Update()
    {
        distanceTravelled += speed * Time.deltaTime;

        transform.position += velocity * speed * Time.deltaTime;

        if (!isRotating)
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg);
        }
        else
        {
            transform.Rotate(0, 0, rotationSpeed);
        }

        if (distanceTravelled >= maxTravelDistance)
        {
            DestroyBullet();
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (gameObject.tag == "PlayerBullet" && other.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().TakeDamage(damage);
        }
        else if (gameObject.tag == "EnemyBullet" && other.tag == "Player")
        {
            other.GetComponent<PlayerController>().TakeDamage(damage);
        }

        DestroyBullet();
    }

    private void DestroyBullet()
    {
        Instantiate(collideEffects, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
