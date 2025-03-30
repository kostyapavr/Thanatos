using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookScript : MonoBehaviour
{
    public GameObject panels;
    public GameObject bookPanel;
    public GameObject bookPanel2;
    public GameObject interactTip;
    public GameObject nextButton;
    public GameObject prevButton;

    public List<GameObject> itemDescriptions;
    public List<GameObject> armorDescriptions;
    bool canInteract = false;
    [HideInInspector]
    public bool isOpen = false;

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
        panels.SetActive(true);
        isOpen = true;

        for (int i = 0; i < itemDescriptions.Count; i++)
        {
            if (PlayerPrefs.GetInt(itemDescriptions[i].name, 0) == 1) itemDescriptions[i].SetActive(true); 
        }
        for (int i = 0; i < armorDescriptions.Count; i++)
        {
            if (PlayerPrefs.GetInt(armorDescriptions[i].name, 0) == 1) armorDescriptions[i].SetActive(true);
        }
    }

    public void CloseBook()
    {
        Time.timeScale = 1;
        OpenFirstPage();
        panels.SetActive(false);
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

            if (Input.GetKeyDown(KeyCode.Escape) && isOpen) CloseBook();
        }
    }

    public void OpenFirstPage()
    {
        bookPanel.SetActive(true);
        bookPanel2.SetActive(false);
        nextButton.SetActive(true);
        prevButton.SetActive(false);
    }

    public void OpenSecondPage()
    {
        bookPanel.SetActive(false);
        bookPanel2.SetActive(true);
        nextButton.SetActive(false);
        prevButton.SetActive(true);
    }
}
