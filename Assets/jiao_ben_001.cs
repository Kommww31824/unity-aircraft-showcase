using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jiao_ben_001 : MonoBehaviour
{
    public float rotaSpeed = 10;
    // Start is called before the first frame update
    void Start()
    {
        //print("start");
    }

    // Update is called once per frame
    void Update()
    {
        //print("update"); 
        transform.Rotate(Vector3.up, rotaSpeed * Time.deltaTime, Space.World);
    }
}
