using UnityEngine;
using UnityEngine.EventSystems;

public class Puzzle : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{   

    public bool lastBloc = false;
    public bool isPlaced = true;
    public GameObject blocVide;
    private Vector3 uiRotation = new Vector3(0f,0f,0f);

    private Vector3 gameRotation = new Vector3(0f,0f,0f);
    private new Camera camera;
    private Vector3 offSet = new Vector3(0f,0f,0f);
    private Vector3 mousePosition = new Vector3(0f,0f,0f);
    private Vector3 uiScale = new Vector3(2f,2f,2f);

    private Inventaire inventaire; 

    private bool isDrag = false;
    private bool isSnap = false;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("CameraISO").GetComponent<Camera>();
        if( !isPlaced ){
            gameRotation = transform.localRotation.eulerAngles;
            //uiRotation = computeUiRotation();
            transform.localRotation = Quaternion.identity;
            transform.Rotate(uiRotation);
            offSet = this.transform.position - camera.transform.position;
            //inventaire = GameObject.Find("Inventaire").GetComponent<Inventaire>();
            //blocVide = Instantiate(blocVide,transform.position+new Vector3(0f,10f,0f),Quaternion.identity,GameObject.Find("Blocs_Vides").transform);
            //blocVide.name = this.name + "_Vide";
        }else{
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if( !isPlaced && !isDrag ){
            this.transform.position = camera.transform.position + offSet;
        }
    }

    public bool isTheLast(){
        return lastBloc;
    }

    public void OnPointerDown (PointerEventData eventData)
    {   
        if( !isPlaced ){
            //print("OnPointerDown "+this.name);
        }
    }

    public void OnBeginDrag (PointerEventData eventData)
    {       
        if( !isPlaced ){  
            //inventaire.drag(transform.gameObject);
            camera.GetComponent<CameraControllerISO>().puzzleOn();
            isDrag = true;
            transform.localRotation = Quaternion.identity;
            transform.Rotate(gameRotation);
            print("ici");
            transform.localScale = new Vector3(0.6f,0.6f,0.6f);
        }
    }

    public void OnDrag (PointerEventData eventData)
    {    
        mousePosition = camera.ScreenToWorldPoint(new Vector3(eventData.position.x,eventData.position.y,10f));
        if( !isPlaced && !isSnap){
            transform.position = camera.ScreenToWorldPoint(new Vector3(eventData.position.x,eventData.position.y,10f));
            if( eventData.position.x > Screen.width-3 ){
                camera.GetComponent<CameraControllerISO>().outOfScreen(new Vector2(1f,0f));
            }else if( eventData.position.y > Screen.height-3 ){
                camera.GetComponent<CameraControllerISO>().outOfScreen(new Vector2(0f,1f));
            }else if( eventData.position.x < 3 ){
                camera.GetComponent<CameraControllerISO>().outOfScreen(new Vector2(-1f,0f));
            }else if( eventData.position.y < 3 ){
                camera.GetComponent<CameraControllerISO>().outOfScreen(new Vector2(0f,-1f));
            }else{
                camera.GetComponent<CameraControllerISO>().outOfScreen(new Vector2(0f,0f));
            }
        }
    }

    public void OnEndDrag (PointerEventData eventData)
    {   
        if( !isPlaced ){
            //inventaire.unDrag(this.gameObject,false);
            camera.GetComponent<CameraControllerISO>().puzzleOff();
            isDrag = false;
            transform.localRotation = Quaternion.identity;
            transform.Rotate(uiRotation);
            transform.localScale = uiScale;
        }
    }

    public void droped(Vector3 p){
        //inventaire.unDrag(this.gameObject,true);
        isPlaced = true;
        transform.position = new Vector3(p.x,0f,p.z);
        transform.localScale = new Vector3(1f,1f,1f);
        camera.GetComponent<CameraControllerISO>().puzzleOff();
        blocVide.SetActive(false);
        GetComponent<BoxCollider>().enabled = false;
    }

    public void snaped(Vector3 p){
        isSnap = true;
        transform.position = new Vector3(p.x,0f,p.z);
        transform.localScale = new Vector3(1f,1f,1f);
    }

    public void disSnaped(){
        isSnap = false;
        transform.localRotation = Quaternion.identity;
        transform.Rotate(gameRotation);
        transform.localScale = new Vector3(0.6f,0.6f,0.6f);
    }

    public Vector3 getMousePosition(){
        return mousePosition;
    }

    public void setOffSet(){
        camera = GameObject.Find("CameraISO").GetComponent<Camera>();
        offSet = this.transform.position - camera.transform.position;
    }

    private Vector3 computeUiRotation(){
        if( gameRotation.y == 0 ){
            return new Vector3(-45f,0,-45f);
        }else if( gameRotation.y == 90){
            return new Vector3(45f,90f,-45f);
        }else if( gameRotation.y == -90 ){
            return new Vector3(-45f,-90f,45f);
        }
        return new Vector3(0f,0f,0f);
    }
    
}
