using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settingsMove : MonoBehaviour
{
    public DataInput dInput;
    // Start is called before the first frame update
    bool isMoved;
    bool isMoving;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void onClick()
    {
        if(!isMoved && !isMoving)
        {
            StartCoroutine(move(10));
            isMoved=true;
        }else if(!isMoving)
        {
            StartCoroutine(move(-10));
            isMoved=false;
        }
    }
    IEnumerator move(int x)
    {
        isMoving=true;
        Vector3 destination=transform.position+new Vector3(x,0,0);
        while(transform.position!=destination)
        {
            transform.position=Vector3.Lerp(transform.position,transform.position+new Vector3(x,0,0),0.05f);
            yield return null;
        }
        isMoving=false;
    }
}
