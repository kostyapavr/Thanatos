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

    private int slowAcceleration = 1;
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

        //swordSpritePos.localPosition = new Vector3(-swordSpritePos.localPosition.x, swordSpritePos.localPosition.y, 0);
        //swordSpritePos.eulerAngles = new Vector3(0, 0, 58);

        hasFlipped = false;
    }

    void CheckAcceleration()
    {
        if (isSlowedDown) return;
        if (LevelController.playerHasBow && LevelController.playerHasSword && LevelController.playerSelectedWeapon == 0)
        {
            acceleration = bothWeaponsAcceleration;
            normalAcceleration = bothWeaponsAcceleration;
        }
        else if (LevelController.playerHasSword && (!LevelController.playerHasBow || LevelController.playerSelectedWeapon == 1))
        {
            acceleration = LevelController.playerHasHelmet ? helmetAcceleration : onlySwordAcceleration;
            normalAcceleration = acceleration;
        }
    }

    void FixedUpdate()
    {
        rb.velocity += inputVector * acceleration;
    }

    public void SlowDown()
    {
        acceleration = slowAcceleration;
        isSlowedDown = true;
    }

    public void ReturnToNormalSpeed()
    {
        acceleration = normalAcceleration;
        isSlowedDown = false;
    }
}