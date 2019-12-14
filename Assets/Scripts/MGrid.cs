using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
public class MGrid : MonoBehaviour
{
    //........Grid Line Properties..........//
    public GameObject LinePrefab;
    [SerializeField]
    [Range(10,200)]
    private float lineSize=100;
    //.....................................//
    public InputField textInput;
    public int Size { get; set; }   //(Size by 2*Size)  grid
    public int[,] Tiles;          //Data in grid that 0 means empty cell
    public GameObject TilePrefab;
    public List<Material> Mats;
    public int Mat_ID=0;
    private XY Current= new XY();   //to set objects index
    private Vector3 cell_Size;      //cell Size to setTilees scale
    private Camera cam;             //refrence to main camera

    struct XY
    {
      public int y;
      public int x;
    }
    private void Start()
    {
        cam = Camera.main;
        GenerateGrid(5);
    }
    private void Update()
    {
        SetTileMatColor();
        if (Input.GetMouseButton(0))
        {
            Draw();
        }
       else if (Input.GetMouseButton(1))
        {
            Erase();
        }
       
    }
    public void GenerateUsingInput()
    {
        if (int.TryParse(textInput.text, out int n)) 
        {
            DeleteGrid();
            GenerateGrid(n);
        } 
    }
    private void GenerateGrid(int size)      //GenerateGrid using Size and cell_size
    {
        Size = size;
        Tiles = new int[Size, 2 * Size];

        cell_Size = new Vector3(transform.localScale.x / Size, transform.localScale.y / (2 * Size),0.1f); 
        int lenght = 2 * Size;
        for (int i = 0; i <= lenght; i++)                   //make horizontal lines (_cylanders_)
        {
            GameObject go = Instantiate(LinePrefab, Vector2.zero, Quaternion.Euler(0, 0, 90));
            go.transform.SetParent(transform);
            go.transform.localPosition = new Vector2(0,(float )i /(lenght)-0.5f);
            go.transform.localScale = new Vector3(go.transform.localScale.x / lineSize, 0.5f, go.transform.localScale.z / lineSize);
        }
        lenght = Size;
        for (int i = 0; i <= lenght; i++)                   //make vertical lines (_cylanders_)
        {
            GameObject go = Instantiate(LinePrefab, Vector2.zero, Quaternion.identity);
            go.transform.SetParent(transform);
            go.transform.localPosition = new Vector2((float)i / (lenght) - 0.5f ,0);
            go.transform.localScale = new Vector3(go.transform.localScale.x / lineSize,0.5f, go.transform.localScale.z / lineSize);
        }
    }
    public void Set_Mat_ID(int ID)          //set id for Tiles matrix and set the material 
    {
        Mat_ID = ID;
    }
    private void SetTileMatColor()         //set a material to prefab to draw
    {

        if (Mat_ID == 1) 
        {
       TilePrefab.GetComponent<Renderer>().material = Mats[0] ;
        }
        else if (Mat_ID == 2)
        {
           TilePrefab.GetComponent<Renderer>().material = Mats[1];
        }
        else if (Mat_ID == 3)
        {
           TilePrefab.GetComponent<Renderer>().material = Mats[2];
        }
        else if (Mat_ID == 4)
        {
           TilePrefab.GetComponent<Renderer>().material = Mats[3];
        }
        else if (Mat_ID == 5)
        {
           TilePrefab.GetComponent<Renderer>().material = Mats[4];
        }
    }
    private void Draw()                     //instantiateTileprefab to mouse pos
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.GetHashCode() != this.gameObject.GetHashCode()) //if hit to another object 
                return;

            GameObject go = Instantiate(TilePrefab, SetTilePos(hit.point), Quaternion.identity);

            go.transform.localScale = cell_Size;
            go.transform.SetParent(transform);

            Tiles[Current.x, Current.y] = Mat_ID;
        }
    }
    private void Erase()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Tile"))
            {
                if (hit.transform.parent.gameObject.GetHashCode() != this.gameObject.GetHashCode()) //if hit to another object 
                    return;

                Destroy(hit.collider.gameObject);
                SetTilePos(hit.point);
                Tiles[Current.x, Current.y] = 0;
            }
        }
    }
    private Vector3 SetTilePos(Vector3 position)      //get position of cell center
    {
        Vector3 result;

        position -= transform.position;             //if this plane is not in 0,0,0
        position = new Vector3(position.x + transform.localScale.x / 2, position.y + transform.localScale.y / 2) ;
        int x = (int)(position.x / cell_Size.x);
        int y = (int)(position.y / cell_Size.y);
        //Debug.Log(x.ToString() + "  " + y.ToString());

        result = new Vector3(x * cell_Size.x + cell_Size.x / 2, y * cell_Size.y + cell_Size.y / 2, position.z) ;
        result = new Vector3(result.x - transform.localScale.x / 2, result.y - transform.localScale.y / 2);
        result += transform.position;

        Current.x = x;
        Current.y = y;
       
        return result;
    }
    private void DeleteGrid()               //delete lines and tiles
    {
        foreach (Transform line in transform) 
        {
            if (line.CompareTag("Line"))
            {
                Destroy(line.gameObject);
            }
        }
        foreach (Transform tile in transform)
        {
            if (tile.CompareTag("Tile"))
            {
                Destroy(tile.gameObject);
            }
        }
    }
    public void Load()
    {
        DeleteGrid();

        GridData data = GridSaveControler.Load();
        GenerateGrid(data.size);

        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size*2; j++)
            {
                if (data.grid[i, j] == 0)
                {
                    Tiles[i, j] = 0;
                }
                else
                {
                    Set_Mat_ID(data.grid[i, j]);
                    SetTileMatColor();
                    Vector3 temp;
                    temp = new Vector3(i * cell_Size.x + cell_Size.x / 2, j * cell_Size.y + cell_Size.y / 2, transform.position.z);
                    temp = new Vector3(temp.x - transform.localScale.x / 2, temp.y - transform.localScale.y / 2);
                    temp += transform.position;
                    GameObject go = Instantiate(TilePrefab, temp, Quaternion.identity);

                    go.transform.localScale = cell_Size;
                    go.transform.SetParent(transform);

                    Tiles[i, j] = Mat_ID;
                }
            }
        }
    }
    public void Save()
    {
        MGrid grid = this;
        GridSaveControler.Save(grid);
    }
}
