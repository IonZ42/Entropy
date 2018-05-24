using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// not complete，音效没法管，绑的小物件太多了
/// 注意与btn的交互
/// </summary>
public class audioMng : MonoBehaviour {
    private bool undestroyableGM = false;
    public List<AudioClip> BGMs = new List<AudioClip>();
    public AudioSource BGM;
    private string nowName;//=null;
    public bool BGMon = true;//=false;
    // Use this for initialization
    void Start()
    {
        if (!undestroyableGM)
        {
            DontDestroyOnLoad(this.gameObject);
            undestroyableGM = true;
        }
        if (nowName == null)
        {
            nowName = "ini";
        }
        setVolume();
    }
    // Update is called once per frame
    void Update()
    {
        string tempName = SceneManager.GetActiveScene().name;
        if (tempName != nowName)
        {
            changeBGM(tempName);
        }
    }
    public void setVolume()
    {
        BGMon = GameObject.FindWithTag("GM").gameObject.GetComponent<sl>().getBGMon();
        if (BGMon) { BGM.volume = 1.0f; }
        else{ BGM.volume = 0.0f; }
    }
    void changeBGM(string sceneName)
    {
        switch (sceneName)//场景关系是有向图，nowName指向tempName
        {
            case "ini":; break;
            case "startUI":if (nowName == "ini") { BGM.Stop();print("here"); BGM.PlayOneShot(BGMs[0],0.3f); }if (nowName == "end") { BGM.Stop(); BGM.PlayOneShot(BGMs[0], 0.3f); } break;//else: same BGM
            case "SettingUI":; break;
            case "LevelUI":if (nowName != "startUI") { BGM.Stop(); BGM.PlayOneShot(BGMs[0], 0.3f); } break;//else:same BGM
            case "l1": BGM.Stop(); BGM.PlayOneShot(BGMs[1], 0.3f); break;
            case "l2": BGM.Stop(); BGM.PlayOneShot(BGMs[2], 0.3f); break;
            case "l3": BGM.Stop(); BGM.PlayOneShot(BGMs[3], 0.3f); break;
            case "end": BGM.Stop(); BGM.PlayOneShot(BGMs[4],0.4f); break;
            default:;break;
        }
        nowName = sceneName;
    }
}
