using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSign : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string object_name;
        object_name = this.gameObject.name;

        // Player 충돌
        if (collision.CompareTag("Player"))
        {
            switch (object_name)
            {
                case "Startsign":
                    SceneManager.LoadScene("Dungeon");
                    break;

                case "Optionsign":
                    /*
                        옵션 창 구현필요
                    */
                    break;

                case "Exitsign":
                    Application.Quit();
                    break;
            }
        }
    }
}
