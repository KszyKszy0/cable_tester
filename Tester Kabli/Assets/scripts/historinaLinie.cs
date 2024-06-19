using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class historinaLinie : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject linia;
    public TMP_Text date;
    public TMP_Text result;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void draw(string tempValue)
    {
        string Date=null;
        string Value=null;
        Debug.Log(tempValue);
        for(int i=0;i<=19;i++)
        {
            Date+=tempValue[i];
        }
        for(int i=0;i<=7;i++)
        {
            Value+=tempValue[i+20];
        }
        date.text=Date;
        if(Value=="12345678")
        {
            result.text="Kabel wykonany poprawnie";
        }else if(Value=="00000000")
        {
            result.text="Brak podłączonego kabla podczas testu";
        }else if (Value=="36145278")
        {
            result.text="Kabel krosowy";
        }else
        {
            result.text="Kabel wykonany niepoprawnie";
        }
        // Debug.Log(Value);
    }
}
