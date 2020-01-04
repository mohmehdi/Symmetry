using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControler : MonoBehaviour
{
    public float smooth;
    public float offcet1;
    public float offcet2;
    public Slider pan;
    public Slider zoom;


 
 void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, zoom.value * offcet1-1),smooth*Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, pan.value * offcet2,transform.position.z), smooth*Time.deltaTime);
    }

 
}
