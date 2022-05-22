using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine.XR;
using UnityEngine;
using Utilla;
using BepInEx;

public class AirStrikeManager : MonoBehaviour
{
    bool primaryDown;
    bool canPlace;
    public bool editMode = false;

    GameObject Cursor = null;
    public GameObject AirStrikeModel = null;
    public GameObject CursorModel = null;
    public GameObject ExplodeModel = null;
    GameObject itemsFolder = null;


    void OnGameInitialized(object sender, EventArgs e)
    {
        itemsFolder = GameObject.Find("ItemFolderMono");
    }

    void Start()
    {
    }

    void Update()
    {
        RaycastHit hitInfo;
        Physics.Raycast(GorillaLocomotion.Player.Instance.rightHandTransform.position + GorillaLocomotion.Player.Instance.rightHandTransform.forward / 8, GorillaLocomotion.Player.Instance.rightHandTransform.forward, out hitInfo);
        if(itemsFolder == null)
        {
            itemsFolder = GameObject.Find("ItemFolderMono");
        }
        if (Cursor != null)
        {
            Cursor.transform.position = hitInfo.point;
            Cursor.transform.forward = hitInfo.normal;
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out primaryDown);
            if (primaryDown)
            {
                if (canPlace)
                {
                    GameObject Missile = Instantiate(AirStrikeModel);
                    Missile.transform.SetParent(itemsFolder.transform, false);
                    Missile.transform.position = hitInfo.point + new Vector3(0, 80f,0);
                    Missile.transform.localScale = new Vector3(50f, 50f, 50f);
                    Airstrike airstrikeControl = Missile.AddComponent<Airstrike>();
                    airstrikeControl.StrikeLocation = hitInfo.point;
                    airstrikeControl.ExplosionOBJ = ExplodeModel;
                    canPlace = false;
                }
            }
            else { canPlace = true; }
        }
        else
        {
            if (editMode)
            {
                    Cursor = Instantiate(CursorModel);
                    Cursor.transform.localScale = new Vector3(20f, 20f, 20f);
            }
            else { if (Cursor != null) { Destroy(Cursor.gameObject); } }
        }
        if (!editMode) { if (Cursor != null) { Destroy(Cursor.gameObject); } }
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


public class Airstrike : MonoBehaviour
{
    public Vector3 StrikeLocation;
    public GameObject ExplosionOBJ;
    private bool canExplode = true;
    public static float speed = 20;
    void Update()
    {
        var newspeed = speed * Time.deltaTime;
        transform.position = transform.position = Vector3.MoveTowards(transform.position, StrikeLocation, newspeed);

        if (Vector3.Distance(transform.position, StrikeLocation) < 0.5f && canExplode)
        {
            Explode();
            canExplode = false;
        }
    }

    void Explode()
    {
        gameObject.GetComponent<AudioSource>().minDistance = 15;
        gameObject.GetComponent<AudioSource>().Play();
        gameObject.GetComponent<Renderer>().enabled = false;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        GameObject ExplodeOBJ = Instantiate(ExplosionOBJ);
        ExplodeOBJ.transform.SetParent(transform);
        ExplodeOBJ.transform.localPosition = Vector3.zero;
        ExplodeOBJ.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);

        Collider[] colliders = Physics.OverlapSphere(transform.position, 10f);
        foreach (Collider nearyby in colliders)
        {
            Rigidbody rig = nearyby.GetComponent<Rigidbody>();
            if (rig != null)
            {
                if (rig.useGravity == true)
                {
                    rig.AddExplosionForce(6000f, transform.position, 10f);
                }
            }
        }
        Collider[] bombs = Physics.OverlapSphere(transform.position, 10);
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
        PlayerRigidbody.AddExplosionForce(80000f, transform.position, 10f);
        Invoke("Finish", 2);
    }
    void Finish()
    {
        Destroy(gameObject);
    }
}
