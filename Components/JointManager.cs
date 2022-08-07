using System;
using UnityEngine.XR;
using UnityEngine;

public class SpringManager : MonoBehaviour
{
    bool primaryDown;
    bool canPlace;
    public bool editMode = true;
    public bool explodeMode = true;
    public bool inRoom;
    public bool BasePlaced = false;
    RaycastHit baseHit;

    SpringJoint joint;
    SpringLine line;

    GameObject Cursor = null;

    void OnGameInitialized(object sender, EventArgs e)
    {

    }


    void Update()
    {
        RaycastHit hitInfo;
        Physics.Raycast(GorillaLocomotion.Player.Instance.rightHandTransform.position + GorillaLocomotion.Player.Instance.rightHandTransform.forward / 8, GorillaLocomotion.Player.Instance.rightHandTransform.forward, out hitInfo);
        if (Cursor != null)
        {
            Cursor.transform.position = hitInfo.point;
            if (hitInfo.transform.gameObject.name.Contains("MonoObject"))
            {
                Cursor.GetComponent<Renderer>().material.color = Color.cyan;
            }
            else
            {
                Cursor.GetComponent<Renderer>().material.color = Color.red;
            }
            Cursor.transform.forward = hitInfo.normal;
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out primaryDown);
            if (primaryDown)
            {
                if (canPlace)
                {
                    if (hitInfo.transform.gameObject.name.Contains("MonoObject"))
                    {
                        PlaceJoint(hitInfo);
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
                Cursor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                Cursor.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                Cursor.GetComponent<Renderer>().material.color = Color.cyan;
                Destroy(Cursor.GetComponent<SphereCollider>());
            }
            else
            {
                { if (Cursor != null) { Destroy(Cursor.gameObject); } }
            }
        }
        if (!editMode) { if (Cursor != null) { Destroy(Cursor.gameObject); } }

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
                GameObject JointOBJ = Instantiate(new GameObject());
                JointOBJ.name = "MSJoint MonoObject";
                JointOBJ.transform.SetParent(baseHit.transform, false);
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
                lineRenderer.startColor = Color.grey;
                lineRenderer.endColor = Color.grey;
                lineRenderer.startWidth = 0.02f;
                lineRenderer.endWidth = 0.02f;
                lineRenderer.positionCount = 2;

                SpringLine line = JointOBJ.AddComponent<SpringLine>();
                line.makeLine = false;
                line.lineRenderer = lineRenderer;
                line.pointone = JointOBJ;
                line.pointtwo = hit.transform.gameObject;
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
        bool primaryDown;
        bool canPlace;
        public bool editMode = false;
        public bool BasePlaced = false;

        GameObject baseHit;

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
                                PlaceJoint(hitInfo);
                                canPlace = false;
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
                    joint.connectedBody = baseHit.GetComponent<Rigidbody>();
                    joint.autoConfigureConnectedAnchor = true;
                }
                BasePlaced = false;
            }
        }
    }

    public class BalloonManager : MonoBehaviour
    {
        public float balloonPower = 2f;
        public float maxSpeed = 1.5f;
        public bool primaryDown;
        public bool editMode;

        private bool canPlace;
        private GameObject Cursor;
        private GameObject itemsFolder = null;
        // Start is called before the first frame update
        void Start()
        {
            itemsFolder = GameObject.Find("ItemFolderMono");
        }

        public void UpdateMultiplier(bool UpOrDown)
        {
            balloonPower = UpOrDown ? Mathf.Clamp(balloonPower + 2f, 2, 20) : Mathf.Clamp(balloonPower - 2f, 2, 20);
        }

        // Update is called once per frame
        void Update()
        {
            if (itemsFolder == null)
            {
                itemsFolder = GameObject.Find("ItemFolderMono");
            }
            if (editMode)
            {
                if (Cursor == null)
                {
                    Cursor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    Cursor.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    Destroy(Cursor.GetComponent<SphereCollider>());
                }
                RaycastHit hitInfo;
                Physics.Raycast(GorillaLocomotion.Player.Instance.rightHandTransform.position + GorillaLocomotion.Player.Instance.rightHandTransform.forward / 8, GorillaLocomotion.Player.Instance.rightHandTransform.forward, out hitInfo);
                Cursor.transform.position = hitInfo.point;
                Cursor.transform.forward = hitInfo.normal;
                InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out primaryDown);
                if (hitInfo.transform.gameObject.GetComponent<Rigidbody>() != null && hitInfo.transform.gameObject.name.Contains("MonoObject"))
                {
                    if (primaryDown)
                    {
                        if (canPlace)
                        {
                            GameObject BalloonOBJ = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                            BalloonOBJ.layer = 8;
                            GameObject PlayerCollision = Instantiate(new GameObject());
                            PlayerCollision.transform.SetParent(BalloonOBJ.transform, false);
                            PlayerCollision.AddComponent<SphereCollider>();
                            PlayerCollision.layer = 0;
                            BalloonOBJ.name = "Balloon MonoObject";
                            BalloonOBJ.transform.parent = itemsFolder.transform;
                            BalloonOBJ.GetComponent<Renderer>().material.color = Color.red;
                            BalloonOBJ.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                            BalloonOBJ.transform.position = hitInfo.point + Cursor.transform.forward / 3f;
                            Balloon balloonScript = BalloonOBJ.AddComponent<Balloon>();
                            LineRenderer lineRenderer = BalloonOBJ.gameObject.AddComponent<LineRenderer>();
                            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                            lineRenderer.startColor = Color.grey;
                            lineRenderer.endColor = Color.grey;
                            lineRenderer.startWidth = 0.02f;
                            lineRenderer.endWidth = 0.02f;
                            lineRenderer.positionCount = 2;

                            SpringLine line = BalloonOBJ.AddComponent<SpringLine>();
                            line.makeLine = false;
                            line.lineRenderer = lineRenderer;
                            line.pointone = BalloonOBJ;
                            line.pointtwo = hitInfo.transform.gameObject;
                            SpringJoint joint = BalloonOBJ.AddComponent<SpringJoint>();
                            joint.maxDistance = 0.5f;
                            joint.spring = 3;
                            joint.damper = 1;
                            joint.connectedBody = hitInfo.rigidbody;
                            balloonScript.power = balloonPower;
                            canPlace = false;
                        }
                    }
                    else
                    {
                        canPlace = true;
                    }
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
            gameObject.GetComponent<Rigidbody>().AddForce(0, -Physics.gravity.y + 3 + power, 0);
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

