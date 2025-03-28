using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandoraBox : MonoBehaviour
{
    public GameObject interactButton;
    public GameObject[] dropItems;
    public Transform[] lootSpawns;
    private bool canIteract = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canIteract = true;
        interactButton.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canIteract = false;
        interactButton.SetActive(false);
    }

    private void Update()
    {
        if (canIteract)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                OpenBox();
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
    }
}
