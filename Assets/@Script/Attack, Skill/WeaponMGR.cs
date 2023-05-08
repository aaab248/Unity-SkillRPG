using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMGR : MonoBehaviour
{
    Dictionary<string, Weapon> weaponMap;

    private void Start()
    {
        string name;
        weaponMap = new Dictionary<string, Weapon>();

        // 인덱스, 데미지, 공격 범위, 공격 속도, 공격 횟수
        //기본 무기
        name = "sword";
        weaponMap.Add(name, new Weapon(0, 1.0f, 1.0f, 0.5f, 3));
        //대검
        name = "bigSword";
        weaponMap.Add(name, new Weapon(1, 2.0f, 1.5f, 1.0f, 2));
        //쌍검
        name = "twinSword";
        weaponMap.Add(name, new Weapon(2, 0.8f, 0.75f, 0.3f, 5));

        //추가되는 무기 속성들 추가
    }
    
    // 딕셔너리에서 key 값으로 value 값인 Weapon을 가져와 return 
    public Weapon GetWeaponValue(string name)
    {
        Weapon WeaponValue;
        weaponMap.TryGetValue(name, out WeaponValue);
        return WeaponValue;
    }
}
