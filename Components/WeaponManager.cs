using System;
using UnityEngine.XR;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    float trigger;
    bool canLaser;
    float lastShot = 0f;
    public float weaponForce = 4f;
    public static float staticweaponForce = 4f;
    private readonly float weaponMult = 2f;
    bool canFire = true;
    bool canChange = true;
    public bool rightHanded = true;
    public bool editMode;
    public bool inRoom;
    public bool primary;
    public bool secondary;

    // "Revolver", "Shotgun", "Melon Cannon", "Sniper", "C4", "Airstrike", "Laser Gun", "Banana Gun" 
    public int currentWeapon = 0;
    private int holdingWeapon = 100;

    private Color[] colourArray = new Color[6] { Color.white,Color.blue,Color.red,Color.green,Color.magenta, Color.black };
    private int currentColour = 0;
    private bool isLit = true;

    public GameObject MelonCannonModel = null;
    public GameObject MelonModel = null;
    public GameObject MelonExplodeModel = null;
    public GameObject RevolverModel = null;
    public GameObject ShotgunModel = null;
    public GameObject SniperModel = null;
    public GameObject LaserGunModel = null;
    public GameObject ToolGunModel = null;
    public GameObject BananaGunModel = null;
    public GameObject HitPointParticle = null;

    private GameObject HeldWeapon = null;
    private GameObject rightHand = null;
    private GameObject leftHand = null;

    public GameObject muzzleFlash;
    public GameObject laserFX;
    GameObject itemsFolder = null;

    void OnGameInitialized(object sender, EventArgs e)
    {
        itemsFolder = GameObject.Find("ItemFolderMono");
    }

    public void UpdateMultiplier(bool UpOrDown)
    {
        weaponForce = UpOrDown ? Mathf.Clamp(weaponForce + 1f, 1, 10) : Mathf.Clamp(weaponForce - 1f, 1, 10);
    }

    void Update()
    {
        staticweaponForce = weaponForce;
        RaycastHit hitInfo;
        Physics.Raycast(GorillaLocomotion.Player.Instance.rightHandTransform.position + GorillaLocomotion.Player.Instance.rightHandTransform.forward / 8, GorillaLocomotion.Player.Instance.rightHandTransform.forward, out hitInfo);
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.trigger, out trigger);
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out primary);
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.secondaryButton, out secondary);
        if (itemsFolder == null)
        {
            itemsFolder = GameObject.Find("ItemFolderMono");
        }
        if(rightHand == null || leftHand == null)
        {
            rightHand = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.01.R");
            leftHand = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body/shoulder.L/upper_arm.L/forearm.L/hand.L/palm.01.L");
        }
        if(editMode && holdingWeapon != currentWeapon)
        {
            Destroy(HeldWeapon);
            WeaponStuff(currentWeapon);
        }

        //Different gun stuff
        if(editMode && trigger > 0.5f)
        {
            if(canFire)
            {
                if (Time.time > lastShot + 0.5f)
                {
                    if (holdingWeapon == 0)
                    {
                        canFire = false;
                        lastShot = Time.time;
                        HeldWeapon.GetComponent<AudioSource>().Play();
                        HeldWeapon.transform.GetComponentInChildren<ParticleSystem>().Play();
                        RaycastHit hit;
                        Physics.Raycast(HeldWeapon.transform.GetChild(0).position, HeldWeapon.transform.GetChild(0).forward, out hit);
                        GameObject hitPoint = Instantiate(HitPointParticle);
                        hitPoint.transform.position = hit.point;
                        hitPoint.transform.forward = hit.normal;
                        hitPoint.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                        if (hit.transform.GetComponent<Rigidbody>() != null)
                        {
                            hit.transform.GetComponent<Rigidbody>().AddExplosionForce(600 * weaponForce * weaponMult, hit.point, 3f);
                        }
                        if (hit.transform.GetComponent<BombDetonate>() != null)
                        {
                            hit.transform.GetComponent<BombDetonate>().Explode();
                        }
                        if(hit.transform.GetComponent<Explode>() != null)
                        {
                            hit.transform.GetComponent<Explode>().Explosion();
                        }
                        Destroy(hitPoint, 3);
                    }
                    if (holdingWeapon == 5)
                    {
                        canFire = false;
                        lastShot = Time.time;
                        HeldWeapon.GetComponent<AudioSource>().Play();
                        HeldWeapon.transform.GetComponentInChildren<ParticleSystem>().Play();
                        RaycastHit hit;
                        Physics.Raycast(HeldWeapon.transform.GetChild(0).position, -HeldWeapon.transform.GetChild(0).up, out hit);
                        GameObject hitPoint = Instantiate(HitPointParticle);
                        hitPoint.transform.position = hit.point;
                        hitPoint.transform.forward = hit.normal;
                        hitPoint.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                        if (hit.transform.GetComponent<Rigidbody>() != null)
                        {
                            hit.transform.GetComponent<Rigidbody>().AddExplosionForce(650 * weaponForce * weaponMult, hit.point, 3f);
                        }
                        if (hit.transform.GetComponent<BombDetonate>() != null)
                        {
                            hit.transform.GetComponent<BombDetonate>().Explode();
                        }
                        if (hit.transform.GetComponent<Explode>() != null)
                        {
                            hit.transform.GetComponent<Explode>().Explosion();
                        }
                        Destroy(hitPoint, 3);
                    }
                }
                if (Time.time > lastShot + 0.75f)
                {
                    if (holdingWeapon == 1)
                    {
                        canFire = false;
                        lastShot = Time.time;
                        GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().AddForce(-HeldWeapon.transform.GetChild(0).forward * weaponForce * 5500f);
                        HeldWeapon.GetComponent<AudioSource>().Play();
                        HeldWeapon.transform.GetComponentInChildren<ParticleSystem>().Play();
                        RaycastHit hit;
                        Physics.Raycast(HeldWeapon.transform.GetChild(0).position, HeldWeapon.transform.GetChild(0).forward, out hit);
                        GameObject hitPoint = Instantiate(HitPointParticle);
                        hitPoint.transform.position = hit.point;
                        hitPoint.transform.forward = hit.normal;
                        hitPoint.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                        if (hit.transform.GetComponent<Rigidbody>() != null)
                        {
                            hit.transform.GetComponent<Rigidbody>().AddExplosionForce(900 * weaponForce * weaponMult, hit.point, 4f);
                        }
                        if (hit.transform.GetComponent<BombDetonate>() != null)
                        {
                            hit.transform.GetComponent<BombDetonate>().Explode();
                        }
                        if (hit.transform.GetComponent<Explode>() != null)
                        {
                            hit.transform.GetComponent<Explode>().Explosion();
                        }
                        Destroy(hitPoint, 3);
                    }
                }
                if (Time.time > lastShot + 1.25f)
                {
                    if (holdingWeapon == 2)
                    {
                        canFire = false;
                        lastShot = Time.time;
                        HeldWeapon.GetComponent<AudioSource>().Play();
                        HeldWeapon.transform.GetComponentInChildren<ParticleSystem>().Play();
                        GameObject melon = Instantiate(MelonModel);
                        melon.transform.position = HeldWeapon.transform.GetChild(1).position;
                        melon.transform.GetComponent<Rigidbody>().AddForce(rightHand.transform.up * 2500f);
                        Bullet melonBullet = melon.AddComponent<Bullet>();
                        melonBullet.gunIndex = 2;
                        melonBullet.melonExplode = MelonExplodeModel;
                    }
                    if(holdingWeapon == 3)
                    {
                        canFire = false;
                        lastShot = Time.time;
                        HeldWeapon.GetComponent<AudioSource>().Play();
                        HeldWeapon.transform.GetComponentInChildren<ParticleSystem>().Play();
                        GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().AddForce(-HeldWeapon.transform.GetChild(0).forward * weaponForce * 7500f);
                        RaycastHit hit;
                        Physics.Raycast(HeldWeapon.transform.GetChild(0).position, HeldWeapon.transform.GetChild(0).forward, out hit);
                        GameObject hitPoint = Instantiate(HitPointParticle);
                        hitPoint.transform.position = hit.point;
                        hitPoint.transform.forward = hit.normal;
                        hitPoint.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                        if (hit.transform.GetComponent<Rigidbody>() != null)
                        {
                            hit.transform.GetComponent<Rigidbody>().AddExplosionForce(1000 * weaponForce * weaponMult, hit.point, 4f);
                        }
                        if(hit.transform.GetComponent<BombDetonate>() != null)
                        {
                            hit.transform.GetComponent<BombDetonate>().Explode();
                        }
                        if (hit.transform.GetComponent<Explode>() != null)
                        {
                            hit.transform.GetComponent<Explode>().Explosion();
                        }
                        Destroy(hitPoint, 3);
                    }
                }
            }
            if(holdingWeapon == 4 && canLaser == true)
            {
                canLaser = false;
                HeldWeapon.GetComponent<AudioSource>().loop = true;
                HeldWeapon.GetComponent<AudioSource>().Play();
                HeldWeapon.transform.GetComponentInChildren<ParticleSystem>().Play();
            }
            if (holdingWeapon == 6 && canFire == true)
            {
                canFire = false;
                //HeldWeapon.GetComponent<AudioSource>().loop = true;
                //HeldWeapon.GetComponent<AudioSource>().Play();
                //HeldWeapon.transform.GetComponentInChildren<ParticleSystem>().Play();
                RaycastHit hit;
                Physics.Raycast(HeldWeapon.transform.GetChild(6).position, HeldWeapon.transform.GetChild(6).forward, out hit);
                if(hit.transform.name.Contains("MonoObject") && hit.transform.GetComponent<Renderer>() != null)
                {
                    hit.transform.GetComponent<Renderer>().material.color = colourArray[currentColour];
                }
            }
        }
        else { canFire = true; canLaser = true; if (holdingWeapon == 4) { HeldWeapon.GetComponent<AudioSource>().Stop(); HeldWeapon.transform.GetComponentInChildren<ParticleSystem>().Stop(); } }
        if(editMode)
        {
            if (primary)
            {
                if (holdingWeapon == 6 && canChange)
                {
                    currentColour++;
                    if (currentColour >= colourArray.Length)
                    {
                        currentColour = 0;
                    }
                    HeldWeapon.transform.GetChild(0).GetComponent<Renderer>().material.color = colourArray[currentColour];
                    canChange = false;
                }
            }
            else { if (!secondary) { canChange = true; } }
            if(secondary)
            {
                if (holdingWeapon == 6 && canChange)
                {
                    HeldWeapon.transform.GetChild(0).GetComponent<Renderer>().material.color = colourArray[currentColour];
                    canChange = false;
                }
            }
        }
        if(!editMode && HeldWeapon!=null)
        {
            Destroy(HeldWeapon);
            holdingWeapon = 100;
        }
    }

    void WeaponStuff(int index)
    {
        if(index == 0)
        {
            holdingWeapon = 0;
            HeldWeapon = Instantiate(RevolverModel);
            HeldWeapon.transform.eulerAngles = new Vector3(0f, 90f, -90f);
            HeldWeapon.transform.localPosition = new Vector3(-0.02f, 0f, 0.035f);
            if(rightHanded)
            {
                HeldWeapon.transform.SetParent(rightHand.transform, false);
            }
        }
        if (index == 1)
        {
            holdingWeapon = 1;
            HeldWeapon = Instantiate(ShotgunModel);
            HeldWeapon.transform.eulerAngles = new Vector3(0f, 90f, -90f);
            HeldWeapon.transform.localPosition = new Vector3(-0.02f, 0f, 0.035f);
            if (rightHanded)
            {
                HeldWeapon.transform.SetParent(rightHand.transform, false);
            }
        }
        if (index == 2)
        {
            holdingWeapon = 2;
            HeldWeapon = Instantiate(MelonCannonModel);
            HeldWeapon.transform.eulerAngles = new Vector3(0f, 90f, -90f);
            HeldWeapon.transform.localPosition = new Vector3(-0.025f, 0.25f, -0.1f);
            if (rightHanded)
            {
                HeldWeapon.transform.SetParent(rightHand.transform, false);
            }
        }
        if (index == 3)
        {
            holdingWeapon = 3;
            HeldWeapon = Instantiate(SniperModel);
            HeldWeapon.transform.eulerAngles = new Vector3(0f, 90f, -90f);
            HeldWeapon.transform.localPosition = new Vector3(-0.02f, 0f, 0.035f);
            if (rightHanded)
            {
                HeldWeapon.transform.SetParent(rightHand.transform, false);
            }
        }
        if (index == 4)
        {
            holdingWeapon = 4;
            HeldWeapon = Instantiate(LaserGunModel);
            HeldWeapon.transform.eulerAngles = new Vector3(0f, 90f, -90f);
            HeldWeapon.transform.localPosition = new Vector3(-0.02f, 0f, 0.035f);
            if (rightHanded)
            {
                HeldWeapon.transform.SetParent(rightHand.transform, false);
            }
        }
        if (index == 5)
        {
            holdingWeapon = 5;
            HeldWeapon = Instantiate(BananaGunModel);
            HeldWeapon.transform.eulerAngles = new Vector3(0f, 0f, 180f);
            HeldWeapon.transform.localScale = new Vector3(45, 45, 45);
            HeldWeapon.transform.localPosition = new Vector3(-0.04f, 0.085f, -0.055f);
            if (rightHanded)
            {
                HeldWeapon.transform.SetParent(rightHand.transform, false);
            }
        }
        if (index == 6)
        {
            holdingWeapon = 6;
            HeldWeapon = Instantiate(ToolGunModel);
            GameObject Point = Instantiate(new GameObject());
            Point.transform.SetParent(HeldWeapon.transform, false);
            Point.transform.localPosition = new Vector3(-0.265f,0.1f,0f);
            Point.transform.localEulerAngles = new Vector3(0, -90, 0);
            HeldWeapon.transform.eulerAngles = new Vector3(0f, 90f, -90f);
            HeldWeapon.transform.localScale = new Vector3(1, 1, 1);
            HeldWeapon.transform.localPosition = new Vector3(-0.03f, 0.02f, 0.035f);
            if (rightHanded)
            {
                HeldWeapon.transform.SetParent(rightHand.transform, false);
            }
        }
    }

    void Leave()
    {
    }
}

public class PhysGunManager : MonoBehaviour
{
    float trigger;
    bool primary;
    bool canFreeze;
    bool shooting;
    public bool editMode;
    public bool inRoom;
    private Vector2 joystick;

    FixedJoint joint;

    GameObject itemsFolder = null;
    GameObject Cursor = null;
    GameObject BasePoint = null;
    GameObject rightHand = null;

    void OnGameInitialized(object sender, EventArgs e)
    {
        itemsFolder = GameObject.Find("ItemFolderMono");
        rightHand = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.01.R");
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
            rightHand = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.01.R");
        }
        if (editMode)
        {
            if(Cursor == null)
            {
                Cursor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                Cursor.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                Destroy(Cursor.GetComponent<SphereCollider>());
            }
            Cursor.transform.position = hitInfo.point;
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.trigger, out trigger);
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out primary);
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primary2DAxis, out joystick);
            if (hitInfo.transform.gameObject.GetComponent<Rigidbody>() != null && hitInfo.transform.gameObject.name.Contains("MonoObject")) { Cursor.GetComponent<Renderer>().material.color = Color.cyan; } else { Cursor.GetComponent<Renderer>().material.color = Color.red; }
            if (BasePoint == null)
            {
                BasePoint = Instantiate(new GameObject());
                BasePoint.transform.SetParent(rightHand.transform,false);
                joint = BasePoint.AddComponent<FixedJoint>();
                joint.autoConfigureConnectedAnchor = true;
            }
            if(BasePoint.GetComponent<Rigidbody>() != null && BasePoint.GetComponent<Rigidbody>().useGravity == true)
            {
                BasePoint.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                BasePoint.GetComponent<Rigidbody>().useGravity = false;
            }
            if (trigger > 0.6f)
            {
                Cursor.GetComponent<Renderer>().enabled = false;
                if (hitInfo.transform.GetComponent<Rigidbody>() != null && hitInfo.transform.gameObject.name.Contains("MonoObject"))
                {
                    if (!shooting)
                    {
                        if (hitInfo.rigidbody != null)
                        {
                            joint.connectedBody = hitInfo.rigidbody;
                            joint.connectedBody.constraints = RigidbodyConstraints.None;
                            joint.connectedMassScale = 1f;
                        }
                        shooting = true;
                    }
                }
                if (primary)
                {
                    if (canFreeze && joint.connectedBody != null)
                    {
                        if (joint.connectedBody.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.None)
                        {
                            //baseConstraint = RigidbodyConstraints.FreezeAll;
                            joint.connectedBody.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                        }
                        else
                        {
                            //baseConstraint = RigidbodyConstraints.FreezeAll;
                            joint.connectedBody.constraints = RigidbodyConstraints.None;
                        }
                        canFreeze = false;
                    }
                }
                else { canFreeze = true; }
                if(joystick.y > 0.1f || joystick.y < -0.1f)
                {
                    BasePoint.transform.localPosition = new Vector3(BasePoint.transform.localPosition.x, BasePoint.transform.localPosition.y + 1f * Time.deltaTime * joystick.y, BasePoint.transform.localPosition.z);
                }
            }
            else
            {
                Cursor.GetComponent<Renderer>().enabled = true;
                shooting = false;
                if (joint.connectedBody != null)
                {
                    joint.connectedBody = null;
                    BasePoint.transform.localPosition = Vector3.zero;
                }
            }
        }
        else
        {
            if(Cursor != null)
            {
                Destroy(Cursor);
                joint.connectedBody = null;
            }
        }
    }

    void Leave()
    {
    }
}
class Bullet : MonoBehaviour
{
    public int gunIndex;
    private bool hasCollided;
    public float bulletSpeed = 10f;
    public GameObject melonExplode;
    GameObject exploded;
    void Update()
    {
        if(gunIndex == 0)
        {
            gameObject.transform.Translate(-transform.right* Time.deltaTime * bulletSpeed);
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (!hasCollided)
        {
            hasCollided = true;
            if (gunIndex == 2)
            {
                gameObject.GetComponent<Renderer>().enabled = false;
                exploded = Instantiate(melonExplode);
                exploded.transform.position = transform.position;
                foreach (Transform child in exploded.transform)
                {
                    child.GetComponent<Rigidbody>().AddExplosionForce(700f, transform.position, 6f);
                }
                Invoke("DestroyMelon", 3f);
            }
            if (gunIndex == 0)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f);
                foreach (Collider nearyby in colliders)
                {
                    Rigidbody rig = nearyby.GetComponent<Rigidbody>();
                    if (rig != null)
                    {
                        if (rig.useGravity == true)
                        {
                            rig.AddExplosionForce(150f, transform.position, 1f);
                        }
                    }
                }
                Destroy(gameObject);
            }
        }
    }
    void DestroyMelon()
    {
        Destroy(exploded);
        Destroy(gameObject);
    }
}