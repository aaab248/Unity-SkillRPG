using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    //... �÷��̾� ���� ����
    public GameObject player;
    public float playerHealth;

    // ���� ���� �̸�
    public string current_WeaponName;
    public Weapon current_Weapon;
    public WeaponMGR weaponDic;

    public StatsUI stats;

    // ��ų ����Ʈ ����
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

        // �ִ� ��ų ����Ʈ �� ���� ��ų ����Ʈ �ʱ�ȭ
        max_SkillPoint = 8;
        current_SkillPoint = max_SkillPoint / 2;
    }

    private void Update()
    {
        //���� ��ü
        Change_Weapon();
    }

    // �÷��̾� ���� ����
    public float GetPlayerStats()
    {
        return playerHealth;
    }
    public void SetPlayerStats(float value)
    {
        playerHealth = value;
    }
    
    // ��ų ����Ʈ ������Ʈ
    public void Set_Skill_Point(int point)
    {
        // ���� ��ų����Ʈ ������Ʈ
        current_SkillPoint += point;
        stats.Update_Skill_Points(point * -1);
    }
    void Change_Weapon()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // ������ �÷��̾ ������ ��ų�� �迭�� ������ �� Ű �Է¿� �ش��ϴ� �ε����� ��ų ȣ��
            current_WeaponName = "bigSword";
            current_Weapon = weaponDic.GetWeaponValue(current_WeaponName);

            current_SkillPoint -= 1;
            stats.Update_Skill_Points(1);

            Debug.Log("�ڽ�Ʈ 1 ��ų ��� �� ���� ��� ��ü");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            current_WeaponName = "twinSword";
            current_Weapon = weaponDic.GetWeaponValue(current_WeaponName);

            current_SkillPoint -= 2;
            stats.Update_Skill_Points(2);

            Debug.Log("�ڽ�Ʈ 2 ��ų ��� �� ���� �ְ� ��ü");

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