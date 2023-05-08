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
        // 이전 씬에서 저장된 체력값 존재 -> 값을 불러와 플레이어의 체력값을 설정
        if (GameManager.instance != null)
        {
            float health = GameManager.instance.GetPlayerStats();
        }

    }
    private void Update()
    {
        input_Vec.x = Input.GetAxisRaw("Horizontal");



        // 자신 및 자식 스프라이트 filp x , 자식 로컬포지션 반대로
        Flip();

        // Space 키입력 제거 및 최대 점프시간 초과
        if (Input.GetKeyUp(KeyCode.Space) || jumpTime > max_JumpTime)
        {
            is_Jump = true;
            jumpTime = 0f;
        }

        // 좌우 이동 키 입력 제거시 속도제어
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

    // 플레이어 이동
    private void Player_Move()
    {
        float h;
        h = input_Vec.x;

        //addforce 이용
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
    //플레이어 점프
    private void Player_Jump()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            if (is_Jump)
            {
                return;
            }
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            // 점프 시간 계산
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
        // ... 캐릭터 애니메이션 재생
        // ... 캐릭터 관련 컴포넌트 제거
        // ... ui 관련 스크립트로 새로 시작 확인 창 생성 -> 마을 맵 씬 전환
    }
}