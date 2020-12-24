using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public class LockTargetObj
    {
        public GameObject obj;
        public float halfHeight;
        public ActorManager actorManager;
        public LockTargetObj(GameObject obj, float halfHeight)
        {
            this.obj = obj;
            this.halfHeight = halfHeight;
            this.actorManager = obj.GetComponent<ActorManager>();
        }
    }

    IUserInput pi;
    ActionController actionController;
    public float HorizontalSpeed = 100;
    public float VerticalSpeed = 100;
    public float maxV_angle = 60;
    public float minV_angle = -30;
    public float smoothDampTime = 1;
    public RawImage  img_lockDot;
    public bool lockState = false;
    public float lockTargetMaxDis = 10f;
    public string lockLayerMaskName = "enemy";
    [SerializeField]
    private LockTargetObj LockTarget;
    GameObject playerHandle;
    GameObject cameraHandle;
    GameObject cameraGo;
    GameObject model;
    float tempEulerAngles_X = 0;
    Vector3 velocity;
    void Start()
    {
        playerHandle = transform.parent.parent.gameObject;
        cameraHandle = transform.parent.gameObject;
        pi = playerHandle.GetComponent<IUserInput>();
        actionController = playerHandle.GetComponent<ActionController>();
        model = actionController.model;

        if (!actionController.isAI)
        {
            cameraGo = Camera.main.gameObject;
            Cursor.lockState = CursorLockMode.Locked;
            img_lockDot.enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (LockTarget == null)
        {
            //水平旋转
            Vector3 tempModelEuler = model.transform.eulerAngles;
            playerHandle.transform.Rotate(Vector3.up, pi.mouseX * Time.fixedDeltaTime * HorizontalSpeed);
            model.transform.eulerAngles = tempModelEuler; //不让角色模型跟着相机旋转

            //垂直
            tempEulerAngles_X += pi.mouseY * Time.fixedDeltaTime * -VerticalSpeed;
            tempEulerAngles_X = Mathf.Clamp(tempEulerAngles_X, minV_angle, maxV_angle);
            cameraHandle.transform.localEulerAngles = new Vector3(tempEulerAngles_X, 0, 0);
        }
        else
        {
            Vector3 targetVec = LockTarget.obj.transform.position - model.transform.position;
            targetVec.y = 0;//保持同一平面
            playerHandle.transform.forward = Vector3.Slerp(playerHandle.transform.forward, targetVec.normalized,0.3f);
            cameraHandle.transform.LookAt(LockTarget.obj.transform.position+Vector3.up*0.8f);
            if (Vector3.Distance(model.transform.position, LockTarget.obj.transform.position) > lockTargetMaxDis || 
                LockTarget.actorManager !=null && LockTarget.actorManager.attributeMgr.isDie)
            {
                LockTarget = null;
            }
        }
        if ( !actionController.isAI)
        {
            //跟随
            cameraGo.transform.position = Vector3.SmoothDamp(cameraGo.transform.position, transform.position, ref velocity, smoothDampTime);
            //cameraGo.transform.position = Vector3.Slerp(cameraGo.transform.position, transform.position, smoothDampTime);
            cameraGo.transform.LookAt(cameraHandle.transform);
        }
    }
    void ShowLockDot()
    {

        lockState = LockTarget != null;
        if (!actionController.isAI)
        {
            img_lockDot.enabled = lockState;
            if (LockTarget != null)
            {
                Vector3 viewPos = Camera.main.WorldToScreenPoint(LockTarget.obj.transform.position + new Vector3(0, LockTarget.halfHeight, 0));
                img_lockDot.rectTransform.position = viewPos;
            }
        }
    }
    void LateUpdate()
    {
        ShowLockDot();
    }

   public void LockUnLock()
    {
            Vector3 origin =model.transform.position + new Vector3(0,1,0);
            Vector3 boxCenter = origin + model.transform.forward * 5.0f;
            int layer = LayerMask.GetMask(lockLayerMaskName);
            Collider[] cols = Physics.OverlapBox(boxCenter, new Vector3(1, 1, 5), model.transform.rotation, layer);
            if (cols.Length ==0 )
            {
                LockTarget = null;
               model.transform.forward = playerHandle.transform.forward;
            }
            else
            {
                foreach (var item in cols)
                {
                    if (LockTarget !=null && LockTarget.obj == item.gameObject)//如果去锁定当前同一个被锁定的目标则取消锁定
                    {
                        LockTarget = null;
                        break;
                    }
                    LockTarget = new LockTargetObj(item.gameObject, item.bounds.extents.y);
                    if (LockTarget.actorManager!=null && !LockTarget.actorManager.attributeMgr.isDie)//锁定没有死亡的敌人
                    {
                        model.transform.forward = playerHandle.transform.forward;
                        break;
                    }
                    else
                        LockTarget = null;
         
                }
            }
    }
}
