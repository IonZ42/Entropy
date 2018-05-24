using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class btn : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}
    /// <summary>
    /// Settings
    /// </summary>
    public void BGMSwitch()
    { //如何关闭所有场景BGM，同时不去除音效？
        GameObject.FindWithTag("GM").gameObject.GetComponent<sl>().setBGMon();
        GameObject.FindWithTag("AM").gameObject.GetComponent<audioMng>().setVolume();
    }
       
    public void soundSwitch()
    { GameObject.FindWithTag("GM").gameObject.GetComponent<sl>().setSoundOn () ; }

    public void deleteSave()
    {
        GameObject.FindWithTag("GM").gameObject.GetComponent<sl>().deleteData();
    }

    /// <summary>
    /// reload stage,remember:fix score,reset time,reset cubes
    /// </summary>
    public void reload()
    {
        /*
        switch (SceneManager.GetActiveScene().name)//重置场景的失败尝试
        {
            case "l1": SceneManager.LoadSceneAsync("l1d"); break;
            case "l2": GameObject.FindWithTag("c2").gameObject.GetComponent<level2>().reset(); break;
            case "l3": GameObject.FindWithTag("c3").gameObject.GetComponent<level3>().reset(); break;
            //case "l4":; break;
            default:; break;
        }
        */
        /*
        GameObject.FindWithTag("GM").gameObject.GetComponent<sl>().scoreMns(50.0f);
        *///算了不扣分了
        print("reload current stage");
    }

    /// <summary>
    /// exit game
    /// </summary>
    public void exit()
    {
        GameObject.FindWithTag("GM").gameObject.GetComponent<sl>().setData();
        Application.Quit();
        print("exit");
    }

    /// <summary>
    ///go to different scenes
    /// </summary>
    public void toSetting()
    {
        print("to setting panel");
        SceneManager.LoadSceneAsync("SettingUI");
    }

    public void toStart()
    {
        print("to start panel");
        SceneManager.LoadSceneAsync("startUI");     
    }

    public void toLevel()
    {
        print("to level choose panel");
        SceneManager.LoadSceneAsync("LevelUI");     
    }

    public void toS1()
    {
        SceneManager.LoadSceneAsync("l1");
        print("to stage 1");
    }
    public void toS2()
    {
        SceneManager.LoadSceneAsync("l2");
        print("to stage 2");
    }
    public void toS3()
    {
        SceneManager.LoadSceneAsync("l3");
        print("to stage 3");
    }
    public void toS4()
    { SceneManager.LoadSceneAsync("end"); print("to stage 4"); }


}
