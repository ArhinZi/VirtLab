using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weighter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public UnityEngine.UI.Text val;
    public GameObject onWeighter = null;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        val.text = (collision.gameObject.GetComponent<Rigidbody>().mass * 9.8f).ToString() + " H";
    }
}
