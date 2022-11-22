using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

namespace MonoSandbox.Components
{
    public class MaterialUtil
    {
        public static void FixStandardShader(Renderer renderer)
        {
            Material mat = renderer.material;
            Material replacementMaterial = new Material(Shader.Find("Standard"));
            replacementMaterial.SetFloat("_Mode", mat.GetFloat("_Mode"));
            replacementMaterial.SetColor("_Color", mat.GetColor("_Color"));
            replacementMaterial.SetFloat("_Glossiness", mat.GetFloat("_Glossiness"));
            replacementMaterial.SetFloat("_Metallic", mat.GetFloat("_Metallic"));
            replacementMaterial.SetInt("_SrcBlend", mat.GetInt("_SrcBlend"));
            replacementMaterial.SetInt("_DstBlend", mat.GetInt("_DstBlend"));

            if (mat.mainTexture != null)
            {
                replacementMaterial.SetTexture("_MainTex", mat.GetTexture("_MainTex"));
                replacementMaterial.mainTextureScale = mat.mainTextureScale;
                replacementMaterial.mainTextureOffset = mat.mainTextureOffset;
            }

            renderer.material = replacementMaterial;
        }

        public static void FixStandardShader(GameObject gameObject)
        {
            Material mat = gameObject.GetComponent<Renderer>().material;
            Material replacementMaterial = new Material(Shader.Find("Standard"));
            replacementMaterial.SetFloat("_Mode", mat.GetFloat("_Mode"));
            replacementMaterial.SetColor("_Color", mat.GetColor("_Color"));
            replacementMaterial.SetFloat("_Glossiness", mat.GetFloat("_Glossiness"));
            replacementMaterial.SetFloat("_Metallic", mat.GetFloat("_Metallic"));
            replacementMaterial.SetInt("_SrcBlend", mat.GetInt("_SrcBlend"));
            replacementMaterial.SetInt("_DstBlend", mat.GetInt("_DstBlend"));

            if (mat.mainTexture != null)
            {
                replacementMaterial.SetTexture("_MainTex", mat.GetTexture("_MainTex"));
                replacementMaterial.mainTextureScale = mat.mainTextureScale;
                replacementMaterial.mainTextureOffset = mat.mainTextureOffset;
            }
            gameObject.GetComponent<Renderer>().material = replacementMaterial;
        }

        public static void FixStandardShader(Transform trasform)
        {
            Material mat = trasform.GetComponent<Renderer>().material;
            Material replacementMaterial = new Material(Shader.Find("Standard"));
            replacementMaterial.SetFloat("_Mode", mat.GetFloat("_Mode"));
            replacementMaterial.SetColor("_Color", mat.GetColor("_Color"));
            replacementMaterial.SetFloat("_Glossiness", mat.GetFloat("_Glossiness"));
            replacementMaterial.SetFloat("_Metallic", mat.GetFloat("_Metallic"));
            replacementMaterial.SetInt("_SrcBlend", mat.GetInt("_SrcBlend"));
            replacementMaterial.SetInt("_DstBlend", mat.GetInt("_DstBlend"));

            if (mat.mainTexture != null)
            {
                replacementMaterial.SetTexture("_MainTex", mat.GetTexture("_MainTex"));
                replacementMaterial.mainTextureScale = mat.mainTextureScale;
                replacementMaterial.mainTextureOffset = mat.mainTextureOffset;
            }
            trasform.GetComponent<Renderer>().material = replacementMaterial;
        }

        public static void FixStandardShadersInObject(GameObject gameObject)
        {
            if (gameObject.transform.GetComponentsInChildren<Renderer>(true).Length == 0)
            {
                FixStandardShader(gameObject);
                return;
            }

            foreach (Renderer ren in gameObject.transform.GetComponentsInChildren<Renderer>(true))
            {
                FixStandardShader(ren);
            }
        }

        public static void FixStandardShadersInObject(Transform transform)
        {
            if (transform.transform.GetComponentsInChildren<Renderer>(true).Length == 0)
            {
                FixStandardShader(transform);
                return;
            }

            foreach (Renderer ren in transform.transform.GetComponentsInChildren<Renderer>(true))
            {
                FixStandardShader(ren);
            }
        }
    }
}
