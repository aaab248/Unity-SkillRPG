using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopBackGround : MonoBehaviour
{
    public string BG_Type;
    public float distance = 44.0f;
    float current_distance;

    private void FixedUpdate()
    {
        Vector3 playerPos = GameManager.instance.player.transform.position;

        current_distance = transform.localPosition.x - playerPos.x;
        // 플레이어가 배경보다 일정 값이상 왼쪽으로 갈 때
        if (current_distance >= distance)
        {
            transform.Translate(Vector3.left * 44 * 2);
        }
        // 플레이어가 배경보다 일정 값이상 오른쪽으로 갈 때
        else if (current_distance <= distance * (-1))
        {
            transform.Translate(Vector3.right * 44 * 2);
        }

        switch (BG_Type)
        {
            case "sky":
                transform.Translate(Vector3.right * GameManager.instance.player.input_Vec.x * (0.01f));
                break;

            case "mountain":
                transform.Translate(Vector3.right * GameManager.instance.player.input_Vec.x * (0.02f));
                break;

            case "hill":
                transform.Translate(Vector3.right * GameManager.instance.player.input_Vec.x * (0.03f));
                break;
        }
    }
}
