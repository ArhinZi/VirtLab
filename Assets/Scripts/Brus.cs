using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brus : MonoBehaviour
{

    private void Start()
    {
        defpos = this.transform.position;
    }
    Vector3 defpos;
    public Vector3 stationpos;
    public int itype = 0;
    public int state = 0;
    //0-base
    //1- on weighter
    //2- on station

    public Rigidbody rb;
    public Vector3 fsum;
    public float mass;
    public float Kstatic;
    public float Kdinamic;
    public float Fstatic=0;
    public float Fdinamic=0;
    public void Update()
    {
        
    }
    private void FixedUpdate()
    {
        Fstatic = mass * 9.8f * Kstatic;
        Fdinamic = mass * 9.8f * Kdinamic;
        if (state == 2)
        {
            Debug.Log("fsum "+fsum.x);
            if (Mathf.Abs(rb.velocity.x) < 0.000001)
            {
                if (Mathf.Abs(fsum.x) <= Fstatic)
                {
                    Vector3 f = new Vector3(fsum.x * -1,0,0);
                    Debug.Log("1" + f.x.ToString());
                    gameObject.GetComponent<Rigidbody>().AddForce(f);
                }
            }
            else
            {
                float xdirect = rb.velocity.normalized.x * -1;
                Debug.Log(xdirect);
                Debug.Log("2" + (xdirect * Fdinamic).ToString());
                gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(xdirect*Fdinamic,0,0), ForceMode.Force);
            }
        }
    }
    
    public void ToWeighter(Vector3 v3)
    {
        this.gameObject.transform.position = v3;
        state = 1;
    }
    public void ToDef()
    {
        this.gameObject.transform.position = defpos;
        state = 0;
    }
    public void ToStation()
    {
        this.gameObject.transform.position = stationpos;
        state = 2;
    }
}
