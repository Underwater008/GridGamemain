using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInf : MonoBehaviour
{
    public GameObject canvas1;
    public GameObject canvas2;

    public void Start()         //��Ϸ������ʾ���棬����˵��
    {
        canvas2.SetActive(false);
        canvas1.SetActive(true);
    }

    public void ShowInfo()          //��ʾ˵�������ؽ���
    {
        canvas1.SetActive(false);
        canvas2.SetActive(true);
    }

    public void CloseInfo()     //�ر�˵������ʾ����
    {
        canvas2.SetActive(false);
        canvas1.SetActive(true);
    }
}
