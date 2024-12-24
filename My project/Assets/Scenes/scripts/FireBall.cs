using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float timeDestroy = 3f;
    public float speed = 3f;
    public Rigidbody2D rb;
    public LayerMask Enemy;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector3 diference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotateZ = Mathf.Atan2(diference.y, diference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotateZ);

        rb.velocity = transform.right * speed;
        Invoke("DestroyBullet", timeDestroy);
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            collider.gameObject.GetComponent<Entity>().GetDamage();
        }
    }



    void DestroyBullet()
    {
        Destroy(this.gameObject);
    }
}
