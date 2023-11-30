using BepInEx;
using GorillaLocomotion;
using HarmonyLib;
using MonoSandbox.Behaviours;
using MonoSandbox.Behaviours.UI;
using System;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Utilla;

namespace MonoSandbox
{
    [ModdedGamemode, Description("HauntedModMenu")]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static bool InRoom;
        private bool _initialized;

        private LayerMask _layerMask;

        private AssetBundle _bundle;

        public GameObject _list, _itemsContainer;
        public AudioClip _pageOpen, _itemOpen;

        private SandboxMenu _listManager;

        private BoxManager boxManager;
        private GravityManager gravityManager;
        private SphereManager sphereManager;
        private BeanManager beanManager;
        private CrateManager crateManager;
        private BathManager bathManager;
        private CouchManager couchManager;
        private RagdollManager ragdollManager;
        private AirStrikeManager airstrikeManager;
        private SpringManager springManager;
        private WeldManager weldManager;
        private FreezeManager freezeManager;
        private PhysGunManager physGunManager;
        private ThrusterManager thrusterManager;
        private C4Manager C4Control;
        private BalloonManager balloonManager;
        private WeaponManager weaponManager;
        private HammerManager hammerManager;
        private GrenadeManager grenadeManager;

        public void OnEnable()
        {
            if (_initialized)
            {
                ragdollManager.IsEditing = false;
                springManager.editMode = false;
                weaponManager.editMode = false;
                thrusterManager.editMode = false;
                C4Control.editMode = false;
                boxManager.IsEditing = false;
                sphereManager.IsEditing = false;
                beanManager.IsEditing = false;
                crateManager.IsEditing = false;
                weldManager.editMode = false;
                bathManager.IsEditing = false;
                balloonManager.editMode = false;
                freezeManager.editMode = false;
                physGunManager.editMode = false;
                gravityManager.editMode = false;
                airstrikeManager.editMode = false;
                couchManager.IsEditing = false;
                hammerManager.editMode = false;
                grenadeManager.editMode = false;
            }
        }

        public void OnDisable()
        {
            if (_initialized)
            {
                ragdollManager.IsEditing = false;
                springManager.editMode = false;
                weaponManager.editMode = false;
                thrusterManager.editMode = false;
                C4Control.editMode = false;
                boxManager.IsEditing = false;
                sphereManager.IsEditing = false;
                beanManager.IsEditing = false;
                crateManager.IsEditing = false;
                weldManager.editMode = false;
                bathManager.IsEditing = false;
                balloonManager.editMode = false;
                freezeManager.editMode = false;
                physGunManager.editMode = false;
                gravityManager.editMode = false;
                airstrikeManager.editMode = false;
                couchManager.IsEditing = false;
                hammerManager.editMode = false;
                grenadeManager.editMode = false;
            }

            _list?.SetActive(enabled);
        }

        public Plugin()
        {
            Events.GameInitialized += OnGameInitialized;
            new Harmony(PluginInfo.GUID).PatchAll(typeof(Plugin).Assembly);
        }

        public void OnGameInitialized(object sender, EventArgs e)
        {
            gameObject.AddComponent<InputHandling>();

            _layerMask = Player.Instance.locomotionEnabledLayers;
            _layerMask |= 1 << 8;

            _itemsContainer = Instantiate(new GameObject());
            _itemsContainer.transform.position = Vector3.zero;
            _itemsContainer.name = "ItemFolderMono";
            RefCache.SandboxContainer = _itemsContainer;

            _bundle = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("MonoSandbox.Assets.sandboxbundle"));

            #region Create Managers

            RefCache.Default = _bundle.LoadAsset<Material>("Default");
            RefCache.Selection = _bundle.LoadAsset<Material>("Selection");
            RefCache.PageSelection = _bundle.LoadAsset<AudioClip>("Step1");
            RefCache.ItemSelection = _bundle.LoadAsset<AudioClip>("Step2");

            C4Control = _itemsContainer.AddComponent<C4Manager>();
            C4Control.C4Model = _bundle.LoadAsset<GameObject>("C4_Weapon");
            C4Control.Mine = _bundle.LoadAsset<GameObject>("Mine_02");
            C4Control.ExplodeModel = _bundle.LoadAsset<GameObject>("Explosion");

            boxManager = _itemsContainer.AddComponent<BoxManager>();

            sphereManager = _itemsContainer.AddComponent<SphereManager>();
            sphereManager.Softbody = _bundle.LoadAsset<GameObject>("BoneSphere");

            sphereManager.Entity = _bundle.LoadAsset<GameObject>("Demon");
            beanManager = _itemsContainer.AddComponent<BeanManager>();
            beanManager.Explosion = _bundle.LoadAsset<GameObject>("Explosion");
            beanManager.Barrel = _bundle.LoadAsset<GameObject>("Barrel");

            gravityManager = _itemsContainer.AddComponent<GravityManager>();

            couchManager = _itemsContainer.AddComponent<CouchManager>();
            couchManager.Couch = _bundle.LoadAsset<GameObject>("Couch");

            crateManager = _itemsContainer.AddComponent<CrateManager>();
            crateManager.Crate = _bundle.LoadAsset<GameObject>("Crate");

            bathManager = _itemsContainer.AddComponent<BathManager>();
            bathManager.Bath = _bundle.LoadAsset<GameObject>("Bath");

            springManager = _itemsContainer.AddComponent<SpringManager>();

            ragdollManager = _itemsContainer.AddComponent<RagdollManager>();

            airstrikeManager = _itemsContainer.AddComponent<AirStrikeManager>();
            airstrikeManager.ExplodeModel = _bundle.LoadAsset<GameObject>("Explosion");
            airstrikeManager.CursorModel = _bundle.LoadAsset<GameObject>("Cursor");
            airstrikeManager.AirStrikeModel = _bundle.LoadAsset<GameObject>("Missile");

            thrusterManager = _itemsContainer.AddComponent<ThrusterManager>();
            thrusterManager.ThrusterModel = _bundle.LoadAsset<GameObject>("Thruster 1");
            thrusterManager.ThrustParticles = _bundle.LoadAsset<GameObject>("Thruster 2");

            weaponManager = _itemsContainer.AddComponent<WeaponManager>();
            weaponManager.ShotgunModel = _bundle.LoadAsset<GameObject>("Shotgun");
            weaponManager.ToolGunModel = _bundle.LoadAsset<GameObject>("ToolGun");
            weaponManager.RevolverModel = _bundle.LoadAsset<GameObject>("Pistol");
            weaponManager.SniperModel = _bundle.LoadAsset<GameObject>("SniperRifle");
            weaponManager.BananaGunModel = _bundle.LoadAsset<GameObject>("Banan");
            weaponManager.LaserGunModel = _bundle.LoadAsset<GameObject>("LaserGun");
            weaponManager.MelonCannonModel = _bundle.LoadAsset<GameObject>("Cannon");
            weaponManager.MelonModel = _bundle.LoadAsset<GameObject>("Melon");
            weaponManager.MelonExplodeModel = _bundle.LoadAsset<GameObject>("MelonExplode");
            weaponManager.HitPointParticle = _bundle.LoadAsset<GameObject>("HitPoint");
            weaponManager.AssultRiffle = _bundle.LoadAsset<GameObject>("AssaultRifle");
            weaponManager.LaserExplode = _bundle.LoadAsset<GameObject>("Explosion 2");

            weldManager = _itemsContainer.AddComponent<WeldManager>();

            freezeManager = _itemsContainer.AddComponent<FreezeManager>();

            balloonManager = _itemsContainer.AddComponent<BalloonManager>();
            balloonManager.Balloon = _bundle.LoadAsset<GameObject>("Balloon");

            ragdollManager.Body = _bundle.LoadAsset<GameObject>("Body");
            ragdollManager.Gorilla = _bundle.LoadAsset<GameObject>("GorillaBody");

            physGunManager = _itemsContainer.AddComponent<PhysGunManager>();

            hammerManager = _itemsContainer.AddComponent<HammerManager>();
            hammerManager.asset = _bundle.LoadAsset<GameObject>("Hammer_Weapon");

            grenadeManager = _itemsContainer.AddComponent<GrenadeManager>();
            grenadeManager.Grenade = _bundle.LoadAsset<GameObject>("Grenade");
            grenadeManager.Explode = _bundle.LoadAsset<GameObject>("Explosion");

            _initialized = true;

            #endregion

            #region Spawn Da List

            _list = Instantiate(_bundle.LoadAsset<GameObject>("List"));
            _listManager = _list.AddComponent<SandboxMenu>();
            _listManager._text = _bundle.LoadAsset<GameObject>("Temp");

            _list.name = "List";
            _list.SetActive(false);
            _list.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text = PluginInfo.Version;

            #endregion

            if (_initialized)
            {
                ragdollManager.IsEditing = false;
                springManager.editMode = false;
                weaponManager.editMode = false;
                thrusterManager.editMode = false;
                C4Control.editMode = false;
                boxManager.IsEditing = false;
                sphereManager.IsEditing = false;
                beanManager.IsEditing = false;
                crateManager.IsEditing = false;
                weldManager.editMode = false;
                bathManager.IsEditing = false;
                balloonManager.editMode = false;
                freezeManager.editMode = false;
                physGunManager.editMode = false;
                gravityManager.editMode = false;
                airstrikeManager.editMode = false;
                couchManager.IsEditing = false;
                hammerManager.editMode = false;
                grenadeManager.editMode = false;
            }

        }

        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            InRoom = true;

            foreach (Transform child in _itemsContainer.transform)
            {
                child.gameObject.SetActive(true);
            }
        }

        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            InRoom = false;

            foreach (Transform child in _itemsContainer.transform)
            {
                child.gameObject.SetActive(false);
            }

            _list.SetActive(false);
        }

        public void Update()
        {
            if (Player.Instance != null) RefCache.HitExists = Physics.Raycast(Player.Instance.rightControllerTransform.position, Player.Instance.rightControllerTransform.forward, out RefCache.Hit, 2000, _layerMask);

            #region List

            if (InRoom && enabled && _initialized)
            {
                bool isActive = InputHandling.LeftGrip > 0.6f;

                if (_list.activeInHierarchy)
                {
                    boxManager.IsEditing = _listManager.objectButtons[0] || _listManager.objectButtons[7];
                    boxManager.IsPlane = _listManager.objectButtons[7];
                    sphereManager.IsEditing = _listManager.objectButtons[1] || _listManager.objectButtons[11] || _listManager.funButtons[0];
                    sphereManager.IsSoftbody = _listManager.objectButtons[11];
                    sphereManager.IsEnemy = _listManager.funButtons[0];
                    beanManager.IsEditing = _listManager.objectButtons[2] || _listManager.objectButtons[4] || _listManager.objectButtons[5];
                    beanManager.IsBarrel = _listManager.objectButtons[4];
                    beanManager.IsWheel = _listManager.objectButtons[5];
                    ragdollManager.IsEditing = _listManager.objectButtons[8] || _listManager.objectButtons[9];
                    ragdollManager.UseGorilla = _listManager.objectButtons[9];
                    crateManager.IsEditing = _listManager.objectButtons[3];
                    couchManager.IsEditing = _listManager.objectButtons[6];
                    bathManager.IsEditing = _listManager.objectButtons[10];

                    weaponManager.editMode = _listManager.weaponButtons[0] || _listManager.weaponButtons[1] || _listManager.weaponButtons[2] || _listManager.weaponButtons[4] || _listManager.weaponButtons[3] || _listManager.weaponButtons[7] || _listManager.weaponButtons[8] || _listManager.toolButtons[4];
                    C4Control.editMode = _listManager.weaponButtons[5] || _listManager.weaponButtons[9];
                    C4Control.IsMine = _listManager.weaponButtons[9];
                    airstrikeManager.editMode = _listManager.weaponButtons[6];
                    hammerManager.editMode = _listManager.weaponButtons[11];
                    grenadeManager.editMode = _listManager.weaponButtons[10];

                    if (_listManager.weaponButtons[0]) weaponManager.currentWeapon = 0;
                    else if (_listManager.weaponButtons[1]) weaponManager.currentWeapon = 1;
                    else if (_listManager.weaponButtons[4]) weaponManager.currentWeapon = 2;
                    else if (_listManager.weaponButtons[3]) weaponManager.currentWeapon = 3;
                    else if (_listManager.weaponButtons[7]) weaponManager.currentWeapon = 4;
                    else if (_listManager.weaponButtons[8]) weaponManager.currentWeapon = 5;
                    else if (_listManager.toolButtons[4]) weaponManager.currentWeapon = 6;
                    else if (_listManager.weaponButtons[2]) weaponManager.currentWeapon = 7;

                    weldManager.editMode = _listManager.toolButtons[0];
                    thrusterManager.editMode = _listManager.toolButtons[1];
                    springManager.editMode = _listManager.toolButtons[2];
                    physGunManager.editMode = _listManager.toolButtons[3];
                    freezeManager.editMode = _listManager.toolButtons[5];
                    gravityManager.editMode = _listManager.toolButtons[6];
                    balloonManager.editMode = _listManager.toolButtons[7];

                    if (_listManager.utilButtons[0])
                    {
                        foreach (Transform child in _itemsContainer.transform)
                        {
                            Destroy(child.gameObject);
                        }
                    }

                    if (_listManager.utilButtons[1] && thrusterManager.objectList.Count > 0)
                    {
                        foreach (GameObject thrusterObj in thrusterManager.objectList)
                        {
                            if (thrusterObj == null) continue;
                            Destroy(thrusterObj);
                        }
                        thrusterManager.objectList.Clear();
                    }

                    if (_listManager.utilButtons[2] && springManager.objectList.Count > 0)
                    {
                        foreach (GameObject springObj in springManager.objectList)
                        {
                            if (springObj == null) continue;
                            Destroy(springObj);
                        }
                        springManager.objectList.Clear();
                    }

                    if (_listManager.utilButtons[3] && balloonManager.objectList.Count > 0)
                    {
                        foreach (GameObject balloonObj in balloonManager.objectList)
                        {
                            if (balloonObj == null) continue;
                            Destroy(balloonObj);
                        }
                        balloonManager.objectList.Clear();
                    }
                }

                if (_list.activeSelf == isActive) return;
                _list.SetActive(isActive);
            }
            else
            {
                if (_list != null && _list.activeSelf)
                {
                    _list.SetActive(false);
                }
                if (_initialized)
                {
                    ragdollManager.IsEditing = false;
                    springManager.editMode = false;
                    weaponManager.editMode = false;
                    thrusterManager.editMode = false;
                    C4Control.editMode = false;
                    boxManager.IsEditing = false;
                    sphereManager.IsEditing = false;
                    beanManager.IsEditing = false;
                    crateManager.IsEditing = false;
                    weldManager.editMode = false;
                    bathManager.IsEditing = false;
                    balloonManager.editMode = false;
                    freezeManager.editMode = false;
                    physGunManager.editMode = false;
                    gravityManager.editMode = false;
                    airstrikeManager.editMode = false;
                    couchManager.IsEditing = false;
                    hammerManager.editMode = false;
                    grenadeManager.editMode = false;
                }
            }
            #endregion
        }
    }
}
