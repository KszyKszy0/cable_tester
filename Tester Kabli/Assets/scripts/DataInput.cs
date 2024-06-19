using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.IO.Ports;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
public class DataInput : MonoBehaviour
{
    SerialPort stream = new SerialPort("COM5");

    string value;
    [SerializeField]
    private TMP_Text text;
    [SerializeField]
    private TMP_Text text2;
    [SerializeField]

    private string dataPath;
    public GameObject lr;
    public List<GameObject> lines= new List<GameObject>();
    public class test
    {
        public string date;
        public string table;
    }
    public List<Coroutine> coroutines=new List<Coroutine>(); 
    public List<GameObject> historyLogs = new List<GameObject>();
    // Start is called before the first frame update
    float t,end;
    bool isTesting;
    public int frequency;
    bool isAutoGoing;
    public GameObject hist;
    public GameObject back;
    public GameObject historyBlock;
    public GameObject content;
    public GameObject scroll;
    public GameObject autoTest;
    public GameObject stopAuto;
    public GameObject s;
    public GameObject c;
    public GameObject mbValue;
    public GameObject mb;
    public GameObject gb;
    public GameObject gbValue;
    public TMP_Text slider;
    public string lastLine;
    public bool isWindow;
    void Start()
    {
        frequency=1;
        if(SerialPort.GetPortNames().ToList().Contains(stream.PortName))
        {
            stream.Open();
        }
        dataPath=Application.dataPath+"test.txt";
        value="00000000";
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            sCoroutine();
        } 
    }
    void goofyAhh()
    {
        isTesting=true;
        closeHistory();
        clearScreen();        //usuwa poprzednie linie
        test temp = new test();                         //nowy obiekt do dodania do historii
        temp.date=System.DateTime.Now.ToString();       //pobranie daty
        Debug.Log(value);
        int repetition=0;
        for(int i=0;i<=value.Length-1;i++)
        {
            if(value[i]==value[0])
            {
                repetition++;
            }
        }
        if(repetition>=8)
        {
            value="00000000";
        }
        if(value=="00300678")
        {
            mb.GetComponent<Image>().color=Color.green;
            mbValue.GetComponent<Image>().color=Color.green;
        }                        
        if(value=="36145278")
        {
            c.GetComponent<Image>().color=Color.green;
            gbValue.GetComponent<Image>().color=Color.green;
            gb.GetComponent<Image>().color=Color.green;
        }
        if(value=="12345678")
        {
            s.GetComponent<Image>().color=Color.green;
            gbValue.GetComponent<Image>().color=Color.green;
            gb.GetComponent<Image>().color=Color.green;
        }
        for(int i=0;i<=7;i++)
        {
            temp.table+=value[i];                   //dopisuje do tabeli z wynikami 
            if(value=="00000000")
            {
                text.text+=0+System.Environment.NewLine;
                text2.text+=0+System.Environment.NewLine;   
            }else                                                   //dopisanie nowych do tekstów
            {
                text.text+=i+1+System.Environment.NewLine;
                text2.text+=i+1+System.Environment.NewLine;
            }                                                                                                          
            if(value[i]!='0')
            {
                var l=Instantiate(lr,new Vector2(-2,4-(0.5f*i)),Quaternion.identity);               //tworzenie i rysowanie linii
                l.GetComponent<LineRenderer>().SetPosition(0,new Vector2(-3.1f,2.8f-(i*0.8f)));
                int r=value[i]-'0';
                l.GetComponent<LineRenderer>().SetPosition(1,new Vector2(3.3f,2.8f-((r-1)*0.8f)));
                lines.Add(l);
            }
        }
        if(lastLine!=temp.date.ToString()+" "+value)
        {
            File.AppendAllText(dataPath,temp.date.ToString()+" "+value+System.Environment.NewLine);
        }
        lastLine=temp.date.ToString()+" "+value;
        isTesting=false;
    }
    public IEnumerator waitForData()
    {
        t=Time.time;
        if(SerialPort.GetPortNames().ToList().Contains(stream.PortName))  // czy stream istnieje
        {
            stream.WriteLine("t");
        while(stream.BytesToRead<=0)
        {
            yield return new WaitForSeconds(0.01f);                        //czeka na dane z arduino
        }
        value=stream.ReadLine();
        }
        Debug.Log(value);
        goofyAhh();
        end=Time.time-t;
    }
    public void sCoroutine()
    {
        StartCoroutine("waitForData"); 
    }
    IEnumerator continuousCoroutine()
    {
        isAutoGoing=true;
        float d;
        while(true)
        {
            while(!isTesting)
            {
                d=1f/frequency;
                yield return new WaitForSeconds((float)d);
                sCoroutine();
            }
            yield return null;
        }
    }
    public void changeStream(string d)
    {
        string [] ports= SerialPort.GetPortNames();
        foreach(string s in ports)
        {
            if(s==d)
            {
                if(SerialPort.GetPortNames().ToList().Contains(stream.PortName))        //czy stream istnieje
                {
                    stream.Close();                                                     //zamyka stary                                           
                }
                stream=new SerialPort(d);                                               //otwiera nowy
                stream.Open();
                Debug.Log(s);
            }
        }
    }
    public void changeStreamAuto()
    {
        if(SerialPort.GetPortNames().ToList().Contains(stream.PortName))                         //sprawdza czy stream istnieje automatycznie
        {
            stream.Close();                                                                      //zamyka stary otwiera nowy
        }
        string [] ports = SerialPort.GetPortNames();
        foreach(string s in ports)                                                              //otwiera automatycznie port
        {
            if(SerialPort.GetPortNames().ToList().Contains(s))
            {
                stream=new SerialPort(s);
                stream.Open();
                Debug.Log(s);
            }
            
        }
        
    }
    public void startAutoTest()
    {
        if(!isAutoGoing)
        {
            autoTest.SetActive(false);
            stopAuto.SetActive(true);
            Coroutine c=StartCoroutine("continuousCoroutine");
            coroutines.Add(c);
        }
        
    }
    public void stopAutoTest()
    {
        autoTest.SetActive(true);
        stopAuto.SetActive(false);
        foreach(Coroutine c in coroutines)
        {
            if(c!=null)
            {
                StopCoroutine(c);
            }
        }
        isAutoGoing=false;
    }
    public void changeFrequency(float change)
    {
        frequency=(int)change;
        slider.text=frequency.ToString()+"Tests/s";
    }
    public void openHistory()
    {
        stopAutoTest();
        clearScreen();
        back.SetActive(true);
        hist.SetActive(false);
        readBlocks();
        StartCoroutine("scrollToTop");
    }
    public void closeHistory()
    {
        foreach(GameObject h in historyLogs)
        {
            if(h!=null)
            {
                Destroy(h);
            }
        }
        hist.SetActive(true);
        back.SetActive(false);
        historyLogs.Clear();
    }
    IEnumerator scrollToTop()
    {
        yield return null;
        scroll.GetComponent<ScrollRect>().verticalNormalizedPosition=1;
    }
    public void clearScreen()
    {
        foreach(GameObject G in lines)          //usuwa poprzednie linie
        {
            if(G!=null)
            {
                Destroy(G);
            }
        }
        text.text="";
        text2.text="";
        s.GetComponent<Image>().color=Color.white;
        c.GetComponent<Image>().color=Color.white;
        mbValue.GetComponent<Image>().color=Color.white;
        mb.GetComponent<Image>().color=Color.white;
        gbValue.GetComponent<Image>().color=Color.white;
        gb.GetComponent<Image>().color=Color.white;
    }
    public void readBlocks()
    {
        StreamReader reader = new StreamReader(dataPath); 
        reader.BaseStream.Seek(0, SeekOrigin.End);
        int count = 0;
        while ((count < 40) && (reader.BaseStream.Position > 0))
        {
            reader.BaseStream.Position--;
            int c = reader.BaseStream.ReadByte();
            if (reader.BaseStream.Position > 0)
                reader.BaseStream.Position--;
            if (c == (int)('\n'))
            {
                ++count;
            }
        }
        string str = reader.ReadToEnd();
        string[] arr = str.Replace("\r", "").Split('\n');
        List<string> test = new List<string>();
        foreach(string s in arr)
        {
            if(s!=null && s!="")
            {
                test.Add(s);
            }
        }
        List<string> temp = new List<string>();
        for(int tempC=test.Count-1;tempC>=0;tempC--)
        {
            temp.Add(test[tempC]);
        }
        test.Clear();
        for(int tempC=0;tempC<=temp.Count-1;tempC++)
        {
            test.Add(temp[tempC]);
            var b=Instantiate(historyBlock,new Vector3(0,0,0),Quaternion.identity,content.transform);
            historyLogs.Add(b); 
        }
        for(int i=0;i<=test.Count-1;i++)
        {
            historyLogs[i].GetComponent<historinaLinie>().draw(test[i]);
        }
        for(int i=0; i<=test.Count-1;i++)
        {
            Debug.Log(test[i]);
        }
        reader.Close();      
    }
    public void onScrollValueChange()
    {
        return;
    }
    public void window()
    {
        if(!isWindow)
        {
            Screen.fullScreen=!Screen.fullScreen;
            isWindow=true;
        }else
        {
            Screen.fullScreen=Screen.fullScreen;
            isWindow=false;
        }
    }
    public void closeApp()
    {
        Application.Quit();
    }
}
//docsy dla mnie jeżeli po podłączeniu są same 000000 to port com jest niepoprawny albo nie było podłączonego kablu podczas pierwszego testu