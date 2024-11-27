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
        Invoke("UpdateHealth", 0.5f);

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
        for (int i = 0; i < lives.Length; i++)
        {
            float d = Mathf.Clamp(hp, 0, 1);
            lives[i].fillAmount = d;
            hp-=d;
        }
    }
}