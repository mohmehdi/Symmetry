using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public int SizeX=20;        
    public int SizeY=40;
    public GameObject[] Brushs;             //list of colors
    public GameObject CurrentBrush;         //color clicked

    private bool[,] pad;                    //to ckeck after colmplete drawing           
    private float startPosY;                //left corner y pos
    private float startPosX;                //left corner x pos
    private Vector3 size;                   // cells size
    private float offcet=0.5f;              //to centerize brushes
    Camera cam;                             

    private void Start()
    {
        cam = Camera.main;
        pad = new bool[SizeX, SizeY];
    }
    private void Update()
    {
        //spawn brushes in grid
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
              int x = (int)(hit.point.x / size.x);
              int y = (int)(hit.point.y / size.y);
                if (hit.collider.GetComponent<ColorBrush>() == null)
                {
               GameObject go= Instantiate(CurrentBrush, new Vector3(x * size.x + size.x / 2, y * size.y + size.y / 2, -0.1f), Quaternion.identity);
                go.transform.localScale = size;
                go.transform.SetParent(transform);
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        size = new Vector3(transform.localScale.x / SizeX * 10, transform.localScale.z / SizeY * 10, 0.1f);

        startPosY = transform.position.y - (transform.localScale.z * 10 / 2);
        startPosX = transform.position.x - (transform.localScale.x * 10 / 2);
        Vector2 offcet = new Vector2(startPosX, startPosY);
        Gizmos.color = Color.red;
        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeY; j++)
            {
                Gizmos.DrawWireCube(new Vector2(i * size.x + size.x / 2, j * size.y + size.y / 2) + offcet, size);
            }
        }
    }
}
