using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    private PlayerMovement player;
    private bool isRunning = false;
    private float tmr = 0.0f;
    private float animDelay = 0.25f;
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
                legs.sprite = legSprites[(curSprite + 1) % legSprites.Count];
                tmr = animDelay;
            }
            else tmr -= Time.fixedDeltaTime;
        }
        else
        {
            if (legs != null && legs.sprite != legSprites[0]) legs.sprite = legSprites[0];
        }
    }
}
