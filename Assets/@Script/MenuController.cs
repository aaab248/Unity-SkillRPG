using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject Menu;

    public void Continue()
    {
        Time.timeScale = 1;
        Menu.SetActive(false);
    }

    public void Game_Exit()
    {
        Application.Quit();
    }

    /*
     메뉴 창에서 필요한 것들 추가로 구현
     */
}
