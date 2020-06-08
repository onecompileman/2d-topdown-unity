using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public FixedJoystick moveJoystick;

    [SerializeField]
    public FixedJoystick shootJoystick;

    [SerializeField]
    public float life;

    [SerializeField]
    public float playerSpeed;

    [SerializeField]
    public float rotationSpeed;

    [SerializeField]
    public SpriteRenderer spriteRenderer;

    [SerializeField]
    public List<BaseWeapon> weapons;

    [SerializeField]
    public AudioClip hitSound;

    [SerializeField]
    public RectTransform lifeImage;

    private int activeWeaponIndex = 0;
    private Vector3 velocity = new Vector3(0, 0, 0);
    private Quaternion rotationToFollow = Quaternion.Euler(0, 0, 90);

    private Color originalShapeColor;

    private float originalLife;

    void Start()
    {
        originalShapeColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b);
        originalLife = life;
    }

    void Update()
    {
        var snapValue = 0.03f;
        var snapValueShoot = 0.02f;

        velocity.Set(0, 0, 0);

        if (Mathf.Abs(moveJoystick.Vertical) >= snapValue || Mathf.Abs(moveJoystick.Horizontal) >= snapValue)
        {
            var rotationZ = (Mathf.Atan2(moveJoystick.Vertical, moveJoystick.Horizontal) * Mathf.Rad2Deg) - 90;

            Quaternion rotationToFollow = Quaternion.Euler(new Vector3(0, 0, rotationZ));

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, rotationZ)), 0.1f);

            velocity.Set(Mathf.Cos((transform.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad), Mathf.Sin((transform.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad), 0);
        }

        if (Mathf.Abs(shootJoystick.Vertical) >= snapValueShoot || Mathf.Abs(shootJoystick.Horizontal) >= snapValueShoot)
        {
            var rotationZ = (Mathf.Atan2(shootJoystick.Vertical, shootJoystick.Horizontal) * Mathf.Rad2Deg) - 90;

            LeanTween.rotateZ(gameObject, rotationZ, 0).setOnComplete(() =>
            {
                weapons[activeWeaponIndex].Fire(rotationZ + 90);
            });

            velocity.Set(moveJoystick.Horizontal, moveJoystick.Vertical, 0);


        }

        if (life <= 0)
        {
            SceneManager.LoadScene("SampleScene");
        }

        UpdateLifeUI();

        transform.position += (velocity.normalized * playerSpeed * Time.deltaTime);
    }

    public void TakeDamage(float damage)
    {
        life -= damage;
        StartCoroutine("TakeDamageEffects");
    }

    private void UpdateLifeUI()
    {
        var originalLifeUIWidth = 240;
        var originalPositionX = 125.2f;

        var lifePercentage = life / originalLife;

        lifePercentage = lifePercentage < 0 ? 0 : lifePercentage;
        var remLifePercentage = 1 - lifePercentage;
        lifeImage.localPosition = new Vector3(originalPositionX - (originalLifeUIWidth * remLifePercentage) / 2, 0, 0);
        lifeImage.sizeDelta = new Vector2(originalLifeUIWidth * lifePercentage, 12);
    }

    private IEnumerator TakeDamageEffects()
    {
        spriteRenderer.color = new Color(255 / 255, 50 / 255, 50 / 255);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(originalShapeColor.r, originalShapeColor.g, originalShapeColor.b);
    }
}
