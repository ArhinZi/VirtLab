using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dinamometer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        defpos = this.transform.position;
        
    }

    public Vector3 defpos;
    public Vector3 stationpos;
    public int state = 0;
    // 0-def
    // 1-station


    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToDef()
    {
        state = 0;
        this.gameObject.transform.position = defpos;
    }

    public void ToStation()
    {
        state = 1;
        this.gameObject.transform.position = stationpos;
        defposzost = ost.transform.localPosition.z;
        defscaleyspr = spr.transform.localScale.y;
    }


    public GameObject ost;
    public GameObject spr;
    float defposzost;
    float defscaleyspr;
    public void ShowForce(float f)
    {
        Vector3 temp = ost.transform.localPosition;
        ost.transform.localPosition = new Vector3(temp.x, temp.y, defposzost + f*(-0.165f));
        temp = spr.transform.localScale;
        spr.transform.localScale = new Vector3(temp.x, defscaleyspr + f*7, temp.z);
    }
}
