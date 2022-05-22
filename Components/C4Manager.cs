using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine.XR;
using UnityEngine;
using Utilla;
using BepInEx;

public class C4Manager : MonoBehaviour
{
    bool primaryDown;
    bool canPlace;
    public bool editMode;
    public bool explodeMode = true;
    public bool inRoom;

    public float multiplier = 4;

    GameObject Explosive = null;
    public GameObject C4Model = null;
    public GameObject ExplodeModel = null;
    GameObject itemsFolder = null;

    void OnGameInitialized(object sender, EventArgs e)
    {
        itemsFolder = GameObject.Find("ItemFolderMono");
    }

    public void UpdateMultiplier(bool UpOrDown)
    {
        multiplier = UpOrDown ? Mathf.Clamp(multiplier + 0.5f, 1, 10) : Mathf.Clamp(multiplier - 0.5f, 1, 10);
    }

    void Update()
    {
        RaycastHit hitInfo;
        Physics.Raycast(GorillaLocomotion.Player.Instance.rightHandTransform.position + GorillaLocomotion.Player.Instance.rightHandTransform.forward / 8, GorillaLocomotion.Player.Instance.rightHandTransform.forward, out hitInfo);
        if(itemsFolder == null)
        {
            itemsFolder = GameObject.Find("ItemFolderMono");
            print("serch");
        }
        if (Explosive != null)
        {
            Explosive.transform.position = hitInfo.point;
            Explosive.transform.forward = hitInfo.normal;
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out primaryDown);
            if (primaryDown)
            {
                if (canPlace)
                {
                    GameObject C4 = Instantiate(C4Model);
                    Destroy(C4.GetComponent<MeshCollider>());
                    C4.AddComponent<BoxCollider>();
                    C4.transform.SetParent(itemsFolder.transform, false);
                    C4.transform.position = hitInfo.point;
                    C4.transform.forward = hitInfo.normal;
                    BombDetonate bombDet = C4.AddComponent<BombDetonate>();
                    bombDet.ExplosionOBJ = ExplodeModel;
                    bombDet.multiplier = multiplier;
                    C4.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
                    canPlace = false;
                }
            }
            else { canPlace = true; }
        }
        else
        {
            if (editMode)
            {
                    Explosive = Instantiate(C4Model);
                    Debug.LogError(Explosive.name);
                    Explosive.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                    Explosive.GetComponent<Renderer>().material.mainTexture = null;
                    Explosive.GetComponent<Renderer>().material.color = Color.cyan;

                    Destroy(Explosive.GetComponent<MeshCollider>());
            }
            else
            { if (Explosive != null) { Destroy(Explosive.gameObject); } }
        }
        if (!editMode) { if (Explosive != null) { Destroy(Explosive.gameObject); } }
    }

    void Leave()
    {
        Destroy(Explosive.gameObject);
        for (int i = 0; i < itemsFolder.transform.childCount; i++)
        {
            Destroy(itemsFolder.transform.GetChild(i).gameObject);
        }
    }
}

public class BombDetonate : MonoBehaviour
{

    bool secButtonDown;
    bool primaryDown;
    bool canExplode = true;
    public bool useDefault = true;
    public float multiplier = 4;
    public float Radius = 10f;
    public GameObject ExplosionOBJ;

    List<InputDevice> list = new List<InputDevice>();
    // Use this for initialization
    void Start()
    {
        InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Right | UnityEngine.XR.InputDeviceCharacteristics.Controller, list);
    }
    // Update is called once per frame
    void Update()
    {
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.secondaryButton, out secButtonDown);
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out primaryDown);
        if (secButtonDown)
        {
            if (canExplode)
            {
                Explode();
            }
        }
    }
    public void Explode()
    {
        if (canExplode)
        {
            canExplode = false;
            gameObject.GetComponent<AudioSource>().Play();
            gameObject.GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            GameObject ExplodeOBJ = Instantiate(ExplosionOBJ);
            ExplodeOBJ.transform.SetParent(transform);
            ExplodeOBJ.transform.localPosition = Vector3.zero;
            ExplodeOBJ.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            Collider[] colliders = Physics.OverlapSphere(transform.position, Radius);
            foreach (Collider nearyby in colliders)
            {
                Rigidbody rig = nearyby.GetComponent<Rigidbody>();
                if (rig != null)
                {
                    rig.AddExplosionForce(1500f * multiplier, transform.position, 8f);
                }
            }
            Collider[] bombs = Physics.OverlapSphere(transform.position, Radius);
            foreach (Collider nearyby in bombs)
            {
                if (nearyby.GetComponent<BombDetonate>() != null)
                {
                    nearyby.GetComponent<BombDetonate>().Explode();
                }
                if (nearyby.GetComponent<Explode>() != null)
                {
                    nearyby.GetComponent<Explode>().Explosion();
                }
            }
            Rigidbody PlayerRigidbody = GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>();
            PlayerRigidbody.AddExplosionForce(2500f * multiplier, transform.position, 5 + (0.75f * multiplier));
            Invoke("Delete", 3);
        }
    }
    void Delete()
    {
        Destroy(gameObject);
    }
}
