using System;
using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine;
using Utilla;
using BepInEx;

public class FreezeManager : MonoBehaviour
{
    bool primaryDown;
    bool canPlace;
    public bool editMode = false;

    GameObject Cursor = null;
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
            Cursor.transform.position = hitInfo.point;
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out primaryDown);
            if (primaryDown)
            {
                if (canPlace)
                {
                    if (hitInfo.transform.gameObject.name.Contains("MonoObject"))
                    {
                        if (hitInfo.transform.gameObject.GetComponent<Rigidbody>() != null)
                        {
                            Rigidbody freezeRB = hitInfo.transform.gameObject.GetComponent<Rigidbody>();
                            if (freezeRB.constraints == RigidbodyConstraints.None)
                            {
                                freezeRB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
                                canPlace = false;   
                            }
                            else
                            {
                                freezeRB.constraints = RigidbodyConstraints.None;
                                canPlace = false;
                            }
                        }
                    }
                }
            }
            else { canPlace = true; }
        }
        else
        {
            if (editMode)
            {
                Cursor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                Debug.LogError(Cursor.name);
                Cursor.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                Cursor.GetComponent<Renderer>().material.color = Color.cyan;

                Destroy(Cursor.GetComponent<SphereCollider>());
            }
            else
            { if (Cursor != null) { Destroy(Cursor.gameObject); } }
        }
        if (!editMode) { if (Cursor != null) { Destroy(Cursor.gameObject); } }
    }
}
public class GravityManager : MonoBehaviour
{
    bool primaryDown;
    bool canPlace;
    public bool editMode = false;

    GameObject Cursor = null;
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
            Cursor.transform.position = hitInfo.point;
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out primaryDown);
            if (primaryDown)
            {
                if (canPlace)
                {
                    canPlace = false;
                    if (hitInfo.transform.gameObject.name.Contains("MonoObject"))
                    {
                        if (hitInfo.transform.gameObject.GetComponent<Rigidbody>() != null)
                        {
                            Rigidbody gravityRB = hitInfo.transform.gameObject.GetComponent<Rigidbody>();
                            gravityRB.useGravity = !gravityRB.useGravity;
                        }
                    }
                }
            }
            else { canPlace = true; }
        }
        else
        {
            if (editMode)
            {
                Cursor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                Debug.LogError(Cursor.name);
                Cursor.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                Cursor.GetComponent<Renderer>().material.color = Color.cyan;

                Destroy(Cursor.GetComponent<SphereCollider>());
            }
            else
            { if (Cursor != null) { Destroy(Cursor.gameObject); } }
        }
        if (!editMode) { if (Cursor != null) { Destroy(Cursor.gameObject); } }
    }
}
