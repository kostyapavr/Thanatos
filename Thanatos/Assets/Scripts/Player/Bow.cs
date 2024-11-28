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
    private ItemManager itemManager;

    public Image bowCharge;

    private void Start()
    {
        itemManager = player.GetComponent<ItemManager>();
    }

    void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotatez = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotatez + offset);

        if (delay <= 0f && itemManager.hasBow && player.GetCurrentArrows() > 0)
        {
            if (Input.GetMouseButton(0))
            {
                if (LevelController.playerHasHelmet) maxChargeTime = 2.0f;
                if (chargeTime < maxChargeTime)
                {
                    chargeTime += Time.deltaTime;
                    bowCharge.fillAmount = chargeTime / maxChargeTime;
                    player.GetComponent<PlayerMovement>().SlowDown();
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (chargeTime > 0.3f) ShootArrow(difference.normalized);
                delay = delayLength;
                bowCharge.fillAmount = 0;
                chargeTime = 0;
                player.GetComponent<PlayerMovement>().ReturnToNormalSpeed();
            }
        }
        else
        {
            delay -= Time.deltaTime;
        }
    }

    void ShootArrow(Vector3 direction, float dmgBoost = 0.0f)
    {
        float boost = chargeTime / maxChargeTime;
        Rigidbody2D rb = Instantiate(Arrow, point.position, transform.rotation).GetComponent<Rigidbody2D>();
        rb.GetComponent<Arrow>().owner = "Player";
        rb.GetComponent<Arrow>().BoostDamage(boost);
        rb.AddForce(direction * arrowSpeed * (1 + boost) + (Vector3)player.GetComponent<Rigidbody2D>().velocity, ForceMode2D.Impulse);
        player.TakeArrow();
    }
}
