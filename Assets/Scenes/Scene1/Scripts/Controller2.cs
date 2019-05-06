using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2 : MonoBehaviour
{
    public bool started = false;
    public bool pause = false;
    public int state = 0;

    public Brus brus;
    public Dinamometer dinam;
    public CanvasCtrl canvas;

    public Vector3 temp_force;
    public float temp_vel;
    public float temp_time;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Wait());
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        if (started && !pause)
        {
            switch (state)
            {
                case 0:
                    brus.ToWeighter();
                    brus.stop = false;
                    dinam.ToDef();
                    StartCoroutine(Wait(1));
                    state = 1;
                    break;
                case 1:
                    canvas.gsweighter = (Math.Round(brus.mass * 9.8f, 2)).ToString();
                    canvas.gs_w_res = (Math.Round(brus.mass * 9.8f, 2)).ToString();
                    StartCoroutine(Wait(2));
                    state = 2;
                    break;
                case 2:
                    brus.ToStation();
                    StartCoroutine(Wait(1));
                    state = 3;
                    break;
                case 3:
                    dinam.ToStation();
                    temp_force = Vector3.zero;
                    StartCoroutine(Wait(1));
                    state = 4;
                    break;
                case 4:
                    float x = Mathf.Abs(brus.gameObject.transform.position.x - brus.stationpos.x);
                    dinam.gameObject.transform.position = new Vector3(dinam.stationpos.x + x, dinam.gameObject.transform.position.y, dinam.gameObject.transform.position.z);
                    temp_force.x = temp_force.x + 0.002f;
                    canvas.gsdinamometr = Math.Round(temp_force.x,2).ToString();
                    brus.GetComponent<Rigidbody>().AddForce(temp_force);
                    brus.fsum = temp_force;
                    dinam.ShowForce(temp_force.x);
                    if (Mathf.Abs(brus.rb.velocity.x) > 0.1f)
                    {
                        canvas.gs_s_res = Math.Round(temp_force.x, 2).ToString();
                        temp_vel = Mathf.Abs(brus.rb.velocity.x);
                        state = 5;
                    }

                    break;
                case 5:
                    x = Mathf.Abs(brus.gameObject.transform.position.x - brus.stationpos.x);
                    dinam.gameObject.transform.position = new Vector3(dinam.stationpos.x + x, dinam.gameObject.transform.position.y, dinam.gameObject.transform.position.z);
                    temp_force.x = temp_force.x - 0.002f;
                    canvas.gsdinamometr = Math.Round(temp_force.x, 2).ToString();
                    brus.GetComponent<Rigidbody>().AddForce(temp_force);
                    brus.fsum = temp_force;
                    dinam.ShowForce(temp_force.x);
                    if (Mathf.Abs(brus.rb.velocity.x) < temp_vel)
                    {
                        canvas.gs_d_res = Math.Round(temp_force.x, 2).ToString();
                        temp_vel = Mathf.Abs(brus.rb.velocity.x);
                        state = 6;
                        temp_time = Time.time;
                    }
                    temp_vel = Mathf.Abs(brus.rb.velocity.x);
                    break;
                case 6:
                    x = Mathf.Abs(brus.gameObject.transform.position.x - brus.stationpos.x);
                    dinam.gameObject.transform.position = new Vector3(dinam.stationpos.x + x, dinam.gameObject.transform.position.y, dinam.gameObject.transform.position.z);
                    brus.GetComponent<Rigidbody>().AddForce(temp_force);
                    brus.fsum = temp_force;
                    
                    if (Time.time - temp_time >= 2)
                    {
                        brus.rb.velocity = Vector3.zero;
                        brus.stop = true;
                        state = 7;
                    }
                    break;
                case 7:
                    dinam.ShowForce(0);
                    break;
                default:
                    break;
            }
        }
    }
    IEnumerator Wait(int secs)
    {
        pause = true;
        yield return new WaitForSeconds(secs);
        pause = false;
    }

    public void start_button()
    {
        started = true;
        state = 0;
        brus.Kdinamic = float.Parse(canvas.gs_kd_input);
        brus.Kstatic = float.Parse(canvas.gs_ks_input);
        brus.mass = float.Parse(canvas.gs_mass_input);

        brus.fsum = Vector3.zero;
        dinam.ShowForce(0);
    }
    public void reset()
    {
        started = false;
        state = -1;
        dinam.ShowForce(0);
        brus.fsum = Vector3.zero;
        brus.ToDef();
        dinam.ToDef();
    }
    GameObject GetRaycastObj()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {

            return (hit.transform.gameObject.transform.parent != null ? hit.transform.gameObject.transform.parent : hit.transform.gameObject.transform).gameObject;

        }
        return null;
    }
    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 20), "Exit"))
        {
            Application.Quit();
        }
    }
}
