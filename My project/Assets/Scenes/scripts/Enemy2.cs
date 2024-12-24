using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Entity
{
    private Transform target;
    public float speed = 2f;
    public Rigidbody2D rb;

    private void Start()
    {
        lives = 5;
    }

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        if ((Vector2.Distance(transform.position, target.position) < 5)) Go();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage();
            lives--;
            Debug.Log("здоровье противника" + lives);
            if (lives < 1)
                Die();
        }
    }
    private void Go()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }
}
