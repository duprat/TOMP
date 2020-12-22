using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventaire : MonoBehaviour
{

    public Vector3[] slots;

    private List<GameObject> blocs = new List<GameObject>();
    private GameObject fond;

    // Start is called before the first frame update
    void Start()
    {   
        fond = transform.GetChild(0).gameObject;
        for(int i = 1; i < transform.childCount; i++){
            blocs.Add(transform.GetChild(i).gameObject);
        }
        int j = 0;
        foreach (GameObject bloc in blocs)
        {
            bloc.transform.position = slots[j];
            bloc.GetComponent<Puzzle>().setOffSet();
            bloc.transform.localScale = new Vector3(0.3f,0.3f,0.3f);
            bloc.GetComponent<Puzzle>().isPlaced = false;
            j++;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void drag(GameObject obj){
        fond.SetActive(false);
        foreach (GameObject bloc in blocs)
        {   
            if( bloc != obj ){
                bloc.SetActive(false);
            }
        }
    }

    public void unDrag(GameObject obj,bool placed){
        fond.SetActive(true);
        if( placed ){
            blocs.Remove(obj);
        }
        foreach (GameObject bloc in blocs)
        {   
            bloc.SetActive(true);
        }
    }
}
