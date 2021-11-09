using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Move : MonoBehaviour
{
    public enum PlayerColor     //ö�������ɫ
    {
        Red = 0,                //��
        Green = 1,              //�� 
        Yellow = 2,             //��
    }

    public AudioSource music;   
    public AudioClip boom;      //��ը��Ч
    public AudioClip liq;       //ҩˮ��Ч
    public AudioClip speedUp;   //������Ч


    public Sprite red;          //��ɫ��sprite
    public Sprite green;        //��ɫ��sprite
    public Sprite yellow;       //��ɫ��sprite

    public GameObject SuccessOrFail;
    public GameObject Text;     //��ɫռ������UI
    public GameObject ScoreText;//����������UI
    public GameObject TimeText;//ʣ��ʱ�������UI
    public GameObject EndScore;//���÷ֵ�����UI
    public GameObject Canvas1;  //��Ϸ�еĻ���
    public GameObject Canvas2;  //��Ϸ�����Ļ���
    public GameObject head;
    public GameObject start;

    public float speed = 0.5f;         //�ƶ��ٶ�
    public PlayerColor color;       //�����ɫ
    public GameObject[] block = new GameObject[3];          //3�ֵذ�(�죬�̣���)
    public GameObject plant;        //�ذ�
    private double temp = 0;        //��ʱ����ֵ�ı���
    private float tempSpeed = 0;    //�����ʼ�ٶȣ����ڸı��ٶȺ�ı����
    public GameObject obj;
             
    public double  score = 0;       //��ǰ����
    public double  score1 = 20f;    //һ��ذ�ĵ÷�
    public float time = 180;        //ʱ��

    public GameObject exit;         //�˳���ť
    public bool isShow;             //�Ƿ���ʾ�˳���ť
    public bool isDeath;
    public GameObject Player;       //���
    List<Transform> bodylist = new List<Transform>();
    public GameObject Body;

    private Vector2 direction;
    public bool ISwith = false;

    private float moveTime = 0.5f;
    private float nowTime = 0;

    private Vector3 headPos;
    public float velocity = 0.1f;

    public bool isStart;

    private SpriteRenderer renderer;    //���ڻ�ȡ�ذ��SpriteRenderer���
    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 0;
        isStart = false;
        isDeath = false;
        direction = Vector2.up;         //��ʼ������
        Canvas2.SetActive(false);       //���ý�����Ϸ����Ϊ����
        Canvas1.SetActive(true);        //������Ϸ�л���Ϊ��ʾ
        score = 0;                      //�÷ֳ�ʼ��
        color = PlayerColor.Yellow;        //������ҳ�ʼ��ɫ
        renderer = GetComponentInChildren<SpriteRenderer>();    //��ȡ�������ģ�͵�SpriteRenderer
        //renderer.color = Color.red;     //����ģ����ɫ
        tempSpeed = speed;              //��ʼ����ʼ�ٶ�
        music = gameObject.AddComponent<AudioSource>(); //����������
        music.playOnAwake = false;          //������Ч���Ὺʼ�Ͳ���
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
            setText();                      //�������ú���
            Canvas1.SetActive(false);       //������Ϸ���еĻ���Ϊ����
            Canvas2.SetActive(true);        //������Ϸ�����Ļ���Ϊ��ʾ
            EndScore.GetComponent<Text>().text = score.ToString("f2") + "";     //��ʾ���յ÷֣��������2λС��
            //Player.SetActive(false);
            SuccessOrFail.GetComponent<Text>().text = "Success";
            CancelInvoke();
        }
        else
        {
            if (time > 0 && isDeath == false)                        //ʱ�����0����Ϸ����������
            {
                time -= Time.deltaTime;
                shoot();
                MoveChar();
                setText();
            }
            else if (time <= 0 || isDeath == true)                  //ʱ��С��0��Ϸ����
            {
                time = 0;                       //ʱ�䲻����Ϊ���������С��0����Ϊ0
                setText();                      //�������ú���
                Canvas1.SetActive(false);       //������Ϸ���еĻ���Ϊ����
                Canvas2.SetActive(true);        //������Ϸ�����Ļ���Ϊ��ʾ
                EndScore.GetComponent<Text>().text = score.ToString("f2") + "";     //��ʾ���յ÷֣��������2λС��
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
        GridManager mange = plant.GetComponent<GridManager>();          //��ȡ���ε�GridManager���
        if (color == PlayerColor.Green)                                 //������ҵ�ǰ����ɫ�ĵذ�վ�ܵذ�İٷֱ�
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
        
        Text.GetComponent<Text>().text = temp.ToString("f2") + "%";         //������ɫ�ٷֱ�UI��ʾ������
        ScoreText.GetComponent<Text>().text = score.ToString("f2") + "";    //���÷���UI��ʾ������
        TimeText.GetComponent<Text>().text = (time).ToString("f2") + "";    //����ʣ��ʱ��UI��ʾ������
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

    //���߼����ߺ͵���
    void shoot()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(new Vector2(transform.position.x, transform.position.y), Vector2.zero);    //�����λ�ü��
        for (int x = 0; x < hit.Length; x++)         //���ȡ����⵽������
        {
            if (hit[x].collider.tag == "Player" || hit[x].collider.tag == "Body" || hit[x].collider.tag == "wall")
            {
                continue;
            }
           else
            {
                if (hit[x].collider.GetComponent<SpeedUp>() != null)        //����Ƿ��Ǽ��ٵ���
                {
                    music.clip = speedUp;                                   //������ЧΪ���ٵ���Ч
                    music.Play();                                           //������Ч
                    StartCoroutine(Speed_Up(hit[x].collider.GetComponent<SpeedUp>().speed));        //Э��ʵ��1��ļ���
                }
                else if (hit[x].collider.GetComponent<explosion>() != null)      //����Ƿ�Ϊ��ը����
                {
                    music.clip = boom;                                       //������ЧΪ��ը����Ч
                    music.Play();                                            //������Ч
                    //���ݱ�ը�����еĳ���ı���ε���ɫ
                    for (int i = (int)(hit[x].collider.transform.position.x - hit[x].collider.GetComponent<explosion>().width); i <= hit[x].collider.transform.position.x + hit[x].collider.GetComponent<explosion>().width; i++)
                    {
                        for (int j = (int)(hit[x].collider.transform.position.y - hit[x].collider.GetComponent<explosion>().height); j <= hit[x].collider.transform.position.y + hit[x].collider.GetComponent<explosion>().height; j++)
                        {
                            //�����Ҫ�ı���ɫ������ĵذ�
                            RaycastHit2D hit1 = Physics2D.Raycast(new Vector2(i, j), Vector2.zero);
                            if (hit1.collider != null && hit1.collider.tag != "Player" && hit1.collider.tag != "Body" && hit1.collider.tag != "wall")      //����ڱ�Եû�д򵽵ذ��򲻸ı���ɫ
                            {
                                ChangeBlock(hit1);      //�ı�hit1���ߴ򵽵ĵذ����ɫ
                            }
                            //ChangeBlock(hit1);
                        }
                    }
                }
                //������߼�⵽����ҩˮ
                else if (hit[x].collider.GetComponent<liquid>() != null)
                {
                    music.clip = liq;                   //������ЧΪҩˮ����Ч
                    music.Play();                       //������Ч
                    AddBody();
                }
                ChangeBlock(hit[x]);                    //�ı���ε���ɫ
            }
        }
    }


    void ChangeBlock(RaycastHit2D hit)
    {
        Vector2 pos = new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);        //�õ�hit��⵽�ĵذ������

        if ((hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite == red) && color != PlayerColor.Red) //����ذ�Ϊ��ɫ����ҵ���ɫ��Ϊ��ɫ
        {
            plant.GetComponent<GridManager>().countRed -= 1;                                                    //�ܵĺ�ɫ�ذ��������һ
        }
        else if ((hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite == green) && color != PlayerColor.Green)//����ذ�Ϊ��ɫ����ҵ���ɫ��Ϊ��ɫ
        {
            plant.GetComponent<GridManager>().countGreen -= 1;                                                      //�ܵ���ɫ�ذ��������һ
        }
        else if ((hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite == yellow) && color != PlayerColor.Yellow)//����ذ�Ϊ��ɫ����ҵ���ɫ��Ϊ��ɫ
        {
            plant.GetComponent<GridManager>().countYellow -= 1;                                                         //�ܵĻ�ɫ�ذ��������һ
        }

        Destroy(hit.collider.gameObject);                                                                       //ɾ�����߼�⵽�ĵذ�
        if (hit.collider.tag != "item")
        {
            GameObject newBlock = Instantiate(block[(int)color]);                                                   //���������ɫ�ĵذ�
            newBlock.transform.position = new Vector3(pos.x, pos.y, 0);                                             //�������ɵذ��λ��
            Vector3 relativePosition = new Vector3(pos.x, pos.y, 0) - plant.transform.position;                     //�����������
            plant.GetComponent<GridManager>().gridTiles[(int)relativePosition.x, (int)relativePosition.y] = newBlock;//���ö�Ӧ����ذ�������λ��Ϊ�·���

            switch ((int)color)                                                     //�����øı��ĸ�����ɫ��������������Ʒ�
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

        GameObject body = Instantiate(Body);//ʵ���������

        body.transform.position = transform.position;

        //body.transform.SetParent(this.transform, true);//����������֮�����UI���������canvas֮�£�false��ʾ��ʹ����������

        bodylist.Add(body.transform);
    }

    void MoveSnake()
    {
        headPos = transform.position;//����������ͷ�ƶ�ǰ��λ��

        gameObject.transform.position = new Vector3(headPos.x + direction.x *speed, headPos.y + direction.y *speed, headPos.z);//��ͷ������λ���ƶ�

        if (bodylist.Count > 0)//������һ�����ӵ�ʱ������ƶ�����

        {
            bodylist[bodylist.Count - 1].position = headPos;
            //bodylist.Last().position = headPos;//β�͸���ͷ�ƶ�����ȡ��������һ��list�������Ƶ�ͷ��λ�á�����β�ƶ�����ͷ�ƶ�ǰ��λ��

            bodylist.Insert(0, bodylist[bodylist.Count - 1]);//��β�Ͳ��뵽list�ĵ�0��λ��

            bodylist.RemoveAt(bodylist.Count - 1);//�Ƴ�List��ĩβ����β����
        }
    }

    IEnumerator Speed_Up(float upSpeed)                 //Э��ʵ�ּ���һ��
    {
        speed = upSpeed;
        yield return new WaitForSeconds(1);             
        speed = tempSpeed;
    }
}
