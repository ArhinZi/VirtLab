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

    public GameObject help_panel;
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
                    brus.ToWeighter(); //переміщення бруса на ваги
                    brus.stop = false;
                    StartCoroutine(Wait(1)); // Виклик затримки виконання програми
                    state = 1;
                    break;
                case 1: // виведення результатів вимірювань на екран
                    canvas.gsweighter = (Math.Round(brus.mass * 9.8f, 2)).ToString() + " H"; 
                    canvas.gs_w_res = (Math.Round(brus.mass * 9.8f, 2)).ToString() + " H";
                    StartCoroutine(Wait(2));
                    state = 2;
                    break;
                case 2:
                    brus.ToStation(); // Переміщення брусу на desk
                    StartCoroutine(Wait(1));
                    state = 3;
                    break;
                case 3:
                    dinam.ToStation();// Закріплення дінамометра
                    temp_force = Vector3.zero;
                    StartCoroutine(Wait(1));
                    state = 4;
                    break;
                case 4: // Додавання сили до тіла доки воно не зрушить з місця
                    float x = Mathf.Abs(brus.gameObject.transform.position.x - brus.stationpos.x);
                    dinam.gameObject.transform.position = new Vector3(dinam.stationpos.x + x, dinam.gameObject.transform.position.y, dinam.gameObject.transform.position.z);
                    temp_force.x = temp_force.x + 0.002f;
                    canvas.gsdinamometr = Math.Round(temp_force.x,2).ToString() + " H";
                    brus.GetComponent<Rigidbody>().AddForce(temp_force);
                    brus.fsum = temp_force;
                    dinam.ShowForce(temp_force.x);
                    if (Mathf.Abs(brus.rb.velocity.x) > 0.1f)
                    {
                        canvas.gs_s_res = Math.Round(temp_force.x, 2).ToString() + " H";
                        temp_vel = Mathf.Abs(brus.rb.velocity.x);
                        state = 5;
                    }

                    break;
                case 5: // Віднімання сили доки тіло не почне рухатись рівномірно(швидкість не стане меншою або рівною тій що була на попередньому році
                    x = Mathf.Abs(brus.gameObject.transform.position.x - brus.stationpos.x);
                    dinam.gameObject.transform.position = new Vector3(dinam.stationpos.x + x, dinam.gameObject.transform.position.y, dinam.gameObject.transform.position.z);
                    temp_force.x = temp_force.x - 0.002f;
                    canvas.gsdinamometr = Math.Round(temp_force.x, 2).ToString() + " H";
                    brus.GetComponent<Rigidbody>().AddForce(temp_force);
                    brus.fsum = temp_force;
                    dinam.ShowForce(temp_force.x);
                    if (Mathf.Abs(brus.rb.velocity.x) <= temp_vel)
                    {
                        canvas.gs_d_res = Math.Round(temp_force.x, 2).ToString() + " H";
                        temp_vel = Mathf.Abs(brus.rb.velocity.x);
                        state = 6;
                        temp_time = Time.time;
                    }
                    temp_vel = Mathf.Abs(brus.rb.velocity.x);
                    break;
                case 6: // рівномірно рухаємо тіло 2 секунди
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

                    canvas.ks_if.interactable = true;
                    canvas.kd_if.interactable = true;
                    canvas.mass_if.interactable = true;
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
        dinam.ToDef();
        brus.Kdinamic = float.Parse(canvas.gs_kd_input);
        brus.Kstatic = float.Parse(canvas.gs_ks_input);
        brus.mass = float.Parse(canvas.gs_mass_input);

        canvas.ks_if.interactable = false;
        canvas.kd_if.interactable = false;
        canvas.mass_if.interactable = false;

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

    public void help_button()
    {
        bool active = help_panel.activeSelf;
        if (active)
        {
            help_panel.SetActive(false);
        }
        else
        {
            help_panel.SetActive(true);
        }
    }

    public void Set_button(string str)
    {
        float s = 0;
        float d = 0;

        switch (str)
        {
            case "1":
                s = 0.1f;
                d = 0.07f;
                break;
            case "2":
                s = 0.54f;
                d = 0.32f;
                break;
            case "3":
                s = 0.8f;
                d = 0.53f;
                break;
            case "4":
                s = 1.1f;
                d = 0.15f;
                break;
            case "5":
                s = 1;
                d = 0.4f;
                break;
            default:
                break;
        }
        Debug.Log("set "+s+" "+d);
        canvas.ks_if.text = s.ToString();
        canvas.kd_if.text = d.ToString();
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
