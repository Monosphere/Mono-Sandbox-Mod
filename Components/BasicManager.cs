using System;
using UnityEngine.XR;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    bool primaryDown;
    bool canPlace;
    public bool editMode = true;
    public bool inRoom;
    public bool isPlane;

    GameObject Cursor = null;
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
            print("serch");
        }
        if (Cursor != null)
        {
            Cursor.transform.position = hitInfo.point + Cursor.transform.forward/4;
            Cursor.transform.forward = hitInfo.normal;
            if(isPlane)
            {
                Cursor.transform.localScale = new Vector3(0.6f, 0.6f, 0.1f);
            }
            else {  Cursor.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);}
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out primaryDown);
            if (primaryDown)
            {
                if (canPlace)
                {
                    if (!isPlane)
                    {
                        GameObject Box = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        Rigidbody BoxRB = Box.AddComponent<Rigidbody>();
                        GameObject userCollision = Instantiate(new GameObject());
                        userCollision.AddComponent<BoxCollider>();

                        Box.layer = 8;
                        Box.name = Box.name + "MonoObject";
                        userCollision.layer = 0;
                        userCollision.transform.SetParent(Box.transform, false);
                        Box.transform.SetParent(itemsFolder.transform, false);

                        BoxRB.useGravity = true;
                        BoxRB.mass = 2.5f;
                        Box.transform.forward = hitInfo.normal;
                        Box.transform.position = hitInfo.point + Box.transform.forward / 4;
                        Box.GetComponent<Renderer>().material.color = Color.black;
                        Box.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                        canPlace = false;
                    }
                    else
                    {
                        GameObject Box = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        Rigidbody BoxRB = Box.AddComponent<Rigidbody>();
                        GameObject userCollision = Instantiate(new GameObject());
                        userCollision.AddComponent<BoxCollider>();

                        Box.layer = 8;
                        Box.name = Box.name + "MonoObject";
                        userCollision.layer = 0;
                        userCollision.transform.SetParent(Box.transform, false);
                        Box.transform.SetParent(itemsFolder.transform, false);

                        BoxRB.useGravity = true;
                        BoxRB.mass = 2.5f;
                        Box.transform.forward = hitInfo.normal;
                        Box.transform.position = hitInfo.point + Box.transform.forward / 4;
                        Box.GetComponent<Renderer>().material.color = Color.black;
                        Box.transform.localScale = new Vector3(0.6f, 0.6f, 0.1f);
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
                    Cursor = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    Cursor.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    Cursor.GetComponent<Renderer>().material.color = Color.cyan;

                    Destroy(Cursor.GetComponent<BoxCollider>());
            }
            else
            { if (Cursor != null) { Destroy(Cursor.gameObject); } }
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

public class SphereManager : MonoBehaviour
{
    bool primaryDown;
    bool canPlace;
    public bool editMode = true;
    public bool inRoom;
    public bool isSoftbody;

    GameObject Cursor = null;
    GameObject itemsFolder = null;

    public GameObject softbodySphere;


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
        if (itemsFolder == null)
        {
            itemsFolder = GameObject.Find("ItemFolderMono");
            print("serch");
        }
        if (Cursor != null)
        {
            Cursor.transform.position = hitInfo.point + Cursor.transform.forward / 4;
            Cursor.transform.forward = hitInfo.normal;
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out primaryDown);
            if (primaryDown)
            {
                if (canPlace)
                {
                    if (!isSoftbody)
                    {
                        GameObject Ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        Rigidbody BallRB = Ball.AddComponent<Rigidbody>();
                        GameObject userCollision = Instantiate(new GameObject());
                        userCollision.AddComponent<SphereCollider>();

                        Ball.layer = 8;
                        Ball.name = Ball.name + "MonoObject";
                        userCollision.layer = 0;
                        userCollision.transform.SetParent(Ball.transform, false);
                        Ball.transform.SetParent(itemsFolder.transform, false);

                        BallRB.useGravity = true;
                        BallRB.mass = 3.5f;

                        Ball.transform.forward = hitInfo.normal;
                        Ball.transform.position = hitInfo.point + Ball.transform.forward / 4;
                        Ball.GetComponent<Renderer>().material.color = Color.black;
                        Ball.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    }else
                    {
                        GameObject Ball = Instantiate(softbodySphere);

                        Ball.layer = 8;
                        Ball.name = Ball.name + "MonoObject";
                        Ball.transform.SetParent(itemsFolder.transform, false);
                        foreach (Transform g in Ball.transform.GetChild(0).GetComponentInChildren<Transform>())
                        {
                            g.name = g.name + "MonoObject";
                        }

                        Ball.transform.forward = hitInfo.normal;
                        Ball.transform.position = hitInfo.point + Ball.transform.forward / 4;
                        Ball.transform.GetChild(1).GetComponent<Renderer>().material.color = Color.black;
                        Ball.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                        BoneSphere rigidSphere = Ball.AddComponent<BoneSphere>();
                    }
                    canPlace = false;
                }
            }
            else { canPlace = true; }
        }
        else
        {
            if (editMode)
            {
                Cursor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                Cursor.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                Cursor.GetComponent<Renderer>().material.color = Color.cyan;

                Destroy(Cursor.GetComponent<SphereCollider>());
            }
            else
            { if (Cursor != null) { Destroy(Cursor.gameObject); } }
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
public class BeanManager : MonoBehaviour
{
    bool primaryDown;
    bool canPlace;
    public bool editMode = true;
    public bool inRoom;
    public bool isBarrel = false;
    public bool isWheel = false;

    GameObject Cursor = null;
    GameObject itemsFolder = null;
    public GameObject barrelOBJ = null;
    public GameObject explosionOBJ = null;


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
        if (itemsFolder == null)
        {
            itemsFolder = GameObject.Find("ItemFolderMono");
            print("serch");
        }
        if (Cursor != null)
        {
            Cursor.transform.position = hitInfo.point + Cursor.transform.up / 4.5f;
            Cursor.transform.up = hitInfo.normal;
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out primaryDown);
            if (primaryDown)
            {
                if (canPlace)
                {
                    if(isBarrel)
                    {
                        GameObject Bean = Instantiate(barrelOBJ);
                        Bean.AddComponent<BoxCollider>().size = new Vector3(0.025f,0.025f,0.025f);
                        Rigidbody BallRB = Bean.AddComponent<Rigidbody>();
                        GameObject userCollision = Instantiate(new GameObject());
                        userCollision.AddComponent<BoxCollider>().size = new Vector3(0.025f, 0.025f, 0.025f); ;
                        Explode explosion = Bean.AddComponent<Explode>();
                        explosion.multiplier = WeaponManager.staticweaponForce;
                        explosion.ExplosionOBJ = explosionOBJ;

                        Bean.layer = 8;
                        Bean.name = Bean.name + "MonoObject";
                        userCollision.layer = 0;
                        userCollision.transform.SetParent(Bean.transform, false);
                        Bean.transform.SetParent(itemsFolder.transform, false);

                        BallRB.useGravity = true;
                        BallRB.mass = 3.5f;

                        Bean.transform.up = hitInfo.normal;
                        Bean.transform.position = hitInfo.point + Bean.transform.up / 2.5f;
                        Bean.transform.localScale = new Vector3(15f, 15f, 15f);
                        canPlace = false;
                    }
                    if(!isWheel && !isBarrel)
                    {
                        GameObject Bean = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                        Rigidbody BallRB = Bean.AddComponent<Rigidbody>();
                        GameObject userCollision = Instantiate(new GameObject());
                        userCollision.AddComponent<CapsuleCollider>().height = 2;

                        Bean.layer = 8;
                        Bean.name = Bean.name + "MonoObject";
                        userCollision.layer = 0;
                        userCollision.transform.SetParent(Bean.transform, false);
                        Bean.transform.SetParent(itemsFolder.transform, false);

                        BallRB.useGravity = true;
                        BallRB.mass = 3.5f;

                        Bean.transform.up = hitInfo.normal;
                        Bean.transform.position = hitInfo.point + Bean.transform.up / 2.5f;
                        Bean.GetComponent<Renderer>().material.color = Color.black;
                        Bean.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                        canPlace = false;
                    }
                    if (isWheel)
                    {
                        GameObject Bean = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                        Bean.transform.localScale = new Vector3(0.3f, 0.05f, 0.3f);
                        Rigidbody BallRB = Bean.AddComponent<Rigidbody>();
                        GameObject userCollision = Instantiate(new GameObject());
                        userCollision.AddComponent<BoxCollider>();

                        Bean.layer = 8;
                        Bean.name = Bean.name + "MonoObject";
                        userCollision.layer = 0;
                        userCollision.transform.SetParent(Bean.transform, false);
                        Bean.transform.SetParent(itemsFolder.transform, false);

                        BallRB.useGravity = true;
                        BallRB.mass = 3.5f;

                        Bean.transform.up = hitInfo.normal;
                        Bean.transform.position = hitInfo.point + Bean.transform.up / 2.5f;
                        Bean.GetComponent<Renderer>().material.color = Color.black;
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
                Cursor.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                Cursor.GetComponent<Renderer>().material.color = Color.cyan;

                Destroy(Cursor.GetComponent<SphereCollider>());
            }
            else
            { if (Cursor != null) { Destroy(Cursor.gameObject); } }
        }
        if (!editMode) { if (Cursor != null) { Destroy(Cursor.gameObject); } }
    }
}

public class BathManager : MonoBehaviour
{
    bool primaryDown;
    bool canPlace;
    public bool editMode = true;
    public bool inRoom;

    public GameObject BathModel = null;
    GameObject Cursor = null;
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
        if (itemsFolder == null)
        {
            itemsFolder = GameObject.Find("ItemFolderMono");
            print("serch");
        }
        if (Cursor != null)
        {
            Cursor.transform.position = hitInfo.point + Cursor.transform.forward / 4;
            Cursor.transform.forward = hitInfo.normal;
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out primaryDown);
            if (primaryDown)
            {
                if (canPlace)
                {
                    GameObject Bath = Instantiate(BathModel);
                    Rigidbody BathRB = Bath.AddComponent<Rigidbody>();
                    Bath.layer = 8;
                    Bath.name = Bath.name + "MonoObject";
                    Bath.transform.SetParent(itemsFolder.transform, false);

                    BathRB.useGravity = true;
                    BathRB.mass = 8f;

                    Bath.transform.forward = hitInfo.normal;
                    Bath.transform.position = hitInfo.point + Bath.transform.forward / 4;
                    Bath.transform.localScale = new Vector3(20f, 20f, 20f);
                    canPlace = false;
                }
            }
            else { canPlace = true; }
        }
        else
        {
            if (editMode)
            {
                Cursor = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Cursor.transform.localScale = new Vector3(0.4f, 1f, 0.4f);
                Cursor.GetComponent<Renderer>().material.color = Color.cyan;

                if (Cursor.GetComponent<BoxCollider>() != null)
                { Destroy(Cursor.GetComponent<BoxCollider>()); }
                else
                {
                    if (Cursor.GetComponent<MeshCollider>() != null)
                    {
                        { Destroy(Cursor.GetComponent<MeshCollider>()); }
                    }
                }
            }
            else
            { if (Cursor != null) { Destroy(Cursor.gameObject); } }
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
public class CrateManager : MonoBehaviour
{
    bool primaryDown;
    bool canPlace;
    public bool editMode = true;
    public bool inRoom;

    public GameObject CrateModel = null;
    GameObject Cursor = null;
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
        if (itemsFolder == null)
        {
            itemsFolder = GameObject.Find("ItemFolderMono");
            print("serch");
        }
        if (Cursor != null)
        {
            Cursor.transform.position = hitInfo.point + Cursor.transform.forward / 4;
            Cursor.transform.forward = hitInfo.normal;
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out primaryDown);
            if (primaryDown)
            {
                if (canPlace)
                {
                    GameObject Crate = Instantiate(CrateModel);
                    Rigidbody CrateRB = Crate.AddComponent<Rigidbody>();
                    GameObject userCollision = Instantiate(new GameObject());
                    Crate.layer = 8;
                    Crate.AddComponent<BoxCollider>().size = new Vector3(0.02f, 0.02f, 0.02f);
                    Crate.name = Crate.name + "MonoObject";
                    userCollision.layer = 0;
                    userCollision.transform.SetParent(Crate.transform, false);
                    Crate.transform.SetParent(itemsFolder.transform, false);

                    CrateRB.useGravity = true;
                    CrateRB.mass = 4.5f;

                    Crate.transform.forward = hitInfo.normal;
                    Crate.transform.position = hitInfo.point + Crate.transform.forward / 3.5f;
                    Crate.transform.localScale = new Vector3(20f, 20f, 20f);
                    userCollision.AddComponent<BoxCollider>().size = new Vector3(0.02f, 0.02f, 0.02f);
                    canPlace = false;
                }
            }
            else { canPlace = true; }
        }
        else
        {
            if (editMode)
            {
                Cursor = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Cursor.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                Cursor.GetComponent<Renderer>().material.color = Color.cyan;

                if (Cursor.GetComponent<BoxCollider>() != null)
                { Destroy(Cursor.GetComponent<BoxCollider>()); }
            }
            else
            { if (Cursor != null) { Destroy(Cursor.gameObject); } }
        }
        if (!editMode) { if (Cursor != null) { Destroy(Cursor.gameObject); } }
    }
}

public class CouchManager : MonoBehaviour
{
    bool primaryDown;
    bool canPlace;
    public bool editMode = true;
    public bool inRoom;

    public GameObject CouchModel = null;
    GameObject Cursor = null;
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
        if (itemsFolder == null)
        {
            itemsFolder = GameObject.Find("ItemFolderMono");
            print("serch");
        }
        if (Cursor != null)
        {
            Cursor.transform.position = hitInfo.point + Vector3.up * 0.4f;
            //Cursor.transform.forward = hitInfo.normal;
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out primaryDown);
            if (primaryDown)
            {
                if (canPlace)
                {
                    GameObject Couch = Instantiate(CouchModel);
                    Rigidbody CouchRB = Couch.AddComponent<Rigidbody>();
                    Couch.layer = 8;

                    Couch.AddComponent<BoxCollider>();
                    Couch.name = Couch.name + "MonoObject";
                    Couch.transform.SetParent(itemsFolder.transform, false);
                    CouchRB.useGravity = true;
                    CouchRB.mass = 8f;

                    Couch.transform.position = hitInfo.point + Vector3.up*0.4f;
                    Couch.transform.localScale = new Vector3(100f, 100f, 100f);
                    canPlace = false;
                }
            }
            else { canPlace = true; }
        }
        else
        {
            if (editMode)
            {
                Cursor = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Cursor.transform.localScale = new Vector3(1.25f, 0.6f, 0.55f);
                Cursor.GetComponent<Renderer>().material.color = Color.cyan;

                if (Cursor.GetComponent<BoxCollider>() != null)
                { Destroy(Cursor.GetComponent<BoxCollider>()); }
                else
                {
                    if (Cursor.GetComponent<MeshCollider>() != null)
                    {
                        { Destroy(Cursor.GetComponent<MeshCollider>()); }
                    }
                }
            }
            else
            { if (Cursor != null) { Destroy(Cursor.gameObject); } }
        }
        if (!editMode) { if (Cursor != null) { Destroy(Cursor.gameObject); } }
    }
}

public class Explode : MonoBehaviour
{
    public GameObject ExplosionOBJ;
    public float multiplier;
    bool canExplode = true;
    public void Explosion()
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

            Collider[] colliders = Physics.OverlapSphere(transform.position, 6);
            foreach (Collider nearyby in colliders)
            {
                Rigidbody rig = nearyby.GetComponent<Rigidbody>();
                if (rig != null)
                {
                    rig.AddExplosionForce(1500f * multiplier, transform.position, 8f);
                }
            }
            Collider[] bombs = Physics.OverlapSphere(transform.position, 6);
            foreach (Collider nearyby in bombs)
            {
                if (nearyby.GetComponent<BombDetonate>() != null)
                {
                    nearyby.GetComponent<BombDetonate>().Explode();
                }
                if(nearyby.GetComponent<Explode>() != null)
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