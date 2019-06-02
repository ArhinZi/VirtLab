using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float deform_koeff;
    public float pos_koeff;
    public float cyl_koeff;

    private void Start()
    {
        canvas.linear.text = Math.Round(canvas.def_linear,3).ToString();
    }
    // Update is called once per frame
    public void FixedUpdate()
    {
        float koeff = float.Parse(canvas.koeff.text);
        float mass = float.Parse(canvas.mass.text);
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
                    
                    direction = spring.def_pos;
                    direction.y = spring.def_pos.y - pos_koeff * mass / koeff / 2;
                    Vector3 scale = spring.def_scale;
                    scale.y = scale.y + deform_koeff * mass / koeff;

                    Vector3 cyl_direction = cylin.basepos;
                    cyl_direction.y = cyl_direction.y - cyl_koeff * mass / koeff;
                    spring.transform.position = Vector3.MoveTowards(spring.transform.position, direction, 0.01f);
                    spring.transform.localScale = Vector3.MoveTowards(spring.transform.localScale, scale, 0.4f);
                    cylin.transform.position = Vector3.MoveTowards(cylin.transform.position, cyl_direction, 0.01f);
                    if (cylin.transform.position == cyl_direction)
                    {
                        state = 2;
                    }
                    break;
                case 2:
                    canvas.linear.text = Math.Round((canvas.def_linear + (mass*9.8/koeff)), 3).ToString();
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
        canvas.koeff.interactable = false;
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
        canvas.koeff.interactable = true;
        canvas.start_button.interactable = true;
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 20), "Exit"))
        {
            Application.Quit();
        }
    }
}
