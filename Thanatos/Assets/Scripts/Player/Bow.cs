using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bow : MonoBehaviour
{
    private float time;
    public float startTime;
    public float offset;

    public GameObject Arrow;
    public Transform point;

    public int arrowSpeed;

    void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotatez = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotatez + offset);

        if (time <= 0f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Rigidbody2D rb = Instantiate(Arrow, point.position, transform.rotation).GetComponent<Rigidbody2D>();
                rb.AddForce(difference.normalized * arrowSpeed, ForceMode2D.Impulse);
                time = startTime;
            }
        }
        else
        {
            time -= Time.deltaTime;
        }
    }
}