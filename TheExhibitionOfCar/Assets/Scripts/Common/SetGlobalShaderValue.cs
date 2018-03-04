using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGlobalShaderValue : MonoBehaviour
{

    public Cubemap cubeMap;
    public Cubemap cubeMapBlur;

    void Start()
    {
        SetMaterial();
    }
    private void SetMaterial()
    {
        if (cubeMap != null)
        {
            Shader.SetGlobalTexture("_CubeMap",cubeMap);
        }
        if (cubeMapBlur != null)
        {
            Shader.SetGlobalTexture("_CubeMapBlur",cubeMapBlur);
        }
    }

    void OnValidate()
    {
        SetMaterial();
    }
}
