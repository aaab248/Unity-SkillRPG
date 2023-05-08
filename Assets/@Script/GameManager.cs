using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    //... 플레이어 상태 관련
    public GameObject player;
    public float playerHealth;

    // 현재 무기 이름
    public string current_WeaponName;
    public Weapon current_Weapon;
    public WeaponMGR weaponDic;

    public StatsUI stats;

    // 스킬 포인트 관련
    public int current_SkillPoint;
    public int max_SkillPoint;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        current_WeaponName = "sword";
        current_Weapon = weaponDic.GetWeaponValue(current_WeaponName);

        // 최대 스킬 포인트 및 현재 스킬 포인트 초기화
        max_SkillPoint = 8;
        current_SkillPoint = max_SkillPoint / 2;
    }

    private void Update()
    {
        //무기 교체
        Change_Weapon();
    }

    // 플레이어 스탯 저장
    public float GetPlayerStats()
    {
        return playerHealth;
    }
    public void SetPlayerStats(float value)
    {
        playerHealth = value;
    }
    
    // 스킬 포인트 업데이트
    public void Set_Skill_Point(int point)
    {
        // 현재 스킬포인트 업데이트
        current_SkillPoint += point;
        stats.Update_Skill_Points(point * -1);
    }
    void Change_Weapon()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // 구현은 플레이어가 장착한 스킬을 배열에 저장한 후 키 입력에 해당하는 인덱스로 스킬 호출
            current_WeaponName = "bigSword";
            current_Weapon = weaponDic.GetWeaponValue(current_WeaponName);

            current_SkillPoint -= 1;
            stats.Update_Skill_Points(1);

            Debug.Log("코스트 1 스킬 사용 및 무기 대검 교체");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            current_WeaponName = "twinSword";
            current_Weapon = weaponDic.GetWeaponValue(current_WeaponName);

            current_SkillPoint -= 2;
            stats.Update_Skill_Points(2);

            Debug.Log("코스트 2 스킬 사용 및 무기 쌍검 교체");

        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            current_WeaponName = "sword";
            current_Weapon = weaponDic.GetWeaponValue(current_WeaponName);
        }
    }

}
/*
if (current_SkillPoint <= 0)
{
    current_SkillPoint = 0;
}
if (current_SkillPoint >= max_SkillPoint)
{
    current_SkillPoint = max_SkillPoint;
}
*/