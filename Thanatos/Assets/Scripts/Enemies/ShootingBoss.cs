using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingBoss : ShootingEnemy
{
    public GameObject hpBar;
    public Image hpBarFill;
    public Transform fireballGrowPosition;

    public GameObject fireballGrow;
    public float maxGrowSize;

    private GameObject fb_grow;
    bool growFireball = false;

    public GameObject lavaBoots;
    public int largeAttackChance;

    private bool doLargeAttack = false;
    private float tmr = 0.0f;
    private float fireballDelay = 0.056f;
    private int fireballCnt = 0;
    private int curFireballCnt = 0;
    private float playerAngle = 0f;
    private Vector2 df = Vector2.zero;

    private bool skipAttacks = false;
    private int skipAttacksCnt = 0;
    private int maxSkipAttacksAfterLarge = 2;

    private int normalAttackCnt = 0;

    public AudioSource audioSource;
    public AudioClip largeAttackClip;

    protected override void Shoot()
    {
        if (skipAttacks)
        {
            if (skipAttacksCnt < maxSkipAttacksAfterLarge)
            {
                skipAttacksCnt++;
                return;
            }
            else
            {
                skipAttacks = false;
                skipAttacksCnt = 0;
            }
        }

        Vector2 diff = player.transform.position - fireballGrowPosition.position;
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        float spreadAngle = angle + Random.Range(-ammoSpread, ammoSpread + 1);
        Vector2 direction = Quaternion.AngleAxis(spreadAngle - angle, Vector3.forward) * diff.normalized;

        if (Random.Range(0, 100) < largeAttackChance && normalAttackCnt > 5)
        {
            LargeAttack(diff, angle);
            return;
        }

        if (!PlayerInSight()) return;

        if (!spriteRenderer.flipX && direction.x < 0)
        {
            fireballGrowPosition.localPosition = new Vector3(-fireballGrowPosition.localPosition.x, fireballGrowPosition.localPosition.y, 0);
            if (fb_grow != null) fb_grow.transform.localPosition = new Vector3(-fb_grow.transform.localPosition.x, fb_grow.transform.localPosition.y, 0);
            spriteRenderer.flipX = true;
        }
        else if (spriteRenderer.flipX && direction.x > 0)
        {
            fireballGrowPosition.localPosition = new Vector3(-fireballGrowPosition.localPosition.x, fireballGrowPosition.localPosition.y, 0);
            if (fb_grow != null) fb_grow.transform.localPosition = new Vector3(-fb_grow.transform.localPosition.x, fb_grow.transform.localPosition.y, 0);
            spriteRenderer.flipX = false;
        }

        GameObject fireball = Instantiate(ammoPrefab, fireballGrowPosition.position, Quaternion.Euler(0, 0, spreadAngle + 270));
        fireball.GetComponent<Fireball>().OwnerType = "Enemy";
        fireball.GetComponent<Fireball>().OwnerID = gameObject.GetInstanceID();
        fireball.GetComponent<Fireball>().Damage = 1.0f;
        fireball.GetComponent<Fireball>().doAoa = true;
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * ammoSpeed + GetComponent<Rigidbody2D>().velocity, ForceMode2D.Impulse);
        growFireball = false;
        normalAttackCnt++;
    }

    protected override void ShowDamage()
    {
        if (!hpBar.activeSelf) hpBar.SetActive(true);
        hpBarFill.fillAmount = currentHealth / health;
        base.ShowDamage();
    }

    public override void FixedUpdate()
    {
        if (growFireball && fb_grow != null)
        {
            if (fb_grow.transform.localScale.x < maxGrowSize)
                fb_grow.transform.localScale += Vector3.one * Time.deltaTime * (2 / shootInterval) * 0.1f;
        }
        else
        {
            if (fb_grow == null)
                fb_grow = Instantiate(fireballGrow, fireballGrowPosition.position, Quaternion.identity, transform);
            fb_grow.transform.localScale = Vector3.zero;
            growFireball = true;
        }

        if (doLargeAttack)
        {
            if (curFireballCnt >= fireballCnt)
            {
                doLargeAttack = false;
                curFireballCnt = 0;
                tmr = fireballDelay;
                skipAttacks = true;
            }

            if (tmr <= 0.0f)
            {
                float spreadAngle = playerAngle + Random.Range(-ammoSpread * 9, ammoSpread * 9 + 1);
                Vector2 direction = Quaternion.AngleAxis(spreadAngle - playerAngle, Vector3.forward) * df.normalized;

                GameObject fireball = Instantiate(ammoPrefab, fireballGrowPosition.position, Quaternion.Euler(0, 0, spreadAngle + 270));
                fireball.GetComponent<Fireball>().OwnerType = "Enemy";
                fireball.GetComponent<Fireball>().OwnerID = gameObject.GetInstanceID();
                fireball.GetComponent<Fireball>().Damage = 1.0f;
                fireball.GetComponent<Fireball>().doAoa = false;
                Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
                rb.AddForce(direction * ammoSpeed + GetComponent<Rigidbody2D>().velocity, ForceMode2D.Impulse);

                growFireball = false;
                tmr = fireballDelay;
                curFireballCnt++;
            }
            else tmr -= Time.deltaTime;
        }
    }

    protected override void SpawnRandomItem()
    {
        int rnd = Random.Range(0, 100);
        
        if (rnd < itemDropChance)
        {
            int startInd = 0;
            if (LevelController.playerHasHelmet) startInd = 1;
            if (LevelController.playerHasArmor) startInd = 2;
            int rndInd = Random.Range(startInd, dropItems.Length);
            Instantiate(dropItems[rndInd], transform.position, Quaternion.identity);
        }
        
        if (lavaBoots != null)
        {
            Instantiate(lavaBoots, transform.position + Vector3.right * 4, Quaternion.identity);
        }
    }

    public override void Die()
    {
        LevelController.bossDeathEvent.Invoke();
        GameObject portal = GameObject.Find("ExitPortal");
        portal.GetComponent<SpriteRenderer>().enabled = true;
        portal.GetComponent<Collider2D>().enabled = true;
        SpawnRandomItem();

        GameObject pb = GameObject.Find("PandoraBox");
        if (pb)
        {
            pb.GetComponent<PandoraBox>().RevealBox();
        }

        Destroy(gameObject);
    }

    public override void TakeDamage(float damage, GameObject sender = null, DamageEffects damageEffect = DamageEffects.Nothing)
    {
        base.TakeDamage(damage, sender, damageEffect);
    }

    public void LargeAttack(Vector2 diff, float pAngle)
    {
        fireballCnt = 16;
        doLargeAttack = true;
        tmr = fireballDelay;
        playerAngle = pAngle;
        df = diff;
        growFireball = false;
        normalAttackCnt = 0;

        audioSource.clip = largeAttackClip;
        audioSource.Play();
    }
}
