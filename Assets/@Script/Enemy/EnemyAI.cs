using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform target_Transform;
    SpriteRenderer enemy_Sprite;
    Enemy enemy;

    Vector2 dirVec;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemy_Sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, target_Transform.position);
        if (distance <= enemy.FOV && enemy.canMove == true)
        {
            Move_Target();
        }
    }

    void Move_Target()
    {
        float dir = target_Transform.position.x - transform.position.x;
        if (dir < 0)
        {
            dir = -1;
        }
        else
        {
            dir = 1;
        }

        if (dir != 0)
        {
            enemy_Sprite.flipX = dir < 0;
        }
        transform.Translate(new Vector2(dir, 0) * enemy.move_Speed * Time.deltaTime);
    }
}
