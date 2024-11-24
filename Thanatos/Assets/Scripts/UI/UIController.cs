using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private Player player;
    private int numberOFLives;
    public GameObject hpIcon;

    private Image[] lives;

    void Start()
    {
        numberOFLives = ResourceManager.Instance.maxPlayerHP;
        lives = new Image[numberOFLives];
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<IDamageable>() as Player;
        SpawnIcons();
        UpdateHealth();

        LevelController.playerHpEvent.AddListener(UpdateHealth);
    }

    void SpawnIcons()
    {
        if (hpIcon == null) return;
        for (int i = numberOFLives - 1; i >= 0; i--)
        {
            Image img = Instantiate(hpIcon, transform.GetChild(0)).transform.GetChild(0).GetComponent<Image>();
            lives[i] = img;
        }
    }

    void UpdateHealth()
    {
        float hp = player.currentHealth;
        int h = Mathf.Min(Mathf.FloorToInt(hp), numberOFLives);
        int i = 0;
        for (i = 0; i < h; i++)
        {
            lives[i].fillAmount = 1;
        }
        if (i != numberOFLives) lives[i].fillAmount = hp - h;
    }
}