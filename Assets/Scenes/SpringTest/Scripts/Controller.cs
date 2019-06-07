using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Controller : MonoBehaviour
{
    public bool started = false;
    public bool pause = false;
    public int state = 0;

    public Cylinder cylin;
    public SpringCtrl spring;
    public CanvasCtrl2 canvas;

    public Vector3 cylin_target_pos;
    public Vector3 spring_target_pos;
    public Vector3 spring_target_scale;

    public float spring_mult;
    public float cylin_mult;

    public float zat_k;
    public float amplitude = 0;

    public float mass;
    public float koeff;

    float time = 0;

    private void Start()
    {
        canvas.linear.text = Math.Round(canvas.def_linear,3).ToString();
        set_rand();
        mass = float.Parse(canvas.mass.text);
    }
    // Update is called once per frame
    public void FixedUpdate()
    {
        
        if (started && !pause)
        {
            switch (state)
            {
                case 0:
                    Vector3 direction = cylin.basepos;
                    cylin.transform.position = Vector3.MoveTowards(cylin.transform.position, direction, 0.03f);
                    if (cylin.transform.position.y >= cylin.basepos.y && cylin.transform.position.z >= cylin.basepos.z)
                    {
                        state = 1;
                    }
                    break;
                case 1:
                    amplitude = (mass * 9.8f / koeff);
                    float w0 = Mathf.Sqrt(koeff/mass);
                    float zat = Mathf.Pow(2.71828f, -1 * zat_k * time);
                    
                    float delta = amplitude + amplitude * zat * Mathf.Cos(w0/2*time + Mathf.PI);

                    cylin.transform.position = new Vector3(cylin.basepos.x, cylin.basepos.y + (cylin_mult * delta), cylin.basepos.z);
                    spring.transform.localScale = new Vector3(spring.def_scale.x, spring.def_scale.y + (spring_mult * delta), spring.def_scale.z);

                    time += Time.deltaTime;
                    if (zat < 0.0001)
                    {
                        state = 2;
                    }
                    break;
                case 2:
                    canvas.linear.text = Math.Round(canvas.def_linear + amplitude,3).ToString();
                    canvas.koeff.text = Math.Round(koeff, 3).ToString();
                    state = 3;
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
        canvas.mass.interactable = false;
        //canvas.koeff.interactable = false;
        canvas.start_button.interactable = false;
    }
    public void reset()
    {
        started = false;
        state = 0;
        spring.transform.position = spring.def_pos;
        spring.transform.localScale = spring.def_scale;
        cylin.transform.position = cylin.defpos;
        canvas.linear.text = Math.Round(canvas.def_linear, 3).ToString();
        canvas.mass.interactable = true;
        //canvas.koeff.interactable = true;
        canvas.start_button.interactable = true;
        time = 0;
    }

    public void set_rand()
    {
        int min = int.Parse(canvas.mink.text);
        int max = int.Parse(canvas.maxk.text);
        Random rand = new Random();
        koeff = rand.Next(min, max);
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 20), "Exit"))
        {
            Application.Quit();
        }
    }
}
