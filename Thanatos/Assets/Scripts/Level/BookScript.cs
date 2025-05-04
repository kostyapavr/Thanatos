using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookScript : MonoBehaviour
{
    public GameObject panels;
    public GameObject bookPanel;
    public GameObject bookPanel2;
    public GameObject bookPanel3;
    public GameObject interactTip;
    public GameObject emptyText;
    public GameObject emptyText2;

    public List<GameObject> itemDescriptions;
    public List<GameObject> armorDescriptions;
    public List<GameObject> weaponDescriptions;
    bool canInteract = false;
    [HideInInspector]
    public bool isOpen = false;
    private int cnt = 0;
    private int cntArmor = 0;

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
        emptyText.SetActive(false);
        emptyText2.SetActive(false);
        cnt = 0;
        cntArmor = 0;

        for (int i = 0; i < itemDescriptions.Count; i++)
        {
            if (PlayerPrefs.GetInt(itemDescriptions[i].name, 0) == 1)
            {
                itemDescriptions[i].SetActive(true);
                cnt++;
            }
        }
        for (int i = 0; i < armorDescriptions.Count; i++)
        {
            if (PlayerPrefs.GetInt(armorDescriptions[i].name, 0) == 1)
            {
                armorDescriptions[i].SetActive(true);
                cntArmor++;
            }
        }
        for (int i = 0; i < weaponDescriptions.Count; i++)
        {
            if (PlayerPrefs.GetInt(weaponDescriptions[i].name, 0) == 1)
            {
                weaponDescriptions[i].SetActive(true);
            }
        }

        if (cnt == 0) emptyText.SetActive(true);
        if (cntArmor == 0) emptyText2.SetActive(true);
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
        bookPanel3.SetActive(false);
    }

    public void OpenSecondPage()
    {
        bookPanel.SetActive(false);
        bookPanel2.SetActive(true);
        bookPanel3.SetActive(false);
    }

    public void OpenThirdPage()
    {
        bookPanel.SetActive(false);
        bookPanel2.SetActive(false);
        bookPanel3.SetActive(true);
    }
}
