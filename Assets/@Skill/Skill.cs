using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public string skill_Type;

    public int damage; // 스킬 데미지
    public float knockbackForce; // 넉백 힘
    public float duration; // 몬스터 피격시 이동불가 지속 시간

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
