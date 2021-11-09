using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject wallPrefab;

    public Sprite[] tileSprites;
    public GameObject[] items = new GameObject[3];          //����3�ֵ���


    public GameObject[,] gridTiles;                 //�洢ȫ���ذ�

    public int gridWidth;                           //�ذ峤
    public int gridHeight;                          //�ذ��
                  

    public int countRed = 0;                        //��ɫ�ذ�����
    public int countGreen = 0;                      //��ɫ�ذ�����
    public int countYellow = 0;                     //��ɫ�ذ�����
    public int itemsCount = 0;                      //��������

    // Start is called before the first frame update
    private void Start()
    { 
        gridTiles = new GameObject[gridWidth + 2, gridHeight + 2];
        for (int x = 0; x < gridWidth + 2; x++)
        {
            for (int y = 0; y < gridHeight + 2; y++)
            {
                MakeTile(x, y);
            }
        }
    }




    void CreateItems()
    {
        int itemx = (int)Random.Range(transform.position.x,transform.position.x +gridWidth-1);        //�������ɵ�x����
        int itemy = (int)Random.Range(transform.position.y,transform.position.y +gridHeight-1);       //�������ɵ�y����
        int randTile = Random.Range(0, items.Length);                                               //���õ���Ϊ��һ��
        GameObject newItem = Instantiate(items[randTile]);                                          //��������
        newItem.transform.position = new Vector3(itemx,itemy,0);                                    //�����µ��ߵ�λ��
        itemsCount += 1;                                                                            //����������һ
    }

    // Update is called once per frame
    void Update()
    {
        if(itemsCount < 30)                                                                     //������С��30
        {
            CreateItems();                                                                      //���ɵ���
        }

    }

    void MakeTile(int xPos, int yPos) {
        if ((xPos != 0) && (yPos != 0) && (xPos != gridWidth + 1) && (yPos != gridHeight + 1))
        {
            GameObject newTile = Instantiate(tilePrefab);
            int randTile = Random.Range(0, tileSprites.Length);
            newTile.GetComponent<SpriteRenderer>().sprite = tileSprites[randTile];
            newTile.transform.position = new Vector3(transform.position.x + xPos, transform.position.y + yPos, 0);
            TileData myData = newTile.GetComponent<TileData>();
            if (randTile == 0)
            {
                myData.tileSpeed = 4;
                countGreen += 1;
            }
            else if (randTile == 1)
            {
                myData.tileSpeed = 1;
                countYellow += 1;
            }
            else if (randTile == 2)
            {
                myData.tileSpeed = 7;
                countRed += 1;
            }
            myData.gridX = xPos;
            myData.gridY = yPos;
            gridTiles[xPos, yPos] = newTile;
        }
        else
        {
            GameObject newTile = Instantiate(wallPrefab);
            newTile.transform.position = new Vector3(transform.position.x + xPos, transform.position.y + yPos, 0);
        }
    }

}
