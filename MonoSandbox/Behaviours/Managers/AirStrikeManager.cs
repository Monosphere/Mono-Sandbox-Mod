using MonoSandbox;
using MonoSandbox.Behaviours;
using UnityEngine;

public class AirStrikeManager : MonoBehaviour
{
    public bool primaryDown, canPlace, editMode;
    public GameObject Cursor, AirStrikeModel, CursorModel, ExplodeModel;
    GameObject itemsFolder = null;

    public void Start()
    {
        itemsFolder = gameObject;
    }

    public void Update()
    {
        RaycastHit hitInfo = RefCache.Hit;

        if (Cursor != null)
        {
            Cursor.transform.position = hitInfo.point;
            Cursor.transform.forward = hitInfo.normal;
            primaryDown = InputHandling.RightPrimary;
            if (primaryDown)
            {
                if (canPlace)
                {
                    GameObject Missile = Instantiate(AirStrikeModel);
                    Missile.transform.SetParent(itemsFolder.transform, false);
                    Missile.transform.position = hitInfo.point + new Vector3(0, 80f, 0);
                    Missile.transform.localScale = new Vector3(50f, 50f, 50f);
                    Airstrike airstrikeControl = Missile.AddComponent<Airstrike>();
                    airstrikeControl.StrikeLocation = hitInfo.point;
                    airstrikeControl.ExplosionOBJ = ExplodeModel;

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
                Cursor = Instantiate(CursorModel);
                Cursor.transform.localScale = new Vector3(20f, 20f, 20f);
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
}


public class Airstrike : MonoBehaviour
{
    public Vector3 StrikeLocation;
    public GameObject ExplosionOBJ;
    private bool canExplode = true;
    public static float speed = 20;

    public void Update()
    {
        var newspeed = speed * Time.deltaTime;
        transform.position = transform.position = Vector3.MoveTowards(transform.position, StrikeLocation, newspeed);

        if (Vector3.Distance(transform.position, StrikeLocation) < 0.5f && canExplode)
        {
            Explode();
            canExplode = false;
        }
    }

    public void Explode()
    {
        gameObject.GetComponent<AudioSource>().minDistance = 15;
        gameObject.GetComponent<AudioSource>().Play();
        gameObject.GetComponent<Renderer>().enabled = false;
        foreach (Transform child in transform) Destroy(child.gameObject);
        GameObject ExplodeOBJ = Instantiate(ExplosionOBJ);
        ExplodeOBJ.transform.SetParent(transform);
        ExplodeOBJ.transform.localPosition = Vector3.zero;
        ExplodeOBJ.transform.localScale = Vector3.one * 0.3f;

        Collider[] bombs = Physics.OverlapSphere(transform.position, 10);
        foreach (Collider nearyby in bombs)
        {
            nearyby.GetComponent<BombDetonate>()?.Explode();
            nearyby.GetComponent<MineDetonate>()?.Explode();
            nearyby.GetComponent<Explode>()?.ExplodeObject();

            Rigidbody rig = nearyby.GetComponent<Rigidbody>();
            if (rig != null && rig.useGravity) rig.AddExplosionForce(14400f, transform.position, 80f, 0.5f, ForceMode.Force);
        }
        Rigidbody PlayerRigidbody = GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>();
        PlayerRigidbody.AddExplosionForce(14400f * Mathf.Sqrt(PlayerRigidbody.mass), transform.position, 80f, 0.5f, ForceMode.Force);
        Invoke(nameof(Finish), 2);
    }

    void Finish()
    {
        Destroy(gameObject);
    }
}
