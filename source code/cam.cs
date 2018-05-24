using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam : MonoBehaviour {

    private bool undestroyableGM = false;
    // Use this for initialization
    void Start()
    {
        if (!undestroyableGM)
        {
            DontDestroyOnLoad(this.gameObject);
            undestroyableGM = true;
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
