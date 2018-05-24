using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class main : MonoBehaviour
{
    private bool undestroyableGM = false;
    private bool btnShowed = false;
    // Use this for initialization
    void Start() {
        if (!undestroyableGM) {
            DontDestroyOnLoad(this.gameObject);
            undestroyableGM = true;
        }
    }
    // Update is called once per frame
    void Update () {
        if(SceneManager.GetActiveScene().name != "LevelUI") { btnShowed = false; }
        else if (!btnShowed){ showBtn(); }
    }

    /// <summary>
    /// control level buttons show or not
    /// </summary>
    void showBtn()
    {
        short a = GetComponent<sl>().getStageUnlock();
        for (short i = 1; i <= a; i++)
        {
            GameObject.FindGameObjectsWithTag("Lbtn"+ i.ToString())[0].SetActive(true);
        }
        for (short i = ++a; i <= 7; i++)
        {
            GameObject.Find(i.ToString()).SetActive(false);
        }
        btnShowed = true;
    }
}
