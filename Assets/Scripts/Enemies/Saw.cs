using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    [SerializeField]
    private List<Transform> pathTransform;
    private List<Vector3> path = new List<Vector3>(2);

    [SerializeField]
    private float speed;

    private Vector3 rotationSpeed = new Vector3(0, 0, 500);

    private bool goAhead;
    private int i = 0;

    private Vector3 target;

    private void Start()
    {
        foreach (var transform in pathTransform)
        {
            path.Add(transform.position);
            Debug.Log(transform.position);
        }
    }

    private void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
        Move();
    }

    private void Move()
    {
        if (i == path.Count - 1) goAhead = false;
        else if (i == 0) goAhead = true;

        if (goAhead) target = path[i + 1];
        else target = path[i - 1];

        var distanceToTarget = (target - transform.position).magnitude;
        var directionToTarget = (target - transform.position).normalized;

        if (distanceToTarget >= 0.05)
        {
            transform.position = transform.position + new Vector3(directionToTarget.x * speed * Time.deltaTime, directionToTarget.y * speed * Time.deltaTime);
        }
        else
        {
            if (goAhead) i++;
            else i--;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().GetDamage();
        }
    }
}
