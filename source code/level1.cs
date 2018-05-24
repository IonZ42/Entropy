using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class level1 : MonoBehaviour
{
    private bool passOK = false;
    private bool isFirst;
    public const int margin = 8;//pixels
    public const int width = 39;//pixels
    private static short[,] iniMap = new short[7, 7] 
    {
{ 0,0,1,2,-1,-1,-1 },
{ 0,1,2,3,-1,-1,-1 },
{ 1,2,2,3,-1,-1,-1 },
{ 2,3,3,0,3,3,2 },
{ -1,-1,-1,3,2,2,1 },
{ -1,-1,-1,3,2,1,0 },
{ -1,-1,-1,2,1,0,0}
    };//why static works??
    public short[,] map = iniMap;
    public cube origin;
    public short size = 7;
    public ArrayList cubeList;
    private cube currentCube;
    public AudioClip swap;
    public AudioClip pass;
    public AudioClip miss;
    public AudioSource AS;
    //public float timer;
    //public float countDown = 120.0f;
    public Text ctdown, score;
    void judgeFirst() { isFirst = GameObject.FindWithTag("GM").gameObject.GetComponent<sl>().getStageUnlock() == 1 ? true : false; }
    //public void reset() {SceneManager.LoadScene("l1", LoadSceneMode.Single); }

    //void Awake() { }
    // Use this for initialization

    void Start()
    {
        initialize();
        judgeFirst();
    }
    // Update is called once per frame
    void Update()
    {
        float remain = 120.0f - Time.timeSinceLevelLoad;
        ctdown.text = (remain>0)? (remain.ToString()) : (0.0f.ToString());
        score.text = GameObject.FindWithTag("GM").gameObject.GetComponent<sl>().getScore().ToString();
        if (passOK)
        {
            passOK = false;
            if (remain > 0) { GameObject.FindWithTag("GM").gameObject.GetComponent<sl>().scoreAdd(remain*5); }
            else { GameObject.FindWithTag("GM").gameObject.GetComponent<sl>().scoreMns(500.0f); }
            print("pass stage 1");
            AS.PlayOneShot(pass);
            Invoke("passStage", 3.0f);
        }
    }
    public void initialize()
    {
        cubeList = new ArrayList();
        for (short i = 0; i < size; i++)
        {
            ArrayList temp = new ArrayList();
            for (short j = 0; j < size; j++)
            {
                cube unit = addCube(i, j);
                temp.Add(unit);
            }
            cubeList.Add(temp);//二维数组
        }
    }
    public void cleanAll()
    {
        map = iniMap;
        foreach(ArrayList tempA in cubeList) { foreach (cube temp in tempA) { temp.clean(); } }
        cubeList = null;
        print("here");
    }
    /// <summary>
    /// only use in beginning
    /// </summary>
    public cube addCube(short row, short col)
    {
        cube unit = Instantiate(origin) as cube;
        unit.transform.parent = this.transform;
        unit.GetComponent<cube>().createUnit(iniMap[row,col]);
        unit.GetComponent<cube>().goToPos(row,col,1);//(short)((int)size-(int)row)
        return unit;
    }
    /// <summary>
    /// 检测到鼠标输入后一切函数的起点
    /// </summary>
    /// <param name="onClick"></param>
    public void select(cube onClick)
    {
        if (onClick.type == -1) { return; }//ignore the air cube
        else
        {
            if (currentCube == null)
            {
                currentCube = onClick; currentCube.isSelected = true;
                return;
            }
            else
            {
                if (Mathf.Abs(currentCube.x - onClick.x) + Mathf.Abs(currentCube.y - onClick.y) == 1)
                {
                    //StartCoroutine(Exchange(currentCube, onClick));
                    exchange(currentCube, onClick);
                }
                else
                {
                    AS.PlayOneShot(miss);
                }
                currentCube.isSelected = false;
                currentCube = null;
            }
        }
    }
    //IEnumerator Exchange(cube a, cube b) { };
    public void exchange(cube a, cube b)
    {
        AS.PlayOneShot(swap);
        setCube(a.x, a.y, b);
        setCube(b.x, b.y, a);
        short tempx = a.x;
        a.x = b.x;b.x = tempx;
        short tempy = a.y;
        a.y = b.y;b.y = tempy;

        a.tweenToPos(a.x, a.y,1);
        b.tweenToPos(b.x, b.y,1);
        check();
    }
    /// <summary>
    /// ???many bugs
    /// </summary>
    public void setCube(short row, short col, cube readyOne)
    {
        ArrayList temp = cubeList[row] as ArrayList;
        temp[col] = readyOne;
        map[row, col] = readyOne.GetComponent<cube>().type;//update the map;
    }
    public cube getCube(short row, short col)
    {
        ArrayList temp = cubeList[row] as ArrayList;
        cube thisOne = temp[col] as cube;
        return thisOne;
    }
    /// <summary>
    /// 双方块橡皮筋法遍历纵横map两点之间是否同色
    /// </summary>
    void check()
    {
        int counter = 0;
        for(short i = 0; i < 4; i++)
        {
            for(short j = 0; j < 3; j++)
            {
                if (map[i, j] != map[i, j + 1]) { counter++; }
            }
        }
        for (short i = 0; i < 3; i++)
        {
            for (short j = 0; j < 4; j++)
            {
                if (map[i, j] != map[i + 1, j]) { counter++; }
            }
        }
        for (short i = 3; i < size; i++)
        {
            for (short j = 3; j < size-1; j++)
            {
                if (map[i, j] != map[i, j + 1]) { counter++; }
            }
        }
        for (short i = 3; i < size-1; i++)
        {
            for (short j = 3; j < size; j++)
            {
                if (map[i, j] != map[i + 1, j]) { counter++; }
            }
        }
        //print(counter.ToString());
        if (counter == 48) { passOK = true; }
    }
    void passStage()
    {
        if (isFirst)
        {
            GameObject.FindWithTag("GM").gameObject.GetComponent<sl>().setStageUnlock(2);
            GameObject.FindWithTag("GM").gameObject.GetComponent<sl>().setData();
            //isFirst = false;
        }
        //passOK = false;
        SceneManager.LoadSceneAsync("l2");
        //cleanAll();//这里调用有个缺点，空白时间太长了（因为延迟调用函数），暂时未解决
    }
}