using MonoSandbox;
using MonoSandbox.Behaviours;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class WeaponManager : MonoBehaviour
{
    public float weaponForce = 4f;
    private float trigger, lastShot;
    private readonly float weaponMult = 2f;

    private bool canFire = true, canChange = true;
    public bool rightHanded = true, editMode, primary, secondary;

    public int currentWeapon = 0;
    private int holdingWeapon = 100;

    private MyGradient colourGradient;
    private float colourTimestamp;

    public GameObject MelonCannonModel, MelonModel, MelonExplodeModel, RevolverModel, ShotgunModel, SniperModel, LaserGunModel, ToolGunModel, BananaGunModel, HitPointParticle, AssultRiffle, LaserExplode;
    public GameObject HeldWeapon, rightHand, leftHand;
    public GameObject muzzleFlash, laserFX;

    public GameObject itemsFolder = null;

    private void Start()
    {
        colourGradient = new MyGradient();
        colourGradient.AddKey(0f, Color.red);
        colourGradient.AddKey(1f, Color.Lerp(Color.red, Color.yellow, 0.5f));
        colourGradient.AddKey(2f, Color.yellow);
        colourGradient.AddKey(3f, Color.green);
        colourGradient.AddKey(4f, Color.blue);
        colourGradient.AddKey(5f, Color.magenta);
        colourGradient.AddKey(6f, Color.white);
        colourGradient.AddKey(7f, Color.black);
    }

    void Update()
    {
        trigger = InputHandling.RightTrigger;
        primary = InputHandling.RightPrimary;
        secondary = InputHandling.RightSecondary;

        if (itemsFolder == null)
        {
            itemsFolder = gameObject;
        }

        if (rightHand == null || leftHand == null)
        {
            rightHand = RefCache.RHand;
            leftHand = RefCache.LHand;
        }

        if (editMode && holdingWeapon != currentWeapon)
        {
            Destroy(HeldWeapon);
            WeaponStuff(currentWeapon);
        }

        //Different gun stuff
        if (editMode && trigger > 0.5f)
        {
            if (canFire)
            {
                if (Time.time > lastShot + 0.5f)
                {
                    if (holdingWeapon == 0)
                    {
                        lastShot = Time.time;
                        HeldWeapon.GetComponent<AudioSource>().PlayOneShot(HeldWeapon.GetComponent<AudioSource>().clip);
                        HeldWeapon.transform.GetComponentInChildren<ParticleSystem>().Play();
                        HeldWeapon.GetComponent<SineGunAnimation>().Play();

                        Physics.Raycast(HeldWeapon.transform.GetChild(0).position, HeldWeapon.transform.GetChild(0).forward, out RaycastHit hit, 1000, GorillaLocomotion.Player.Instance.locomotionEnabledLayers);
                        GameObject hitPoint = Instantiate(HitPointParticle);
                        hitPoint.transform.position = hit.point;
                        hitPoint.transform.forward = hit.normal;
                        hitPoint.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                        hit.transform.GetComponent<Rigidbody>()?.AddExplosionForce(600 * weaponForce * weaponMult, hit.point, 3f);
                        hit.transform.GetComponent<BombDetonate>()?.Explode();
                        hit.transform.GetComponent<Explode>()?.ExplodeObject();
                        Destroy(hitPoint, 3);

                        HapticManager.Haptic(HapticManager.HapticType.Use);
                        canFire = false;
                    }
                    if (holdingWeapon == 5)
                    {
                        lastShot = Time.time;
                        HeldWeapon.GetComponent<AudioSource>().PlayOneShot(HeldWeapon.GetComponent<AudioSource>().clip);
                        HeldWeapon.transform.GetComponentInChildren<ParticleSystem>().Play();
                        HeldWeapon.GetComponent<SineGunAnimation>().Play();

                        Physics.Raycast(HeldWeapon.transform.GetChild(0).position, -HeldWeapon.transform.GetChild(0).up, out RaycastHit hit, 1000, GorillaLocomotion.Player.Instance.locomotionEnabledLayers);
                        GameObject hitPoint = Instantiate(HitPointParticle);
                        hitPoint.transform.position = hit.point;
                        hitPoint.transform.forward = hit.normal;
                        hitPoint.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                        hit.transform.GetComponent<Rigidbody>()?.AddExplosionForce(650 * weaponForce * weaponMult, hit.point, 3f);
                        hit.transform.GetComponent<BombDetonate>()?.Explode();
                        hit.transform.GetComponent<Explode>()?.ExplodeObject();
                        Destroy(hitPoint, 3);

                        HapticManager.Haptic(HapticManager.HapticType.Use);
                        canFire = false;
                    }
                    if (holdingWeapon == 6)
                    {
                        lastShot = Time.time;
                        HeldWeapon.GetComponent<AudioSource>().PlayOneShot(HeldWeapon.GetComponent<AudioSource>().clip);
                        HeldWeapon.GetComponent<SineGunAnimation>().Play();

                        Physics.Raycast(HeldWeapon.transform.GetChild(6).position, -HeldWeapon.transform.GetChild(6).right, out RaycastHit hit, 1000, GorillaLocomotion.Player.Instance.locomotionEnabledLayers);
                        if (hit.transform.name.Contains("MonoObject") && hit.transform.GetComponent<Renderer>() != null)
                        {
                            hit.transform.GetComponent<Renderer>().material.color = colourGradient.Evaluate(colourTimestamp);
                        }

                        HapticManager.Haptic(HapticManager.HapticType.Use);
                        canFire = false;
                    }
                }
                if (Time.time > lastShot + 0.75f)
                {
                    if (holdingWeapon == 7)
                    {
                        lastShot = Time.time;
                        GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().AddForce(-HeldWeapon.transform.GetChild(0).forward * weaponForce * 2200);
                        HeldWeapon.GetComponent<AudioSource>().PlayOneShot(HeldWeapon.GetComponent<AudioSource>().clip);
                        HeldWeapon.transform.GetComponentInChildren<ParticleSystem>().Play();
                        HeldWeapon.GetComponent<SineGunAnimation>().Play();

                        Physics.Raycast(HeldWeapon.transform.GetChild(0).position, HeldWeapon.transform.GetChild(0).forward, out RaycastHit hit, 1000, GorillaLocomotion.Player.Instance.locomotionEnabledLayers);
                        GameObject hitPoint = Instantiate(HitPointParticle);
                        hitPoint.transform.position = hit.point;
                        hitPoint.transform.forward = hit.normal;
                        hitPoint.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                        hit.transform.GetComponent<Rigidbody>()?.AddExplosionForce(900 * weaponForce * weaponMult, hit.point, 4f);
                        hit.transform.GetComponent<BombDetonate>()?.Explode();
                        hit.transform.GetComponent<Explode>()?.ExplodeObject();
                        Destroy(hitPoint, 3);

                        HapticManager.Haptic(HapticManager.HapticType.Use);
                        canFire = false;
                    }
                    if (holdingWeapon == 1)
                    {
                        lastShot = Time.time;
                        GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().AddForce(-HeldWeapon.transform.GetChild(0).forward * weaponForce * 2200);
                        HeldWeapon.GetComponent<AudioSource>().PlayOneShot(HeldWeapon.GetComponent<AudioSource>().clip);
                        HeldWeapon.transform.GetComponentInChildren<ParticleSystem>().Play();
                        HeldWeapon.GetComponent<SineGunAnimation>().Play();

                        Physics.Raycast(HeldWeapon.transform.GetChild(0).position, HeldWeapon.transform.GetChild(0).forward, out RaycastHit hit, 1000, GorillaLocomotion.Player.Instance.locomotionEnabledLayers);
                        GameObject hitPoint = Instantiate(HitPointParticle);
                        hitPoint.transform.position = hit.point;
                        hitPoint.transform.forward = hit.normal;
                        hitPoint.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                        hit.transform.GetComponent<Rigidbody>()?.AddExplosionForce(900 * weaponForce * weaponMult, hit.point, 4f);
                        hit.transform.GetComponent<BombDetonate>()?.Explode();
                        hit.transform.GetComponent<Explode>()?.ExplodeObject();
                        Destroy(hitPoint, 3);

                        HapticManager.Haptic(HapticManager.HapticType.Use);
                        canFire = false;
                    }
                }
                if (Time.time > lastShot + 1.25f)
                {
                    if (holdingWeapon == 2)
                    {
                        lastShot = Time.time;
                        HeldWeapon.GetComponent<AudioSource>().PlayOneShot(HeldWeapon.GetComponent<AudioSource>().clip);
                        HeldWeapon.transform.GetComponentInChildren<ParticleSystem>().Play();
                        HeldWeapon.GetComponent<SineGunAnimation>().Play();

                        GameObject melon = Instantiate(MelonModel);
                        melon.transform.position = HeldWeapon.transform.GetChild(1).position;
                        melon.transform.GetComponent<Rigidbody>().AddForce(rightHand.transform.up * 2500f);
                        melon.transform.GetComponent<Rigidbody>().angularVelocity = rightHand.transform.up * 100f;
                        Bullet melonBullet = melon.AddComponent<Bullet>();
                        melonBullet.gunIndex = 2;
                        melonBullet.melonExplode = MelonExplodeModel;

                        HapticManager.Haptic(HapticManager.HapticType.Use);
                        canFire = false;
                    }
                    if (holdingWeapon == 3)
                    {
                        lastShot = Time.time;
                        HeldWeapon.GetComponent<AudioSource>().PlayOneShot(HeldWeapon.GetComponent<AudioSource>().clip);
                        HeldWeapon.transform.GetComponentInChildren<ParticleSystem>().Play();
                        HeldWeapon.GetComponent<SineGunAnimation>().Play();

                        GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().AddForce(-HeldWeapon.transform.GetChild(0).forward * weaponForce * 4500);
                        Physics.Raycast(HeldWeapon.transform.GetChild(0).position, HeldWeapon.transform.GetChild(0).forward, out RaycastHit hit, 1000, GorillaLocomotion.Player.Instance.locomotionEnabledLayers);
                        GameObject hitPoint = Instantiate(HitPointParticle);
                        hitPoint.transform.position = hit.point;
                        hitPoint.transform.forward = hit.normal;
                        hitPoint.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                        hit.transform.GetComponent<Rigidbody>()?.AddExplosionForce(1000 * weaponForce * weaponMult, hit.point, 4f);
                        hit.transform.GetComponent<BombDetonate>()?.Explode();
                        hit.transform.GetComponent<Explode>()?.ExplodeObject();
                        Destroy(hitPoint, 3);

                        HapticManager.Haptic(HapticManager.HapticType.Use);
                        canFire = false;
                    }
                    if (holdingWeapon == 4)
                    {
                        lastShot = Time.time;
                        HeldWeapon.GetComponent<AudioSource>().PlayOneShot(HeldWeapon.GetComponent<AudioSource>().clip);
                        HeldWeapon.transform.GetComponentInChildren<ParticleSystem>().Play();
                        HeldWeapon.GetComponent<SineGunAnimation>().Play();

                        GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().AddForce(-HeldWeapon.transform.GetChild(0).forward * weaponForce * 4500);
                        Physics.Raycast(HeldWeapon.transform.GetChild(0).position, HeldWeapon.transform.GetChild(0).forward, out RaycastHit hit, 1000, GorillaLocomotion.Player.Instance.locomotionEnabledLayers);

                        GameObject ExplodeOBJ = Instantiate(LaserExplode);
                        ExplodeOBJ.transform.SetParent(itemsFolder.transform);
                        ExplodeOBJ.transform.localPosition = hit.point;
                        ExplodeOBJ.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                        Destroy(ExplodeOBJ, 3);

                        Collider[] colliders = Physics.OverlapSphere(hit.point, 24f);
                        foreach (Collider nearyby in colliders)
                        {
                            Rigidbody rig = nearyby.GetComponent<Rigidbody>();
                            rig?.AddExplosionForce(2500f * 4f, hit.point, 8f);
                        }

                        Collider[] bombs = Physics.OverlapSphere(hit.point, 24f);
                        foreach (Collider nearyby in bombs)
                        {
                            nearyby.GetComponent<BombDetonate>()?.Explode();
                            nearyby.GetComponent<MineDetonate>()?.Explode();
                            nearyby.GetComponent<Explode>()?.ExplodeObject();
                        }

                        Rigidbody PlayerRigidbody = GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>();
                        PlayerRigidbody.AddExplosionForce(2500f * 4f * Mathf.Sqrt(PlayerRigidbody.mass), hit.point, 5 + (0.75f * 4f));

                        HapticManager.Haptic(HapticManager.HapticType.Use);
                        canFire = false;
                    }
                }
            }
        }
        else
        {
            canFire = true;
        }

        if (editMode && holdingWeapon == 6)
        {
            if (primary && !secondary)
            {
                colourTimestamp = Mathf.Clamp(colourTimestamp + Time.deltaTime * 1.8f, 0f, 7f);
                HeldWeapon.transform.GetChild(0).GetComponent<Renderer>().material.color = colourGradient.Evaluate(colourTimestamp);
            }
            else if (!primary && secondary)
            {
                colourTimestamp = Mathf.Clamp(colourTimestamp - Time.deltaTime * 1.8f, 0f, 7f);
                HeldWeapon.transform.GetChild(0).GetComponent<Renderer>().material.color = colourGradient.Evaluate(colourTimestamp);
            }
        }

        if (!editMode && HeldWeapon != null)
        {
            Destroy(HeldWeapon);
            HeldWeapon = null;

            holdingWeapon = 100;
        }
    }

    void WeaponStuff(int index)
    {
        if (index == 0)
        {
            holdingWeapon = 0;
            HeldWeapon = Instantiate(RevolverModel);
            HeldWeapon.transform.eulerAngles = new Vector3(0f, 90f, -90f);
            HeldWeapon.transform.localPosition = new Vector3(-0.02f, 0f, 0.035f);
            if (rightHanded)
            {
                HeldWeapon.transform.SetParent(rightHand.transform, false);
            }
            HeldWeapon.AddComponent<SineGunAnimation>().Efficiency = 1.5f;
        }
        else if (index == 1)
        {
            holdingWeapon = 1;
            HeldWeapon = Instantiate(ShotgunModel);
            HeldWeapon.transform.eulerAngles = new Vector3(0f, 90f, -90f);
            HeldWeapon.transform.localPosition = new Vector3(-0.02f, 0f, 0.035f);
            if (rightHanded)
            {
                HeldWeapon.transform.SetParent(rightHand.transform, false);
            }
            HeldWeapon.AddComponent<SineGunAnimation>().Efficiency = 1.3f;
        }
        else if (index == 2)
        {
            holdingWeapon = 2;
            HeldWeapon = Instantiate(MelonCannonModel);
            HeldWeapon.transform.eulerAngles = new Vector3(0f, 90f, -90f);
            HeldWeapon.transform.localPosition = new Vector3(-0.025f, 0.25f, -0.1f);
            if (rightHanded)
            {
                HeldWeapon.transform.SetParent(rightHand.transform, false);
            }
            HeldWeapon.AddComponent<SineGunAnimation>().Efficiency = 1.8f;
        }
        else if (index == 3)
        {
            holdingWeapon = 3;
            HeldWeapon = Instantiate(SniperModel);
            HeldWeapon.transform.eulerAngles = new Vector3(0f, 90f, -90f);
            HeldWeapon.transform.localPosition = new Vector3(-0.02f, 0f, 0.035f);
            if (rightHanded)
            {
                HeldWeapon.transform.SetParent(rightHand.transform, false);
            }
            HeldWeapon.AddComponent<SineGunAnimation>().Efficiency = 1.3f;
        }
        else if (index == 4)
        {
            holdingWeapon = 4;
            HeldWeapon = Instantiate(LaserGunModel);
            HeldWeapon.transform.eulerAngles = new Vector3(0f, 90f, -90f);
            HeldWeapon.transform.localPosition = new Vector3(-0.02f, 0f, 0.035f);
            if (rightHanded)
            {
                HeldWeapon.transform.SetParent(rightHand.transform, false);
            }
            HeldWeapon.AddComponent<SineGunAnimation>().Efficiency = 1.3f;
        }
        else if (index == 5)
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

            SineGunAnimation sineAnim = HeldWeapon.AddComponent<SineGunAnimation>();
            sineAnim.Efficiency = 1.5f;
            sineAnim.UseX = true;
        }
        else if (index == 6)
        {
            holdingWeapon = 6;
            HeldWeapon = Instantiate(ToolGunModel);
            GameObject Point = new GameObject();
            Point.transform.SetParent(HeldWeapon.transform, false);
            Point.transform.localPosition = new Vector3(-0.265f, 0.1f, 0f);
            Point.transform.localEulerAngles = new Vector3(0, -90, 0);
            HeldWeapon.transform.eulerAngles = new Vector3(0f, 90f, -90f);
            HeldWeapon.transform.localScale = new Vector3(1, 1, 1);
            HeldWeapon.transform.localPosition = new Vector3(-0.03f, 0.02f, 0.035f);
            if (rightHanded)
            {
                HeldWeapon.transform.SetParent(rightHand.transform, false);
            }
            HeldWeapon.AddComponent<SineGunAnimation>().Efficiency = 0.5f;
            HeldWeapon.transform.GetChild(0).GetComponent<Renderer>().material.color = colourGradient.Evaluate(colourTimestamp);

        }
        else if (index == 7)
        {
            holdingWeapon = 7;
            HeldWeapon = Instantiate(AssultRiffle);

            HeldWeapon.transform.eulerAngles = new Vector3(0f, 90f, -90f);
            HeldWeapon.transform.localPosition = new Vector3(-0.02f, 0.048f, 0.021f);
            HeldWeapon.transform.SetParent(rightHand.transform, false);
            HeldWeapon.AddComponent<SineGunAnimation>();
        }
    }
}

public class HammerManager : MonoBehaviour
{
    public bool editMode;
    private float lastTime;

    public GameObject asset, holdable, point;
    private AudioSource _hammerSource;
    private GorillaVelocityEstimator _velEstimator;

    public void Update()
    {
        if (editMode && holdable == null)
        {
            holdable = Instantiate(asset);
            holdable.transform.eulerAngles = new Vector3(0f, 90f, -90f);
            holdable.transform.localScale = new Vector3(-1, 1, 1);
            holdable.transform.localPosition = new Vector3(-0.03f, 0.02f, 0.035f);
            holdable.transform.SetParent(RefCache.RHand.transform, false);

            point = holdable.transform.GetChild(0).gameObject;
            _hammerSource = holdable.GetComponent<AudioSource>();
            _velEstimator = point.AddComponent<GorillaVelocityEstimator>();
        }
        else if (!editMode && holdable != null)
        {
            Destroy(holdable);
        }
        else if (editMode && holdable != null && Time.time > (lastTime + 0.25f) && _velEstimator.angularVelocity.magnitude > 4f)
        {
            Collider[] colliders = Physics.OverlapSphere(point.transform.position, 0.07f);
            foreach (var collider in colliders)
            {
                Rigidbody rb = collider.GetComponentInParent<Rigidbody>();
                if (rb && collider.name.Contains("MonoObject"))
                {
                    lastTime = Time.time;
                    rb.AddExplosionForce(1800 * Mathf.Clamp(_velEstimator.angularVelocity.magnitude * 1.8f, 1.25f, 3f), Vector3.Lerp(collider.transform.position, holdable.transform.position, 0.4f), 10f);

                    HapticManager.Haptic(HapticManager.HapticType.Use);
                    _hammerSource.pitch = Mathf.Clamp(_velEstimator.angularVelocity.magnitude / 20f, 0.9f, 1f);
                    _hammerSource.Play();

                    rb.GetComponent<BombDetonate>()?.Explode();
                    rb.GetComponent<Explode>()?.ExplodeObject();
                }
            }
        }
    }
}

public class GrenadeManager : MonoBehaviour
{
    public enum GrenadeState
    {
        Idle, Activated, Cooldown
    }

    public bool editMode;
    public GameObject Grenade, Holdable, Explode, Ring, Folder;

    private GrenadeState state;
    private IEnumerator routine;

    private GorillaVelocityEstimator _velEstimator;
    private float grip;

    public void Start()
    {
        Folder = gameObject;
    }

    public void Update()
    {
        if (editMode && Holdable == null)
        {
            Holdable = Instantiate(Grenade);
            Holdable.transform.localPosition = new Vector3(-0.028f, 0.0138f, 0.0027f);
            Holdable.transform.localScale = new Vector3(-1, 1, 1) * 1.1f;
            Holdable.transform.eulerAngles = new Vector3(152.336f, 84.623f, -101.461f);
            Holdable.transform.SetParent(RefCache.RHand.transform, false);

            Ring = Holdable.transform.GetChild(0).gameObject;
            _velEstimator = Holdable.AddComponent<GorillaVelocityEstimator>();
        }
        else if (!editMode && Holdable != null)
        {
            Destroy(Holdable);
            state = GrenadeState.Idle;

            if (routine != null)
            {
                StopCoroutine(routine);
                ((IDisposable)routine).Dispose();
                routine = null;
            }
        }

        if (editMode)
        {
            grip = InputHandling.RightGrip;
            if (state == GrenadeState.Idle && grip > 0.66f)
            {
                state = GrenadeState.Activated;

                Ring.transform.SetParent(null, true);
                Ring.GetComponent<AudioSource>().Play();
                Rigidbody rb = Ring.AddComponent<Rigidbody>();
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                rb.interpolation = RigidbodyInterpolation.Interpolate;
                rb.velocity = _velEstimator.linearVelocity + GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.velocity;

                routine = Explosion();
                StartCoroutine(routine);
            }
            else if (state == GrenadeState.Activated && grip < 0.3f)
            {
                Holdable.transform.SetParent(null, true);
                Rigidbody rb = Holdable.AddComponent<Rigidbody>();
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                rb.interpolation = RigidbodyInterpolation.Interpolate;
                rb.velocity = _velEstimator.linearVelocity * 1.6f;
                rb.angularVelocity = _velEstimator.angularVelocity;
            }
        }
    }

    public void OnDisable()
    {
        if (routine == null) return;
        StopCoroutine(routine);
    }

    public IEnumerator Explosion()
    {
        yield return new WaitForSeconds(5);

        state = GrenadeState.Cooldown;
        Holdable.GetComponent<AudioSource>().Play();
        Holdable.GetComponent<Renderer>().enabled = false;

        GameObject ExplodeOBJ = Instantiate(Explode);
        ExplodeOBJ.transform.SetParent(transform);
        ExplodeOBJ.transform.localPosition = Holdable.transform.position;
        ExplodeOBJ.transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);

        Collider[] colliders = Physics.OverlapSphere(Holdable.transform.position, 10f);
        foreach (Collider nearyby in colliders)
        {
            Rigidbody rig = nearyby.GetComponent<Rigidbody>();
            rig?.AddExplosionForce(1500f * 5, Holdable.transform.position, 8f);

            nearyby.GetComponent<BombDetonate>()?.Explode();
            nearyby.GetComponent<MineDetonate>()?.Explode();
            nearyby.GetComponent<Explode>()?.ExplodeObject();
        }
        Rigidbody PlayerRigidbody = GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>();
        PlayerRigidbody.AddExplosionForce(2500f * 5 * Mathf.Sqrt(PlayerRigidbody.mass), Holdable.transform.position, 5 + (0.75f * 5f));

        yield return new WaitForSeconds(3f);

        state = GrenadeState.Idle;
        Destroy(Holdable);
        Holdable = null;

        yield break;
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

    private GameObject BasePoint, Cursor, itemsFolder, rightHand;

    void Start()
    {
        itemsFolder = gameObject;
        rightHand = RefCache.RHand;
    }

    void Update()
    {
        RaycastHit hitInfo = RefCache.Hit;
        if (itemsFolder == null)
        {
            itemsFolder = gameObject;
            rightHand = RefCache.RHand;
        }

        if (editMode)
        {
            if (Cursor == null)
            {
                Cursor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                Cursor.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                Cursor.GetComponent<Renderer>().material = new Material(RefCache.Selection);
                Destroy(Cursor.GetComponent<SphereCollider>());
            }

            Cursor.transform.position = hitInfo.point;
            trigger = InputHandling.RightTrigger;
            primary = InputHandling.RightPrimary;
            joystick = ControllerInputPoller.Primary2DAxis(XRNode.RightHand);

            Cursor.GetComponent<Renderer>().material.color = (hitInfo.collider != null && hitInfo.collider.attachedRigidbody != null && hitInfo.transform.gameObject.name.Contains("MonoObject")) ? new Color(0f, 1f, 1f, 0.4509804f) : new Color(1f, 0f, 0f, 0.4509804f);
            if (BasePoint == null)
            {
                BasePoint = Instantiate(new GameObject());
                BasePoint.transform.SetParent(rightHand.transform, false);
                joint = BasePoint.AddComponent<FixedJoint>();
                joint.autoConfigureConnectedAnchor = true;
            }
            if (BasePoint.GetComponent<Rigidbody>() != null && BasePoint.GetComponent<Rigidbody>().useGravity == true)
            {
                BasePoint.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                BasePoint.GetComponent<Rigidbody>().useGravity = false;
            }
            if (trigger > 0.6f)
            {
                Cursor.GetComponent<Renderer>().enabled = false;
                if ((hitInfo.collider != null && hitInfo.collider.attachedRigidbody != null && hitInfo.transform.gameObject.name.Contains("MonoObject")))
                {
                    if (!shooting)
                    {
                        if (hitInfo.rigidbody != null)
                        {
                            joint.connectedBody = hitInfo.collider.attachedRigidbody;
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
                if (joystick.y > 0.1f || joystick.y < -0.1f)
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
            if (Cursor != null)
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

public class Bullet : MonoBehaviour
{
    public int gunIndex;
    private bool hasCollided;
    public float bulletSpeed = 10f;
    public GameObject melonExplode;
    GameObject exploded;
    void Update()
    {
        if (gunIndex == 0)
        {
            gameObject.transform.Translate(-transform.right * Time.deltaTime * bulletSpeed);
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (!hasCollided)
        {
            hasCollided = true;
            if (gunIndex == 2)
            {
                other.transform.GetComponent<Enemy>()?.Damage(2f, 5f, 3f);

                gameObject.SetActive(false);
                exploded = Instantiate(melonExplode);
                exploded.transform.position = transform.position;
                foreach (Transform child in exploded.transform)
                {
                    child.GetComponent<Rigidbody>().velocity = (child.position - transform.position) * 50f;
                    child.gameObject.AddComponent<SineScaleOut>().Delay = 2f;
                }
                Invoke(nameof(DestroyMelon), 4.5f);
            }
            if (gunIndex == 0)
            {
                other.transform.GetComponent<Enemy>()?.Damage(2f, 5f, 3f);

                Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f);
                foreach (Collider nearyby in colliders)
                {
                    Rigidbody rig = nearyby.GetComponent<Rigidbody>();
                    if (rig != null && rig.useGravity)
                    {
                        rig.AddExplosionForce(150f, transform.position, 1f);
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