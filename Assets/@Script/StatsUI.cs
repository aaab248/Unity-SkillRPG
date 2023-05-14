using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsUI : MonoBehaviour
{
    public GameObject[] skill_Points_Images;

    public TMP_Text combo_Text;

    // 스킬 포인트 UI 
    Stack<GameObject> point_Stacks;
    Stack<GameObject> disable_Point_Stacks;

    private void Awake()
    {
        point_Stacks = new Stack<GameObject>();
        disable_Point_Stacks = new Stack<GameObject>();
    }
    void Start()
    {
        for (int i = 0; i < GameManager.instance.current_SkillPoint; i++)
        {
            skill_Points_Images[i].SetActive(true);
            point_Stacks.Push(skill_Points_Images[i]);
        }
        for (int j = GameManager.instance.max_SkillPoint - 1; j > GameManager.instance.current_SkillPoint - 1; j--)
        {
            skill_Points_Images[j].SetActive(false);
            disable_Point_Stacks.Push(skill_Points_Images[j]);
        }
    }
    private void Update()
    {
        // hitCombo UI 갱신
        if(GameManager.instance.hitCombo_TimerOn == true)
        {
            combo_Text.gameObject.SetActive(true);
            combo_Text.text = "Hit" + GameManager.instance.hitCombo_Num;
        }
        else
        {
            combo_Text.gameObject.SetActive(false);
        }
    }

    // 현재 스킬 포인트 UI 갱신 함수
    public void Update_Skill_Points(int cost)
    {
        GameObject sP_Img;

        if(cost < 0)
        {
            sP_Img = disable_Point_Stacks.Pop();
            sP_Img.SetActive(true);
            point_Stacks.Push(sP_Img);
        }
        else if(cost > 0)
        {
            for(int i = 0; i < cost; i++)
            {
                sP_Img = point_Stacks.Pop();
                sP_Img.SetActive(false);
                disable_Point_Stacks.Push(sP_Img);
            }
        }
    }
}
