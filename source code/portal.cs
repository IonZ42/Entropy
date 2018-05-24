using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class portal : MonoBehaviour {

	// Use this for initialization
	void Start () {
        /*
        switch (SceneManager.GetActiveScene().name)
        {
            case "ini": SceneManager.LoadSceneAsync("startUI"); print("never back"); break;
            case "l1d": SceneManager.LoadSceneAsync("l1"); break;
            case "l2d": SceneManager.LoadSceneAsync("l2"); break;
            case "l3d": SceneManager.LoadSceneAsync("l3"); break;
            default:; break;
        }
        */
        SceneManager.LoadSceneAsync("startUI"); print("never back");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
