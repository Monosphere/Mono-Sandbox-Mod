using BepInEx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Utilla;
using HarmonyLib;
using Bepinject;
using MonoSandbox.ComputerInterface;
using MonoSandbox.Components;

namespace MonoSandbox
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [ModdedGamemode]
    [Description("HauntedModMenu")]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        static bool inRoom;

        bool canStart;

        public static float nextClick;

        private float grip;
        Vector3 previous;

        AssetBundle WeaponBundle1;
        AssetBundle WeaponBundle2;
        AssetBundle ObjectBundle2;
        AssetBundle listpack;

        GameObject itemsFolder;
        public static GameObject monoList;
        public static GameObject hand;

        ListManager Listmanager;

        public static ThrusterManager thrusterManager;
        public static C4Manager C4Control;
        public static BalloonManager balloonManager;
        public static WeaponManager weaponManager;

        BoxManager boxManager;
        GravityManager gravityManager;
        SphereManager sphereManager;
        BeanManager beanManager;
        CrateManager crateManager;
        BathManager bathManager;
        CouchManager couchManager;
        RagdollManager ragdollManager;
        AirStrikeManager airstrikeManager;
        SpringManager springManager;
        WeldManager weldManager;
        FreezeManager freezeManager;
        PhysGunManager physGunManager;
        List<InputDevice> list = new List<InputDevice>();

        Harmony harmony;

        void OnEnable()
        {
            if (harmony == null)
            {
                harmony = new Harmony(PluginInfo.GUID);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }

            if (canStart)
            {
                ragdollManager.editMode = false;
                springManager.editMode = false;
                weaponManager.editMode = false;
                thrusterManager.editMode = false;
                C4Control.editMode = false;
                boxManager.editMode = false;
                sphereManager.editMode = false;
                beanManager.editMode = false;
                crateManager.editMode = false;
                weldManager.editMode = false;
                bathManager.editMode = false;
                balloonManager.editMode = false;
                freezeManager.editMode = false;
                physGunManager.editMode = false;
                gravityManager.editMode = false;
                airstrikeManager.editMode = false;
                couchManager.editMode = false;
                ListManager.objectButtons = new bool[12];
            }
        }

        void OnDisable()
        {
            if (harmony != null)
            {
                harmony.UnpatchSelf();
            }

            if (canStart)
            {
                ragdollManager.editMode = false;
                springManager.editMode = false;
                weaponManager.editMode = false;
                thrusterManager.editMode = false;
                C4Control.editMode = false;
                boxManager.editMode = false;
                sphereManager.editMode = false;
                beanManager.editMode = false;
                crateManager.editMode = false;
                weldManager.editMode = false;
                bathManager.editMode = false;
                balloonManager.editMode = false;
                freezeManager.editMode = false;
                physGunManager.editMode = false;
                gravityManager.editMode = false;
                airstrikeManager.editMode = false;
                couchManager.editMode = false;
                ListManager.objectButtons = new bool[12];
            }

            if (monoList != null)
                monoList.SetActive(enabled);
        }

        public void Awake()
        {
            Events.GameInitialized += OnGameInitialized;
            Zenjector.Install<MainInstaller>().OnProject();
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            itemsFolder = Instantiate(new GameObject());
            itemsFolder.transform.position = Vector3.zero;
            itemsFolder.name = "ItemFolderMono";

            hand = PluginInfo.leftHand;

            InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Right | UnityEngine.XR.InputDeviceCharacteristics.Controller, list);
            WeaponBundle1 = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("MonoSandbox.Assets.weapons2"));
            ObjectBundle2 = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("MonoSandbox.Assets.objects2"));
            listpack = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("MonoSandbox.Assets.listpack"));
            WeaponBundle2 = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("MonoSandbox.Assets.weaponspack"));

            #region CreateManagers
            C4Control = itemsFolder.AddComponent<C4Manager>();
            C4Control.C4Model = WeaponBundle1.LoadAsset<GameObject>("Objects").transform.GetChild(3).gameObject;
            C4Control.ExplodeModel = ObjectBundle2.LoadAsset<GameObject>("Objects2").transform.GetChild(2).gameObject;
            boxManager = itemsFolder.AddComponent<BoxManager>();
            sphereManager = itemsFolder.AddComponent<SphereManager>();
            sphereManager.softbodySphere = WeaponBundle2.LoadAsset<GameObject>("WeaponPack 1").transform.GetChild(23).gameObject;
            sphereManager.entityOBJ = WeaponBundle2.LoadAsset<GameObject>("WeaponPack 1").transform.GetChild(24).gameObject;
            beanManager = itemsFolder.AddComponent<BeanManager>();
            beanManager.explosionOBJ = WeaponBundle2.LoadAsset<GameObject>("WeaponPack 1").transform.GetChild(10).gameObject;
            beanManager.barrelOBJ = WeaponBundle2.LoadAsset<GameObject>("WeaponPack 1").transform.GetChild(12).gameObject;
            gravityManager = itemsFolder.AddComponent<GravityManager>();
            couchManager = itemsFolder.AddComponent<CouchManager>();
            couchManager.CouchModel = WeaponBundle2.LoadAsset<GameObject>("WeaponPack 1").transform.GetChild(19).gameObject;
            crateManager = itemsFolder.AddComponent<CrateManager>();
            crateManager.CrateModel = WeaponBundle2.LoadAsset<GameObject>("WeaponPack 1").transform.GetChild(11).gameObject;
            bathManager = itemsFolder.AddComponent<BathManager>();
            bathManager.BathModel = WeaponBundle2.LoadAsset<GameObject>("WeaponPack 1").transform.GetChild(13).gameObject;
            
            springManager = itemsFolder.AddComponent<SpringManager>();
            ragdollManager = itemsFolder.AddComponent<RagdollManager>();
            airstrikeManager = itemsFolder.AddComponent<AirStrikeManager>();
            airstrikeManager.ExplodeModel = WeaponBundle2.LoadAsset<GameObject>("WeaponPack 1").transform.GetChild(10).gameObject;
            airstrikeManager.CursorModel = WeaponBundle2.LoadAsset<GameObject>("WeaponPack 1").transform.GetChild(9).gameObject;
            airstrikeManager.AirStrikeModel = WeaponBundle2.LoadAsset<GameObject>("WeaponPack 1").transform.GetChild(5).gameObject;
            thrusterManager = itemsFolder.AddComponent<ThrusterManager>();
            thrusterManager.ThrusterModel = ObjectBundle2.LoadAsset<GameObject>("Objects2").transform.GetChild(6).gameObject;
            thrusterManager.ThrustParticles = WeaponBundle2.LoadAsset<GameObject>("WeaponPack 1").transform.GetChild(14).gameObject;
            weaponManager = itemsFolder.AddComponent<WeaponManager>();
            weaponManager.ShotgunModel = WeaponBundle2.LoadAsset<GameObject>("WeaponPack 1").transform.GetChild(2).gameObject;
            weaponManager.ToolGunModel = WeaponBundle2.LoadAsset<GameObject>("WeaponPack 1").transform.GetChild(18).gameObject;
            weaponManager.RevolverModel = WeaponBundle2.LoadAsset<GameObject>("WeaponPack 1").transform.GetChild(3).gameObject;
            weaponManager.SniperModel = WeaponBundle2.LoadAsset<GameObject>("WeaponPack 1").transform.GetChild(1).gameObject;
            weaponManager.BananaGunModel = WeaponBundle2.LoadAsset<GameObject>("WeaponPack 1").transform.GetChild(8).gameObject;
            weaponManager.LaserGunModel = WeaponBundle2.LoadAsset<GameObject>("WeaponPack 1").transform.GetChild(6).gameObject;
            weaponManager.MelonCannonModel = WeaponBundle2.LoadAsset<GameObject>("WeaponPack 1").transform.GetChild(15).gameObject;
            weaponManager.MelonModel = WeaponBundle2.LoadAsset<GameObject>("WeaponPack 1").transform.GetChild(16).gameObject;
            weaponManager.MelonExplodeModel = WeaponBundle2.LoadAsset<GameObject>("WeaponPack 1").transform.GetChild(17).gameObject;
            weaponManager.HitPointParticle = WeaponBundle2.LoadAsset<GameObject>("WeaponPack 1").transform.GetChild(22).gameObject;
            weldManager = itemsFolder.AddComponent<WeldManager>();
            freezeManager = itemsFolder.AddComponent<FreezeManager>();
            balloonManager = itemsFolder.AddComponent<BalloonManager>();
            ragdollManager.RagdollModel = ObjectBundle2.LoadAsset<GameObject>("Objects2").transform.GetChild(1).gameObject;
            ragdollManager.GorillaModel = ObjectBundle2.LoadAsset<GameObject>("Objects2").transform.GetChild(4).gameObject;
            physGunManager = itemsFolder.AddComponent<PhysGunManager>();
            canStart = true;
            #endregion
            #region Spawn Da List
            monoList = Instantiate(listpack.LoadAsset<GameObject>("ListPack").transform.GetChild(0).gameObject);
            Listmanager = monoList.AddComponent<ListManager>();
            ListManager.ButtonTextBasic = listpack.LoadAsset<GameObject>("ListPack").transform.GetChild(1).gameObject;
            monoList.name = "List";
            foreach(Renderer ren in monoList.transform.GetComponentsInChildren<Renderer>(true))
            {
                MaterialUtil.FixStandardShader(ren);
            }
            monoList.SetActive(false);
            monoList.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = string.Format("{0} Mod v{1}", PluginInfo.Name, PluginInfo.Version);
            #endregion

            if (canStart)
            {
                ragdollManager.editMode = false;
                springManager.editMode = false;
                weaponManager.editMode = false;
                thrusterManager.editMode = false;
                C4Control.editMode = false;
                boxManager.editMode = false;
                sphereManager.editMode = false;
                beanManager.editMode = false;
                crateManager.editMode = false;
                weldManager.editMode = false;
                bathManager.editMode = false;
                balloonManager.editMode = false;
                freezeManager.editMode = false;
                physGunManager.editMode = false;
                gravityManager.editMode = false;
                airstrikeManager.editMode = false;
                couchManager.editMode = false;
            }

        }

        /* This attribute tells Utilla to call this method when a modded room is joined */
        [ModdedGamemodeJoin] public void OnJoin(string gamemode)
        {
            inRoom = true;
            ListManager.objectButtons = new bool[12];
        }

        /* This attribute tells Utilla to call this method when a modded room is left */
        [ModdedGamemodeLeave] public void OnLeave(string gamemode)
        {
            /* Deactivate your mod here */
            /* This code will run regardless of if the mod is enabled*/
            ListManager.objectButtons = new bool[12];
            foreach (Transform child in itemsFolder.transform)
            {
                Destroy(child.gameObject);
            }
            monoList.SetActive(false);
            inRoom = false;
        }

        internal void FixedUpdate()
        {
            if (GorillaLocomotion.Player.Instance != null)
                Physics.Raycast(GorillaLocomotion.Player.Instance.rightHandTransform.position, GorillaLocomotion.Player.Instance.rightHandTransform.forward, out PluginInfo.raycastHit, GorillaLocomotion.Player.Instance.locomotionEnabledLayers);

            #region List
            if (inRoom && enabled)
            {
                InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.grip, out grip);
                if (grip > 0.6f)
                {
                    if (monoList.activeInHierarchy && canStart)
                    {
                        if (ListManager.objectButtons[0]) { boxManager.editMode = true; boxManager.isPlane = false; } else { if (!ListManager.objectButtons[7]) { boxManager.editMode = false; } }
                        if (ListManager.objectButtons[1]) { sphereManager.editMode = true; sphereManager.isSoftbody = false; } else { if (!ListManager.objectButtons[11]) { sphereManager.editMode = false; } }
                        if (ListManager.objectButtons[2]) { beanManager.editMode = true; beanManager.isBarrel = false; beanManager.isWheel = false; } else { if (!ListManager.objectButtons[4] && !ListManager.objectButtons[5]) { beanManager.editMode = false; } }
                        if (ListManager.objectButtons[3]) { crateManager.editMode = true; } else { crateManager.editMode = false; }
                        if (ListManager.objectButtons[4]) { beanManager.editMode = true; beanManager.isBarrel = true; beanManager.isWheel = false; }
                        if (ListManager.objectButtons[5]) { beanManager.editMode = true; beanManager.isBarrel = false; beanManager.isWheel = true; }
                        if (ListManager.objectButtons[6]) { couchManager.editMode = true; } else { couchManager.editMode = false; }
                        if (ListManager.objectButtons[7]) { boxManager.editMode = true; boxManager.isPlane = true; }
                        if (ListManager.objectButtons[8]) { ragdollManager.editMode = true; ragdollManager.useGorilla = false; } else { if (!ListManager.objectButtons[9]) { ragdollManager.editMode = false; } }
                        if (ListManager.objectButtons[9]) { ragdollManager.editMode = true; ragdollManager.useGorilla = true; } else { if (!ListManager.objectButtons[8]) { ragdollManager.editMode = false; } }
                        if (ListManager.objectButtons[10]) { bathManager.editMode = true; } else { bathManager.editMode = false; }
                        if (ListManager.objectButtons[11]) { sphereManager.editMode = true; sphereManager.isSoftbody = true; }

                        if (ListManager.weaponButtons[0]) { weaponManager.editMode = true; weaponManager.currentWeapon = 0; }
                        if (ListManager.weaponButtons[1]) { weaponManager.editMode = true; weaponManager.currentWeapon = 1; }
                        if (ListManager.weaponButtons[2]) { weaponManager.editMode = true; weaponManager.currentWeapon = 2; }
                        if (ListManager.weaponButtons[3]) { weaponManager.editMode = true; weaponManager.currentWeapon = 3; }
                        if (ListManager.weaponButtons[4]) { C4Control.editMode = true; } else { C4Control.editMode = false; }
                        if (ListManager.weaponButtons[5]) { airstrikeManager.editMode = true; } else { airstrikeManager.editMode = false; }
                        if (ListManager.weaponButtons[6]) { weaponManager.editMode = true; weaponManager.currentWeapon = 4; }
                        if (ListManager.weaponButtons[7]) { weaponManager.editMode = true; weaponManager.currentWeapon = 5; }

                        if (ListManager.toolButtons[0]) { weldManager.editMode = true; } else { weldManager.editMode = false; }
                        if (ListManager.toolButtons[1]) { thrusterManager.editMode = true; } else { thrusterManager.editMode = false; }
                        if (ListManager.toolButtons[2]) { springManager.editMode = true; } else { springManager.editMode = false; }
                        if (ListManager.toolButtons[3]) { physGunManager.editMode = true; } else { physGunManager.editMode = false; }
                        if (ListManager.toolButtons[4]) { weaponManager.editMode = true; weaponManager.currentWeapon = 6; }
                        if (ListManager.toolButtons[5]) { freezeManager.editMode = true; } else { freezeManager.editMode = false; }
                        if (ListManager.toolButtons[6]) { gravityManager.editMode = true; } else { gravityManager.editMode = false; }
                        if (ListManager.toolButtons[7]) { balloonManager.editMode = true; } else { balloonManager.editMode = false; }

                        if (ListManager.utilButtons[0])
                        {
                            foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
                            {
                                if (gameObj.name.Contains("Balloon") && gameObj.name.Contains("MonoObject"))
                                {
                                    Destroy(gameObj);
                                }
                            }
                        }
                        if (ListManager.utilButtons[1])
                        {
                            foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
                            {
                                if (gameObj.name.Contains("MSJoint") && gameObj.name.Contains("MonoObject"))
                                {
                                    Destroy(gameObj);
                                }
                            }
                        }
                        if (ListManager.utilButtons[2])
                        {
                            foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
                            {
                                if (gameObj.name.Contains("Thruster") && gameObj.name.Contains("MonoObject"))
                                {
                                    Destroy(gameObj);
                                }
                            }
                        }
                        if (ListManager.utilButtons[3])
                        {
                            foreach (Transform child in itemsFolder.transform)
                            {
                                Destroy(child.gameObject);
                            }
                        }

                        if (!ListManager.weaponButtons[0] && !ListManager.weaponButtons[1] && !ListManager.weaponButtons[2] && !ListManager.weaponButtons[3] && !ListManager.weaponButtons[6] && !ListManager.weaponButtons[7] && !ListManager.toolButtons[4])
                        {
                            weaponManager.currentWeapon = 100;
                            weaponManager.editMode = false;
                        }

                        if (ListManager.funButtons[0])
                        {
                            sphereManager.editMode = true;
                            sphereManager.isEnemy = true;
                            sphereManager.isBouncy = false;
                            sphereManager.isSoftbody = false;
                        }

                    }
                    else { monoList.SetActive(true); }
                }
                else { if (monoList.activeInHierarchy) { monoList.SetActive(false); } }
            }
            else
            {
                if (canStart)
                {
                    ragdollManager.editMode = false;
                    springManager.editMode = false;
                    weaponManager.editMode = false;
                    thrusterManager.editMode = false;
                    C4Control.editMode = false;
                    boxManager.editMode = false;
                    sphereManager.editMode = false;
                    beanManager.editMode = false;
                    crateManager.editMode = false;
                    weldManager.editMode = false;
                    bathManager.editMode = false;
                    balloonManager.editMode = false;
                    freezeManager.editMode = false;
                    physGunManager.editMode = false;
                    gravityManager.editMode = false;
                    airstrikeManager.editMode = false;
                    couchManager.editMode = false;
                }
            }
            #endregion
        }
        #region List & Buttons
        public class ListManager : MonoBehaviour
        {
            GameObject MenuParent;

            public static GameObject ButtonTextBasic;

            public static GameObject ObjectsParent = null;
            public static GameObject ToolsParent = null;
            public static GameObject UtilsParent = null;
            public static GameObject WeaponsParent = null;
            public static GameObject FunParent = null;

            GameObject SideButtonsParent;
            Canvas canvas;

            public static bool[] objectButtons = new bool[12];
            public static bool[] weaponButtons = new bool[12];
            public static bool[] toolButtons = new bool[8];
            public static bool[] utilButtons = new bool[4];
            public static bool[] funButtons = new bool[2];

            string[] objectNames = new string[12] { "Box", "Sphere", "Bean", "Crate", "Barrel", "Wheel", "Couch", "Plane", "Body", "Gorilla", "Bathtub", "Soft Sphere" };
            string[] weaponNames = new string[8] { "Revolver", "Shotgun", "Melon Cannon", "Sniper", "C4", "Airstrike", "Laser Gun", "Banana Gun" };
            string[] toolNames = new string[8] { "Weld", "Thruster", "Spring", "Gravity Gun", "Colorize", "Freeze", "Toggle Gravity", "Balloon"};
            string[] utilNames = new string[4] { "Delete Balloons", "Delete Springs", "Delete Thrusters", "Delete All"};
            string[] funNames = new string[1] { "Entity" };

            public static int currentPage;

            // Start is called before the first frame update
            void Start()
            {
                MenuParent = transform.GetChild(1).gameObject;
                canvas = MenuParent.transform.GetChild(0).gameObject.GetComponent<Canvas>();
                ObjectsParent = Instantiate(new GameObject());
                WeaponsParent = Instantiate(new GameObject());
                ToolsParent = Instantiate(new GameObject());
                UtilsParent = Instantiate(new GameObject());
                FunParent = Instantiate(new GameObject());
                SideButtonsParent = Instantiate(new GameObject());
                SideButtonsParent.name = "SideButtons";
                SideButtonsParent.transform.SetParent(MenuParent.transform, false);
                AddPage(objectNames, "Objects", 4, ObjectsParent, 0);
                AddPage(weaponNames, "Weapon", 4, WeaponsParent, 1);
                AddPage(toolNames, "Tools", 4, ToolsParent, 2);
                AddPage(utilNames, "Utils", 4, UtilsParent, 3);
                AddPage(funNames, "Fun", 4, FunParent, 4);
                monoList.transform.SetParent(Plugin.hand.transform, false);
                monoList.transform.localPosition = new Vector3(0, 0, 0.05f);
                monoList.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                monoList.transform.localEulerAngles = new Vector3(180, -90, 180);
            }

            void Update()
            {
                if (WeaponsParent != null)
                {
                    if (currentPage != 0) { ObjectsParent.SetActive(false); canvas.transform.GetChild(0).gameObject.SetActive(false); } else { ObjectsParent.SetActive(true); canvas.transform.GetChild(0).gameObject.SetActive(true); }
                    if (currentPage != 1) { WeaponsParent.SetActive(false); canvas.transform.GetChild(2).gameObject.SetActive(false); } else { WeaponsParent.SetActive(true); canvas.transform.GetChild(2).gameObject.SetActive(true); }
                    if (currentPage != 2) { ToolsParent.SetActive(false); canvas.transform.GetChild(4).gameObject.SetActive(false); } else { ToolsParent.SetActive(true); canvas.transform.GetChild(4).gameObject.SetActive(true); }
                    if (currentPage != 3) { UtilsParent.SetActive(false); canvas.transform.GetChild(6).gameObject.SetActive(false); } else { UtilsParent.SetActive(true); canvas.transform.GetChild(6).gameObject.SetActive(true); }
                    if (currentPage != 4) { FunParent.SetActive(false); canvas.transform.GetChild(8).gameObject.SetActive(false); } else { FunParent.SetActive(true); canvas.transform.GetChild(8).gameObject.SetActive(true); }
                }
            }

            void AddPage(string[] buttonNames, string pageName, int perline, GameObject buttonParent, int pIndex)
            {
                int currentSpot = 0;
                int currentLine = 0;
                buttonParent.transform.SetParent(MenuParent.transform, false);
                buttonParent.name = pageName;
                GameObject textParent = Instantiate(new GameObject());
                textParent.name = pageName;
                textParent.transform.parent = canvas.transform;
                GameObject sideButton = GameObject.CreatePrimitive(PrimitiveType.Cube);
                sideButton.layer = 18;
                sideButton.GetComponent<BoxCollider>().isTrigger = true;
                sideButton.transform.SetParent(SideButtonsParent.transform, false);
                sideButton.transform.localScale = new Vector3(0.02f, 0.07f, 0.225f);
                sideButton.transform.localPosition = new Vector3(0, 0.085f - ((float)pIndex * 0.1f), 0.745f);
                MaterialUtil.FixStandardShader(sideButton);
                PageButton PageButtonScript = sideButton.AddComponent<PageButton>();
                PageButtonScript.page = pIndex;

                GameObject pageLabel = Instantiate(ButtonTextBasic);
                pageLabel.transform.SetParent(canvas.transform, false);
                pageLabel.name = pageName;
                pageLabel.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 90, 0);
                pageLabel.transform.position = sideButton.transform.position + new Vector3(-0.015f, 0, 0);
                pageLabel.GetComponent<Text>().text = pageName;
                pageLabel.GetComponent<Text>().color = Color.black;

                for (int i = 0; i < buttonNames.Length; i++)
                {
                    GameObject button = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    button.layer = 18;
                    button.GetComponent<BoxCollider>().isTrigger = true;
                    Button buttonScript = button.AddComponent<Button>();
                    buttonScript.thisButton = (int)i;
                    button.transform.localScale = new Vector3(0.025f, 0.145f, 0.145f);
                    button.transform.SetParent(buttonParent.transform, false);
                    currentLine = (int)Mathf.Floor(i / perline);
                    button.transform.localPosition = new Vector3(0.02f, -currentLine * (button.transform.localScale.y + 0.03f), currentSpot * (button.transform.localScale.z + 0.02f));
                    currentSpot++;
                    if (currentSpot == perline)
                    {
                        currentSpot = 0;
                    }

                    GameObject objLabel = Instantiate(ButtonTextBasic);
                    objLabel.transform.SetParent(textParent.transform, false);
                    objLabel.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 90, 0);
                    objLabel.transform.position = button.transform.position + new Vector3(-0.015f, 0, 0);
                    objLabel.GetComponent<Text>().text = buttonNames[i];
                    objLabel.GetComponent<Text>().color = Color.black;

                    MaterialUtil.FixStandardShader(button);
                }
            }
        }

        public class Button : MonoBehaviour
        {
            private void Update()
            {
                if (ListManager.currentPage == 0)
                {
                    if (ListManager.objectButtons[thisButton] == true) { gameObject.GetComponent<Renderer>().material.color = Color.blue; } else { gameObject.GetComponent<Renderer>().material.color = Color.white; }
                }
                else { ListManager.objectButtons = new bool[12]; }
                if (ListManager.currentPage == 1)
                {
                    if (ListManager.weaponButtons[thisButton] == true) { gameObject.GetComponent<Renderer>().material.color = Color.blue; } else { gameObject.GetComponent<Renderer>().material.color = Color.white; }
                }
                else { ListManager.weaponButtons = new bool[12]; }
                if (ListManager.currentPage == 2)
                {
                    if (ListManager.toolButtons[thisButton] == true) { gameObject.GetComponent<Renderer>().material.color = Color.blue; } else { gameObject.GetComponent<Renderer>().material.color = Color.white; }
                }
                else { ListManager.toolButtons = new bool[8]; }
                if (ListManager.currentPage == 3)
                {
                    if (ListManager.utilButtons[thisButton] == true) { gameObject.GetComponent<Renderer>().material.color = Color.blue; } else { gameObject.GetComponent<Renderer>().material.color = Color.white; }
                }
                else { ListManager.utilButtons = new bool[4]; }
                if (ListManager.currentPage == 4)
                {
                    if (ListManager.funButtons[thisButton] == true) { gameObject.GetComponent<Renderer>().material.color = Color.blue; } else { gameObject.GetComponent<Renderer>().material.color = Color.white; }
                }
                else { ListManager.funButtons = new bool[2]; }
            }
            public int thisButton;
            private void OnTriggerEnter(Collider other)
            {
                GorillaTriggerColliderHandIndicator component;
                try {component = other.GetComponent<GorillaTriggerColliderHandIndicator>();}
                catch{Debug.LogError("invalid presser");return;}

                if (component.isLeftHand)
                    return;

                GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);

                Debug.Log("Collided with " + other.gameObject.layer);
                if (ListManager.currentPage == 0)
                {
                    ListManager.objectButtons = new bool[12];
                    ListManager.objectButtons[thisButton] = true;
                }
                if (ListManager.currentPage == 1)
                {
                    ListManager.weaponButtons = new bool[12];
                    ListManager.weaponButtons[thisButton] = true;
                }
                if (ListManager.currentPage == 2)
                {
                    ListManager.toolButtons = new bool[8];
                    ListManager.toolButtons[thisButton] = true;
                }
                if (ListManager.currentPage == 3)
                {
                    ListManager.utilButtons = new bool[4];
                    ListManager.utilButtons[thisButton] = true;
                }
                if (ListManager.currentPage == 4)
                {
                    ListManager.funButtons = new bool[2];
                    ListManager.funButtons[thisButton] = true;
                }
                // just use an list mono not an array >:(
            }
        }
        public class PageButton : MonoBehaviour
        {
            public int page;
            private void Update()
            {
                if (ListManager.currentPage == page) { gameObject.GetComponent<Renderer>().material.color = Color.blue; } else { gameObject.GetComponent<Renderer>().material.color = Color.white; }
            }
            private void OnTriggerEnter(Collider other)
            {
                GorillaTriggerColliderHandIndicator component;
                try { component = other.GetComponent<GorillaTriggerColliderHandIndicator>(); }
                catch { Debug.LogError("invalid presser"); return; }

                GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);

                Debug.Log("Moved to page  " + page);
                ListManager.currentPage = page;
            }
        }
        #endregion

        [HarmonyPatch(typeof(GorillaTagger))]
        [HarmonyPatch("Awake", MethodType.Normal)]
        private class GorillaTaggerPatch
        {
            public static void Postfix(GorillaTagger __instance)
            {
                PluginInfo.leftHand = __instance.offlineVRRig.leftHandTransform.parent.Find("palm.01.L").gameObject;
                PluginInfo.rightHand = __instance.offlineVRRig.rightHandTransform.parent.Find("palm.01.R").gameObject;
            }
        }
    }
}
