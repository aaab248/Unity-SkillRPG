using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player_Object;
    float camera_Speed = 0.3f;

    public float min_transX, min_transY, max_transX, max_transY;

    private void FixedUpdate()
    {
        Vector3 target_Pos = new Vector3(player_Object.transform.position.x, player_Object.transform.position.y, this.transform.position.z);

        target_Pos.x = Mathf.Clamp(target_Pos.x, min_transX, max_transX);
        target_Pos.y = Mathf.Clamp(target_Pos.y, min_transY, max_transY);

        transform.position = Vector3.Lerp(transform.position, target_Pos, camera_Speed);
    }

    // 카메라 흔들림 효과
    IEnumerator ShakeCamera(float duration, float magnitude)
    {
        Vector3 originalPos = Camera.main.transform.position ;
        float elasped = 0f;

        while (elasped < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elasped += Time.deltaTime;

            yield return null;
        }

        Camera.main.transform.position = originalPos;
    }
    public void DoShakeCamera(float duration, float magnitude)
    {
        StartCoroutine(ShakeCamera(duration, magnitude));
    }
}
