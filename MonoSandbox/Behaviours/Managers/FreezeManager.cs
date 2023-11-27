using MonoSandbox;
using MonoSandbox.Behaviours;
using UnityEngine;

public class FreezeManager : MonoBehaviour
{
    bool primaryDown;
    bool canPlace;
    public bool editMode = false;

    GameObject Cursor = null;
    GameObject itemsFolder = null;

    void Start()
    {
        itemsFolder = gameObject;
    }

    void Update()
    {
        RaycastHit hitInfo = RefCache.Hit;
        if (Cursor != null)
        {
            bool isAllowed = hitInfo.transform.gameObject.name.Contains("MonoObject") && hitInfo.collider != null && hitInfo.collider.attachedRigidbody != null;
            Cursor.GetComponent<Renderer>().material.color = isAllowed ? new Color(0.392f, 0.722f, 0.820f, 0.4509804f) : new Color(0.8314f, 0.2471f, 0.1569f, 0.4509804f);

            Cursor.transform.position = hitInfo.point;

            primaryDown = InputHandling.RightPrimary;
            if (primaryDown)
            {
                if (canPlace && isAllowed)
                {
                    Rigidbody freezeRB = hitInfo.collider.attachedRigidbody;
                    freezeRB.constraints = freezeRB.constraints == RigidbodyConstraints.None ? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.None;

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
}
public class GravityManager : MonoBehaviour
{
    bool primaryDown;
    bool canPlace;
    public bool editMode = false;

    GameObject Cursor = null;

    void Update()
    {
        RaycastHit hitInfo = RefCache.Hit;

        if (Cursor != null)
        {
            bool isAllowed = hitInfo.transform.gameObject.name.Contains("MonoObject") && hitInfo.collider != null && hitInfo.collider.attachedRigidbody != null;
            Cursor.GetComponent<Renderer>().material.color = isAllowed ? new Color(0.392f, 0.722f, 0.820f, 0.4509804f) : new Color(0.8314f, 0.2471f, 0.1569f, 0.4509804f);

            Cursor.transform.position = hitInfo.point;

            primaryDown = InputHandling.RightPrimary;
            if (primaryDown)
            {
                if (canPlace && hitInfo.transform.gameObject.name.Contains("MonoObject") && hitInfo.collider != null && hitInfo.collider.attachedRigidbody != null)
                {
                    Rigidbody gravityRB = hitInfo.collider.attachedRigidbody;
                    gravityRB.useGravity = !gravityRB.useGravity;

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
}
