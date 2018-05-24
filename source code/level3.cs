using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class level3 : MonoBehaviour
{
    private bool passOK = false;
    private bool isFirst;
    public const int margin = 8;//pixels
    public const int width = 39;//pixels
    private static short[,] iniMap = new short[7, 7]
    {
{ -1,-1,7,7,7,-1,-1 },
{ -1,1,-1,1,-1,1,-1 },
{ 7,-1,-1,2,-1,-1,7 },
{ 7,1,2,2,2,1,7 },
{ 7,-1,-1,2,-1,-1,7 },
{ -1,1,-1,1,-1,1,-1 },
{ -1,-1,7,7,7,-1,-1 }
    };
    public short[,] map = iniMap;
    public cube origin;
    public short size = 7;
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
    void judgeFirst() { isFirst = GameObject.FindWithTag("GM").gameObject.GetComponent<sl>().getStageUnlock() == 3 ? true : false; }
    
    // Update is called once per frame
    void Update()
    {
        float remain = 300.0f - Time.timeSinceLevelLoad;
        ctdown.text = (remain > 0) ? (remain.ToString()) : (0.0f.ToString());
        score.text = GameObject.FindWithTag("GM").gameObject.GetComponent<sl>().getScore().ToString();
        if (passOK)
        {
            passOK = false;
            if (remain > 0) { GameObject.FindWithTag("GM").gameObject.GetComponent<sl>().scoreAdd(remain * 5); }
            else { GameObject.FindWithTag("GM").gameObject.GetComponent<sl>().scoreMns(500.0f); }
            print("pass stage 3");
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
        unit.GetComponent<cube>().goToPos(row, col, 3);//or:(short)((int)size-(int)row)
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
        setCube(a.x, a.y, b);
        setCube(b.x, b.y, a);
        short tempx = a.x;
        a.x = b.x; b.x = tempx;
        short tempy = a.y;
        a.y = b.y; b.y = tempy;

        a.tweenToPos(a.x, a.y, 3);
        b.tweenToPos(b.x, b.y, 3);
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
    void partRotate(short row, short col, cube readyOne)
    {
        setCube(row, col, readyOne);
        readyOne.x = row;readyOne.y = col;
        readyOne.rotateToPos(readyOne.x, readyOne.y, 3);
    }
    //只不过是暴力调用16次！CPU撑得住，大轮：[0,2],[0,3],[0,4],[1,5],[2,6],[3,6],[4,6],[5,5],[6,4],[6,3],[6,2],[5,1],[4,0],[3,0],[2,0],[1,1]共16个
    public void rotate()
    {
        cube temp = getCube(1, 1);
        partRotate(1, 1, getCube(0, 2));
        partRotate(0, 2, getCube(0, 3));
        partRotate(0, 3, getCube(0, 4));
        partRotate(0, 4, getCube(1, 5));
        partRotate(1, 5, getCube(2, 6));
        partRotate(2, 6, getCube(3, 6));
        partRotate(3, 6, getCube(4, 6));
        partRotate(4, 6, getCube(5, 5));
        partRotate(5, 5, getCube(6, 4));
        partRotate(6, 4, getCube(6, 3));
        partRotate(6, 3, getCube(6, 2));
        partRotate(6, 2, getCube(5, 1));
        partRotate(5, 1, getCube(4, 0));
        partRotate(4, 0, getCube(3, 0));
        partRotate(3, 0, getCube(2, 0));
        partRotate(2, 0, temp);
    }
    /// <summary>
    /// 大轮+双杠，橡皮筋法遍历纵横map两点之间是否同色
    /// </summary>
    void check()
    {
        rotate();
        int counter = 0;
        for(short i = 0; i < 6; i++) { if (map[i, 3] != map[i + 1, 3]) { counter++; } }//双杠
        for(short j = 0; j < 6; j++) { if (map[3, j] != map[3, j + 1]) { counter++; } }
        //只不过是暴力if16次！CPU撑得住，大轮：[0,2],[0,3],[0,4],[1,5],[2,6],[3,6],[4,6],[5,5],[6,4],[6,3],[6,2],[5,1],[4,0],[3,0],[2,0],[1,1]共16个
        if(map[1, 1]!=map[0, 2]){ counter++; }
        if(map[0, 2]!=map[0, 3]){ counter++; }
        if(map[0, 3]!=map[0, 4]){ counter++; }
        if(map[0, 4]!=map[1, 5]){ counter++; }
        if(map[1, 5]!=map[2, 6]){ counter++; }
        if(map[2, 6]!=map[3, 6]){ counter++; }
        if(map[3, 6]!=map[4, 6]){ counter++; }
        if(map[4, 6]!=map[5, 5]){ counter++; }
        if(map[5, 5]!=map[6, 4]){ counter++; }
        if(map[6, 4]!=map[6, 3]){ counter++; }
        if(map[6, 3]!=map[6, 2]){ counter++; }
        if(map[6, 2]!=map[5, 1]){ counter++; }
        if(map[5, 1]!=map[4, 0]){ counter++; }
        if(map[4, 0]!=map[3, 0]){ counter++; }
        if(map[3, 0]!=map[2, 0]){ counter++; }
        if(map[2, 0]!=map[1, 1]){ counter++; }
        //print(counter.ToString());
        if (counter == 28) { passOK = true; }
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
            GameObject.FindWithTag("GM").gameObject.GetComponent<sl>().setStageUnlock(4);
            GameObject.FindWithTag("GM").gameObject.GetComponent<sl>().setData();
            //isFirst = false;
        }
        //passOK = false;
        print("demo is over now!thanks for playing");
        SceneManager.LoadSceneAsync("end");
        //cleanAll();
    }
}
