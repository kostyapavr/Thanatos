using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bow : MonoBehaviour
{
    private float delay;
    private float chargeTime;
    private float maxChargeTime = 3.0f;
    [HideInInspector] public bool isCharging = false;
    public float delayLength;
    public float offset;

    public GameObject Arrow;
    public Transform point;

    public int arrowSpeed;

    public Player player;
    //private ItemManager itemManager;

    public Image bowCharge;

    public GameObject bowShootPosition;

    public Sprite normalSprite;

    [HideInInspector] public bool canShoot = true;

    private SpriteRenderer bowSprite;

    private void Start()
    {
        bowSprite = GetComponent<SpriteRenderer>();
        //itemManager = player.GetComponent<ItemManager>();
    }

    void Update()
    {
        if (LevelController.playerHasBow) bowSprite.enabled = LevelController.playerSelectedWeapon == 0 ? true : false;

        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotatez = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        point.rotation = Quaternion.Euler(0f, 0f, rotatez + offset);

        if (delay <= 0f && LevelController.playerHasBow && player.GetCurrentArrows() > 0 && canShoot)
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
                bowShootPosition.SetActive(false);
            }
        }
        else
        {
            delay -= Time.deltaTime;
        }
    }

    void ShootArrow(Vector3 direction, float dmgBoost = 0.0f)
    {
        float boost = chargeTime / (LevelController.playerHasHelmet ? maxChargeTime - 1.5f : maxChargeTime);
        Rigidbody2D rb = Instantiate(Arrow, point.position, point.rotation).GetComponent<Rigidbody2D>();
        rb.GetComponent<Arrow>().OwnerType = "Player";
        rb.GetComponent<Arrow>().OwnerID = gameObject.GetInstanceID();
        rb.GetComponent<Arrow>().Damage += boost;
        rb.AddForce(direction * arrowSpeed * (1 + boost) + (Vector3)player.GetComponent<Rigidbody2D>().velocity, ForceMode2D.Impulse);
        player.TakeArrow();
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
        gameObject.SetActive(false);
    }

    public void ShowBow()
    {
        canShoot = true;
        gameObject.SetActive(true);
        player.GetComponent<SpriteRenderer>().sprite = normalSprite;
    }
}
