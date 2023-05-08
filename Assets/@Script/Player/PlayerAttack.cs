using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("--------[ Attack ]")]
    public bool is_Attack = false;
    public float attack_Time = 0f;
    public int attack_Index = 0;

    public Weapon pWeapon; // ���� ���� Ŭ���� ����

    public LayerMask enemyLayer;
    public List<Enemy> Enemies = new List<Enemy>(); // �÷��̾ ���� ���� �� ����Ʈ

    private void Start()
    {
        pWeapon = GameManager.instance.current_Weapon;
    }

    private void Update()
    {
        // �������� ���� �ð� ���
        attack_Time += Time.deltaTime;

        // ���� �ٲ�� ���� ���� ��ü �� �ε��� �� �ð� �ʱ�ȭ
        if (pWeapon != GameManager.instance.current_Weapon)
        {
            attack_Time = 0f;
            attack_Index = 0;
            pWeapon = GameManager.instance.current_Weapon;
        }

        // ���� �ð� ���� ���ϸ� ���� �ε��� 0 �ʱ�ȭ
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
            // ���ݽ� �ð� �ʱ�ȭ
            attack_Time = 0f;
            is_Attack = true;

            // ������ �ִ� ���� �˻�
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, pWeapon.attack_Range, enemyLayer);

            // �ݶ��̴� �迭�� �ִ� ������ �˻� -> �ǰ� ���� �� ����Ʈ�� ������ �������Է� �Լ� ȣ�� �� ����Ʈ�� �߰�
            foreach (Collider2D enemy in hitEnemies)
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();

                if (enemyScript != null && !Enemies.Contains(enemyScript))
                {
                    enemyScript.TakeDamage(pWeapon.damage, 0.3f);
                    Enemies.Add(enemyScript);
                }
            }
            // ���� ������ -> ����Ʈ �ʱ�ȭ
            StartCoroutine(AttackDelay(pWeapon.attack_speed));

            // ���� �ε��� �ʱ�ȭ
            attack_Index++;
         }
    }
    IEnumerator AttackDelay(float delay_Time)
    {
        yield return new WaitForSeconds(delay_Time);
        Enemies.Clear(); // ����Ʈ ���� ����
        is_Attack = false;
    }
}
