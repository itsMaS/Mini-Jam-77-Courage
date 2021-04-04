using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    private void Awake()
    {
        Instance = this;
    }

    public Camera cam;

    public Vector3 worldPoint { get; private set; }

    private void Update()
    {
        Plane m_Plane = new Plane(Vector3.up, Vector3.zero);

        //Create a ray from the Mouse click position
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        //Initialise the enter variable
        float enter = 0.0f;

        if (m_Plane.Raycast(ray, out enter))
        {
            //Get the point that is clicked
            Vector3 hitPoint = ray.GetPoint(enter);
            worldPoint = hitPoint;
            Debug.DrawLine(cam.transform.position, hitPoint);
        }
    }
}
