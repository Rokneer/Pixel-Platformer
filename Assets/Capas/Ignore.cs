using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ignore : MonoBehaviour
{
    public GameObject Minimap;
    // Start is called before the first frame update
    void Start()
    {   
        if (Input.GetKeyDown(KeyCode.M)){
            Minimap.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.M))
        {
            Minimap.SetActive(false);
        }
    }

  
}
