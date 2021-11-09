using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject wallPrefab;

    public Sprite[] tileSprites;
    public GameObject[] items = new GameObject[3];          //储存3种道具


    public GameObject[,] gridTiles;                 //存储全部地板

    public int gridWidth;                           //地板长
    public int gridHeight;                          //地板高
                  

    public int countRed = 0;                        //红色地板数量
    public int countGreen = 0;                      //绿色地板数量
    public int countYellow = 0;                     //黄色地板数量
    public int itemsCount = 0;                      //道具数量

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
        int itemx = (int)Random.Range(transform.position.x,transform.position.x +gridWidth-1);        //道具生成的x坐标
        int itemy = (int)Random.Range(transform.position.y,transform.position.y +gridHeight-1);       //道具生成的y坐标
        int randTile = Random.Range(0, items.Length);                                               //设置道具为哪一种
        GameObject newItem = Instantiate(items[randTile]);                                          //创建道具
        newItem.transform.position = new Vector3(itemx,itemy,0);                                    //设置新道具的位置
        itemsCount += 1;                                                                            //道具数量加一
    }

    // Update is called once per frame
    void Update()
    {
        if(itemsCount < 30)                                                                     //道具数小于30
        {
            CreateItems();                                                                      //生成道具
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
