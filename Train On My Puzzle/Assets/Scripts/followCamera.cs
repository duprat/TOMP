using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCamera : MonoBehaviour
{   

    private Vector3 offSet = new Vector3(0f,0f,0f);
    private new Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("CameraISO").GetComponent<Camera>();
        offSet = this.transform.position - camera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = camera.transform.position + offSet;
    }
}
