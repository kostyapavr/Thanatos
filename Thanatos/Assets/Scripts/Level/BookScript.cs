using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookScript : MonoBehaviour
{
    public GameObject bookPanel;
    public GameObject interactTip;
    bool canInteract = false;
    bool isOpen = false;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider == null) return;
        if (collider.GetComponent<Player>())
        {
            ShowInteractTip();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider == null) return;
        if (collider.GetComponent<Player>())
        {
            HideInteractTip();
        }
    }

    private void ShowInteractTip()
    {
        interactTip.SetActive(true);
        canInteract = true;
    }

    private void HideInteractTip()
    {
        interactTip.SetActive(false);
        canInteract = false;
    }

    public void OpenBook()
    {
        Time.timeScale = 0;
        bookPanel.SetActive(true);
        isOpen = true;
    }

    public void CloseBook()
    {
        Time.timeScale = 1;
        bookPanel.SetActive(false);
        isOpen = false;
    }

    private void Update()
    {
        if (canInteract)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isOpen) OpenBook();
                else CloseBook();
            }
        }
    }
}
