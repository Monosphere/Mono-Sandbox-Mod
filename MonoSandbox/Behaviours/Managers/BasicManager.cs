using MonoSandbox;
using MonoSandbox.Behaviours;
using UnityEngine;

public class BoxManager : PlacementHandling
{
    public bool IsPlane;

    public override GameObject CursorRef
    {
        get
        {
            GameObject cursor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cursor.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            Destroy(cursor.GetComponent<Collider>());
            return cursor;
        }
    }

    public override void DrawCursor(RaycastHit hitInfo)
    {
        base.DrawCursor(hitInfo);

        Cursor.transform.position = hitInfo.point + Cursor.transform.forward / 4;
        Cursor.transform.forward = hitInfo.normal;
    }

    public override void Activated(RaycastHit hitInfo)
    {
        base.Activated(hitInfo);

        if (!IsPlane)
        {
            GameObject Box = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject userCollision = new GameObject();
            userCollision.AddComponent<BoxCollider>();

            Box.layer = 8;
            Box.name = string.Concat(Box.name, "MonoObject");
            userCollision.layer = 0;
            userCollision.transform.SetParent(Box.transform, false);
            Box.transform.SetParent(SandboxContainer.transform, false);

            Rigidbody BoxRB = Box.AddComponent<Rigidbody>();
            BoxRB.useGravity = true;
            BoxRB.mass = 2.5f;

            Box.transform.forward = hitInfo.normal;
            Box.transform.position = hitInfo.point + Box.transform.forward / 4;
            Box.GetComponent<Renderer>().material = RefCache.Default;
            Box.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        }
        else
        {
            GameObject Box = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject userCollision = new GameObject();
            userCollision.AddComponent<BoxCollider>();

            Box.layer = 8;
            Box.name = string.Concat(Box.name, "MonoObject");
            userCollision.layer = 0;
            userCollision.transform.SetParent(Box.transform, false);
            Box.transform.SetParent(SandboxContainer.transform, false);

            Rigidbody BoxRB = Box.AddComponent<Rigidbody>();
            BoxRB.useGravity = true;
            BoxRB.mass = 2.5f;

            Box.transform.forward = hitInfo.normal;
            Box.transform.position = hitInfo.point + Box.transform.forward / 4;
            Box.GetComponent<Renderer>().material = RefCache.Default;
            Box.transform.localScale = new Vector3(0.6f, 0.6f, 0.1f);
        }
    }

    public void LateUpdate()
    {
        if (Cursor != null)
        {
            Cursor.transform.localScale = IsPlane ? new Vector3(0.6f, 0.6f, 0.1f) : new Vector3(0.4f, 0.4f, 0.4f);
        }
    }
}


public class SphereManager : PlacementHandling
{
    public bool IsEnemy, IsSoftbody;
    public GameObject Softbody, Entity;

    public override GameObject CursorRef
    {
        get
        {
            GameObject cursor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            cursor.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            Destroy(cursor.GetComponent<Collider>());
            return cursor;
        }
    }

    public override void DrawCursor(RaycastHit hitInfo)
    {
        base.DrawCursor(hitInfo);

        Cursor.transform.position = hitInfo.point + Cursor.transform.forward / 4;
        Cursor.transform.forward = hitInfo.normal;
    }

    public override void Activated(RaycastHit hitInfo)
    {
        base.Activated(hitInfo);

        if (!IsSoftbody && !IsEnemy)
        {
            GameObject Ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Rigidbody BallRB = Ball.AddComponent<Rigidbody>();
            GameObject userCollision = new GameObject();
            userCollision.AddComponent<SphereCollider>();

            Ball.layer = 8;
            Ball.name = string.Concat(Ball.name, "MonoObject");
            userCollision.layer = 0;
            userCollision.transform.SetParent(Ball.transform, false);
            Ball.transform.SetParent(SandboxContainer.transform, false);

            BallRB.useGravity = true;
            BallRB.mass = 3.5f;

            Ball.transform.forward = hitInfo.normal;
            Ball.transform.position = hitInfo.point + Ball.transform.forward / 4;
            Ball.GetComponent<Renderer>().material = RefCache.Default;
            Ball.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        }
        else if (IsSoftbody && !IsEnemy)
        {
            GameObject Ball = Instantiate(Softbody);

            Ball.layer = 8;
            Ball.name = string.Concat(Ball.name, "MonoObject");
            Ball.transform.SetParent(SandboxContainer.transform, false);
            foreach (Transform g in Ball.transform.GetChild(0).GetComponentInChildren<Transform>())
            {
                g.name += "MonoObject";
            }

            Ball.transform.forward = hitInfo.normal;
            Ball.transform.position = hitInfo.point + Ball.transform.forward / 4;
            Ball.transform.GetChild(1).GetComponent<Renderer>().material = RefCache.Default;
            Ball.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            Ball.AddComponent<BoneSphere>();
        }
        else if (!IsSoftbody && IsEnemy)
        {
            GameObject Ball = Instantiate(Entity);

            Ball.name = "MonoObject";
            Ball.AddComponent<SphereCollider>();

            Enemy enemy = Ball.AddComponent<Enemy>();
            enemy.Health = 40f;
            enemy.Defence = 1.75f;

            Ball.transform.SetParent(SandboxContainer.transform, false);
            Ball.transform.position = hitInfo.point + Vector3.up / 2f;
        }
    }
}

public class BeanManager : PlacementHandling
{
    public bool IsBarrel, IsWheel;
    public GameObject Barrel, Explosion;

    public void Start()
    {
        Offset = 4.5f;
    }

    public override GameObject CursorRef
    {
        get
        {
            GameObject cursor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            cursor.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            Destroy(cursor.GetComponent<Collider>());
            return cursor;
        }
    }

    public override void DrawCursor(RaycastHit hitInfo)
    {
        base.DrawCursor(hitInfo);

        Cursor.transform.position = hitInfo.point + Cursor.transform.up / 4.5f;
        Cursor.transform.up = hitInfo.normal;
    }

    public override void Activated(RaycastHit hitInfo)
    {
        base.Activated(hitInfo);

        if (IsBarrel)
        {
            GameObject Bean = Instantiate(Barrel);
            Bean.AddComponent<BoxCollider>().size = new Vector3(0.025f, 0.025f, 0.025f);
            Rigidbody BallRB = Bean.AddComponent<Rigidbody>();
            GameObject userCollision = new GameObject();
            userCollision.AddComponent<BoxCollider>().size = new Vector3(0.025f, 0.025f, 0.025f); ;
            Explode explosion = Bean.AddComponent<Explode>();
            explosion.Multiplier = 4f;
            explosion.Explosion = Explosion;

            Bean.layer = 8;
            Bean.name = string.Concat(Bean.name, "MonoObject");
            userCollision.layer = 0;
            userCollision.transform.SetParent(Bean.transform, false);
            Bean.transform.SetParent(SandboxContainer.transform, false);

            BallRB.useGravity = true;
            BallRB.mass = 3.5f;

            Bean.transform.up = hitInfo.normal;
            Bean.transform.position = hitInfo.point + Bean.transform.up / 2.5f;
            Bean.transform.localScale = new Vector3(15f, 15f, 15f);
        }
        else if (IsWheel)
        {
            GameObject Bean = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            Bean.transform.localScale = new Vector3(0.3f, 0.05f, 0.3f);
            Rigidbody BallRB = Bean.AddComponent<Rigidbody>();
            GameObject userCollision = new GameObject();
            userCollision.AddComponent<BoxCollider>();

            Bean.layer = 8;
            Bean.name = string.Concat(Bean.name, "MonoObject");
            userCollision.layer = 0;
            userCollision.transform.SetParent(Bean.transform, false);
            Bean.transform.SetParent(SandboxContainer.transform, false);

            BallRB.useGravity = true;
            BallRB.mass = 3.5f;

            Bean.transform.up = hitInfo.normal;
            Bean.transform.position = hitInfo.point + Bean.transform.up / 2.5f;
            Bean.GetComponent<Renderer>().material = RefCache.Default;
        }
        else
        {
            GameObject Bean = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            Rigidbody BallRB = Bean.AddComponent<Rigidbody>();
            GameObject userCollision = new GameObject();
            userCollision.AddComponent<CapsuleCollider>().height = 2;

            Bean.layer = 8;
            Bean.name = string.Concat(Bean.name, "MonoObject");
            userCollision.layer = 0;
            userCollision.transform.SetParent(Bean.transform, false);
            Bean.transform.SetParent(SandboxContainer.transform, false);

            BallRB.useGravity = true;
            BallRB.mass = 3.5f;

            Bean.transform.up = hitInfo.normal;
            Bean.transform.position = hitInfo.point + Bean.transform.up / 2.5f;
            Bean.GetComponent<Renderer>().material = RefCache.Default;
            Bean.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        }
    }
}

public class BathManager : PlacementHandling
{
    public GameObject Bath;

    public override GameObject CursorRef
    {
        get
        {
            GameObject cursor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cursor.transform.localScale = new Vector3(0.4f, 1f, 0.4f);
            Destroy(cursor.GetComponent<Collider>());
            return cursor;
        }
    }

    public override void DrawCursor(RaycastHit hitInfo)
    {
        base.DrawCursor(hitInfo);

        Cursor.transform.position = hitInfo.point + Cursor.transform.forward / 4;
        Cursor.transform.forward = hitInfo.normal;
    }

    public override void Activated(RaycastHit hitInfo)
    {
        base.Activated(hitInfo);

        GameObject BathObject = Instantiate(Bath);
        Rigidbody BathRB = BathObject.AddComponent<Rigidbody>();
        BathObject.layer = 8;
        BathObject.name = string.Concat(BathObject.name, "MonoObject");
        BathObject.transform.SetParent(SandboxContainer.transform, false);

        BathRB.useGravity = true;
        BathRB.mass = 8f;

        BathObject.transform.forward = hitInfo.normal;
        BathObject.transform.position = hitInfo.point + BathObject.transform.forward / 4;
        BathObject.transform.localScale = new Vector3(20f, 20f, 20f);
    }
}

public class CrateManager : PlacementHandling
{
    public GameObject Crate;

    public override GameObject CursorRef
    {
        get
        {
            GameObject cursor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cursor.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            Destroy(cursor.GetComponent<Collider>());
            return cursor;
        }
    }

    public override void DrawCursor(RaycastHit hitInfo)
    {
        base.DrawCursor(hitInfo);

        Cursor.transform.position = hitInfo.point + Cursor.transform.forward / 4;
        Cursor.transform.forward = hitInfo.normal;
    }

    public override void Activated(RaycastHit hitInfo)
    {
        base.Activated(hitInfo);

        GameObject CrateObject = Instantiate(Crate);
        Rigidbody BoxRB = CrateObject.AddComponent<Rigidbody>();
        GameObject userCollision = new GameObject();
        userCollision.AddComponent<BoxCollider>();

        CrateObject.layer = 8;
        CrateObject.name = string.Concat(CrateObject.name, "MonoObject");
        userCollision.layer = 0;
        userCollision.transform.SetParent(CrateObject.transform, false);
        CrateObject.transform.SetParent(SandboxContainer.transform, false);

        BoxRB.useGravity = true;
        BoxRB.mass = 2.5f;
        CrateObject.transform.forward = hitInfo.normal;
        CrateObject.transform.position = hitInfo.point + CrateObject.transform.forward / 4;
    }
}

public class CouchManager : PlacementHandling
{
    public GameObject Couch;

    public override GameObject CursorRef
    {
        get
        {
            GameObject cursor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cursor.transform.localScale = new Vector3(1.25f, 0.6f, 0.55f);
            Destroy(cursor.GetComponent<Collider>());
            return cursor;
        }
    }

    public override void DrawCursor(RaycastHit hitInfo)
    {
        base.DrawCursor(hitInfo);

        Cursor.transform.position = hitInfo.point + Vector3.up * 0.4f;
    }

    public override void Activated(RaycastHit hitInfo)
    {
        base.Activated(hitInfo);

        GameObject CouchObject = Instantiate(Couch);
        Rigidbody CouchRB = CouchObject.AddComponent<Rigidbody>();
        CouchObject.layer = 8;

        CouchObject.AddComponent<BoxCollider>();
        CouchObject.name = string.Concat(CouchObject.name, "MonoObject");
        CouchObject.transform.SetParent(SandboxContainer.transform, false);
        CouchRB.useGravity = true;
        CouchRB.mass = 8f;

        CouchObject.transform.position = hitInfo.point + Vector3.up * 0.4f;
        CouchObject.transform.localScale = new Vector3(100f, 100f, 100f);
    }
}


public class Explode : MonoBehaviour
{
    public GameObject Explosion;
    public float Multiplier;
    private bool Exploding = true;

    public void ExplodeObject()
    {
        if (Exploding)
        {
            Exploding = false;

            gameObject.GetComponent<AudioSource>().Play();
            gameObject.GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;

            GameObject ExplodeOBJ = Instantiate(Explosion);
            ExplodeOBJ.transform.SetParent(transform);
            ExplodeOBJ.transform.localPosition = Vector3.zero;
            ExplodeOBJ.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            Collider[] colliders = Physics.OverlapSphere(transform.position, 6);
            foreach (Collider nearyby in colliders)
            {
                Rigidbody rig = nearyby.GetComponent<Rigidbody>();
                rig?.AddExplosionForce(1500f * Multiplier, transform.position, 8f);

                nearyby.GetComponent<BombDetonate>()?.Explode();
                nearyby.GetComponent<MineDetonate>()?.Explode();
                nearyby.GetComponent<Explode>()?.ExplodeObject();
            }

            Rigidbody PlayerRigidbody = GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>();
            PlayerRigidbody.AddExplosionForce(2500f * Multiplier * Mathf.Sqrt(PlayerRigidbody.mass), transform.position, 5 + (0.75f * Multiplier));

            Invoke(nameof(Destroy), 3);
        }
    }

    void Delete()
    {
        Destroy(gameObject);
    }
}