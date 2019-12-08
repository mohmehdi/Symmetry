using UnityEngine;

public class Grid : MonoBehaviour
{
    //........Grid Properties..........//
    public GameObject LinePrefab;
    [SerializeField]
    [Range(10,200)]
    private float lineSize=100;
    //................................//
    public int Size { get; set; }   //(Size by 2*Size) rectangle grid
    public int[,] Brushes;
    public GameObject BrushPrefab;
    private XY Current= new XY();
    private Vector3 cell_Size; //every cell Size
    private Camera cam;

    struct XY
    {
      public int y;
      public int x;
    }
    private void Start()
    {
        cam = Camera.main;
        Size = 5;
        GenerateGrid();
        Brushes = new int[Size,2* Size];
    }
    private void Update()
    {
        SetColor();
        if (Input.GetMouseButton(0))
        {
            Draw();
        }
        if (Input.GetMouseButton(1))
        {
            Erase();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < 2*Size; j++)
                {
                    Debug.Log(Brushes[i, j]);
                }
            }
        }
    }
    public void SetColor()
    {
        BrushPrefab.GetComponent<Renderer>().material.color = Color.blue ;
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

    private void Draw()
    {
        //instatiate or destroy game object
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.GetHashCode() != this.gameObject.GetHashCode())
                return;

            GameObject go = Instantiate(BrushPrefab, SetBrush(hit.point), Quaternion.identity);


            go.transform.localScale = cell_Size;
            go.transform.SetParent(transform);

            Brushes[Current.x, Current.y] = 1;
        }
    }
    private void Erase()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Tile"))
            {
                Destroy(hit.collider.gameObject);
                SetBrush(hit.point);
                Brushes[Current.x, Current.y] = 0;
            }
        }
    }
    private Vector3 SetBrush(Vector3 position) 
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

        Current.x = x;
        Current.y = y;
       
        return result;
    }
}
