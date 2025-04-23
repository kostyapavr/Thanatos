using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    private PlayerMovement player;
    private bool isRunning = false;
    private float tmr = 0.0f;
    private float animDelay = 0.075f;
    private int curSprite = 0;

    public SpriteRenderer legs;
    public List<Sprite> legSprites;

    void Start()
    {
        player = GetComponent<PlayerMovement>();
    }

    
    void FixedUpdate()
    {
        if (isRunning)
        {
            if (tmr <= 0.0f)
            {
                if (curSprite >= legSprites.Count) curSprite = 0;
                legs.sprite = legSprites[curSprite];
                tmr = animDelay / (player.acceleration/7.0f);
                curSprite++;
            }
            else tmr -= Time.deltaTime;
        }
    }

    public void SetRunning()
    {
        isRunning = true;
    }

    public void SetStopped()
    {
        isRunning = false;
        if (legs != null && legs.sprite != legSprites[0]) legs.sprite = legSprites[0];
    }
}
