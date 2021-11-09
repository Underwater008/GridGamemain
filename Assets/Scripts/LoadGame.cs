using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    public void OnClickEnd()                    //���ؽ�����Ϸ�ĳ���
    {
        SceneManager.LoadScene("EndScene");
    }
    public void OnClickStart()              //������Ϸ�ĳ���
    {
        SceneManager.LoadScene("GameScene");
    }
    public void loadMainMeau()                  //�������˵�
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Exit()                          //�˳���Ϸ
    {
        Application.Quit();
    }
}
