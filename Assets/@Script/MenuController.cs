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
     �޴� â���� �ʿ��� �͵� �߰��� ����
     */
}
