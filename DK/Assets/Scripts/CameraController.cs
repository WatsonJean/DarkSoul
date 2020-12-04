using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    IUserInput pi;
    ActionController actionController;
    public float HorizontalSpeed = 100;
    public float VerticalSpeed = 100;
    public float maxV_angle = 60;
    public float minV_angle = -30;
    public float smoothDampTime = 1;
    GameObject playerHandle;
    GameObject cameraHandle;
    GameObject cameraGo;
    float tempEulerAngles_X = 0;
    Vector3 velocity;
    void Awake()
    {
        playerHandle = transform.parent.parent.gameObject;
        cameraHandle = transform.parent.gameObject;
        cameraGo = Camera.main.gameObject;
        pi = playerHandle.GetComponent<IUserInput>();
        actionController = playerHandle.GetComponent<ActionController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //水平旋转
        Vector3 tempModelEuler = actionController.model.transform.eulerAngles;
        playerHandle.transform.Rotate(Vector3.up, pi.mouseX * Time.deltaTime * HorizontalSpeed);
        actionController.model.transform.eulerAngles = tempModelEuler; //不让角色模型跟着相机旋转

        //垂直
        tempEulerAngles_X += pi.mouseY * Time.deltaTime * -VerticalSpeed;
        tempEulerAngles_X = Mathf.Clamp(tempEulerAngles_X, minV_angle, maxV_angle);
        cameraHandle.transform.localEulerAngles = new Vector3(tempEulerAngles_X, 0, 0);
        //跟随
        cameraGo.transform.position = Vector3.SmoothDamp(cameraGo.transform.position, transform.position, ref velocity, smoothDampTime);
        // cameraGo.transform.position = Vector3.Slerp(cameraGo.transform.position, transform.position, smoothDampTime*Time.deltaTime);
        cameraGo.transform.LookAt(cameraHandle.transform);

    }

    void LateUpdate()
    {


    }
}
