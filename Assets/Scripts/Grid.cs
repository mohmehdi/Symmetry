using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int size { get; set; }   //(size by 2*size) rectangle grid
    public ColorBrush[,] maxtrixBrushes;
    private ColorBrush Current;
    private Vector2 cell_Size; //every cell size



    private void GenerateGrid()
    {
        //GenerateGrid using size and cell_size
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
