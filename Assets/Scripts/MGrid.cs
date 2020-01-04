using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;

public class MGrid : MonoBehaviour
{
    public AudioSource draw_erase_voice;
    private bool Is_pen = true;
    private bool touchd = false;
    //........Grid Line Properties..........//
    public GameObject LinePrefab;
    [SerializeField]
    [Range(10,1500)]
    private float lineSize=100;//less values means thicker lines 
    //.....................................//
    public InputField textInput;
    public int Size { get; set; }   //(Size by 2*Size)  grid
    public int[,] Tiles;          //Data in grid that 0 means empty cell
    public GameObject TilePrefab;
    public List<Material> Mats;
    public int Mat_ID=0;
    private XY Current= new XY();   //to set objects index
    public Vector3 cell_Size;      //cell Size to setTilees scale
    private Camera cam;             //refrence to main camera
    public GameObject particles;
    Color Color;
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
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() || Input.touchCount>1 ) //if mouse is over ui not gameobject
            return;



        if (Input.GetMouseButtonDown(0))
        {
            touchd = true;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.GetHashCode() == this.gameObject.GetHashCode()) //if hit to another object 
                    Is_pen = true;
                else if (hit.collider.CompareTag("Tile"))
                    Is_pen = false;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            touchd = false;
        }


        if (Is_pen && touchd)
        {
            Draw();
        }
        else if (!Is_pen && touchd )
        {
            Erase();
        } 
    }
    public void SaveLevelData_Dev()   //to make levels data : size and tiles matrix >> save as txt to use in gamemanager
    {
        string path = "E:\\unity projects\\Symmetry\\LevelSave.txt";

        File.Create(path).Close();
        string builder = "";
        for (int i = 0; i < Size; i++)
        {
            builder += "{";
            for (int j = 0; j < 2*Size; j++)
            {
                builder += Tiles[i, j].ToString() + ",";
            }
            builder = builder.Remove(builder.LastIndexOf(','));
            builder += "},\r\n";
        }
        builder = builder.Remove(builder.LastIndexOf(','));
        File.WriteAllText(path, builder);
    }
    public void GenerateUsingInput()
    {
        if (int.TryParse(textInput.text, out int n)) 
        {
            DeleteGrid();
            GenerateGrid(n);
        } 
    }
    public void GenerateGrid(int size)      //GenerateGrid using Size and cell_size
    {
        Size = size;
        Tiles = new int[Size, 2 * Size];

        lineSize = size *10;

        cell_Size = new Vector3(transform.localScale.x / Size, transform.localScale.y / (2 * Size),0.01f); 
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
    public void SetTileMatColor()         //set a material to prefab to draw
    {

        if (Mat_ID<=Mats.Count) 
        {
       TilePrefab.GetComponent<Renderer>().material = Mats[Mat_ID-1] ;
            Color = Mats[Mat_ID-1].color;
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

            GameObject par=  Instantiate(particles, go.transform.position + new Vector3(0,0,-0.1f), Quaternion.identity);
            par.transform.localScale = par.transform.localScale / (Size/5);
            var main = par.GetComponent<ParticleSystem>().main;
            main.startColor = Color;
            Destroy(par, 0.5f);

            go.transform.localScale = cell_Size;
            go.transform.SetParent(transform);

            Tiles[Current.x, Current.y] = Mat_ID;

            System.Random rand = new System.Random();

            draw_erase_voice.pitch = 1;
            draw_erase_voice.panStereo = 1;
            draw_erase_voice.Play();
        }
    }
    private void Erase()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);            //transform mouse position to worlds ray
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Tile"))
            {
                if (hit.transform.parent.gameObject.GetHashCode() != this.gameObject.GetHashCode()) //if hit to another object 
                    return;

                GameObject par= Instantiate(particles, hit.point, Quaternion.identity);
                par.transform.localScale = par.transform.localScale / (Size / 5);
                var main = par.GetComponent<ParticleSystem>().main;
                main.startColor = hit.transform.gameObject.GetComponent<Renderer>().material.color;

                Destroy(hit.collider.gameObject);

                SetTilePos(hit.point);

                Destroy(par, 0.5f);
                Tiles[Current.x, Current.y] = 0;

                draw_erase_voice.pitch = 1.1f;
                draw_erase_voice.panStereo = -1;
                draw_erase_voice.Play();
            }
        }
    }
    private Vector3 SetTilePos(Vector3 position)      //get position of cell center
    {
        Vector3 result;

        position -= transform.position;             //if this plane is not in 0,0,0
        position = new Vector3(position.x + transform.localScale.x / 2, position.y + transform.localScale.y / 2) ;
        int x = (int)(position.x / cell_Size.x);        //mouse position is on which cell in x axis
        int y = (int)(position.y / cell_Size.y);        //mouse position is on which cell in y axis

        //uncomment if u wanna test
        //Debug.Log(x.ToString() + "  " + y.ToString());    

        result = new Vector3(x * cell_Size.x + cell_Size.x / 2, y * cell_Size.y + cell_Size.y / 2, position.z) ;
        result = new Vector3(result.x - transform.localScale.x / 2, result.y - transform.localScale.y / 2);
        result += transform.position;

        Current.x = x;
        Current.y = y;
       
        return result;
    }
    public void DeleteGrid()               //delete lines and tiles
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

    public void Load()                      //same as LoadPettern in GameManager script
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
