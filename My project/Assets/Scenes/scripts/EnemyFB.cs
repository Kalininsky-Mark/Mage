using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFB : MonoBehaviour
{
    public float timeDestroy = 3f;
    public float speed = 3f;
    public Rigidbody2D rb;
    public LayerMask Heros;
    public LayerMask Tiles;
    public Transform target;


    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Vector2 diference = target.position - transform.position;
        float rotateZ = Mathf.Atan2(diference.y, diference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotateZ); rb.velocity = transform.right * speed;

        rb.velocity = transform.right * speed;
        Invoke("DestroyBullet", timeDestroy);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Hero.Instance.GetDamage();
        }
    }



    void DestroyBullet()
    {
        Destroy(this.gameObject);
    }
}
