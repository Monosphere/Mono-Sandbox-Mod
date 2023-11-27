using GorillaTag;
using MonoSandbox.Behaviours;
using UnityEngine;

public class RagdollManager : PlacementHandling
{
    public bool UseGorilla;
    public GameObject Gorilla, Body;

    public void Start()
    {
        Offset = 4.5f;
    }

    public override GameObject CursorRef
    {
        get
        {
            GameObject cursor = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cursor.transform.localScale = new Vector3(0.4f, 0.3f, 0.4f);
            Destroy(cursor.GetComponent<Collider>());
            return cursor;
        }
    }

    public override void DrawCursor(RaycastHit hitInfo)
    {
        base.DrawCursor(hitInfo);

        Cursor.transform.position = hitInfo.point + Vector3.up * 0.15f;
    }

    public override void Activated(RaycastHit hitInfo)
    {
        base.Activated(hitInfo);

        if (UseGorilla)
        {
            GameObject Ragdoll = Instantiate(Gorilla);
            Ragdoll.name += "MonoObject_Ragdoll";
            Ragdoll.transform.SetParent(SandboxContainer.transform, false);

            foreach (Transform g in Ragdoll.transform.GetChild(1).GetComponentsInChildren<Transform>())
            {
                g.gameObject.layer = 8;
                g.name = string.Concat(g.name, "MonoObject");
            }

            Ragdoll.transform.position = hitInfo.point + new Vector3(0f, 0.45f, 0f);

            GTColor.HSVRanges ragdollRanges = new GTColor.HSVRanges(0f, 1f, 0.8f, 0.6f, 1f, 1f);
            Material ragdollMaterial = new Material(Ragdoll.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material)
            {
                color = GTColor.RandomHSV(ragdollRanges)
            };
            Ragdoll.GetComponentInChildren<SkinnedMeshRenderer>().material = ragdollMaterial;
        }
        else
        {
            GameObject Ragdoll = Instantiate(Body);
            Ragdoll.name += "MonoObject_Ragdoll";
            Ragdoll.transform.SetParent(SandboxContainer.transform, false);

            foreach (Transform g in Ragdoll.transform.GetChild(0).GetComponentsInChildren<Transform>())
            {
                g.gameObject.layer = 8;
                g.name = string.Concat(g.name, "MonoObject");
            }

            Ragdoll.transform.position = hitInfo.point + new Vector3(0f, 0.6f, 0f);
            Ragdoll.transform.localScale = new Vector3(0.4f, 0.4f, 0.5f);
            Ragdoll.transform.GetChild(1).GetComponent<Renderer>().material.color = Color.grey;

            Destroy(Ragdoll.GetComponent<MeshCollider>());
        }
    }
}