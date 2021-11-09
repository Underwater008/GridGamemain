using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour
{
    public int tileSpeed;
    public int gridX;
    public int gridY;

    private void OnMouseDown()
    {
        Debug.Log(gridX + "," + gridY);
    }
    private void Update()
    {
        //transform.Translate(Vector2.right * tileSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
    }
}
