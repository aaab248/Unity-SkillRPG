using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    public string type;
    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            this.gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        float ran_X = Random.Range(-0.6f, 0.6f);
        float ran_Y = Random.Range(0f, 1.5f);
        rigid.AddForce(new Vector2(ran_X, ran_Y) * 8.0f, ForceMode2D.Impulse);
        Invoke("Stop_Move", 0.2f);
    }

    void Stop_Move()
    {
        rigid.velocity = Vector2.zero;
    }
}
