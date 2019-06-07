using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Controller2 : MonoBehaviour
{
    public bool started = false;
    public bool pause = false;
    public bool pause2 = false;
    public int state = 0;
    public bool stepper = false;

    public Brus brus;
    public Dinamometer dinam;
    public CanvasCtrl canvas;

    public Vector3 temp_force;
    public float temp_vel;
    public float temp_time;

    public GameObject help_panel;
    public HelpCtrl helpctrl;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Wait());
        
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        if (started && !pause && !pause2)
        {
            switch (state)
            {
                case 0:
                    canvas.gsweighter = "0 H";
                    canvas.gsdinamometr = "0 H";
                    brus.ToWeighter(); //переміщення бруса на ваги
                    brus.stop = false;
                    StartCoroutine(Wait(1)); // Виклик затримки виконання програми
                    state = 1;
                    break;
                case 1: // виведення результатів вимірювань на екран
                    canvas.gsweighter = (Math.Round(brus.mass * 9.8f, 2)).ToString() + " H"; 
                    canvas.gs_w_res = (Math.Round(brus.mass * 9.8f, 2)).ToString() + " H";
                    StartCoroutine(Wait(2));
                    if (stepper) pause_button();
                    state = 2;
                    break;
                case 2:
                    brus.ToStation(); // Переміщення брусу на desk
                    StartCoroutine(Wait(1));
                    state = 3;
                    break;
                case 3:
                    dinam.ToStation();// Закріплення дінамометра
                    StartCoroutine(Wait(1));
                    if (stepper) pause_button();
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
                        if (stepper) pause_button();
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
        pause2 = false;
        Time.timeScale = 1;
        temp_force = Vector3.zero;
        started = true;
        state = 0;
        dinam.ToDef();
        brus.Kdinamic = float.Parse(canvas.gs_kd_input, CultureInfo.InvariantCulture);
        brus.Kstatic = float.Parse(canvas.gs_ks_input, CultureInfo.InvariantCulture);
        brus.mass = float.Parse(canvas.gs_mass_input, CultureInfo.InvariantCulture);

        canvas.ks_if.interactable = false;
        canvas.kd_if.interactable = false;
        canvas.mass_if.interactable = false;

        brus.fsum = Vector3.zero;
        dinam.ShowForce(0);
    }
    public void reset()
    {
        pause2 = false;
        Time.timeScale = 1;

        started = false;
        state = -1;
        dinam.ShowForce(0);
        brus.fsum = Vector3.zero;
        brus.ToDef();
        dinam.ToDef();

        canvas.ks_if.interactable = true;
        canvas.kd_if.interactable = true;
        canvas.mass_if.interactable = true;
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

    public void pause_button()
    {
        if (pause2)
        {
            pause2 = false;
            stepper = false;
            Time.timeScale = 1;
        }
        else
        {
            pause2 = true;
            Time.timeScale = 0;
        }
    }

    public void next_button()
    {
        if (!started) start_button();
        pause2 = false;
        Time.timeScale = 1;
        stepper = true;
    }

    public void Set_button(string str)
    {
        float s = 0;
        float d = 0;

        switch (str)
        {
            case "1":
                s = float.Parse(helpctrl.if11.text, CultureInfo.InvariantCulture);
                d = float.Parse(helpctrl.if21.text, CultureInfo.InvariantCulture);
                break;
            case "2":
                s = float.Parse(helpctrl.if12.text, CultureInfo.InvariantCulture);
                d = float.Parse(helpctrl.if22.text, CultureInfo.InvariantCulture);
                break;
            case "3":
                s = float.Parse(helpctrl.if13.text, CultureInfo.InvariantCulture);
                d = float.Parse(helpctrl.if23.text, CultureInfo.InvariantCulture);
                break;
            case "4":
                s = float.Parse(helpctrl.if14.text, CultureInfo.InvariantCulture);
                d = float.Parse(helpctrl.if24.text, CultureInfo.InvariantCulture);
                break;
            case "5":
                s = float.Parse(helpctrl.if15.text, CultureInfo.InvariantCulture);
                d = float.Parse(helpctrl.if25.text, CultureInfo.InvariantCulture);
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
