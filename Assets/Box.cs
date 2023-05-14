using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    SpriteRenderer spriteRend;
    public Sprite[] sprites;
    int BoxHp = 2;

    private void Awake()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        spriteRend.sprite = sprites[0];
    }

    public void Box_Damaged()
    {
        BoxHp -= 1;
        StartCoroutine(BoxShake());
        if (BoxHp <= -1)
        {
            gameObject.SetActive(false);
            return;
        }
        spriteRend.sprite = sprites[2 - BoxHp];
    }
    IEnumerator BoxShake()
    {
        Vector3 originPos = transform.localPosition;
        float elasped = 0f;

        while (elasped < 0.1f)
        {
            float x = Random.Range(-1f, 1f) * 0.1f;
            float y = Random.Range(-1f, 1f) * 0.1f;

            transform.localPosition = new Vector3(originPos.x + x, originPos.y + y, originPos.z);

            elasped += Time.deltaTime;

            yield return null;
        }
    transform.localPosition = originPos;
    }
}
