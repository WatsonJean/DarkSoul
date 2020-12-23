using UnityEngine;
using System.Collections;

public class HittedMatEffect : MonoBehaviour
{
    bool mbActive = false;
    bool mbInit = false;
    Material mMat = null;
    public float mLife;
    private static int s_InnerColor = -1;
    private static int s_AllPower = -1;
    private static int s_AlphaPower = -1;

    void Awake()
    {
        s_InnerColor = Shader.PropertyToID("_InnerColor");
        s_AllPower = Shader.PropertyToID("_AllPower");
        s_AlphaPower = Shader.PropertyToID("_AlphaPower");
    }

    // Use this for initialization
    /// <summary>
    /// 设置材质颜色
    /// </summary>
    /// <param name="color"></param>
    public void SetColor(Color color)
    {
        mMat.SetColor(s_InnerColor, color);

    }

    public void SetLifeTime(float time)
    {
        mLife = time;
    }

    public void Active()
    {
        if (!mbInit)
            AddEffect();
        mMat.SetFloat(s_AllPower, 0.9f);
        mbActive = true;
        mLife = 0.2f;

    }

    void Update()
    {
        if (!mbActive)
            return;
        mLife -= Time.deltaTime;
        if (mLife < 0)
        {
            mbActive = false;
            mMat.SetFloat(s_AllPower, 0);
        }
        float v = Mathf.Sin((1 - mLife) * 8 * Mathf.PI) + 2;
        mMat.SetFloat(s_AlphaPower, v);
    }

    void AddEffect()
    {
        Object mat = Resources.Load("Material/TransparentRim");
        mMat = GameObject.Instantiate(mat) as Material;
        foreach (var curMeshRender in transform.GetComponentsInChildren<Renderer>())
        {
            Material[] newMaterialArray = new Material[curMeshRender.materials.Length + 1];
            for (int i = 0; i < curMeshRender.materials.Length; i++)
            {
                if (curMeshRender.materials[i].name.Contains("HittedMatEffect"))
                {
                    return;
                }
                else
                {
                    newMaterialArray[i] = curMeshRender.materials[i];
                }
            }
            if (null != mMat)
                newMaterialArray[curMeshRender.materials.Length] = mMat;
            curMeshRender.materials = newMaterialArray;
        }
        mbInit = true;
    }

    void RemoveEffect()
    {
        foreach (var curMeshRender in transform.GetComponentsInChildren<Renderer>())
        {
            int newMaterialArrayCount = 0;
            for (int i = 0; i < curMeshRender.materials.Length; i++)
            {
                if (curMeshRender.materials[i].name.Contains("HittedMatEffect"))
                {
                    newMaterialArrayCount++;
                }
            }

            if (newMaterialArrayCount > 0)
            {
                Material[] newMaterialArray = new Material[newMaterialArrayCount];
                int curMaterialIndex = 0;
                for (int i = 0; i < curMeshRender.materials.Length; i++)
                {
                    if (curMaterialIndex >= newMaterialArrayCount)
                    {
                        break;
                    }
                    if (!curMeshRender.materials[i].name.Contains("HittedMatEffect"))
                    {
                        newMaterialArray[curMaterialIndex] = curMeshRender.materials[i];
                        curMaterialIndex++;
                    }
                }
                curMeshRender.materials = newMaterialArray;
            }

        }
    }
}