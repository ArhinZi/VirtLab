using UnityEngine;
using System.Collections;

public class CameraCtrlV : MonoBehaviour
{

    public float speed = 10;

    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode up = KeyCode.W;
    public KeyCode down = KeyCode.S;
    public KeyCode rotCamA = KeyCode.Q;
    public KeyCode rotCamB = KeyCode.E;
    public KeyCode dCamA = KeyCode.R;
    public KeyCode uCamB = KeyCode.F;

    public Transform startPoint;
    public float maxHeight = 15;
    public float minHeight = 5;
    public int rotationLimit = 1000;

    private float camRotation;
    public float camUD;
    private float height;
    private float tmpHeight;
    private float h, v;
    private bool L, R, U, D;

    void Start()
    {
        height = 2;
        tmpHeight = height;
        camRotation = -90;

    }


    void Update()
    {
        if (Input.GetKey(left) || L) h = -0.25f; else if (Input.GetKey(right) || R) h = 0.25f; else h = 0;
        if (Input.GetKey(down) || D) v = -0.25f; else if (Input.GetKey(up) || U) v = 0.25f; else v = 0;

        if (Input.GetKey(rotCamB)) camRotation -= 1; else if (Input.GetKey(rotCamA)) camRotation += 1;
        //if (Input.GetKey(dCamA)) camUD -= 1; else if (Input.GetKey(uCamB)) camUD += 1;
        //camRotation = Mathf.Clamp(camRotation, 0, rotationLimit);

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            tmpHeight+=0.25f;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            tmpHeight-=0.25f;
        }

        tmpHeight = Mathf.Clamp(tmpHeight, minHeight, maxHeight);
        height = Mathf.Lerp(height, tmpHeight, 3 * Time.deltaTime);

        Vector3 direction = new Vector3(h, 0, v);
        transform.Translate(direction * speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, height, transform.position.z);
        transform.rotation = Quaternion.Euler(camUD, camRotation, 0);
    }
}