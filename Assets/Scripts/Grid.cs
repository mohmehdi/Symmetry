using UnityEngine;

public class Grid : MonoBehaviour
{
    public GameObject LinePrefab;
    public int size { get; set; }   //(size by 2*size) rectangle grid
    public ColorBrush[,] maxtrixBrushes;
    private ColorBrush Current;
    private Vector2 cell_Size; //every cell size

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        //GenerateGrid using size and cell_size
        // LinePrefab.transform.SetParent(transform);
        Vector2 left_corner = new Vector2(transform.localScale.x / 2 - transform.position.x, transform.localScale.y / 2 - transform.position.y);
        // LinePrefab.transform.localPosition = left_corner;

        size = 5;
        cell_Size = new Vector2(transform.localScale.x / size, transform.localScale.y / (2 * size));
        //debug.log(cell_size.x.tostring() + "    " + cell_size.y.tostring());
        for (int i = 0; i < 2 * size + 1; i++)
        {
            GameObject go = Instantiate(LinePrefab, Vector2.zero, Quaternion.Euler(0, 0, 90));
            go.transform.SetParent(transform);
            go.transform.localPosition = new Vector2(0,(float )i /(2*size)-0.5f);
            go.transform.localScale = new Vector3( go.transform.localScale.x/10,0.5f, go.transform.localScale.z/10);
            
        }
        for (int i = 0; i < size + 1; i++)
        {
            GameObject go = Instantiate(LinePrefab, Vector2.zero, Quaternion.identity);
            go.transform.SetParent(transform);
            go.transform.localPosition = new Vector2((float)i / (size) - 0.5f ,0);
            go.transform.localScale = new Vector3(go.transform.localScale.x / 10,0.5f, go.transform.localScale.z / 10);
        }
    }
    public void SetBrush(int ID)
    {
        //select brushes in color pallet adn set it to current
        //ID=0 is eraser
    }

    private void Draw()
    {
        //instatiate or destroy game object
    }
}
