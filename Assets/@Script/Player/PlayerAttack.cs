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

    public LayerMask Layer; // �˻� Layer
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
        if (attack_Time >= 2.0f)
        {
            attack_Index = 0;
            attack_Time = 0f;
        }

        if (Input.GetKeyDown(KeyCode.A) && player.isHit == false)
        {
            Player_Attack();
        }
    }

    void Player_Attack()
    {
        if (is_Attack == false && player.canJump == true )
        {
            // ���ݽ� �ð� �ʱ�ȭ
            attack_Time = 0f;
            is_Attack = true;

            player.canMove = false;
            playerRigid.velocity = Vector2.zero;


            playerAnime.SetBool("Attack", true);
            playerAnime.SetInteger("AttackIndex", attack_Index);

            StartCoroutine(AttackObject());

            // ���� �ε��� �߰� �� ���� ��� ����
            attack_Index++;
            attack_Index = attack_Index % 3;
         }
    }

    IEnumerator AttackObject()
    {
        // ������ �ִ� ���� �˻�
        Collider2D[] hit_Objs = Physics2D.OverlapCircleAll(transform.position, 0.6f, Layer);

        yield return new WaitForSeconds(0.1f);

        // �ݶ��̴� �迭�� �ִ� ������ �˻� -> �ǰ� ���� �� ����Ʈ�� ������ �������Է� �Լ� ȣ�� �� ����Ʈ�� �߰�
        foreach (Collider2D hit_Object in hit_Objs)
        {
            switch(hit_Object.tag)
            {
                // �� ������Ʈ ����
                case "Enemy":

                    Enemy enemyScript = hit_Object.GetComponent<Enemy>();

                    if (enemyScript != null && !Enemies.Contains(enemyScript))
                    {
                        // ���� ���� �� ī�޶� ��鸲
                        Camera.main.GetComponent<CameraController>().DoShakeCamera(0.2f, 0.1f);
                        // �� �ǰ��Լ� ȣ��
                        enemyScript.TakeDamage(1.0f, 0.3f);
                        // �ڷ� �˹�
                        hit_Object.GetComponent<Rigidbody2D>().AddForce(new Vector2(1f, 2f).normalized * 3f, ForceMode2D.Impulse);

                        // ����Ʈ�� �ǰ� �� �߰�
                        Enemies.Add(enemyScript);
                        // �޺� Ÿ�̸� ����
                        GameManager.instance.StartComboTimer();
                        GameManager.instance.hitCombo_Num++;
                    }
                    break;

                // �ڽ� ������Ʈ ����
                case "Box":

                    Box boxScript = hit_Object.GetComponent<Box>();
                    boxScript.Box_Damaged();
                    break;
            }

         
        }
    }
}
