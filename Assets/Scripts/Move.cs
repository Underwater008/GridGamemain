using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Move : MonoBehaviour
{
    public enum PlayerColor     //枚举玩家颜色
    {
        Red = 0,                //红
        Green = 1,              //绿 
        Yellow = 2,             //黄
    }

    public AudioSource music;   
    public AudioClip boom;      //爆炸音效
    public AudioClip liq;       //药水音效
    public AudioClip speedUp;   //加速音效


    public Sprite red;          //红色的sprite
    public Sprite green;        //绿色的sprite
    public Sprite yellow;       //黄色的sprite

    public GameObject SuccessOrFail;
    public GameObject Text;     //颜色占比文字UI
    public GameObject ScoreText;//分数的文字UI
    public GameObject TimeText;//剩余时间的文字UI
    public GameObject EndScore;//最后得分的文字UI
    public GameObject Canvas1;  //游戏中的画布
    public GameObject Canvas2;  //游戏结束的画布
    public GameObject head;
    public GameObject start;

    public float speed = 0.5f;         //移动速度
    public PlayerColor color;       //玩家颜色
    public GameObject[] block = new GameObject[3];          //3种地板(红，绿，黄)
    public GameObject plant;        //地板
    private double temp = 0;        //临时储存值的变量
    private float tempSpeed = 0;    //保存初始速度，用于改变速度后改变回来
    public GameObject obj;
             
    public double  score = 0;       //当前分数
    public double  score1 = 20f;    //一块地板的得分
    public float time = 180;        //时间

    public GameObject exit;         //退出按钮
    public bool isShow;             //是否显示退出按钮
    public bool isDeath;
    public GameObject Player;       //玩家
    List<Transform> bodylist = new List<Transform>();
    public GameObject Body;

    private Vector2 direction;
    public bool ISwith = false;

    private float moveTime = 0.5f;
    private float nowTime = 0;

    private Vector3 headPos;
    public float velocity = 0.1f;

    public bool isStart;

    private SpriteRenderer renderer;    //用于获取地板的SpriteRenderer组件
    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 0;
        isStart = false;
        isDeath = false;
        direction = Vector2.up;         //初始方向朝上
        Canvas2.SetActive(false);       //设置结束游戏画布为隐藏
        Canvas1.SetActive(true);        //设置游戏中画布为显示
        score = 0;                      //得分初始化
        color = PlayerColor.Yellow;        //设置玩家初始颜色
        renderer = GetComponentInChildren<SpriteRenderer>();    //获取玩家自身模型的SpriteRenderer
        //renderer.color = Color.red;     //设置模型颜色
        tempSpeed = speed;              //初始化初始速度
        music = gameObject.AddComponent<AudioSource>(); //添加声音组件
        music.playOnAwake = false;          //设置音效不会开始就播放
        isShow = false;
        exit.SetActive(isShow);
        head.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
        InvokeRepeating("MoveSnake", 0, velocity);
    }

    // Update is called once per frame
    void Update()
    {
        if (plant.GetComponent<GridManager>().countRed == plant.GetComponent<GridManager>().gridHeight * plant.GetComponent<GridManager>().gridWidth)
        {
            setText();                      //调用设置函数
            Canvas1.SetActive(false);       //设置游戏运行的画布为隐藏
            Canvas2.SetActive(true);        //设置游戏结束的画布为显示
            EndScore.GetComponent<Text>().text = score.ToString("f2") + "";     //显示最终得分，结果保留2位小数
            //Player.SetActive(false);
            SuccessOrFail.GetComponent<Text>().text = "Success";
            CancelInvoke();
        }
        else
        {
            if (time > 0 && isDeath == false)                        //时间大于0，游戏折正常进行
            {
                time -= Time.deltaTime;
                shoot();
                MoveChar();
                setText();
            }
            else if (time <= 0 || isDeath == true)                  //时间小于0游戏结束
            {
                time = 0;                       //时间不可能为负数，如果小于0设置为0
                setText();                      //调用设置函数
                Canvas1.SetActive(false);       //设置游戏运行的画布为隐藏
                Canvas2.SetActive(true);        //设置游戏结束的画布为显示
                EndScore.GetComponent<Text>().text = score.ToString("f2") + "";     //显示最终得分，结果保留2位小数
                Player.SetActive(false);
                CancelInvoke();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isShow = !isShow;
            exit.SetActive(isShow);
            if(isShow == true)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }

        }
    }

    void setText()
    {
        GridManager mange = plant.GetComponent<GridManager>();          //获取地形的GridManager组件
        if (color == PlayerColor.Green)                                 //计算玩家当前的颜色的地板站总地板的百分比
        {
            temp = (double)mange.countGreen / (double)(mange.countGreen + mange.countRed + mange.countYellow);
        }
        else if(color == PlayerColor.Red)
        {
            temp = (double)mange.countRed / (double)(mange.countGreen + mange.countRed + mange.countYellow);
        }
        else if(color == PlayerColor.Yellow)
        {
            temp = (double)mange.countYellow / (double)(mange.countGreen + mange.countRed + mange.countYellow);
        }
        temp *= 100;
        
        Text.GetComponent<Text>().text = temp.ToString("f2") + "%";         //设置颜色百分比UI显示的数据
        ScoreText.GetComponent<Text>().text = score.ToString("f2") + "";    //设置分数UI显示的数据
        TimeText.GetComponent<Text>().text = (time).ToString("f2") + "";    //设置剩余时间UI显示的数据
    }

    void MoveChar()
    {

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && direction != Vector2.down)
        {
            direction = Vector2.up;
            head.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
            if(isStart == false)
            {
                isStart = true;
                Time.timeScale = 1;
                start.SetActive(false);
            }
        }
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && direction != Vector2.right)
        {
            direction = Vector2.left;
            head.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            if (isStart == false)
            {
                isStart = true;
                Time.timeScale = 1;
                start.SetActive(false);
            }
        }
        if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && direction != Vector2.up)
        {
            direction = Vector2.down;
            head.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            if (isStart == false)
            {
                isStart = true;
                Time.timeScale = 1;
                start.SetActive(false);
            }
        }
        if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && direction != Vector2.left)
        {
            direction = Vector2.right;
            head.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            if (isStart == false)
            {
                isStart = true;
                Time.timeScale = 1;
                start.SetActive(false);
            }
        }
    }

    //射线检测道具和地形
    void shoot()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(new Vector2(transform.position.x, transform.position.y), Vector2.zero);    //在玩家位置检测
        for (int x = 0; x < hit.Length; x++)         //逐个取出检测到的物体
        {
            if (hit[x].collider.tag == "Player" || hit[x].collider.tag == "Body" || hit[x].collider.tag == "wall")
            {
                continue;
            }
           else
            {
                if (hit[x].collider.GetComponent<SpeedUp>() != null)        //检测是否是加速道具
                {
                    music.clip = speedUp;                                   //设置音效为加速的音效
                    music.Play();                                           //播放音效
                    StartCoroutine(Speed_Up(hit[x].collider.GetComponent<SpeedUp>().speed));        //协程实现1秒的加速
                }
                else if (hit[x].collider.GetComponent<explosion>() != null)      //检测是否为爆炸道具
                {
                    music.clip = boom;                                       //设置音效为爆炸的音效
                    music.Play();                                            //播放音效
                    //根据爆炸函数中的长宽改变地形的颜色
                    for (int i = (int)(hit[x].collider.transform.position.x - hit[x].collider.GetComponent<explosion>().width); i <= hit[x].collider.transform.position.x + hit[x].collider.GetComponent<explosion>().width; i++)
                    {
                        for (int j = (int)(hit[x].collider.transform.position.y - hit[x].collider.GetComponent<explosion>().height); j <= hit[x].collider.transform.position.y + hit[x].collider.GetComponent<explosion>().height; j++)
                        {
                            //检测需要改变颜色的坐标的地板
                            RaycastHit2D hit1 = Physics2D.Raycast(new Vector2(i, j), Vector2.zero);
                            if (hit1.collider != null && hit1.collider.tag != "Player" && hit1.collider.tag != "Body" && hit1.collider.tag != "wall")      //如果在边缘没有打到地板则不改变颜色
                            {
                                ChangeBlock(hit1);      //改变hit1射线打到的地板的颜色
                            }
                            //ChangeBlock(hit1);
                        }
                    }
                }
                //如果射线检测到的是药水
                else if (hit[x].collider.GetComponent<liquid>() != null)
                {
                    music.clip = liq;                   //设置音效为药水的音效
                    music.Play();                       //播放音效
                    AddBody();
                }
                ChangeBlock(hit[x]);                    //改变地形的颜色
            }
        }
    }


    void ChangeBlock(RaycastHit2D hit)
    {
        Vector2 pos = new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);        //得到hit检测到的地板的坐标

        if ((hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite == red) && color != PlayerColor.Red) //如果地板为红色而玩家的颜色不为红色
        {
            plant.GetComponent<GridManager>().countRed -= 1;                                                    //总的红色地板的数量减一
        }
        else if ((hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite == green) && color != PlayerColor.Green)//如果地板为绿色而玩家的颜色不为绿色
        {
            plant.GetComponent<GridManager>().countGreen -= 1;                                                      //总的绿色地板的数量减一
        }
        else if ((hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite == yellow) && color != PlayerColor.Yellow)//如果地板为黄色而玩家的颜色不为黄色
        {
            plant.GetComponent<GridManager>().countYellow -= 1;                                                         //总的黄色地板的数量减一
        }

        Destroy(hit.collider.gameObject);                                                                       //删除射线检测到的地板
        if (hit.collider.tag != "item")
        {
            GameObject newBlock = Instantiate(block[(int)color]);                                                   //生成玩家颜色的地板
            newBlock.transform.position = new Vector3(pos.x, pos.y, 0);                                             //设置生成地板的位置
            Vector3 relativePosition = new Vector3(pos.x, pos.y, 0) - plant.transform.position;                     //计算相对坐标
            plant.GetComponent<GridManager>().gridTiles[(int)relativePosition.x, (int)relativePosition.y] = newBlock;//设置对应储存地板的数组的位置为新方块

            switch ((int)color)                                                     //将设置改变后的各个颜色方块的数量，并计分
            {
                case 0:
                    if (hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite != red && hit.collider.tag != "item")
                    {
                        plant.GetComponent<GridManager>().countRed += 1;
                        score += temp * score1 / 100;
                    }
                    break;
                case 1:
                    if (hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite != green && hit.collider.tag != "item")
                    {
                        plant.GetComponent<GridManager>().countGreen += 1;
                        score += temp * score1 / 100;
                    }
                    break;
                case 2:
                    if (hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite != yellow && hit.collider.tag != "item")
                    {
                        plant.GetComponent<GridManager>().countYellow += 1;
                        score += temp * score1 / 100;
                    }
                    break;
            }
        }
    }

    void AddBody()
    {

        GameObject body = Instantiate(Body);//实例对象出来

        body.transform.position = transform.position;

        //body.transform.SetParent(this.transform, true);//将身体生成之后放在UI界面的容器canvas之下，false表示不使用世界坐标

        bodylist.Add(body.transform);
    }

    void MoveSnake()
    {
        headPos = transform.position;//保存下来蛇头移动前的位置

        gameObject.transform.position = new Vector3(headPos.x + direction.x *speed, headPos.y + direction.y *speed, headPos.z);//蛇头向期望位置移动

        if (bodylist.Count > 0)//至少有一个身子的时候才做移动操作

        {
            bodylist[bodylist.Count - 1].position = headPos;
            //bodylist.Last().position = headPos;//尾巴跟着头移动，获取身体的最后一个list，把它移到头的位置。将蛇尾移动到蛇头移动前的位置

            bodylist.Insert(0, bodylist[bodylist.Count - 1]);//将尾巴插入到list的第0号位置

            bodylist.RemoveAt(bodylist.Count - 1);//移除List最末尾的蛇尾引用
        }
    }

    IEnumerator Speed_Up(float upSpeed)                 //协程实现加速一秒
    {
        speed = upSpeed;
        yield return new WaitForSeconds(1);             
        speed = tempSpeed;
    }
}
