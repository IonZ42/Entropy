using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showScore : MonoBehaviour {
    public Text SCORE;

	// Use this for initialization
	void Start () {
        SCORE.text = "Your Score:"+GameObject.FindWithTag("GM").gameObject.GetComponent<sl>().getScore().ToString();
    }
	// Update is called once per frame
	void Update () {
		
	}
}
