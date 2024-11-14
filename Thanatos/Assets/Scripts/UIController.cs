using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public int health;
    public int numberOFLives;	

    public Image[] lives;
    public Sprite FullLive;
    public Sprite HalfLive;
    public Sprite emptyLive;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < lives.Length; i++)
        {
            if (i < health)
            {
                lives[i].sprite = FullLive;
            }
           else
            {
                lives[i].sprite = emptyLive;
            }

            if (i < numberOFLives)
            {
                lives[i].enabled = true;
            }
            else
            {
                lives[i].enabled = false;
            }
        }
    }
}