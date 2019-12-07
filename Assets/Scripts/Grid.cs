using UnityEngine;

public class Grid : MonoBehaviour
{
    //........Grid Properties..........//
    public GameObject LinePrefab;
    [SerializeField]
    [Range(10,200)]
    private float lineSize;
    //................................//
    public int Size { get; set; }   //(Size by 2*Size) rectangle grid
    public ColorBrush[,] maxtrixBrushes;
    public GameObject BrushPrefab;
    private ColorBrush Current;
    private Vector3 cell_Size; //every cell Size
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        Size = 5;
        GenerateGrid();
    }
    private void Update()
    {
        SetBrush(1);
        if (Input.GetMouseButton(0))
        {
            Draw();
        }
        if (Input.GetMouseButton(1))
        {
            Erase();
        }
    }
    private void GenerateGrid()
    {
                                        //GenerateGrid using Size and cell_size

        cell_Size = new Vector3(transform.localScale.x / Size, transform.localScale.y / (2 * Size),0.1f); //cells Size
        int lenght = 2 * Size;
        for (int i = 0; i <= lenght; i++)     //make horizontal lines (_cylanders_)
        {
            GameObject go = Instantiate(LinePrefab, Vector2.zero, Quaternion.Euler(0, 0, 90));
            go.transform.SetParent(transform);
            go.transform.localPosition = new Vector2(0,(float )i /(lenght)-0.5f);
            go.transform.localScale = new Vector3(go.transform.localScale.x / lineSize, 0.5f, go.transform.localScale.z / lineSize);
        }
        lenght = Size;
        for (int i = 0; i <= lenght; i++)                                   //make vertical lines (_cylanders_)
        {
            GameObject go = Instantiate(LinePrefab, Vector2.zero, Quaternion.identity);
            go.transform.SetParent(transform);
            go.transform.localPosition = new Vector2((float)i / (lenght) - 0.5f ,0);
            go.transform.localScale = new Vector3(go.transform.localScale.x / lineSize,0.5f, go.transform.localScale.z / lineSize);
        }
    }
    public void SetBrush(int ID)
    {
        //select brushes in color pallet adn set it to current
        //ID=0 is eraser
        Current = BrushPrefab.GetComponent<ColorBrush>();
        if (ID==1)
        {
            Current.color_ID = 1;
            Material mat=  BrushPrefab.GetComponent<Renderer>().material;
            mat.color = Color.blue;
        }
        else if(ID==0)
        {
            Current.color_ID = 0;
        }
    }

    private void Draw()
    {
                                                  //instatiate or destroy game object
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
            if (hit.collider.gameObject.GetHashCode() != this.gameObject.GetHashCode())
                return;

                GameObject go = Instantiate(BrushPrefab, Vector3.zero, Quaternion.identity);
                SetBrush(hit.point, ref go);
            }
    }
    private void Erase()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.GetComponent<ColorBrush>() != null) 
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
    private void SetBrush(Vector3 position ,ref GameObject go) 
    {
        Vector3 result;

        position -= transform.position;
        position = new Vector3(position.x + transform.localScale.x / 2, position.y + transform.localScale.y / 2) ;
        int x = (int)(position.x / cell_Size.x);
        int y = (int)(position.y / cell_Size.y);
       // Debug.Log(x.ToString() + "  " + y.ToString());

        result = new Vector3(x * cell_Size.x + cell_Size.x / 2, y * cell_Size.y + cell_Size.y / 2, position.z) ;
        result = new Vector3(result.x - transform.localScale.x / 2, result.y - transform.localScale.y / 2);
        result += transform.position;

        go.transform.position = result;
        go.transform.localScale = cell_Size;
        go.transform.SetParent(transform);

        go.GetComponent<ColorBrush>().set(x, y, 1);
    }
}
