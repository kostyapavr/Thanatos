using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 inputVector = new Vector2(0.0f, 0.0f);
    [HideInInspector] public int acceleration;
    public AnimationPlayer playerAnim;
    public Transform attackPoint;
    public Transform bow;
    public Transform helmet;
    public Transform bowShootPos;
    public Transform defendShieldPos;
    public Transform harpeSwordPos;
    public Transform peleusSwordPos;
    public Transform legs;
    public Player player;
    public Shield shield;

    private int slowAcceleration = 1;
    private int puddleAcceleration = 2;
    private int slowEffectAcceleration = 3;
    private int normalAcceleration = 7;
    private int onFireAcceleration = 5;
    private int bothWeaponsAcceleration = 6;
    private int onlySwordAcceleration = 8;
    private int helmetAcceleration = 9;
    [HideInInspector]
    public bool hasFlipped = false;

    private bool isSlowedDown = false;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        acceleration = normalAcceleration;
    }

    private void Start()
    {
        CheckAcceleration();
    }

    void Update()
    {
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (inputVector.x < 0.0f && !hasFlipped && !bow.GetComponent<Bow>().isCharging && !shield.anim)
        {
            FlipPlayer();
        }
        else if (inputVector.x > 0.0f && hasFlipped && !bow.GetComponent<Bow>().isCharging && !shield.anim)
        {
            UnflipPlayer();
        }

        CheckAcceleration();
    }

    public void FlipPlayer()
    {
        if (hasFlipped) return;
        GetComponent<SpriteRenderer>().flipX = true;
        attackPoint.localPosition = new Vector3(-attackPoint.localPosition.x, attackPoint.localPosition.y, 0);
        bow.localPosition = new Vector3(-bow.localPosition.x, bow.localPosition.y, 0);

        helmet.localPosition = new Vector3(-helmet.localPosition.x, helmet.localPosition.y, 0);
        helmet.GetComponent<SpriteRenderer>().flipX = true;

        bowShootPos.localPosition = new Vector3(-bowShootPos.localPosition.x, bowShootPos.localPosition.y, 0);
        bowShootPos.GetComponent<SpriteRenderer>().flipY = true;

        player.slowEffectSprite.transform.localPosition = new Vector3(-1, player.slowEffectSprite.transform.localPosition.y, 0);
        player.ambrosiaEffectSprite.transform.localPosition = new Vector3(-1f, player.ambrosiaEffectSprite.transform.localPosition.y, 0);
        player.miasmaEffectSprite.transform.localPosition = new Vector3(-1f, player.miasmaEffectSprite.transform.localPosition.y, 0);

        player.lavaBoots.transform.localPosition = new Vector3(-0.172f, player.lavaBoots.transform.localPosition.y, 0);
        player.lavaBoots.GetComponent<SpriteRenderer>().flipX = true;

        player.armorSprite.transform.localPosition = new Vector3(-player.armorSprite.transform.localPosition.x, player.armorSprite.transform.localPosition.y, 0);
        player.armorSprite.GetComponent<SpriteRenderer>().flipX = true;

        player.achillesArmorSprite.transform.localPosition = new Vector3(-player.achillesArmorSprite.transform.localPosition.x, player.achillesArmorSprite.transform.localPosition.y, 0);
        player.achillesArmorSprite.GetComponent<SpriteRenderer>().flipX = true;

        player.achillesHelmetSprite.transform.localPosition = new Vector3(-player.achillesHelmetSprite.transform.localPosition.x, player.achillesHelmetSprite.transform.localPosition.y, 0);
        player.achillesHelmetSprite.GetComponent<SpriteRenderer>().flipX = true;

        player.shield.transform.localPosition = new Vector3(-player.shield.transform.localPosition.x, player.shield.transform.localPosition.y, 0);
        player.shield.gameObject.GetComponent<SpriteRenderer>().flipX = true;

        defendShieldPos.localPosition = new Vector3(-defendShieldPos.localPosition.x, defendShieldPos.localPosition.y, 0);
        defendShieldPos.gameObject.GetComponent<SpriteRenderer>().flipX = true;

        harpeSwordPos.localPosition = new Vector3(-harpeSwordPos.localPosition.x, harpeSwordPos.localPosition.y, 0);
        harpeSwordPos.gameObject.GetComponent<SpriteRenderer>().flipX = true;

        peleusSwordPos.localPosition = new Vector3(-peleusSwordPos.localPosition.x, peleusSwordPos.localPosition.y, 0);
        peleusSwordPos.gameObject.GetComponent<SpriteRenderer>().flipX = true;

        legs.localPosition = new Vector3(-legs.localPosition.x, legs.localPosition.y, 0);
        legs.gameObject.GetComponent<SpriteRenderer>().flipX = true;

        //swordSpritePos.localPosition = new Vector3(-swordSpritePos.localPosition.x, swordSpritePos.localPosition.y, 0);
        //swordSpritePos.eulerAngles = new Vector3(0, 0, 170);

        hasFlipped = true;
    }

    public void UnflipPlayer()
    {
        if (!hasFlipped) return;
        GetComponent<SpriteRenderer>().flipX = false;
        attackPoint.localPosition = new Vector3(-attackPoint.localPosition.x, attackPoint.localPosition.y, 0);
        bow.localPosition = new Vector3(-bow.localPosition.x, bow.localPosition.y, 0);

        helmet.localPosition = new Vector3(-helmet.localPosition.x, helmet.localPosition.y, 0);
        helmet.GetComponent<SpriteRenderer>().flipX = false;

        bowShootPos.localPosition = new Vector3(-bowShootPos.localPosition.x, bowShootPos.localPosition.y, 0);
        bowShootPos.GetComponent<SpriteRenderer>().flipY = false;

        player.slowEffectSprite.transform.localPosition = new Vector3(-0.64f, player.slowEffectSprite.transform.localPosition.y, 0);
        player.ambrosiaEffectSprite.transform.localPosition = new Vector3(-0.64f, player.ambrosiaEffectSprite.transform.localPosition.y, 0);
        player.miasmaEffectSprite.transform.localPosition = new Vector3(-0.64f, player.miasmaEffectSprite.transform.localPosition.y, 0);

        player.lavaBoots.transform.localPosition = new Vector3(0.154f, player.lavaBoots.transform.localPosition.y, 0);
        player.lavaBoots.GetComponent<SpriteRenderer>().flipX = false;

        player.armorSprite.transform.localPosition = new Vector3(-player.armorSprite.transform.localPosition.x, player.armorSprite.transform.localPosition.y, 0);
        player.armorSprite.GetComponent<SpriteRenderer>().flipX = false;

        player.achillesArmorSprite.transform.localPosition = new Vector3(-player.achillesArmorSprite.transform.localPosition.x, player.achillesArmorSprite.transform.localPosition.y, 0);
        player.achillesArmorSprite.GetComponent<SpriteRenderer>().flipX = false;

        player.achillesHelmetSprite.transform.localPosition = new Vector3(-player.achillesHelmetSprite.transform.localPosition.x, player.achillesHelmetSprite.transform.localPosition.y, 0);
        player.achillesHelmetSprite.GetComponent<SpriteRenderer>().flipX = false;

        player.shield.transform.localPosition = new Vector3(-player.shield.transform.localPosition.x, player.shield.transform.localPosition.y, 0);
        player.shield.gameObject.GetComponent<SpriteRenderer>().flipX = false;

        defendShieldPos.localPosition = new Vector3(-defendShieldPos.localPosition.x, defendShieldPos.localPosition.y, 0);
        defendShieldPos.gameObject.GetComponent<SpriteRenderer>().flipX = false;

        harpeSwordPos.localPosition = new Vector3(-harpeSwordPos.localPosition.x, harpeSwordPos.localPosition.y, 0);
        harpeSwordPos.gameObject.GetComponent<SpriteRenderer>().flipX = false;

        peleusSwordPos.localPosition = new Vector3(-peleusSwordPos.localPosition.x, peleusSwordPos.localPosition.y, 0);
        peleusSwordPos.gameObject.GetComponent<SpriteRenderer>().flipX = false;

        legs.localPosition = new Vector3(-legs.localPosition.x, legs.localPosition.y, 0);
        legs.gameObject.GetComponent<SpriteRenderer>().flipX = false;

        //swordSpritePos.localPosition = new Vector3(-swordSpritePos.localPosition.x, swordSpritePos.localPosition.y, 0);
        //swordSpritePos.eulerAngles = new Vector3(0, 0, 58);

        hasFlipped = false;
    }

    void CheckAcceleration()
    {
        if (inputVector != Vector2.zero) playerAnim.SetRunning();
        else playerAnim.SetStopped();

        if (isSlowedDown) return;
        if ((LevelController.playerHasBow || LevelController.playerHasFireBow) && LevelController.currentPlayerWeapon.Name.Contains("Bow"))
        {
            acceleration = bothWeaponsAcceleration;
            normalAcceleration = bothWeaponsAcceleration;
        }
        else if (LevelController.playerHasSword && LevelController.currentPlayerWeapon.Name.Contains("Sword"))
        {
            acceleration = LevelController.playerHasHelmet ? helmetAcceleration : onlySwordAcceleration;
            normalAcceleration = acceleration;
        }
    }

    void FixedUpdate()
    {
        rb.velocity += inputVector * acceleration;
    }

    public void SlowDown_Effect()
    {
        acceleration = slowEffectAcceleration;
        isSlowedDown = true;
    }

    public void SlowDown()
    {
        acceleration = slowAcceleration;
        isSlowedDown = true;
    }

    public void PuddleSlowDown()
    {
        acceleration = puddleAcceleration;
        isSlowedDown = true;
    }

    public void ReturnToNormalSpeed()
    {
        if (player.is_enemySlowEffect)
        {
            acceleration = slowEffectAcceleration;
            return;
        }
        acceleration = normalAcceleration;
        isSlowedDown = false;
    }

    public void SetOnFire()
    {
        acceleration = onFireAcceleration;
        isSlowedDown = true;
        Invoke("ReturnToNormalSpeed", 5f);
    }
}