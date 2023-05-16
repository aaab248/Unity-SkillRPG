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
        // 이전 씬에서 저장된 체력값 존재 -> 값을 불러와 플레이어의 체력값을 설정
        if (GameManager.instance != null)
        {
            float health = GameManager.instance.GetPlayerStats();
        }

    }
    private void Update()
    {
        input_Vec.x = Input.GetAxisRaw("Horizontal");

        // 이동 및 점프 애니메이션 값 전달
        anime.SetFloat("X", Mathf.Abs(input_Vec.x));
        anime.SetFloat("Y", rigid.velocity.y);

        if(rigid.velocity.y < -0.1f)
        {
            anime.SetBool("Fall", true);
        }

        // 자신 및 자식 스프라이트 filp x , 자식 로컬포지션 반대로
        if(canMove == true)
        {
            Flip();
        }

        // Space 키입력 제거 및 최대 점프시간 초과
        if(Input.GetKeyUp(KeyCode.Space) || jumpTime > max_JumpTime)
        {
            canJump = false;
            jumpTime = 0f;
        }

        // 좌우 이동 키 입력 제거시 속도제어
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

    void Flip() // 좌우 바라보는 방향에 따라
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
    // 플레이어 대쉬
    void Player_Dash()
    {
    }

    // 플레이어 이동
    private void Player_Move()
    {

        //addforce 이용
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
    //플레이어 점프
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

            // 점프 시간 계산
            jumpTime += Time.deltaTime;
        }     
    }
    public void FallAnimeStart()
    {
        anime.SetBool("Fall", true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 땅에 착지
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            anime.SetBool("Fall", false);
            anime.SetBool("WallSlide", false);

            canJump = true;
        }
        // 벽에 붙었을 때 애니메이션 시작 및 점프 초기화
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            anime.SetBool("WallSlide", true);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            // 플레이어 기준으로 적의 위치가 왼쪽, 오른쪽 확인
            int dirVec_X = transform.position.x > collision.transform.position.x ? 1 : -1;
            // 적 정보 확인
            Enemy EnemyInfo = collision.gameObject.GetComponent<Enemy>();

            // 적 정보에 따라 넉백 및 플레이어 피격 데미지
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

    // ... 사다리 Test
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

        // 콜라이더 및 rigid simulated, 스크립트 비활성화
        rigid.simulated = false;
        transform.GetComponent<Collider2D>().enabled = false;
        transform.GetComponent<PlayerController>().enabled = false;
        // 자식 오브젝트 비활성화
        foreach (Transform child in childs)
        {
            child.gameObject.SetActive(false);
        }
        // ... 캐릭터 애니메이션 재생
        // ... 캐릭터 관련 컴포넌트 제거
        // ... ui 관련 스크립트로 새로 시작 확인 창 생성 -> 마을 맵 씬 전환
    }



    // 공격 애니메이션 종료시 호출
    void AttackAnimationEnd()
    {
        StartCoroutine(AttackAnimationEndCor());
    }
    IEnumerator AttackAnimationEndCor()
    {
        yield return new WaitForSeconds(0.01f);
        AttackEnd();
    }
    
    // 공격이 끝날 때 호출 ( 애니메이션 종료, 플레이어 피격 시)
    void AttackEnd()
    {
        canMove = true;
        canJump = true;

        childs[0].GetComponent<PlayerAttack>().is_Attack = false;
        childs[0].GetComponent<PlayerAttack>().Enemies.Clear(); // 리스트 삭제
        anime.SetBool("Attack", false);
    }
}