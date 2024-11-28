using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Sprite basicHelmetSprite;
    public Sprite basicBowSprite;
    public Sprite basicSwordSprite;

    public bool hasBow = false;
    public bool hasSword = false;
    public bool hasHelmet = false;

    private SpriteRenderer player;

    void Start()
    {
        player = GetComponent<SpriteRenderer>();

        hasBow = LevelController.playerHasBow;
        hasSword = LevelController.playerHasSword;
        hasHelmet = LevelController.playerHasHelmet;

        if (hasBow) player.sprite = basicBowSprite;
        else if (hasSword && !hasBow) player.sprite = basicSwordSprite;
        
        if (hasHelmet && hasSword) player.sprite = basicHelmetSprite;
    }

    private void Update()
    {
        hasBow = LevelController.playerHasBow;
        hasSword = LevelController.playerHasSword;
        hasHelmet = LevelController.playerHasHelmet;

        if (hasBow) player.sprite = basicBowSprite;
        else if (hasSword) player.sprite = basicSwordSprite;

        if (hasHelmet && hasSword) player.sprite = basicHelmetSprite;
    }
}
