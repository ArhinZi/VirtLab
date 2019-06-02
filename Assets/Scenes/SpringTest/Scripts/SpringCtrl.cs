using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringCtrl : MonoBehaviour
{

    public Vector3 def_pos;
    public Vector3 def_scale;

    // Start is called before the first frame update
    void Start()
    {
        def_pos = gameObject.transform.position;
        def_scale = gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
