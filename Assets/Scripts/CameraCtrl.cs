using UnityEngine;
using System.Collections;

public class CameraCtrl : MonoBehaviour
{

    public float speed = 5;

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
    private float camUD;
    private float height;
    private float tmpHeight;
    private float h, v;
    private bool L, R, U, D;

    void Start()
    {
        height = 2;
        tmpHeight = height;
        camRotation = 0;
        camUD = 50;
        transform.position = new Vector3(startPoint.position.x, height, startPoint.position.z);

    }


    void Update()
    {
        if (Input.GetKey(left) || L) h = -1; else if (Input.GetKey(right) || R) h = 1; else h = 0;
        if (Input.GetKey(down) || D) v = -1; else if (Input.GetKey(up) || U) v = 1; else v = 0;

        if (Input.GetKey(rotCamB)) camRotation -= 1; else if (Input.GetKey(rotCamA)) camRotation += 1;
        //if (Input.GetKey(dCamA)) camUD -= 1; else if (Input.GetKey(uCamB)) camUD += 1;
        //camRotation = Mathf.Clamp(camRotation, 0, rotationLimit);

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            float s = gameObject.GetComponent<Camera>().orthographicSize;
            if(s >= 0.2) gameObject.GetComponent<Camera>().orthographicSize -= 0.1f;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            float s = gameObject.GetComponent<Camera>().orthographicSize;
            if (s >= 0) gameObject.GetComponent<Camera>().orthographicSize += 0.1f;
        }

        tmpHeight = Mathf.Clamp(tmpHeight, minHeight, maxHeight);
        height = Mathf.Lerp(height, tmpHeight, 3 * Time.deltaTime);

        Vector3 direction = new Vector3(h, v, 0);
        transform.Translate(direction * speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, height, transform.position.z);
        transform.rotation = Quaternion.Euler(camUD, camRotation, 0);
    }
}