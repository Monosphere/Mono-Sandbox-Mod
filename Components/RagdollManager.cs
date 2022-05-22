using System;
using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine;
using Utilla;
using BepInEx;

public class RagdollManager : MonoBehaviour
{
    bool primaryDown;
    bool canPlace;
    public bool useGorilla = false;
    public bool editMode = false;
    public bool explodeMode = true;

    public SpringJoint joint;

    GameObject Cursor = null;
    public GameObject RagdollModel = null;
    public GameObject GorillaModel = null;
    GameObject itemsFolder = null;


    void OnGameInitialized(object sender, EventArgs e)
    {
        itemsFolder = GameObject.Find("ItemFolderMono");
    }

    void Update()
    {
        RaycastHit hitInfo;
        Physics.Raycast(GorillaLocomotion.Player.Instance.rightHandTransform.position + GorillaLocomotion.Player.Instance.rightHandTransform.forward / 8, GorillaLocomotion.Player.Instance.rightHandTransform.forward, out hitInfo);
        if (itemsFolder == null)
        {
            itemsFolder = GameObject.Find("ItemFolderMono");
        }
        if (Cursor != null)
        {
            Cursor.transform.position = hitInfo.point + new Vector3(0,0.15f,0);
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out primaryDown);
            if (primaryDown)
            {
                if (canPlace)
                {
                    if (useGorilla)
                    {
                        GameObject Ragdoll = Instantiate(GorillaModel);
                        Ragdoll.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
                        Ragdoll.name = Ragdoll.name + "MonoObject_Ragdoll";
                        Ragdoll.transform.SetParent(itemsFolder.transform, false);
                        foreach (Transform g in Ragdoll.transform.GetChild(0).GetComponentsInChildren<Transform>())
                        {
                            g.gameObject.layer = 8;
                            g.name = g.name + "MonoObject";
                        }
                        /*foreach(Rigidbody g in Ragdoll.transform.GetChild(0).GetComponentsInChildren<Rigidbody>())
                        {
                            g.interpolation = RigidbodyInterpolation.Interpolate;
                        }*/
                        Ragdoll.transform.position = hitInfo.point + new Vector3(0f, 0.45f, 0f);
                        canPlace = false;
                    }
                    else
                    {
                        GameObject Ragdoll = Instantiate(RagdollModel);
                        Ragdoll.name = Ragdoll.name + "MonoObject_Ragdoll";
                        Ragdoll.transform.SetParent(itemsFolder.transform, false);
                        foreach (Transform g in Ragdoll.transform.GetChild(0).GetComponentsInChildren<Transform>())
                        {
                            g.gameObject.layer = 8;
                            g.name = g.name + "MonoObject";
                        }
                        Ragdoll.transform.position = hitInfo.point + new Vector3(0f, 0.6f, 0f);
                        Ragdoll.transform.localScale = new Vector3(0.4f, 0.4f, 0.5f);
                        Ragdoll.transform.GetChild(1).GetComponent<Renderer>().material.color = Color.grey;
                        Destroy(Ragdoll.GetComponent<MeshCollider>());
                        canPlace = false;
                    }
                }
            }
            else { canPlace = true; }
        }
        else
        {
            if (editMode)
            {
                Cursor = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                Debug.LogError(Cursor.name);
                Cursor.transform.localScale = new Vector3(0.4f, 0.3f, 0.4f);
                Cursor.GetComponent<Renderer>().material.color = Color.cyan;

                Destroy(Cursor.GetComponent<CapsuleCollider>());
            }
            else
            { if (Cursor != null) { Destroy(Cursor.gameObject); } }
        }
        if(!editMode){if (Cursor != null) { Destroy(Cursor.gameObject); }}
    }
    void Leave()
    {
            Destroy(Cursor.gameObject);
            for (int i = 0; i < itemsFolder.transform.childCount; i++)
        {
                Destroy(itemsFolder.transform.GetChild(i).gameObject);
        }

    }
}
