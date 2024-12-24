using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public bool isReadyToCast = true;
    public GameObject fireball;
    private Transform target;

    private void Start()
    {
        lives = 5;
    }

    private void Update()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (isReadyToCast && (Vector2.Distance(transform.position, target.position) < 5)) Cast();
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
    private void Cast()
    {
        isReadyToCast = false;
        {
            Instantiate(fireball, transform.position, Quaternion.identity);
        }
        StartCoroutine(CastCoolDown());
    }
    private IEnumerator CastCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        isReadyToCast = true;
    }
}
