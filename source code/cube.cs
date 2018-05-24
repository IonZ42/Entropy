using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class cube : MonoBehaviour
{
    public const int margin = 8;//pixels
    public const int width = 39;//pixels
    public short type;
    public float left;
    public float up;
    public short y = 0;
    public short x = 0;
    public GameObject[] prefabs;
    private GameObject unit;
    #region level_controllers
    public short stage = 1;
    private level1 lc1;
    private level2 lc2;
    private level3 lc3;
    #endregion
    private SpriteRenderer spriteRenderer;
    public bool isSelected
    {
        set
        {
            if (value)//magic!
            {
                spriteRenderer.color = Color.grey;
            }
            else { spriteRenderer.color = Color.white; }
        }
    }
    // Use this for initialization
    void Start()
    {
        switch (stage)
        {
            case 1: lc1 = GameObject.FindWithTag("c1").gameObject.GetComponent<level1>(); break;
            case 2: lc2 = GameObject.FindWithTag("c2").gameObject.GetComponent<level2>(); break;
            case 3: lc3 = GameObject.FindWithTag("c3").gameObject.GetComponent<level3>(); break;
            default:; break;
        }
        spriteRenderer = unit.GetComponent<SpriteRenderer>();
    }
    public void leftUp()
    {
        switch (stage)
        {
            case 1: left = 153.5f; up=460.5f; break;
            case 2: left = 396.5f; up=451.5f; break;
            case 3: left = 101.5f; up=500.5f; break;
            default: break;
        }
    }
    /// <summary>
    /// para:row,col
    /// </summary>
    public void goToPos(short X, short Y,short stageNum)
    {
        y = Y; x = X;stage = stageNum;
        leftUp();
        this.gameObject.transform.position = new Vector3(left + y * width + margin * y, up - x * width - margin * x, -20);//y-(y>0?1:0)
    }
    /// <summary>
    /// para:row,col
    /// </summary>
    public void tweenToPos(short X, short Y, short stageNum)
    {
        y = Y; x = X; stage = stageNum;
        this.gameObject.transform.DOMove(new Vector3(left + y * width + margin * y, up - x * width - margin * x, -20), 0.4f);
    }
    /// <summary>
    /// 加特效，看看能不能行
    /// </summary>
    public void rotateToPos(short X, short Y, short stageNum)
    {
        y = Y; x = X; stage = stageNum;
        this.gameObject.transform.DOJump(new Vector3(left + y * width + margin * y, up - x * width - margin * x, -20), 0.2f, 1, 0.4f);
    }
    public void createUnit(short TYPE)
    {
        if (TYPE != -1)
        {
            unit = Instantiate(prefabs[TYPE]) as GameObject;
            unit.transform.parent = this.transform;//多彩实例分别是list容量个cube的子物体
            unit.transform.parent.GetComponent<cube>().type = TYPE;
        }
        else {
            unit = Instantiate(prefabs[8]) as GameObject;//is blank
            unit.transform.parent = this.transform;
            unit.transform.parent.GetComponent<cube>().type = -1;
        }
    }
    /// <summary>
    /// 暂时只有第二关用到，0变6或6变0
    /// </summary>
    public void changeColorA()
    {
        spriteRenderer.sprite =prefabs[6].GetComponent<SpriteRenderer>().sprite;
    }
    public void changeColorB()
    {
        spriteRenderer.sprite = prefabs[0].GetComponent<SpriteRenderer>().sprite;
    }
    void OnMouseDown()
    {
        switch (stage)
        {
            case 1: lc1.select(this);  break;
            case 2: lc2.select(this); break;
            case 3: lc3.select(this); break;
            default:; break;
        }
    }
    public void clean()
    {
        Destroy(unit.gameObject);
        switch (stage)
        {
            case 1: lc1 = null; break;
            case 2: lc2 = null; break;
            case 3: lc3 = null; break;
            default:; break;
        }
        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
