using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    public float life;

    [SerializeField]
    public float speed;

    [SerializeField]
    public float fireRate;

    [SerializeField]
    public Bullet bullet;

    [SerializeField]
    public float distanceToShoot;

    [SerializeField]
    public int bulletsToFire;

    [SerializeField]
    public bool canShoot;

    [SerializeField]
    public float damage;

    [SerializeField]
    public float bulletAngle;

    [SerializeField]
    public GameObject explodeEffects;

    [SerializeField]
    public AudioClip enemyShootSound;

    [SerializeField]
    public AudioClip enemyExplodeSound;

    [SerializeField]
    public bool isAutoRotateFire;

    [SerializeField]
    public bool isAutoRotate;

    [SerializeField]
    public SpriteRenderer spriteRenderer;

    private float degreeOffset = 0;
    private Color originalShapeColor;
    private PlayerController player;
    private Vector3 velocity = new Vector3(0, 0, 0);
    private bool canAttack = true;

    private Quaternion toLookAt;

    private float angleBetween;

    private Rigidbody2D rb;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        originalShapeColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b);
    }

    void Update()
    {
        var playerDistance = Vector3.Distance(player.transform.position, transform.position);
        velocity.Set(0, 0, 0);

        if (life <= 0)
        {
            OnDeathEffect();
        }

        if (playerDistance > distanceToShoot)
        {
            velocity = player.transform.position - transform.position;
            velocity.Normalize();


        }

        if (!isAutoRotate)
        {
            var targetVector = player.transform.position - transform.position;
            angleBetween = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg;
            var lookSpeed = 3f;

            toLookAt = Quaternion.Euler(0, 0, angleBetween - 90);

            transform.rotation = Quaternion.Slerp(transform.rotation, toLookAt, lookSpeed * Time.deltaTime);

        }

        if (isAutoRotateFire)
        {
            degreeOffset++;
        }

        Shoot();

        // transform.position += (velocity * speed * Time.deltaTime);
        RepelToOtherEnemies();

    }

    private void RepelToOtherEnemies()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        var repelRange = 5f;

        Vector2 repelForce = Vector2.zero;

        foreach (var enemy in enemies)
        {
            if (enemy.GetInstanceID() != gameObject.GetInstanceID() && Vector3.Distance(transform.position, enemy.transform.position) <= repelRange)
            {
                repelForce += (rb.position - (Vector2)enemy.transform.position).normalized;
            }
        }

        Vector2 newPos = (Vector2)velocity * speed * Time.deltaTime;
        newPos += repelForce * Time.deltaTime * speed;
        transform.position += (Vector3)newPos;
    }

    private void Shoot()
    {
        if (canAttack && canShoot)
        {
            StartCoroutine("AttackCooldown");
            AudioSource.PlayClipAtPoint(enemyShootSound, transform.position);

            var angle = isAutoRotateFire ? degreeOffset : transform.rotation.eulerAngles.z;
            angle = !isAutoRotate ? angleBetween : angle;

            var startingAngle = bulletsToFire > 1 ? angle - (bulletAngle / 2) : angle;
            var incrementAngle = bulletAngle / (bulletsToFire - 1);
            for (int i = 0; i < bulletsToFire; i++)
            {
                var radAngle = startingAngle * Mathf.Deg2Rad;
                var velocity = new Vector3(Mathf.Cos(radAngle), Mathf.Sin(radAngle), 0);

                var bulletObj = Instantiate(bullet.gameObject, transform.position, Quaternion.identity);
                bulletObj.GetComponent<Bullet>().velocity = velocity;
                bulletObj.GetComponent<Bullet>().damage = damage;

                startingAngle += incrementAngle;
            }

        }
    }

    public void TakeDamage(float damage)
    {
        life -= damage;
        StartCoroutine("TakeDamageEffects");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (canAttack && other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);

            AttackCooldown();
        }
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(fireRate);
        canAttack = true;
    }

    private IEnumerator TakeDamageEffects()
    {
        spriteRenderer.color = new Color(255 / 255, 50 / 255, 50 / 255);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(originalShapeColor.r, originalShapeColor.g, originalShapeColor.b);
    }

    private void OnDeathEffect()
    {
        Instantiate(explodeEffects, transform.position, Quaternion.identity);

        AudioSource.PlayClipAtPoint(enemyExplodeSound, transform.position);

        Destroy(gameObject);
    }
}
