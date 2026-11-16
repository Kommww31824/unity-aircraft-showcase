using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fly_jiaoben : MonoBehaviour
{
    public float speed = 0;
    public float angle = 0.1f;
    public float gravity = 0.05f; // 重力加速度
    public float liftForce = 0.15f; // 升力
    private float currentXRotation; // 原Z轴旋转，现改为X轴旋转
    private float currentZRotation; // 原X轴旋转，现改为Z轴旋转
    private Camera mainCamera;
    private Vector3 TargetOffet = new Vector3(0, 14, -30); // X/Z互换


    void Start()
    {
        // 初始化旋转值（X/Z互换）
        currentXRotation = transform.localEulerAngles.x;
        currentZRotation = transform.localEulerAngles.z;

        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {
        print(mainCamera.transform.localPosition);
        mainCamera.transform.localPosition = transform.position + TargetOffet;
        mainCamera.transform.LookAt(transform);

        // 移动控制（X/Z互换）
        if (Input.GetKey(KeyCode.W))
        {
            speed = speed + 0.01f;
            this.transform.Translate(new Vector3(0.05f, 0, 0) * speed); // X轴移动
        }
        if (Input.GetKey(KeyCode.S))
        {
            speed = speed + 0.01f;
            this.transform.Translate(new Vector3(-0.05f, 0, 0) * speed); // X轴移动
        }

        if (speed > 15f)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                this.transform.Translate(0, 0.1f, 0);
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                this.transform.Translate(0, -0.1f, 0);
            }
        }

        // Z轴旋转控制（原X轴旋转）
        currentZRotation = Mathf.Clamp(currentZRotation, -10, 10);
        if (Input.GetKey(KeyCode.Q))
        {
            currentZRotation += angle;
        }
        if (Input.GetKey(KeyCode.E))
        {
            currentZRotation -= angle;
        }

        // X轴旋转控制（原Z轴旋转）
        currentXRotation = Mathf.Clamp(currentXRotation, -10, 10);
        if (Input.GetKey(KeyCode.A))
        {
            currentXRotation += angle;
            this.transform.Translate(new Vector3(0, 0, 0.05f) * speed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            currentXRotation -= angle;
            this.transform.Translate(new Vector3(0, 0, -0.05f) * speed);
        }

        // 应用旋转（X/Z互换）
        transform.localRotation = Quaternion.Euler(
            currentXRotation,  // X轴旋转（原Z轴）
            transform.localEulerAngles.y,
            currentZRotation   // Z轴旋转（原X轴）
        );
        
        // 应用重力和升力
        ApplyGravityAndLift();
    }
    
    void ApplyGravityAndLift()
    {
        // 当速度达到15f时，开始受到重力影响
        if (speed >= 15f)
        {
            // 默认受到重力向下落
            transform.Translate(0, -gravity, 0);
            
            // 当按空格键飞行时，提供升力抵抗重力
            if (Input.GetKey(KeyCode.Space))
            {
                transform.Translate(0, liftForce, 0);
            }
        }
    }
    
    // 碰撞检测，检测是否与地面（r物体）接触   
}

