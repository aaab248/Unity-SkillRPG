using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public int weapon_Index; // �ε���
    public float damage; // ������
    public float attack_Range; // ����
    public float attack_speed; // �ӵ�
    public int num_of_Attack; // Ƚ��

    public Weapon(int _weapon_Index, float _damage, float _attack_Range, float _attack_speed, int _num_of_Attack)
    {
        weapon_Index = _weapon_Index;
        damage = _damage;
        attack_Range = _attack_Range;
        attack_speed = _attack_speed;
        num_of_Attack = _num_of_Attack;
    }
}

