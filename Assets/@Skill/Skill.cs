using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public string skill_Type;

    public int damage; // ��ų ������
    public float knockbackForce; // �˹� ��
    public float duration; // ���� �ǰݽ� �̵��Ұ� ���� �ð�

    public GameObject enemy_Obj;

    private void OnEnable()
    {
        // skill_Anime = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemy_Obj = collision.gameObject;
            UseSkill(enemy_Obj);
        }
    }
    public virtual void UseSkill(GameObject target) { }
}
