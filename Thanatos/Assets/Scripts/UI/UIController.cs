using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private Player player;
    private int numberOFLives;
    public GameObject hpIcon;

    public GameObject arrowIcon;
    public TMP_Text arrowText;

    private Image[] lives;

    void Start()
    {
        numberOFLives = ResourceManager.Instance.MaxPlayerHP;
        lives = new Image[numberOFLives];
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<IDamageable>() as Player;
        SpawnIcons();
        Invoke("UpdateHealth", 0.1f);
        Invoke("UpdateArrows", 0.1f);

        LevelController.playerHpEvent.AddListener(UpdateHealth);
        LevelController.playerArrowShootEvent.AddListener(UpdateArrows);
    }

    void SpawnIcons()
    {
        if (hpIcon != null)
        {
            for (int i = numberOFLives - 1; i >= 0; i--)
            {
                Image img = Instantiate(hpIcon, transform.GetChild(0)).transform.GetChild(0).GetComponent<Image>();
                lives[i] = img;
            }
        }

        if (LevelController.playerHasBow)
        {
            arrowIcon.SetActive(true);
        }
    }

    void UpdateHealth()
    {
        float hp = player.currentHealth;
        for (int i = 0; i < lives.Length; i++)
        {
            float d = Mathf.Clamp(hp, 0, 1);
            lives[i].fillAmount = d;
            hp-=d;
        }
    }

    void UpdateArrows()
    {
        int currentArrows = player.GetCurrentArrows();
        arrowText.text = currentArrows.ToString();
    }
}