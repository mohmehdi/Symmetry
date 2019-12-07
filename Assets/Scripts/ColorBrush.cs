using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class ColorBrush : MonoBehaviour
{
    public int X;
    public int Y;
    public int color_ID;
    public void setColor()
    {
        //set color using ID
    }
    public void set(int x,int y, int id)
    {
        X=x;
        Y=y;
        color_ID = id;
    }
}
