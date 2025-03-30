using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandoraBox : MonoBehaviour
{
    public GameObject interactButton;
    public GameObject[] dropItems;
    public Transform[] lootSpawns;
    public Sprite openedSprite;
    private bool canIteract = false;
    private bool boxRevealed = false;
    private bool boxOpened = false;

    private void Start()
    {
        HideBox();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (boxRevealed && !boxOpened)
        {
            canIteract = true;
            interactButton.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (boxRevealed && !boxOpened)
        {
            canIteract = false;
            interactButton.SetActive(false);
        }
    }

    private void Update()
    {
        if (canIteract)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!boxOpened) OpenBox();
            }
        }
    }

    private void OpenBox()
    {
        if (lootSpawns.Length == 0 || dropItems.Length == 0) return;

        foreach (Transform t in lootSpawns)
        {
            int rnd = Random.Range(0, dropItems.Length);
            Instantiate(dropItems[rnd], t.position, t.rotation);
        }

        GetComponent<SpriteRenderer>().sprite = openedSprite;
        boxOpened = true;
        interactButton.SetActive(false);
    }

    public void RevealBox()
    {
        boxRevealed = true;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public void HideBox()
    {
        boxRevealed = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
