using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControllerISO : MonoBehaviour
{   
    public float speed = 10;
    public GameObject focusObject;
    public float minimumZoom = 3f;
    public float baseZoom;

    private Vector3 targetPosition;
    private new Camera camera;
    private Vector3 direction;
    private float zoom;
    private Vector3 offSet;
    private bool isLocked = true;
    private bool puzzleMod = false;
    private float originalZoom;

    // Start is called before the first frame update
    void Start()
    {   
        camera = this.GetComponent<Camera>(); 
        offSet = targetPosition - transform.position;
        camera.orthographicSize = baseZoom;
        originalZoom = camera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {   
        if( isLocked ){
            LockPosition();
        }else{
            transform.position = Vector3.Lerp(transform.position,transform.position+direction,camera.orthographicSize/speed);
        }
        if( !((camera.orthographicSize + zoom) < minimumZoom) ){
            camera.orthographicSize += zoom;
        }
    }

    //Appuie sur la touche Move
    public void OnMove(InputValue val){
        if(puzzleMod){
            direction = new Vector3(val.Get<Vector2>().x,0,val.Get<Vector2>().y);
        }else{
            direction = new Vector3(val.Get<Vector2>().x,val.Get<Vector2>().y,val.Get<Vector2>().x);
        }
    }

    //Centre la caméra sur l'objet focus
    private void LockPosition(){
        targetPosition = focusObject.GetComponent<Transform>().position;
        Vector3 desiredPosition = targetPosition - offSet;
        Vector3 smoothPosition = Vector3.Lerp(transform.position,desiredPosition,0.125f);
        transform.position = smoothPosition;
    }

    //Appuie sur la touche Lock
    public void OnLock(){
        if( puzzleMod ){
            puzzleOff();
        }else{
            isLocked = !isLocked;
        }
    }

    //Appuie sur la touche Zoom
    public void OnZoom(InputValue val){
        print("onZoom");
        zoom = val.Get<float>() * Time.deltaTime * speed * 2;
    }

    public Vector3 getfocusObjectPosition(){
        return focusObject.GetComponent<Transform>().position;
    }

    public void puzzleOn(){
        isLocked = false;
        puzzleMod = true;
        camera.transform.position = getfocusObjectPosition() + new Vector3(0f,30,0f);
        //camera.transform.position = transform.position + offSet + new Vector3(0f,30f,0f);
        camera.transform.localRotation = Quaternion.identity;
        camera.transform.Rotate(new Vector3(90f,0f,0f));
        camera.orthographicSize = 100;
    }

    public void puzzleOff(){
        isLocked = true;
        puzzleMod = false;
        camera.transform.localRotation = Quaternion.identity;
        camera.transform.Rotate(new Vector3(30f,-45f,0));
        camera.orthographicSize = originalZoom;
    }

    public void outOfScreen(Vector2 dir){
        direction = new Vector3(dir.x,0,dir.y) * 2;
    }
}
