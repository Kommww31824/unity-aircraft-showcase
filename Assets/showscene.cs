/*using System.Collections;
using System.Collections.Generic;
using HighlightingSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class showscene : MonoBehaviour
{
    public GameObject infoWindow;
    private TMP_Text modelText;
    private Camera camera;


    // Start is called before the first frame update
    void Start()
    {
        infoWindow.SetActive(false);
        modelText = infoWindow.transform.Find("modelText").GetComponent<TMP_Text>();
        
    }

    // Update is called once per frame
    void Update()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0)) 
        {
            if (Physics.Raycast(ray, out hit, LayerMask.NameToLayer("show"))&& !EventSystem.current.IsPointerOverGameObject()) 
            {
                if (hit.collider.gameObject.GetComponent<Highlighter>() != null)
                {
                    modelText.text = hit.collider.gameObject.
                    GetComponent<infoshow>().infor;
                    infoWindow.SetActive(true);
                    print("jihuo");
                }

            }
        }
    }
}
*/
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using HighlightingSystem;

public class ShowScene : MonoBehaviour
{
    public GameObject infoWindow;
    private TMP_Text modelText;
    private Camera mainCamera;

    void Start()
    {
        infoWindow.SetActive(false);
        modelText = infoWindow.transform.Find("modelText").GetComponent<TMP_Text>();
        mainCamera = Camera.main; // 直接使用主相机
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 正确设置层掩码
            int showLayer = LayerMask.NameToLayer("show");
            if (showLayer == -1)
            {
                Debug.LogError("Layer 'show' not defined!");
                return;
            }
            LayerMask layerMask = 1 << showLayer; // 转换位掩码

            if (Physics.Raycast(ray, out hit, 100f, layerMask))
            {
                // 检查必要组件
                if (hit.collider.TryGetComponent(out Highlighter highlighter) &&
                    hit.collider.TryGetComponent(out infoshow info))
                {
                    modelText.text = info.infor;
                    infoWindow.SetActive(true);
                    Debug.Log($"Activated for: {hit.collider.name}");
                }
            }
        }
    }

    // 可靠的UI穿透检测
    private bool IsPointerOverUI()
    {
        if (EventSystem.current != null)
        {
#if UNITY_EDITOR
            return EventSystem.current.IsPointerOverGameObject();
#else
                return Input.touchCount > 0 && 
                       EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
#endif
        }
        return false;
    }
}