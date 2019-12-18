using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControler : MonoBehaviour
{
    public float speed;
    Vector3 Center;
    Camera cam;

    private void Start()
    {
        Center = transform.position;
        cam = Camera.main;
    }
    public int zoom = 10;
    public int normal = 60;
    public float smooth = 5;

    private bool isZoomed = false;


    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isZoomed = !isZoomed;
        }
      
        if (isZoomed)
        {
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, zoom, Time.deltaTime * smooth);
        }

        else
        {
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, normal, Time.deltaTime * smooth);
        }
    }
}
