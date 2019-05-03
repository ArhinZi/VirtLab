using UnityEngine;

public class Controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public Weighter weighter;
    public Dinamometer dinamometer;
    public Brus[] bss;
    public bool fixeverithing = false;
    public Vector3 fixvector = new Vector3();
    public UnityEngine.UI.Text dinamtext;
    public UnityEngine.UI.InputField inputtext;
    Vector3 force;
    // Update is called once per frame
    void Update()
    {
        Transform obj = null;
        if (Input.GetMouseButtonDown(0))
        {
            obj = GetRaycastObj().transform;
            Debug.Log(obj);
        }

        if (obj != null && !fixeverithing)
        {
            Brus b = obj.GetComponent<Brus>();
            if (b != null)
            {
                if (b.state == 0 && weighter.onWeighter == null)
                {
                    weighter.onWeighter = b.gameObject;

                    Vector3 p = weighter.gameObject.transform.position;
                    p.y += 0.1f;
                    b.ToWeighter(p);
                }
                else if (b.state == 1)
                {
                    if (b.itype == 0)
                    {
                        weighter.onWeighter = null;
                        weighter.val.text = "0.0 H";
                        b.ToStation();
                    }
                    if (b.itype == 1)
                    {
                        if (bss[0].state == 2)
                        {
                            weighter.onWeighter = null;
                            weighter.val.text = "0.0 H";
                            b.ToStation();
                        }
                        else
                        {
                            weighter.onWeighter = null;
                            weighter.val.text = "0.0 H";
                            b.ToDef();
                        }
                    }
                }
                else if (b.state == 2)
                {
                    if (b.itype == 0)
                    {
                        foreach (var item in bss)
                        {
                            if (item.state == 2)
                                item.ToDef();
                        }
                        if (dinamometer.state == 1)
                        {
                            dinamometer.ToDef();
                        }
                    }
                    else
                    {
                        b.ToDef();
                    }
                }
            }
            Dinamometer din = obj.GetComponent<Dinamometer>();
            if (din != null)
            {
                if (din.state == 0 && bss[0].state == 2)
                {
                    din.ToStation();
                    dinamometer.gameObject.GetComponent<FixedJoint>().connectedBody = bss[0].GetComponent<Rigidbody>();
                }
                else if(din.state == 1)
                {
                    Vector3 mouse = Input.mousePosition;
                    //mouse.z = Camera.main.farClipPlane;
                    fixvector = din.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(mouse));
                    fixeverithing = true;
                    //screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                    //offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(gameObject.transform.position).z));
                }
            }
        }
        if (Input.GetMouseButton(0) && fixeverithing)
        {
            Vector3 v = dinamometer.transform.TransformPoint(fixvector);
            Vector3 mouse = Input.mousePosition;
            //mouse.z = Camera.main.farClipPlane;
            float force = (Camera.main.ScreenToWorldPoint(mouse).x - v.x);
            //Debug.Log(Camera.main.ScreenToWorldPoint(mouse));
            //Debug.Log(v);
            //Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            //float force = (Camera.main.ScreenToWorldPoint(curScreenPoint) - v).x ;
            //Debug.Log(Camera.main.ScreenToWorldPoint(curScreenPoint));

            //Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset ;
            //dinamometer.transform.position = curPosition;
            Vector3 v3 = new Vector3(3f,0,0)*force;
            bss[0].GetComponent<Brus>().fsum = v3;
            dinamtext.text = ((float)System.Math.Round(v3.x, 2)).ToString() + " H";
            
        }
        if (Input.GetMouseButtonUp(0))
        {
            dinamtext.text = "0.0 H";
            bss[0].GetComponent<Brus>().fsum = Vector3.zero;
            fixeverithing = false;
            dinamometer.GetComponent<Rigidbody>().velocity = Vector3.zero;
            bss[0].GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
    private void FixedUpdate()
    {
        Debug.Log("fsum2 "+ bss[0].GetComponent<Brus>().fsum.x);
        //Debug.Log("F" + bss[0].GetComponent<Brus>().fsum.ToString());
        dinamometer.GetComponent<Rigidbody>().AddForce(bss[0].GetComponent<Brus>().fsum, ForceMode.Force);
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

    public void Refresh()
    {
            dinamometer.ToDef();
            foreach (var brus in bss)
            {

                if (brus.state == 2)
                {
                    brus.ToDef();
                    brus.ToStation();
                    brus.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    brus.GetComponent<Rigidbody>().useGravity = false;
                }
                brus.GetComponent<Rigidbody>().useGravity = true;

            }
        
    }

    public void SetMass()
    {
        bss[0].mass = float.Parse(inputtext.text);
        bss[0].rb.mass = float.Parse(inputtext.text);
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 20), "Exit"))
        {
            Application.Quit();
        }
    }
}
