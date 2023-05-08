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

        // �ε���, ������, ���� ����, ���� �ӵ�, ���� Ƚ��
        //�⺻ ����
        name = "sword";
        weaponMap.Add(name, new Weapon(0, 1.0f, 1.0f, 0.5f, 3));
        //���
        name = "bigSword";
        weaponMap.Add(name, new Weapon(1, 2.0f, 1.5f, 1.0f, 2));
        //�ְ�
        name = "twinSword";
        weaponMap.Add(name, new Weapon(2, 0.8f, 0.75f, 0.3f, 5));

        //�߰��Ǵ� ���� �Ӽ��� �߰�
    }
    
    // ��ųʸ����� key ������ value ���� Weapon�� ������ return 
    public Weapon GetWeaponValue(string name)
    {
        Weapon WeaponValue;
        weaponMap.TryGetValue(name, out WeaponValue);
        return WeaponValue;
    }
}
