using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCtrl : MonoBehaviour
{
    public Text weighter;
    public Text dinamometr;
    public Text ks_input;
    public Text kd_input;
    public Text mass_input;

    public Text w_res;
    public Text s_res;
    public Text d_res;

    public string gsweighter
    {
        set
        {
            weighter.text = value;
        }
    }
    public string gsdinamometr
    {
        set
        {
            dinamometr.text = value;
        }
    }
    public string gs_ks_input
    {
        get
        {
            return ks_input.text;
        }
    }
    public string gs_kd_input
    {
        get
        {
            return kd_input.text;
        }
    }
    public string gs_mass_input
    {
        get
        {
            return mass_input.text;
        }
    }

    public string gs_w_res
    {
        set
        {
            w_res.text = value;
        }
    }
    public string gs_s_res
    {
        set
        {
            s_res.text = value;
        }
    }
    public string gs_d_res
    {
        set
        {
            d_res.text = value;
        }
    }
}
