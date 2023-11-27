using MonoSandbox;
using MonoSandbox.Behaviours;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterManager : MonoBehaviour
{
    public List<GameObject> objectList = new List<GameObject>();

    public bool primaryDown, canPlace, editMode;
    public float multiplier = 4f;

    public GameObject Cursor, itemsFolder, ThrusterModel, ThrustParticles;

    public void Start()
    {
        itemsFolder = gameObject;
    }

    public void Update()
    {
        RaycastHit hitInfo = RefCache.Hit;

        if (Cursor != null)
        {
            bool isAllowed = hitInfo.collider != null && hitInfo.collider.attachedRigidbody != null && hitInfo.transform.gameObject.name.Contains("MonoObject");
            Cursor.GetComponent<Renderer>().material.color = isAllowed ? new Color(0.392f, 0.722f, 0.820f, 0.4509804f) : new Color(0.8314f, 0.2471f, 0.1569f, 0.4509804f);

            Cursor.transform.position = hitInfo.point;
            Cursor.transform.forward = -hitInfo.normal;
            primaryDown = InputHandling.RightPrimary;
            if (primaryDown)
            {
                if (canPlace && isAllowed)
                {
                    GameObject Thruster = Instantiate(ThrusterModel);
                    Thruster.transform.localScale = new Vector3(10f, 10f, 10f);
                    Thruster.transform.SetParent(hitInfo.collider.transform, true);
                    Thruster.transform.position = hitInfo.point;
                    Thruster.name = "Thruster MonoObject";
                    objectList.Add(Thruster);
                    ThrusterControls control = Thruster.AddComponent<ThrusterControls>();
                    control.rb = hitInfo.collider.attachedRigidbody;
                    control.multiplier = multiplier;
                    control.particle = Instantiate(ThrustParticles);
                    Thruster.GetComponent<Renderer>().material.color = Color.black;
                    Thruster.transform.forward = -hitInfo.normal;

                    HapticManager.Haptic(HapticManager.HapticType.Create);
                    canPlace = false;
                }
            }
            else
            {
                canPlace = true;
            }
        }
        else
        {
            if (editMode)
            {
                Cursor = Instantiate(ThrusterModel);
                Cursor.transform.localScale = new Vector3(10f, 10f, 10f);
                Cursor.GetComponent<Renderer>().material = new Material(RefCache.Selection);
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
        particle.transform.localPosition = new Vector3(0, 0, -0.014f);
        particle.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    void Update()
    {
        gripDown = InputHandling.RightGrip;
        if (gripDown > 0.3f)
        {
            if (!particle.GetComponent<AudioSource>().isPlaying)
            {
                particle.GetComponent<ParticleSystem>().Play(true);
                particle.GetComponent<AudioSource>().Play();
            }

            HapticManager.Haptic(HapticManager.HapticType.Constant);
            rb.AddForceAtPosition(transform.forward * 10 * multiplier, transform.position);
        }
        else
        {
            if (particle.GetComponent<ParticleSystem>().isPlaying)
            {
                particle.GetComponent<ParticleSystem>().Stop(true);
                particle.GetComponent<AudioSource>().Stop();
            }
        }
    }
}
