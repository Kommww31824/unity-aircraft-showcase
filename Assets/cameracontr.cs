using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameracontr : MonoBehaviour
{

    public float maxVerticalAngle = 90f;
    public float minVerticalAngle = -90f;

    public float lerpSpeed = 10f;
    private float targetRotationX = 0f;
    private float targetRotationY = 0f;

    public bool istrue = false;
    public Camera guding;
    public Camera ziyou;

    public float rotationSpeed = 10f;
    private void Awake()
    {
        ziyou.gameObject.SetActive(true);
        guding.gameObject.SetActive(false);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float rotationXInput = -Input.GetAxis("Mouse Y");
            float rotationYInput = -Input.GetAxis("Mouse X");
            targetRotationX -= rotationXInput * rotationSpeed;
            targetRotationY += rotationYInput * rotationSpeed;

            targetRotationX = Mathf.Clamp(targetRotationX, minVerticalAngle, maxVerticalAngle);
            Quaternion targetRotation = Quaternion.Euler(targetRotationX, targetRotationY, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpSpeed * Time.deltaTime);
        }
    }

    public void CamerButton()
    {
        if (istrue)
        {
            guding.gameObject.SetActive(true);
            ziyou.gameObject.SetActive(false);
            istrue = !istrue;
        }
        else 
        {
            ziyou.gameObject.SetActive(true);
            guding.gameObject.SetActive(false);
            istrue = !istrue;
        }
    }
}
