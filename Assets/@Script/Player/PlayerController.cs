using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator anime;

    public PlayerAttack attack;
    public Transform[] childs;

    [Header("--------[ Health ]")]
    public float maxHealth;
    public float currentHealth;

    [Header("--------[ Move ]")]
    public Vector2 input_Vec;
    public float movePower;
    public float maxSpeed;
    public bool canMove = true;

    [Header("--------[ Jump ]")]
    public float jumpPower;
    public float jumpTime;
    public float max_JumpTime;
    public bool is_Jump;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        // anime = GetComponent<Animator>();
    }

    private void Start()
    {
        // ���� ������ ����� ü�°� ���� -> ���� �ҷ��� �÷��̾��� ü�°��� ����
        if (GameManager.instance != null)
        {
            float health = GameManager.instance.GetPlayerStats();
        }

    }
    private void Update()
    {
        input_Vec.x = Input.GetAxisRaw("Horizontal");



        // �ڽ� �� �ڽ� ��������Ʈ filp x , �ڽ� ���������� �ݴ��
        Flip();

        // Space Ű�Է� ���� �� �ִ� �����ð� �ʰ�
        if (Input.GetKeyUp(KeyCode.Space) || jumpTime > max_JumpTime)
        {
            is_Jump = true;
            jumpTime = 0f;
        }

        // �¿� �̵� Ű �Է� ���Ž� �ӵ�����
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.3f, rigid.velocity.y);
        }
    }

    private void FixedUpdate()
    {
        if(canMove == true)
        {
            Player_Move();
            Player_Jump();
        }
    }

    public void AttackAnimeEnd()
    {
        canMove = true;
    }

    void Flip()
    {
        if (input_Vec.x < 0)
        {
            sprite.flipX = true;
            foreach (Transform child in childs)
            {
                child.GetComponent<SpriteRenderer>().flipX = true;
                child.localPosition = new Vector3(-1f, 0f, 0f);
            }
        }
        else if (input_Vec.x > 0)
        {
            sprite.flipX = false;
            foreach (Transform child in childs)
            {
                child.GetComponent<SpriteRenderer>().flipX = false;
                child.localPosition = new Vector3(1f, 0f, 0f);
            }
        }
    }

    // �÷��̾� �̵�
    private void Player_Move()
    {
        float h;
        h = input_Vec.x;

        //addforce �̿�
        rigid.AddForce(Vector2.right * h * movePower, ForceMode2D.Force);
        if(rigid.velocity.x > maxSpeed)
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
        else if(rigid.velocity.x < maxSpeed * (-1))
        {
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        }
    }
    //�÷��̾� ����
    private void Player_Jump()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            if (is_Jump)
            {
                return;
            }
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            // ���� �ð� ���
            jumpTime += Time.deltaTime;
        }     
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StartCoroutine(JumpDelay());
        }
    }
    IEnumerator JumpDelay()
    {
        yield return new WaitForSeconds(0.1f);
        is_Jump = false;
    }

    void Player_TakeDamage(float damage)
    {
        currentHealth -= damage;
        GameManager.instance.SetPlayerStats(currentHealth);

        if(currentHealth < 0)
        {
            Player_Die();
        }
    }

    void Player_Die()
    {
        // ... ĳ���� �ִϸ��̼� ���
        // ... ĳ���� ���� ������Ʈ ����
        // ... ui ���� ��ũ��Ʈ�� ���� ���� Ȯ�� â ���� -> ���� �� �� ��ȯ
    }
}