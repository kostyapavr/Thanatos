using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bow : MonoBehaviour
{
    private float delay;
    private float chargeTime;
    private float maxChargeTime = 3.0f;
    public float delayLength;
    public float offset;

    public GameObject Arrow;
    public Transform point;

    public int arrowSpeed;

    public Player player;
    //private ItemManager itemManager;

    public Image bowCharge;

    public GameObject bowShootPosition;

    [HideInInspector] public bool canShoot = true;

    private SpriteRenderer bowSprite;

    private void Start()
    {
        bowSprite = GetComponent<SpriteRenderer>();
        //itemManager = player.GetComponent<ItemManager>();
    }

    void Update()
    {
        if (LevelController.playerHasBow) bowSprite.enabled = true;

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
                }

                bowSprite.enabled = false;
                bowShootPosition.SetActive(true);
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (chargeTime > 0.3f) ShootArrow(difference.normalized);
                delay = delayLength;
                bowCharge.fillAmount = 0;
                chargeTime = 0;
                player.GetComponent<PlayerMovement>().ReturnToNormalSpeed();
                bowSprite.enabled = true;
                bowShootPosition.SetActive(false);
            }
        }
        else
        {
            delay -= Time.deltaTime;
            if (!canShoot && bowSprite.enabled) bowSprite.enabled = false;
            else if (canShoot && !bowSprite.enabled) bowSprite.enabled = true;
        }
    }

    void ShootArrow(Vector3 direction, float dmgBoost = 0.0f)
    {
        float boost = chargeTime / maxChargeTime;
        Rigidbody2D rb = Instantiate(Arrow, point.position, point.rotation).GetComponent<Rigidbody2D>();
        rb.GetComponent<Arrow>().owner = "Player";
        rb.GetComponent<Arrow>().BoostDamage(boost);
        rb.AddForce(direction * arrowSpeed * (1 + boost) + (Vector3)player.GetComponent<Rigidbody2D>().velocity, ForceMode2D.Impulse);
        player.TakeArrow();
    }
}
