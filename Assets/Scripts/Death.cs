using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    private float DeathTime = 0.2f;
    private float nowTime = 0;
    private bool canDeath;
    // Start is called before the first frame update
    void Start()
    {
        nowTime = 0;
        canDeath = false;
    }

    // Update is called once per frame
    void Update()
    {
        nowTime += Time.deltaTime;
        if(nowTime >= DeathTime)
        {
            canDeath = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && canDeath == true)
        {
            collision.gameObject.GetComponent<Move>().isDeath = true;
            Debug.Log("enter");
        }
        else if(collision.tag == "Player" && this.gameObject.tag == "wall")
        {
            collision.gameObject.GetComponent<Move>().isDeath = true;
            Debug.Log("enter");
        }
    }
}
