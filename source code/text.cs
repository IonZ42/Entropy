using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class text : MonoBehaviour
{
    public Text ta, tb;
    // Use this for initialization
    void Start()
    {
        changeMusicText(); changeVoiceText();
    }
    //public void whenOpenSetting() {  }
    public void changeMusicText()
    {
        //text.transform.position = new Vector3(50.5f, 75.4f, -5);
        ta.text= GameObject.FindWithTag("GM").gameObject.GetComponent<sl>().getBGMon() ? "On" : "Off";
    }
    public void changeVoiceText()
    {
        //text.transform.position = new Vector3(50.5f, -39.9f, -5);
        tb.text = GameObject.FindWithTag("GM").gameObject.GetComponent<sl>().getSoundOn() ? "On" : "Off";
    }
    // Update is called once per frame
    void Update()
    {
    }
}