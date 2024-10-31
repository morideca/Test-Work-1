using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Killer : MonoBehaviour
{
    private Vector2 upPoint;
    private Vector2 downPoint;

    [SerializeField]
    private float heigh;
    [SerializeField]
    private float speedUp;
    [SerializeField]
    private float speedDown;

    [SerializeField]
    private float cooldown;

    private bool push = false;

    void Start()
    {
        downPoint = transform.position;
        upPoint = new Vector2(transform.position.x, transform.position.y + heigh);
    }

    void Update()
    {
        if (push) Push();
        else RiseUp();
    }

    private void Push()
    {
        if (transform.position.y > downPoint.y)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - speedDown * Time.deltaTime);
        }
        else if (push) Invoke("PushOff", cooldown);
    }

    private void RiseUp()
    {
        if (transform.position.y < upPoint.y)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + speedUp * Time.deltaTime);
        }
        else if (!push) PushOn();
    }

    private void PushOn()
    {
        push = true;
    }

    private void PushOff()
    {
        push = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().GetDamage();
        }
    }
}
