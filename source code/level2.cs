using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class level2 : MonoBehaviour {

    private bool passOK = false;
    private bool isFirst;
    public const int margin = 8;//pixels
    public const int width = 39;//pixels
    private static short[,] iniMap = new short[9, 9]
    {
{ -1,-1,-1,-1,4,-1,-1,-1,-1 },
{ -1,-1,-1,-1,4,-1,-1,-1,-1 },
{ -1,-1,-1,5,5,5,-1,-1,-1 },
{ -1,-1,5,0,0,0,5,-1,-1 },
{ 4,4,5,0,4,0,5,4,4 },
{ -1,-1,5,0,0,0,5,-1,-1 },
{ -1,-1,-1,5,5,5,-1,-1,-1 },
{ -1,-1,-1,-1,4,-1,-1,-1,-1 },
{ -1,-1,-1,-1,4,-1,-1,-1,-1 }
    };
    public short[,] map = iniMap;
    public cube origin;
    public short size = 9;
    public ArrayList cubeList;
    private cube currentCube;
    public AudioClip swap;
    public AudioClip pass;
    public AudioClip miss;
    public AudioSource AS;
    public Text ctdown, score;
    //void Awake() { }
    // Use this for initialization
    void Start()
    {
        initialize();
        judgeFirst();
    }
    void judgeFirst() { isFirst = GameObject.FindWithTag("GM").gameObject.GetComponent<sl>().getStageUnlock() == 2 ? true : false; }

    // Update is called once per frame
    void Update()
    {
        float remain = 180.0f - Time.timeSinceLevelLoad;
        ctdown.text = (remain > 0) ? (remain.ToString()) : (0.0f.ToString());
        score.text = GameObject.FindWithTag("GM").gameObject.GetComponent<sl>().getScore().ToString();
        if (passOK)
        {
            passOK = false;
            if (remain > 0) { GameObject.FindWithTag("GM").gameObject.GetComponent<sl>().scoreAdd(remain * 5); }
            else { GameObject.FindWithTag("GM").gameObject.GetComponent<sl>().scoreMns(500.0f); }
            print("pass stage 2");
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
    public cube addCube(short row, short col)
    {
        cube unit = Instantiate(origin) as cube;
        unit.transform.parent = this.transform;//为controller子物体
        unit.GetComponent<cube>().createUnit(iniMap[row, col]);
        unit.GetComponent<cube>().goToPos(row, col, 2);//or:(short)((int)size-(int)row)
        return unit;
    }
    /// <summary>
    /// 检测到鼠标输入后一切函数的起点
    /// </summary>
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
        if (a.type == 6) { a.changeColorB();a.type = 0; }else if (a.type == 0) { a.changeColorA(); a.type = 6; }
        if (b.type == 6) { b.changeColorB(); b.type = 0; } else if (b.type == 0) { b.changeColorA();b.type = 6; }
        setCube(a.x, a.y, b);
        setCube(b.x, b.y, a);
        short tempx = a.x;
        a.x = b.x; b.x = tempx;
        short tempy = a.y;
        a.y = b.y; b.y = tempy;

        a.tweenToPos(a.x, a.y, 2);
        b.tweenToPos(b.x, b.y, 2);
        check();
    }
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
    /// 四角大块橡皮筋法遍历纵横map两点之间是否同色
    /// </summary>
    void check()
    {
        int counter = 0;
        if (map[0, 4] != map[1, 4]) { counter++; }//检测四角
        if (map[4, 0] != map[4, 1]) { counter++; }
        if (map[4, 7] != map[4, 8]) { counter++; }
        if (map[7, 4] != map[7, 8]) { counter++; }
        //检测中间，视为大方块，与-1必然不同的有8个
        for(short i = 2; i < 7; i++)
        {
            for(short j = 2; j < 6; j++)
            {
                if (map[i, j] != map[i, j + 1]) { counter++; }
            }
        }
        for (short i = 2; i < 6; i++)
        {
            for (short j = 2; j < 7; j++)
            {
                if (map[i, j] != map[i + 1, j]) { counter++; }
            }
        }
        counter -= 8;
        //print(counter.ToString());
        if (counter == 36) { passOK = true; }
    }
    public void cleanAll()
    {
        map = iniMap;
        foreach (ArrayList tempA in cubeList) { foreach (cube temp in tempA) { temp.clean(); } }
    }
    void passStage()
    {
        if (isFirst)
        {
            GameObject.FindWithTag("GM").gameObject.GetComponent<sl>().setStageUnlock(3);
            GameObject.FindWithTag("GM").gameObject.GetComponent<sl>().setData();
            //isFirst = false;
        }
        //passOK = false;
        SceneManager.LoadSceneAsync("l3");
        //cleanAll();
    }
}
