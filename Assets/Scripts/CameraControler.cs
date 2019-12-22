using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControler : MonoBehaviour
{
    //  public float speed;
    //  Vector3 Center;
    //  Camera cam;
    // private float smooth =0.2f;
    public float smooth;
    public float offcet1;
    public float offcet2;
    public Slider pan;
    public Slider zoom;
    private void Start()
    {
        //Center = transform.position;
        //cam = Camera.main;
    }


 
 void    Update()
    {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, zoom.value * offcet1-1),smooth*Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, pan.value * offcet2,transform.position.z), smooth*Time.deltaTime);


        //float ScrollWheelChange = Input.GetAxis("Mouse ScrollWheel");           //This little peece of code is written by JelleWho https://github.com/jellewie
        //if (ScrollWheelChange != 0)
        //{        

        //If the scrollwheel has changed
        //Vector3 mousepos = cam.ScreenPointToRay(Input.mousePosition).direction;
        //float R = ScrollWheelChange * 15;                                   //The radius from current camera
        //float PosX = Camera.main.transform.eulerAngles.x + 90;              //Get up and down
        //float PosY = -1 * (Camera.main.transform.eulerAngles.y - 90);       //Get left to right
        //PosX = PosX / 180 * Mathf.PI;                                       //Convert from degrees to radians
        //PosY = PosY / 180 * Mathf.PI;                                       //^
        //float X = R * Mathf.Sin(PosX) * Mathf.Cos(PosY);                    //Calculate new coords
        //float Z = R * Mathf.Sin(PosX) * Mathf.Sin(PosY);                    //^
        //float Y = R * Mathf.Cos(PosX);                                      //^
        //float CamX = Camera.main.transform.position.x;                      //Get current camera postition for the offset
        //float CamY = Camera.main.transform.position.y;                      //^
        //float CamZ = Camera.main.transform.position.z;                      //^
        //Camera.main.transform.position = new Vector3(Mathf.Clamp(CamX + X + mousepos.x, -2, 2), Mathf.Clamp(CamY + Y + mousepos.y, -2, 2), Mathf.Clamp(CamZ + Z, -5, -1));//Move the main camera
        //}
    }

 
}
