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
    public Vector3 weighterpos;
    public int itype = 0;
    public int state = 0;
    public bool stop = false;
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
        rb.AddForce(Vector3.down*mass*9.8f);
        //Debug.Log("Added " + Vector3.down * mass * 9.8f);
        //if(rb.velocity.x < 0)
        //{
        //    rb.velocity = Vector3.zero;
        //}

        Fstatic = mass * 9.8f * Kstatic;
        Fdinamic = mass * 9.8f * Kdinamic;
        if (state == 2 && !stop)
        {
            if (Mathf.Abs(rb.velocity.x) < 0.1)
            {
                if (Mathf.Abs(fsum.x) <= Fstatic)
                {
                    Vector3 f = new Vector3((fsum.x ) * -1,0,0);
                    gameObject.GetComponent<Rigidbody>().AddForce(f);
                    //Debug.Log("Added0 "+ f);
                }
            }
            else
            {
                float xdirect = rb.velocity.normalized.x * -1;
                Vector3 f = new Vector3(xdirect * Fdinamic, 0, 0);
                gameObject.GetComponent<Rigidbody>().AddForce(f);
                //Debug.Log("Added1 " + f);
            }
        }
        else if (stop)
        {
            rb.velocity = Vector3.zero;
        }
    }
    
    public void ToWeighter()
    {
        this.gameObject.transform.position = weighterpos;
        rb.velocity = Vector3.zero;
        state = 1;
    }
    public void ToDef()
    {
        this.gameObject.transform.position = defpos;
        rb.velocity = Vector3.zero;
        state = 0;
    }
    public void ToStation()
    {
        this.gameObject.transform.position = stationpos;
        rb.velocity = Vector3.zero;
        state = 2;
    }
}
