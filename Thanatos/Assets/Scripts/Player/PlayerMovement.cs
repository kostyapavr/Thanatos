using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 inputVector = new Vector2(0.0f, 0.0f);
    [HideInInspector] public int acceleration;
    public Transform attackPoint;
    public Transform bow;
    public Transform helmet;
    public Transform bowShootPos;
    public Player player;

    private int slowAcceleration = 1;
    private int puddleAcceleration = 2;
    private int slowEffectAcceleration = 3;
    private int normalAcceleration = 7;
    private int bothWeaponsAcceleration = 5;
    private int onlySwordAcceleration = 8;
    private int helmetAcceleration = 9;
    private bool hasFlipped = false;

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

        if (inputVector.x < 0.0f && !hasFlipped && !bow.GetComponent<Bow>().isCharging)
        {
            FlipPlayer();
        }
        else if (inputVector.x > 0.0f && hasFlipped && !bow.GetComponent<Bow>().isCharging)
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

        //swordSpritePos.localPosition = new Vector3(-swordSpritePos.localPosition.x, swordSpritePos.localPosition.y, 0);
        //swordSpritePos.eulerAngles = new Vector3(0, 0, 58);

        hasFlipped = false;
    }

    void CheckAcceleration()
    {
        if (isSlowedDown) return;
        if (LevelController.playerHasBow && LevelController.playerHasSword && LevelController.currentPlayerWeapon.Name.Contains("Bow"))
        {
            acceleration = bothWeaponsAcceleration;
            normalAcceleration = bothWeaponsAcceleration;
        }
        else if (LevelController.playerHasSword && (!LevelController.playerHasBow || LevelController.currentPlayerWeapon.Name.Contains("Sword")))
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
}