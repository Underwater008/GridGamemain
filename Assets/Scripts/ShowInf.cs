using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInf : MonoBehaviour
{
    public GameObject canvas1;
    public GameObject canvas2;

    public void Start()         //游戏运行显示界面，隐藏说明
    {
        canvas2.SetActive(false);
        canvas1.SetActive(true);
    }

    public void ShowInfo()          //显示说明，隐藏界面
    {
        canvas1.SetActive(false);
        canvas2.SetActive(true);
    }

    public void CloseInfo()     //关闭说明，显示界面
    {
        canvas2.SetActive(false);
        canvas1.SetActive(true);
    }
}
