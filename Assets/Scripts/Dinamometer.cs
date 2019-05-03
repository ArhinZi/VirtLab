using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dinamometer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        defpos = this.transform.position;
        rb = this.GetComponent<Rigidbody>();
        GetComponent<FixedJoint>().connectedBody = nulgo.GetComponent<Rigidbody>();
    }

    public Vector3 defpos;
    public Vector3 stationpos;
    public GameObject nulgo;
    public int state = 0;
    // 0-def
    // 1-station

    Rigidbody rb;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToDef()
    {
        state = 0;
        GetComponent<FixedJoint>().connectedBody = nulgo.GetComponent<Rigidbody>();
        this.gameObject.transform.position = defpos;
    }

    public void ToStation()
    {
        state = 1;
        this.gameObject.transform.position = stationpos;
    }
}
