using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bow : MonoBehaviour
{
    private float delay;
    private float chargeTime;
    private float maxChargeTime = 2.5f;
    [HideInInspector] public bool isCharging = false;
    public float delayLength;
    public float offset;

    public GameObject Arrow;
    public GameObject FireArrow;
    public GameObject ApolloArrow;
    public Transform point;

    public int arrowSpeed;

    public Player player;
    //private ItemManager itemManager;

    public Image bowCharge;

    public GameObject bowShootPosition;

    public Sprite normalSprite;
    public Sprite fireBowSprite;
    public Sprite apolloBowSprite;

    [HideInInspector] public bool canShoot = true;

    private SpriteRenderer bowSprite;

    private void Start()
    {
        bowSprite = GetComponent<SpriteRenderer>();
        //itemManager = player.GetComponent<ItemManager>();
    }

    void Update()
    {
        bowSprite.enabled = false;
        if (LevelController.playerHasBow)
        {
            bowSprite.enabled = true;
            bowSprite.sprite = normalSprite;
        }
        if (LevelController.playerHasFireBow)
        {
            bowSprite.enabled = true;
            bowSprite.sprite = fireBowSprite;
        }
        if (LevelController.playerHasApolloBow)
        {
            bowSprite.enabled = true;
            bowSprite.sprite = apolloBowSprite;
        }

        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotatez = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        point.rotation = Quaternion.Euler(0f, 0f, rotatez + offset);

        if (delay <= 0f && (LevelController.playerHasBow || LevelController.playerHasFireBow || LevelController.playerHasApolloBow) && player.GetCurrentArrows() > 0 && canShoot)
        {
            if (Input.GetMouseButton(0))
            {
                float mxChargeTime = maxChargeTime;
                if (LevelController.playerHasHelmet) mxChargeTime = 1.5f;

                if (chargeTime < mxChargeTime)
                {
                    chargeTime += Time.deltaTime;
                    bowCharge.fillAmount = chargeTime / mxChargeTime;
                    player.GetComponent<PlayerMovement>().SlowDown();

                    CheckFlip(rotatez);
                }

                isCharging = true;
                bowSprite.enabled = false;
                bowShootPosition.SetActive(true);
                if (LevelController.playerHasFireBow) bowShootPosition.GetComponent<SpriteRenderer>().sprite = fireBowSprite;
                if (LevelController.playerHasApolloBow) bowShootPosition.GetComponent<SpriteRenderer>().sprite = apolloBowSprite;
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (chargeTime > 0.2f) ShootArrow(difference.normalized);
                delay = delayLength;
                bowCharge.fillAmount = 0;
                chargeTime = 0;
                isCharging = false;
                player.GetComponent<PlayerMovement>().ReturnToNormalSpeed();
                bowSprite.enabled = true;
                if (LevelController.playerHasFireBow) bowSprite.sprite = fireBowSprite;
                if (LevelController.playerHasApolloBow) bowSprite.sprite = apolloBowSprite;
                bowShootPosition.SetActive(false);
            }
            if (Input.GetMouseButtonUp(1) && isCharging)
            {
                delay = delayLength;
                bowCharge.fillAmount = 0;
                chargeTime = 0;
                isCharging = false;
                player.GetComponent<PlayerMovement>().ReturnToNormalSpeed();
                bowSprite.enabled = true;
                if (LevelController.playerHasFireBow) bowSprite.sprite = fireBowSprite;
                if (LevelController.playerHasApolloBow) bowSprite.sprite = apolloBowSprite;
                bowShootPosition.SetActive(false);
            }
        }
        else
        {
            delay -= Time.deltaTime;
        }
    }

    void ShootArrow(Vector3 direction)
    {
        float boost = chargeTime / (LevelController.playerHasHelmet ? maxChargeTime - 1.5f : maxChargeTime);
        Rigidbody2D rb;
        if (LevelController.playerHasFireBow) rb = Instantiate(FireArrow, point.position, point.rotation).GetComponent<Rigidbody2D>();
        else if (LevelController.playerHasApolloBow) rb = Instantiate(ApolloArrow, point.position, point.rotation).GetComponent<Rigidbody2D>();
        else rb = Instantiate(Arrow, point.position, point.rotation).GetComponent<Rigidbody2D>();
        rb.GetComponent<Arrow>().OwnerType = "Player";
        rb.GetComponent<Arrow>().OwnerID = gameObject.GetInstanceID();
        rb.GetComponent<Arrow>().Damage += boost;
        if (LevelController.playerHasFireBow) rb.GetComponent<Arrow>().damageEffect = DamageEffects.StopOneShot;
        if (LevelController.playerHasApolloBow) rb.GetComponent<Arrow>().damageEffect = DamageEffects.FreezeInPlace;
        rb.AddForce(direction * arrowSpeed * (1 + boost) + (Vector3)player.GetComponent<Rigidbody2D>().velocity, ForceMode2D.Impulse);
        player.TakeArrow();
        player.PlayBowSound();
    }

    void CheckFlip(float angle)
    {
        if (Mathf.Abs(angle) < 90)
        {
            player.GetComponent<PlayerMovement>().UnflipPlayer();
            //playerFlipped = false;
        }
        else
        {
            player.GetComponent<PlayerMovement>().FlipPlayer();
            //playerFlipped = true;
        }
    }

    public void HideBow()
    {
        canShoot = false;
        bowSprite.enabled = false;
        gameObject.SetActive(false);
    }

    public void ShowBow()
    {
        canShoot = true;
        gameObject.SetActive(true);
        bowSprite.enabled = true;
        bowSprite.sprite = normalSprite;
        if (LevelController.currentPlayerWeapon.Name == "Fire Bow")
        {
            bowSprite.sprite = fireBowSprite;
        }
        else if (LevelController.currentPlayerWeapon.Name == "Apollo Bow")
        {
            bowSprite.sprite = apolloBowSprite;
        }
    }

    public void IncreaseDelayLength()
    {
        delayLength = 1.5f;
        maxChargeTime = 3f;
        Invoke("ReturnDelayToNormal", 5f);
    }

    public void ReturnDelayToNormal()
    {
        delayLength = 0.2f;
        maxChargeTime = 2.5f;
    }
}
