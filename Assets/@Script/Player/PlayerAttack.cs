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

    public Weapon pWeapon; // 현재 무기 클래스 변수

    public LayerMask enemyLayer; // 적 Layer
    public List<Enemy> Enemies = new List<Enemy>(); // 플레이어가 공격 중인 적 리스트

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

        if (Input.GetKeyDown(KeyCode.A) && player.isHit == false)
        {
            Player_Attack();
        }
    }

    void Player_Attack()
    {
        if (is_Attack == false && playerRigid.velocity.y == 0 )
        {
            // 공격시 시간 초기화
            attack_Time = 0f;
            is_Attack = true;

            player.canMove = false;
            playerRigid.velocity = Vector2.zero;


            playerAnime.SetBool("Attack", true);
            playerAnime.SetInteger("AttackIndex", attack_Index);

            StartCoroutine(AttackObject());

            // 공격 인덱스 추가 및 0,1,2 고정
            attack_Index++;
            attack_Index = attack_Index % 3;
         }
    }

    IEnumerator AttackObject()
    {
        // 범위에 있는 적들 검색
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 0.6f, enemyLayer);

        yield return new WaitForSeconds(0.1f);

        // 콜라이더 배열에 있는 적들을 검색 -> 피격 중인 적 리스트에 없으면 데미지입력 함수 호출 및 리스트에 추가
        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();

            if (enemyScript != null && !Enemies.Contains(enemyScript))
            {
                // 공격 적중 시 카메라 흔들림
                Camera.main.GetComponent<CameraController>().DoShakeCamera(0.2f, 0.1f);
                // 적 피격함수 호출
                enemyScript.TakeDamage(pWeapon.damage, 0.3f);
                // 뒤로 넉백
                enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(1f, 2f).normalized * 3f, ForceMode2D.Impulse);
                
                // 리스트에 피격 적 추가
                Enemies.Add(enemyScript);
                // 콤보 타이머 시작
                GameManager.instance.StartComboTimer();
                GameManager.instance.hitCombo_Num++;
            }

        }
    }
}
