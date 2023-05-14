using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("--------[ Attack ]")]
    public bool is_Attack = false;
    public float attack_Time = 0f;
    public int attack_Index = 0;

    public PlayerController player;

    Rigidbody2D playerRigid;
    Animator playerAnime;

    public Weapon pWeapon; // ���� ���� Ŭ���� ����

    public LayerMask enemyLayer; // �� Layer
    public List<Enemy> Enemies = new List<Enemy>(); // �÷��̾ ���� ���� �� ����Ʈ

    private void Awake()
    {
        playerRigid = transform.GetComponentInParent<Rigidbody2D>();
        playerAnime = transform.GetComponentInParent<Animator>();
    }
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

        if (Input.GetKeyDown(KeyCode.A) && player.isHit == false)
        {
            Player_Attack();
        }
    }

    void Player_Attack()
    {
        if (is_Attack == false && playerRigid.velocity.y == 0 )
        {
            // ���ݽ� �ð� �ʱ�ȭ
            attack_Time = 0f;
            is_Attack = true;

            player.canMove = false;
            playerRigid.velocity = Vector2.zero;


            playerAnime.SetBool("Attack", true);
            playerAnime.SetInteger("AttackIndex", attack_Index);

            StartCoroutine(AttackObject());

            // ���� �ε��� �߰� �� 0,1,2 ����
            attack_Index++;
            attack_Index = attack_Index % 3;
         }
    }

    IEnumerator AttackObject()
    {
        // ������ �ִ� ���� �˻�
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 0.6f, enemyLayer);

        yield return new WaitForSeconds(0.1f);

        // �ݶ��̴� �迭�� �ִ� ������ �˻� -> �ǰ� ���� �� ����Ʈ�� ������ �������Է� �Լ� ȣ�� �� ����Ʈ�� �߰�
        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();

            if (enemyScript != null && !Enemies.Contains(enemyScript))
            {
                // ���� ���� �� ī�޶� ��鸲
                Camera.main.GetComponent<CameraController>().DoShakeCamera(0.2f, 0.1f);
                // �� �ǰ��Լ� ȣ��
                enemyScript.TakeDamage(pWeapon.damage, 0.3f);
                // �ڷ� �˹�
                enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(1f, 2f).normalized * 3f, ForceMode2D.Impulse);
                
                // ����Ʈ�� �ǰ� �� �߰�
                Enemies.Add(enemyScript);
                // �޺� Ÿ�̸� ����
                GameManager.instance.StartComboTimer();
                GameManager.instance.hitCombo_Num++;
            }

        }
    }
}
