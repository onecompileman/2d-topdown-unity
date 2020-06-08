using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    [SerializeField]
    public Bullet bullet;

    [SerializeField]
    public int bulletToFire;

    [SerializeField]
    public float bulletAngle;

    [SerializeField]
    public float fireRate;

    private AudioSource audioSource;

    private bool canFire = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Fire(float angle)
    {
        if (canFire)
        {
            audioSource.Play();

            var startingAngle = bulletToFire > 1 ? angle - (bulletAngle / 2) : angle;
            var incrementAngle = bulletAngle / (bulletToFire - 1);
            for (int i = 0; i < bulletToFire; i++)
            {
                var radAngle = startingAngle * Mathf.Deg2Rad;
                var velocity = new Vector3(Mathf.Cos(radAngle), Mathf.Sin(radAngle), 0);

                var bulletObj = Instantiate(bullet.gameObject, transform.position, Quaternion.identity);
                bulletObj.GetComponent<Bullet>().velocity = velocity;

                startingAngle += incrementAngle;
            }

            StartCoroutine("FireCooldown");
        }
    }

    private IEnumerator FireCooldown()
    {
        canFire = false;
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }
}
