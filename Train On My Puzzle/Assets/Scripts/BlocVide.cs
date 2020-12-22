using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlocVide : MonoBehaviour, IDropHandler
{   

    // Start is called before the first frame update
    void Start()
    {  
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrop(PointerEventData data)
    {
        if (data.pointerDrag != null)
        {
            data.pointerDrag.GetComponent<Puzzle>().droped(transform.position);
            transform.gameObject.SetActive(false);
        }
    }

    void OnTriggerStay(Collider other){

        if(other.tag == "RailBloc" ){
            //print("Collision entre "+this+" et "+other);
            if( !other.GetComponent<Puzzle>().isPlaced ){
                Vector3 tmp = other.gameObject.GetComponent<Puzzle>().getMousePosition();
                Vector3 mousePosition = new Vector3(tmp.x,0f,tmp.z);
                Vector3 centerPosition = new Vector3(transform.position.x,0f,transform.position.z);
                if( Vector3.Distance(mousePosition,centerPosition) < 15){
                    other.gameObject.GetComponent<Puzzle>().snaped(transform.position);
                }else{
                    other.gameObject.GetComponent<Puzzle>().disSnaped();
                }
            }
        }
    }
    
}
