using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    public GameObject[] skill_Prefabs;
    public Transform[] skill_SpawnPoints;

    private void Awake()
    {

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(Attack_Obj_Set(skill_Prefabs[0]));
        }
    }
    IEnumerator Attack_Obj_Set(GameObject obj)
    {
        GameObject skill_obj = Instantiate(obj, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        Destroy(skill_obj);
    }
}

