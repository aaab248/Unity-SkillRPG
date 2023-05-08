using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public int weapon_Index; // 인덱스
    public float damage; // 데미지
    public float attack_Range; // 범위
    public float attack_speed; // 속도
    public int num_of_Attack; // 횟수

    public Weapon(int _weapon_Index, float _damage, float _attack_Range, float _attack_speed, int _num_of_Attack)
    {
        weapon_Index = _weapon_Index;
        damage = _damage;
        attack_Range = _attack_Range;
        attack_speed = _attack_speed;
        num_of_Attack = _num_of_Attack;
    }
}

