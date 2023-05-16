using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator anime;

    public Transform[] childs;

    [Header("--------[ Health ]")]
    public float maxHealth;
    public float currentHealth;
    public bool isHit = false;

    [Header("--------[ Move ]")]
    public Vector2 input_Vec;
    public float movePower;
    public float maxSpeed;
    public bool canMove = true;

    [Header("--------[ Jump ]")]
    public float jumpPower;
    public float jumpTime;
    public float max_JumpTime;
    public bool canJump = true;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anime = GetComponent<Animator>();
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

        // �̵� �� ���� �ִϸ��̼� �� ����
        anime.SetFloat("X", Mathf.Abs(input_Vec.x));
        anime.SetFloat("Y", rigid.velocity.y);

        if(rigid.velocity.y < -0.1f)
        {
            anime.SetBool("Fall", true);
        }

        // �ڽ� �� �ڽ� ��������Ʈ filp x , �ڽ� ���������� �ݴ��
        if(canMove == true)
        {
            Flip();
        }

        // Space Ű�Է� ���� �� �ִ� �����ð� �ʰ�
        if(Input.GetKeyUp(KeyCode.Space) || jumpTime > max_JumpTime)
        {
            canJump = false;
            jumpTime = 0f;
        }

        // �¿� �̵� Ű �Է� ���Ž� �ӵ�����
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.1f, rigid.velocity.y);
        }
    }

    private void FixedUpdate()
    {
        if (canMove == true)
        {
            Player_Move();
            Player_Jump();
        }
    }

    void Flip() // �¿� �ٶ󺸴� ���⿡ ����
    {
        if (input_Vec.x < 0)
        {
            sprite.flipX = true;
            foreach (Transform child in childs)
            {
                child.localPosition = new Vector3(-0.75f, 0f, 0f);
            }
        }
        else if (input_Vec.x > 0)
        {
            sprite.flipX = false;
            foreach (Transform child in childs)
            {
                child.localPosition = new Vector3(0.75f, 0f, 0f);
            }
        }
    }
    // �÷��̾� �뽬
    void Player_Dash()
    {
    }

    // �÷��̾� �̵�
    private void Player_Move()
    {

        //addforce �̿�
        rigid.AddForce(Vector2.right * input_Vec.x * movePower, ForceMode2D.Force);
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

            if (canJump == false)
            {
                return;
            }

            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

            anime.SetTrigger("JumpTrigger");

            // ���� �ð� ���
            jumpTime += Time.deltaTime;
        }     
    }
    public void FallAnimeStart()
    {
        anime.SetBool("Fall", true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ���� ����
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            anime.SetBool("Fall", false);
            anime.SetBool("WallSlide", false);

            canJump = true;
        }
        // ���� �پ��� �� �ִϸ��̼� ���� �� ���� �ʱ�ȭ
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            anime.SetBool("WallSlide", true);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            // �÷��̾� �������� ���� ��ġ�� ����, ������ Ȯ��
            int dirVec_X = transform.position.x > collision.transform.position.x ? 1 : -1;
            // �� ���� Ȯ��
            Enemy EnemyInfo = collision.gameObject.GetComponent<Enemy>();

            // �� ������ ���� �˹� �� �÷��̾� �ǰ� ������
            rigid.AddForce(new Vector2(dirVec_X, 5f).normalized * EnemyInfo.attack_KnockBack, ForceMode2D.Impulse);
            Player_TakeDamage(EnemyInfo.attack_Dmg);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            anime.SetBool("WallSlide", false);
        }
    }

    // ... ��ٸ� Test
    private void OnTriggerStay2D (Collider2D collision)
    {
        if(collision.gameObject.CompareTag("ladder"))
        {
            float h = Input.GetAxis("Vertical");
            if(h != 0)
            {
                rigid.gravityScale = 0;
                transform.Translate(new Vector2(transform.position.x, transform.position.y + h * 0.1f));
            }
            else
            {
                rigid.gravityScale = 3;
            }
        }
    }

    void Player_TakeDamage(float damage)
    {
        if(isHit)
        {
            return;
        }

        AttackEnd();

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Player_Die();
            return;
        }

        isHit = true;
        canMove = false;

        anime.SetTrigger("HitTrigger");
        anime.SetBool("IsHit", true);

        GameManager.instance.SetPlayerStats(currentHealth);
    }

    void HitAnimationEnd()
    {
        anime.SetBool("IsHit", false);
        isHit = false;
        canMove = true;
    }



    void Player_Die()
    {
        anime.SetTrigger("Death");

        // �ݶ��̴� �� rigid simulated, ��ũ��Ʈ ��Ȱ��ȭ
        rigid.simulated = false;
        transform.GetComponent<Collider2D>().enabled = false;
        transform.GetComponent<PlayerController>().enabled = false;
        // �ڽ� ������Ʈ ��Ȱ��ȭ
        foreach (Transform child in childs)
        {
            child.gameObject.SetActive(false);
        }
        // ... ĳ���� �ִϸ��̼� ���
        // ... ĳ���� ���� ������Ʈ ����
        // ... ui ���� ��ũ��Ʈ�� ���� ���� Ȯ�� â ���� -> ���� �� �� ��ȯ
    }



    // ���� �ִϸ��̼� ����� ȣ��
    void AttackAnimationEnd()
    {
        StartCoroutine(AttackAnimationEndCor());
    }
    IEnumerator AttackAnimationEndCor()
    {
        yield return new WaitForSeconds(0.01f);
        AttackEnd();
    }
    
    // ������ ���� �� ȣ�� ( �ִϸ��̼� ����, �÷��̾� �ǰ� ��)
    void AttackEnd()
    {
        canMove = true;
        canJump = true;

        childs[0].GetComponent<PlayerAttack>().is_Attack = false;
        childs[0].GetComponent<PlayerAttack>().Enemies.Clear(); // ����Ʈ ����
        anime.SetBool("Attack", false);
    }
}