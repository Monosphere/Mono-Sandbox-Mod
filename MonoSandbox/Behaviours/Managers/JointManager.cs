using MonoSandbox;
using MonoSandbox.Behaviours;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SpringManager : MonoBehaviour
{
    public List<GameObject> objectList = new List<GameObject>();

    public bool primaryDown, canPlace, editMode, BasePlaced;
    public RaycastHit baseHit;
    public SpringJoint joint;
    public GameObject Cursor;

    public void Update()
    {
        RaycastHit hitInfo = RefCache.Hit;
        if (Cursor != null)
        {
            Cursor.transform.position = hitInfo.point;

            bool isAllowed = hitInfo.transform.gameObject.name.Contains("MonoObject");
            Cursor.GetComponent<Renderer>().material.color = isAllowed ? new Color(0.392f, 0.722f, 0.820f, 0.4509804f) : new Color(0.8314f, 0.2471f, 0.1569f, 0.4509804f);

            primaryDown = InputHandling.RightPrimary;
            if (primaryDown)
            {
                if (canPlace && isAllowed && RefCache.HitExists)
                {
                    PlaceJoint(hitInfo);

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
                Cursor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                Cursor.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                Cursor.GetComponent<Renderer>().material = new Material(RefCache.Selection);
                Destroy(Cursor.GetComponent<SphereCollider>());
            }
            else if (Cursor)
            {
                Destroy(Cursor.gameObject);
            }
        }
        if (!editMode && Cursor)
        {
            Destroy(Cursor.gameObject);
        }

        void PlaceJoint(RaycastHit hit)
        {
            if (!BasePlaced)
            {
                baseHit = hit;
                BasePlaced = true;
            }
            else
            {
                foreach (Transform child in baseHit.transform)
                {
                    if (child.name.Contains("MSJoint"))
                    {
                        return;
                    }
                }
                GameObject JointOBJ = new GameObject();
                JointOBJ.name = "MSJoint MonoObject";
                JointOBJ.transform.SetParent(baseHit.transform, false);
                objectList.Add(JointOBJ);
                FixedJoint parentFix = JointOBJ.AddComponent<FixedJoint>();
                parentFix.connectedBody = baseHit.transform.GetComponent<Rigidbody>();
                joint = JointOBJ.transform.gameObject.AddComponent<SpringJoint>();
                joint.minDistance = Vector3.Distance(baseHit.transform.position, hit.transform.position) - 1f;
                joint.damper = 30f;
                joint.spring = 10f;
                joint.massScale = 12f;
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedBody = hit.transform.GetComponent<Rigidbody>();
                LineRenderer lineRenderer = JointOBJ.gameObject.AddComponent<LineRenderer>();
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.startColor = Color.white;
                lineRenderer.endColor = Color.white;
                lineRenderer.startWidth = 0.012f;
                lineRenderer.endWidth = 0.012f;
                lineRenderer.positionCount = 2;

                SpringLine line = JointOBJ.AddComponent<SpringLine>();
                line.makeLine = false;
                line.lineRenderer = lineRenderer;
                line.pointone = JointOBJ;
                line.pointtwo = hit.transform.gameObject;

                HapticManager.Haptic(HapticManager.HapticType.Create);
                BasePlaced = false;
            }
        }
    }
}

public class SpringLine : MonoBehaviour
{
    public GameObject pointone;
    public GameObject pointtwo;
    public bool makeLine = false;
    public LineRenderer lineRenderer = null;

    void Update()
    {
        if (lineRenderer == null && makeLine)
        {
            lineRenderer = pointone.gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.grey;
            lineRenderer.endColor = Color.grey;
            lineRenderer.startWidth = 0.02f;
            lineRenderer.endWidth = 0.02f;
            lineRenderer.positionCount = 1;
            lineRenderer.SetPosition(0, pointone.transform.position);
        }
        else
        {
            if (makeLine)
            {
                lineRenderer.SetPosition(0, pointone.transform.position);
                if (pointtwo != null)
                {
                    if (lineRenderer.positionCount == 1)
                    {
                        lineRenderer.positionCount = 2;
                    }
                    lineRenderer.SetPosition(1, pointtwo.transform.position);
                }
            }
        }
        lineRenderer.SetPosition(0, pointone.transform.position);
        lineRenderer.SetPosition(1, pointtwo.transform.position);
    }
}

public class WeldManager : MonoBehaviour
{
    public bool primaryDown, canPlace, editMode, BasePlaced;
    public GameObject baseHit, Cursor;

    void Update()
    {
        RaycastHit hitInfo = RefCache.Hit;

        if (Cursor != null)
        {
            Cursor.transform.position = hitInfo.point;
            primaryDown = InputHandling.RightPrimary;

            bool isAllowed = hitInfo.transform.gameObject.name.Contains("MonoObject") && hitInfo.collider != null && hitInfo.collider.attachedRigidbody != null;
            Cursor.GetComponent<Renderer>().material.color = isAllowed ? new Color(0.392f, 0.722f, 0.820f, 0.4509804f) : new Color(0.8314f, 0.2471f, 0.1569f, 0.4509804f);

            if (primaryDown)
            {
                if (canPlace && isAllowed && RefCache.HitExists)
                {
                    PlaceJoint(hitInfo);

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
                Cursor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                Cursor.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                Cursor.GetComponent<Renderer>().material = new Material(RefCache.Selection);

                Destroy(Cursor.GetComponent<SphereCollider>());
            }
            else if (Cursor)
            {
                Destroy(Cursor.gameObject);
            }
        }
        if (!editMode && Cursor)
        {
            Destroy(Cursor.gameObject);
        }
    }

    void PlaceJoint(RaycastHit hit)
    {
        if (!BasePlaced)
        {
            baseHit = hit.transform.gameObject;
            BasePlaced = true;
        }
        else
        {
            if (baseHit.GetInstanceID() != hit.transform.gameObject.GetInstanceID())
            {
                FixedJoint joint = hit.transform.gameObject.AddComponent<FixedJoint>();
                joint.connectedBody = hit.collider.attachedRigidbody;
                joint.autoConfigureConnectedAnchor = true;
            }
            BasePlaced = false;
        }
    }
}

public class BalloonManager : MonoBehaviour
{
    public List<GameObject> objectList = new List<GameObject>();

    public float balloonPower = 2f;
    public float maxSpeed = 1.5f;

    public bool primaryDown, editMode, canPlace;
    public GameObject Cursor, itemsFolder, Balloon;

    public void Start()
    {
        itemsFolder = gameObject;
    }

    public void Update()
    {
        if (editMode)
        {
            if (!Cursor)
            {
                Cursor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                Cursor.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                Cursor.GetComponent<Renderer>().material = new Material(RefCache.Selection);
                Destroy(Cursor.GetComponent<SphereCollider>());
            }

            RaycastHit hitInfo = RefCache.Hit;
            Cursor.transform.position = hitInfo.point;

            bool isAllowed = hitInfo.collider != null && hitInfo.collider.attachedRigidbody != null && hitInfo.transform.gameObject.name.Contains("MonoObject");
            Cursor.GetComponent<Renderer>().material.color = isAllowed ? new Color(0.392f, 0.722f, 0.820f, 0.4509804f) : new Color(0.8314f, 0.2471f, 0.1569f, 0.4509804f);

            primaryDown = InputHandling.RightPrimary;
            if (primaryDown)
            {
                if (canPlace && isAllowed && RefCache.HitExists)
                {
                    foreach (Transform child in hitInfo.transform)
                    {
                        if (child.name.Contains("Balloon MonoObject"))
                        {
                            return;
                        }
                    }

                    GameObject BalloonOBJ = Instantiate(Balloon);
                    BalloonOBJ.layer = 8;
                    BalloonOBJ.name = "Balloon MonoObject";
                    objectList.Add(BalloonOBJ);
                    BalloonOBJ.transform.parent = itemsFolder.transform;
                    BalloonOBJ.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    BalloonOBJ.transform.position = hitInfo.point + (Vector3.up * 0.3f) + Cursor.transform.forward / 3f;

                    Balloon balloonScript = BalloonOBJ.AddComponent<Balloon>();
                    LineRenderer lineRenderer = BalloonOBJ.gameObject.AddComponent<LineRenderer>();
                    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                    lineRenderer.startColor = Color.white;
                    lineRenderer.endColor = Color.white;
                    lineRenderer.startWidth = 0.012f;
                    lineRenderer.endWidth = 0.012f;
                    lineRenderer.positionCount = 2;

                    SpringLine line = BalloonOBJ.AddComponent<SpringLine>();
                    line.makeLine = false;
                    line.lineRenderer = lineRenderer;
                    line.pointone = BalloonOBJ.transform.GetChild(0).gameObject;
                    line.pointtwo = hitInfo.transform.gameObject;
                    SpringJoint joint = BalloonOBJ.AddComponent<SpringJoint>();
                    joint.maxDistance = 0f;
                    joint.spring = 20f;
                    joint.damper = 10f;
                    joint.connectedBody = hitInfo.collider.attachedRigidbody;
                    balloonScript.power = balloonPower;

                    BalloonOBJ.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
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
            if (Cursor != null)
            {
                Destroy(Cursor);
            }
        }
    }

}
public class Balloon : MonoBehaviour
{
    public float power;
    public float maxSpeed;

    void Start()
    {
        maxSpeed = Mathf.Clamp(0.2f + power * 0.4f, -5, 5);
    }

    private void FixedUpdate()
    {
        Vector3 newVelocity = gameObject.GetComponent<Rigidbody>().velocity.normalized;
        newVelocity *= maxSpeed;
        gameObject.GetComponent<Rigidbody>().velocity = newVelocity;
        gameObject.GetComponent<Rigidbody>().AddForce(0, -Physics.gravity.y + (float)Math.Pow(3, 4) + power, 0);
    }
}

public class BoneSphere : MonoBehaviour
{

    [Header("Bones")]
    public GameObject root = null;
    public GameObject x = null;
    public GameObject x2 = null;
    public GameObject y = null;
    public GameObject y2 = null;
    public GameObject z = null;
    public GameObject z2 = null;
    [Header("Spring Joint Settings")]
    [Tooltip("Strength of spring")]
    public float Spring = 800f;
    [Tooltip("Higher the value the faster the spring oscillation stops")]
    public float Damper = 0.2f;
    [Header("Other Settings")]
    public Softbody.ColliderShape Shape = Softbody.ColliderShape.Sphere;
    public float ColliderSize = 0.002f;
    public float RigidbodyMass = 0.5f;
    public bool ViewLines = true;

    private void Start()
    {
        root = transform.GetChild(0).GetChild(0).gameObject;
        x = transform.GetChild(0).GetChild(1).gameObject;
        x2 = transform.GetChild(0).GetChild(2).gameObject;
        y = transform.GetChild(0).GetChild(3).gameObject;
        y2 = transform.GetChild(0).GetChild(4).gameObject;
        z = transform.GetChild(0).GetChild(5).gameObject;
        z2 = transform.GetChild(0).GetChild(6).gameObject;
        Softbody.Init(Shape, ColliderSize, RigidbodyMass, Spring, Damper, RigidbodyConstraints.FreezeRotation);

        Softbody.AddCollider(ref root, Softbody.ColliderShape.Sphere, 0.005f, 10f);
        Softbody.AddCollider(ref x);
        Softbody.AddCollider(ref x2);
        Softbody.AddCollider(ref y);
        Softbody.AddCollider(ref y2);
        Softbody.AddCollider(ref z);
        Softbody.AddCollider(ref z2);

        Softbody.AddSpring(ref x, ref root);
        Softbody.AddSpring(ref x2, ref root);
        Softbody.AddSpring(ref y, ref root);
        Softbody.AddSpring(ref y2, ref root);
        Softbody.AddSpring(ref z, ref root);
        Softbody.AddSpring(ref z2, ref root);
    }
}

public static class Softbody
{
    #region --- helpers ---
    public enum ColliderShape
    {
        Box,
        Sphere,
    }
    #endregion

    public static ColliderShape Shape;
    public static float ColliderSize;
    public static float RigidbodyMass;
    public static float Spring;
    public static float Damper;
    public static RigidbodyConstraints Constraints;

    public static void Init(ColliderShape shape, float collidersize, float rigidbodymass, float spring, float damper, RigidbodyConstraints constraints)
    {
        Shape = shape;
        ColliderSize = collidersize;
        RigidbodyMass = rigidbodymass;
        Spring = spring;
        Damper = damper;
        Constraints = constraints;
    }
    public static Rigidbody AddCollider(ref GameObject go)
    {
        return AddCollider(ref go, Shape, ColliderSize, RigidbodyMass);
    }
    public static SpringJoint AddSpring(ref GameObject go1, ref GameObject go2)
    {
        SpringJoint sp = AddSpring(ref go1, ref go2, Spring, Damper);
        return sp;
    }

    public static Rigidbody AddCollider(ref GameObject go, ColliderShape shape, float size, float mass)
    {
        switch (shape)
        {
            case ColliderShape.Box:
                BoxCollider bc = go.AddComponent<BoxCollider>();
                bc.size = new Vector3(size, size, size);
                break;
            case ColliderShape.Sphere:
                SphereCollider sc = go.AddComponent<SphereCollider>();
                sc.radius = size;
                break;
        }

        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.mass = mass;
        rb.drag = 0f;
        rb.angularDrag = 10f;
        rb.constraints = Constraints;
        return rb;
    }
    public static SpringJoint AddSpring(ref GameObject go1, ref GameObject go2, float spring, float damper)
    {
        SpringJoint sp = go1.AddComponent<SpringJoint>();
        sp.connectedBody = go2.GetComponent<Rigidbody>();
        sp.spring = spring;
        sp.damper = damper;
        return sp;
    }
}

