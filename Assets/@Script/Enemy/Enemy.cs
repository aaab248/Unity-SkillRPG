using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemy_Name;

    public int max_Hp;
    public float current_Hp;

    public float attack_Dmg;
    public float attack_KnockBack;
    public float attack_Speed;

    public float move_Speed;

    public float FOV;

    // enemy �ǰ� �� �̵� �Ұ���
    public bool canMove = true;

    public GameObject itemA;
    public GameObject itemB;
    public GameObject itemC;

    Rigidbody2D rigid;
    Animator anime;
   
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
    }
  
    // �÷��̾��� �Ϲ� ���� �ǰ�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Attack" )
        {
            // ���� �˹�
            rigid.velocity = Vector2.zero;
            rigid.AddForce(new Vector2(0f, 3f), ForceMode2D.Impulse);

            TakeDamage(GameManager.instance.current_Weapon.damage, 0.3f);
            GameManager.instance.Set_Skill_Point(1);
        }
    }

    // ������ �ǰ� �� ������ ����
    public void TakeDamage(float damage, float duration)     
    {
        // ���� ��������ŭ ü�� ����
        current_Hp -= damage;

        if (current_Hp <= 0)
        {
            Die();
            Drop_Items();
        }

        //... �ǰ� �ִϸ��̼� ��� �� �ٽ� �ǰ� ���� �� ����
        anime.SetBool("IsIdle", false);
        if (anime.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            anime.Play("Hit", -1, 0f);
        }
        else
        {
            anime.SetTrigger("Hit");
        }

        StartCoroutine(DisableMovement(duration));
    }
    IEnumerator DisableMovement(float time)
    {
        // ���� �ð� ���� ���� ������ ����
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    // Hit �ִϸ��̼� ���� -> Idle �ִϸ��̼����� ��ȯ 
    public void OnHitAnimationEnd()
    {
        anime.SetBool("IsIdle", true);
    }

    // enemy ���
    void Die()
    {
        anime.SetTrigger("Dead");
        GetComponent<EnemyAI>().enabled = false;
        GetComponent <Collider2D>().enabled = false;
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(gameObject, 1f);
    }

    // ��� �� ������ ���
    void Drop_Items()
    {
        int ran = Random.Range(1, 4);
        for (int i = 0; i <= ran; i++)
        {
            int ran_Item = Random.Range(0, 10);
            if (ran_Item < 4)
            {
                Debug.Log("������ ����");
            }
            else if (ran_Item < 6)
            {
                Instantiate(itemA, transform.position, transform.rotation);
            }
            else if (ran_Item < 8)
            {
                Instantiate(itemB, transform.position, transform.rotation);
            }
            else if (ran_Item < 10)
            {
                Instantiate(itemC, transform.position, transform.rotation);
            }
        }
    }
}
