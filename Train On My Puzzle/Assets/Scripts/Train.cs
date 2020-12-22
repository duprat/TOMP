using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{   

    public static int nbPoints = 13;

    private GameObject[] wayPoint;
    private int currentWayPoint = 0;
    private GameObject[] blocs;
    private GameObject currentBlock = null;
    private List<GameObject> visitedBlocs;
    private bool lastBloc = false;
    private Vector3 position;
    private bool end = false;

    public GameObject train;
    public GameObject firstBloc;
    public float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        blocs = GameObject.FindGameObjectsWithTag("RailBloc");
        visitedBlocs = new List<GameObject>();
        wayPoint = new GameObject[nbPoints];
        position = train.transform.position;
        currentBlock = firstBloc;
        getwayPoint();
        currentWayPoint = 8;
    }

    // Update is called once per frame
    void Update()
    {     

        if( ! end ){

            position = train.transform.position;

            if( currentBlock != null ){
                Vector3 direction = Vector3.Normalize(wayPoint[currentWayPoint].transform.position-position);
                train.transform.position = Vector3.MoveTowards(position,wayPoint[currentWayPoint].transform.position,Time.deltaTime * speed);
                position = train.transform.position;
                train.transform.rotation = Quaternion.LookRotation(direction);
                if( Vector3.Distance(position,wayPoint[currentWayPoint].transform.position) < 1){
                    currentWayPoint++;
                }
            }

            if( currentWayPoint == nbPoints && !(visitedBlocs.Count == blocs.Length)){
                getCurrentBloc();
            }


            //Crash du train
            if( currentBlock == null && !lastBloc){
                print("CRASH");
                end = true;
            }

            //Fin du circuit
            if( currentBlock == null && lastBloc){
                print("FIN");
                end = true;
            }
        }
    }

    void getwayPoint(){
        GameObject points = currentBlock.transform.GetChild(0).GetChild(0).gameObject;
        Vector3 firstPosition = points.transform.GetChild(0).gameObject.transform.position;
        Vector3 lastPosition = points.transform.GetChild(nbPoints-1).gameObject.transform.position;
        float distanceFirst = Vector3.Distance(position,firstPosition);
        float distanceLast = Vector3.Distance(position,lastPosition);

        if( distanceFirst < distanceLast ){
            for(int i = 0; i < nbPoints; i++){
                wayPoint[i] = points.transform.GetChild(i).gameObject;
            }
        }else{
            int j = nbPoints-1;
            for(int i = 0; i < nbPoints; i++){
                wayPoint[i] = points.transform.GetChild(j).gameObject;
                j--;
            }
        }
        currentWayPoint = 0;
    }

    void getCurrentBloc(){
        if(currentBlock != null ){
            visitedBlocs.Add(currentBlock);
        }  

        currentBlock = null;

        float distance = 10000;
        float tmpDistance = 0;

        foreach(GameObject bloc in blocs){
            tmpDistance = Vector3.Distance(firstPointOfBloc(bloc),position);
            if( tmpDistance < distance && !visitedBlocs.Contains(bloc) && tmpDistance < 20 && bloc.GetComponent<Puzzle>().isPlaced){
                distance = tmpDistance;
                currentBlock = bloc;
            }
        }

        if( currentBlock != null ){
            getwayPoint();
            lastBloc = currentBlock.GetComponent<Puzzle>().isTheLast();
        }

    }

    Vector3 firstPointOfBloc(GameObject bloc){
        GameObject points = bloc.transform.GetChild(0).GetChild(0).gameObject;
        Vector3 firstPosition = points.transform.GetChild(0).gameObject.transform.position;
        Vector3 lastPosition = points.transform.GetChild(nbPoints-1).gameObject.transform.position;

        if( Vector3.Distance(position,firstPosition) < Vector3.Distance(position,lastPosition) ){
            return firstPosition;
        }else{
            return lastPosition;
        }

    }

    public void setSpeed(float _speed_)
    {

    }
}
