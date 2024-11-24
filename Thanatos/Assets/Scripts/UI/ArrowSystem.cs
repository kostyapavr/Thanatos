using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ArrowSystem : MonoBehaviour
{
    
    public int numberOfArrows;

    public Image[] Arrow;

    public Sprite FullArrows;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Arrow.Length; i++)
        {
            if (i < numberOfArrows)
            {
                Arrow[i].enabled = true;
            }
            else
            {
                Arrow[i].enabled = false;
            }
        }
    }  
}
