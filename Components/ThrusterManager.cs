using System;
using UnityEngine.XR;
using UnityEngine;
using MonoSandbox.Components;

public class ThrusterManager : MonoBehaviour
{
    bool primaryDown;
    bool canPlace;
    public bool editMode;
    public bool explodeMode = true;
    public bool inRoom;

    public float multiplier = 4f;

    GameObject Cursor = null;
    public GameObject ThrusterModel = null;
    public GameObject ThrustParticles = null;
    GameObject itemsFolder = null;


    void OnGameInitialized(object sender, EventArgs e)
    {
        itemsFolder = GameObject.Find("ItemFolderMono");
    }

    public void UpdateMultiplier(bool UpOrDown)
    {
        multiplier = UpOrDown ? Mathf.Clamp(multiplier + 0.5f, 1, 10) : Mathf.Clamp(multiplier - 0.5f,1,10);
    }

    void Update()
    {
        RaycastHit hitInfo;
        hitInfo = MonoSandbox.PluginInfo.raycastHit; 
        if (itemsFolder == null)
        {
            itemsFolder = GameObject.Find("ItemFolderMono");
            print("serch");
        }
        if (Cursor != null)
        {
            if (hitInfo.transform.gameObject.GetComponent<Rigidbody>() != null && hitInfo.transform.gameObject.name.Contains("MonoObject")){ Cursor.GetComponent<Renderer>().material.color = Color.cyan; } else { Cursor.GetComponent<Renderer>().material.color = Color.red; }
            Cursor.transform.position = hitInfo.point;
            Cursor.transform.forward = -hitInfo.normal;
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out primaryDown);
            if (primaryDown)
            {
                if (canPlace)
                {
                    if (hitInfo.transform.gameObject.GetComponent<Rigidbody>() != null && hitInfo.transform.gameObject.name.Contains("MonoObject"))
                    {
                        GameObject Thruster = Instantiate(ThrusterModel);
                        Thruster.transform.SetParent(hitInfo.collider.transform, false);
                        Thruster.transform.localScale = new Vector3(10f, 10f, 10f);
                        Thruster.transform.position = hitInfo.point;
                        Thruster.name = "Thruster MonoObject";
                        ThrusterControls control = Thruster.AddComponent<ThrusterControls>();
                        control.rb = hitInfo.transform.gameObject.GetComponent<Rigidbody>();
                        control.multiplier = multiplier;
                        control.particle = Instantiate(ThrustParticles);
                        Thruster.GetComponent<Renderer>().material.color = Color.black;
                        Thruster.transform.forward = -hitInfo.normal;
                        canPlace = false;
                        MaterialUtil.FixStandardShadersInObject(Thruster);
                    }
                }
            }
            else { canPlace = true; }
        }
        else
        {
            if (editMode)
            {
                    Cursor = Instantiate(ThrusterModel);
                    Cursor.transform.localScale = new Vector3(10f, 10f, 10f);
                    //Cursor.GetComponent<Renderer>().material.color = Color.cyan;
            }
            else { if (Cursor != null) { Destroy(Cursor.gameObject); } }
        }
        if (!editMode) { if (Cursor != null) { Destroy(Cursor.gameObject); } }
    }
}

public class ThrusterControls : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject particle;
    float gripDown;
    public float multiplier = 4;
    void Start()
    {
        particle.transform.SetParent(transform, false);
        particle.transform.localEulerAngles = new Vector3(180, 0, 0);
        particle.transform.localPosition = new Vector3(0,0, -0.014f);
        particle.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    void Update()
    {
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.grip, out gripDown);
        if (gripDown > 0.1f)
        {
            if (!particle.GetComponent<AudioSource>().isPlaying)
            {
                particle.GetComponent<ParticleSystem>().Play(true);
                particle.GetComponent<AudioSource>().Play();
            }
            rb.AddForceAtPosition(transform.forward * 100 * multiplier, transform.position);
        }
        else
        {
            if(particle.GetComponent<ParticleSystem>().isPlaying)
            {
                particle.GetComponent<ParticleSystem>().Stop(true);
                particle.GetComponent<AudioSource>().Stop();
            }
        }
    }
}
