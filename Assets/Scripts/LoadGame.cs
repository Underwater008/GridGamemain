using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    public void OnClickEnd()                    //加载结束游戏的场景
    {
        SceneManager.LoadScene("EndScene");
    }
    public void OnClickStart()              //加载游戏的场景
    {
        SceneManager.LoadScene("GameScene");
    }
    public void loadMainMeau()                  //加载主菜单
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Exit()                          //退出游戏
    {
        Application.Quit();
    }
}
