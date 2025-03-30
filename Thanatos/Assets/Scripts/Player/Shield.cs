using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Player player;
    public LayerMask enemyLayers;
    bool shieldVisible = false;
    bool shieldActive = false;

    private float attackDelay = 1.5f;
    private float time = 0.0f;

    [HideInInspector]
    public bool anim = false;
    private int animCnt = 0;
    private int animMaxCnt = 12;

    private float animFrameInterval = 0.1f;
    private float animTime = 0.0f;

    private Vector3 animMoveVector = new Vector3(0.15f, 0, 0);
    private int vectMult = 1;

    public GameObject defendShieldPos;

    public void HideShield()
    {
        spriteRenderer.enabled = false;
        shieldActive = false;
        shieldVisible = false;
    }

    public void ShowShield()
    {
        spriteRenderer.enabled = true;
        shieldActive = false;
        shieldVisible = true;
    }

    private void Update()
    {
        if (shieldVisible)
        {
            if (time <= 0.0f && Input.GetMouseButtonDown(0) && !shieldActive)
            {
                Attack();
                time = attackDelay;
                anim = true;
                if (player.GetComponent<PlayerMovement>().hasFlipped) vectMult = -1;
                else vectMult = 1;
            }
            else time -= Time.deltaTime;

            if (Input.GetMouseButton(1))
            {
                shieldActive = true;
                defendShieldPos.SetActive(true);
                spriteRenderer.enabled = false;
                LevelController.playerShieldActive = true;
                player.GetComponent<PlayerMovement>().SlowDown();
            }
            
            if (Input.GetMouseButtonUp(1))
            {
                shieldActive = false;
                defendShieldPos.SetActive(false);
                spriteRenderer.enabled = true;
                LevelController.playerShieldActive = false;
                player.GetComponent<PlayerMovement>().ReturnToNormalSpeed();
            }
        }

        if (anim)
        {
            if (animTime <= 0.0f)
            {
                if (animCnt >= animMaxCnt)
                {
                    anim = false;
                    animCnt = 0;
                    animTime = animFrameInterval;
                }

                if (animCnt >= animMaxCnt / 2)
                {

                    transform.localPosition -= animMoveVector * vectMult;
                }
                else
                {
                    transform.localPosition += animMoveVector * vectMult;
                }

                animCnt++;
            }
            else animTime -= Time.deltaTime;
        }
    }

    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 5f, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (LevelController.currentPlayerWeapon == null)
            {
                Debug.Log("Current weapon is null!");
                return;
            }
            enemy.GetComponent<Enemy>().TakeDamage(LevelController.currentPlayerWeapon.Damage);
        }
    }
}
