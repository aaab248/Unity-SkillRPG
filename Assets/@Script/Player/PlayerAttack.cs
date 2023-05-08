using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("--------[ Attack ]")]
    public bool is_Attack = false;
    public float attack_Time = 0f;
    public int attack_Index = 0;

    public Weapon pWeapon; // 현재 무기 클래스 변수

    public LayerMask enemyLayer;
    public List<Enemy> Enemies = new List<Enemy>(); // 플레이어가 공격 중인 적 리스트

    private void Start()
    {
        pWeapon = GameManager.instance.current_Weapon;
    }

    private void Update()
    {
        // 공격하지 않은 시간 계산
        attack_Time += Time.deltaTime;

        // 무기 바뀌면 무기 변수 교체 후 인덱스 및 시간 초기화
        if (pWeapon != GameManager.instance.current_Weapon)
        {
            attack_Time = 0f;
            attack_Index = 0;
            pWeapon = GameManager.instance.current_Weapon;
        }

        // 일정 시간 공격 안하면 공격 인덱스 0 초기화
        if (attack_Time >= (pWeapon.attack_speed * 2f))
        {
            attack_Index = 0;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Player_Attack();
        }
    }

    void Player_Attack()
    {
        if (is_Attack == false)
        {
            // 공격시 시간 초기화
            attack_Time = 0f;
            is_Attack = true;

            // 범위에 있는 적들 검색
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, pWeapon.attack_Range, enemyLayer);

            // 콜라이더 배열에 있는 적들을 검색 -> 피격 중인 적 리스트에 없으면 데미지입력 함수 호출 및 리스트에 추가
            foreach (Collider2D enemy in hitEnemies)
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();

                if (enemyScript != null && !Enemies.Contains(enemyScript))
                {
                    enemyScript.TakeDamage(pWeapon.damage, 0.3f);
                    Enemies.Add(enemyScript);
                }
            }
            // 공격 딜레이 -> 리스트 초기화
            StartCoroutine(AttackDelay(pWeapon.attack_speed));

            // 공격 인덱스 초기화
            attack_Index++;
         }
    }
    IEnumerator AttackDelay(float delay_Time)
    {
        yield return new WaitForSeconds(delay_Time);
        Enemies.Clear(); // 리스트 내용 삭제
        is_Attack = false;
    }
}
