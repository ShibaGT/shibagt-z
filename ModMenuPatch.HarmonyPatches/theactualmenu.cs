using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExitGames.Client.Photon;
using GorillaNetworking;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.XR;
using System.Diagnostics;
using GorillaLocomotion.Swimming;
using System.Threading;
using BepInEx.Configuration;
using System.Net;
using static UnityEngine.UI.GridLayoutGroup;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using PlayFab.ClientModels;
using UnityEngine.UIElements;
using Oculus.Platform.Samples.VrHoops;
using Oculus.Platform;
using static ModMenuPatch.HarmonyPatches.theactualmenu;
using System.Reflection;
using System.Security.Cryptography;
using Photon.Voice;
using shibagtzgui;
using static Autodesk.Fbx.FbxTime;
using static UnityEngine.UI.Image;
using GorillaLocomotion.Gameplay;
using Mono.Cecil.Cil;
using Steamworks;
using GorillaLocomotion;
using UnityEngine.Animations.Rigging;
using System.Text;

namespace ModMenuPatch.HarmonyPatches;

[HarmonyPatch(typeof(GorillaLocomotion.Player))]
[HarmonyPatch("LateUpdate", MethodType.Normal)]
internal class theactualmenu
{
    public enum PhotonEventCodes
    {
        left_jump_photoncode = 69,
        right_jump_photoncode,
        left_jump_deletion,
        right_jump_deletion
    }

    public class TimedBehaviour : MonoBehaviour
    {
        public bool complete = false;

        public bool loop = true;

        public float progress = 0f;

        protected bool paused = false;

        protected float startTime;

        public static bool used = false;

        protected float duration = 2f;

        public virtual void Start()
        {
            startTime = Time.time;
        }

        public virtual void Update()
        {
            if (complete)
            {
                return;
            }
            progress = Mathf.Clamp((Time.time - startTime) / duration, 0f, 1f);
            if (Time.time - startTime > duration)
            {
                if (loop)
                {
                    OnLoop();
                }
                else
                {
                    complete = true;
                }
            }
        }

        public virtual void OnLoop()
        {
            startTime = Time.time;
        }
    }

    public class ColorChanger : TimedBehaviour
    {
        public Renderer gameObjectRenderer;

        public Gradient colors = null;

        public Color color;

        public bool timeBased = true;

        public override void Start()
        {
            base.Start();
            gameObjectRenderer = GetComponent<Renderer>();
        }
    }

    public static bool ResetSpeed = false;

    public static void turnoffallmods()
    {
        for (int i = 0; i < 100; i++)
        {
            buttonsActive[i] = false;
        }
    }

    private static string[] buttons = new string[]
    {
        "disconnect [ud]", //0
		"join random priv [ud]", //1
		"join random pub [ud]", //2
		"platforms [ud]", //3
		"invis platforms [ud]", //4
		"fly [ud]", //5
		"big monke/long arms [ud]", //6
		"steal identity gun [ud]",//7
		"bigandsmall/size changer [cs]", //8
		"sticky platforms [ud]", //9
		"noclip [ud]", //10
		"not working", //11
		"fly and noclip [ud]", //12
		"beacons [ud]", //13
		"theme changer [ud]", //14
        "disable afk kick [ud]", //15
        "freeze monke [t] [ud]", //16
        "hitboxes [ud]", //17
		"no rotate platforms [ud]", //18
		"break gamemode [ud] [lm]", //19
		"insta tag [ud] [lm]", //20
		"rock game [ud] [lm]", //21
		"ghostcam [ud]", //22
		"trigger to invis [ud]", //23
		"no tag freeze [ud]", //24
		"teleport gun [d?]", //25
		"moon walk [ud]", //26
		"infection game [ud] [lm]", //27
		"trigger fly [ud]", //28
		"first person camera [aspect] [ud]", //29
		"trigger speed boost / mosa [nw]", //30
        "esp [ud]", //31
        "platform menu [ud]", //32
        "unreleased sweater [cs] [ud]", //33
        "plank platforms [ud]", //34
        "tag all [ud] [aspect]", //35
        "tag gun [ud] [aspect]", //36
        "bubble pop spam [m] [t] [ud]", //37
        "umbrella spam [m] [t] [ud]", //38
        "elf spam [m] [t] [ud]", //39
        "turkey spam [m] [t] [ud]", //40
        "take ownership [ud]", //41
        "untag gun [lm] [ud]", //42
        "no tag [ud]", //43
        "removed due to detected", //44
        "go to blue team [m] [d?]", //45
        "go to red team [m] [d?]", //46
        "save everyones ids to file [ud]", //47
        "disable rain [ud] [cs]", //48
        "removed due to USELESS", //49
        "disable quitbox [the thing that closes ur game] [ud]", //50
        "speed boost / mosa settings [nw]", //51
        "have a literal panic attack [ud]", //52
        "tag aura [ud] [aspect]", //53
        "car monk [t] [aspect] [ud]", //54
        "up and down [t] [ud]", //55
        "gun [hold grip then trigger] [m] [ud]", //56
        "mute gun [cs] [ud]", //57
        "report gun [ud]", //58
        "press down joystick to leave [ud]", //59
        "trigger platforms [ud]", //60
        "beacons for only untagged [ud]", //61
        "beacons for only tagged [ud]", //62
        "follow closest / random [ud]", //63
        "really fast turn speed / helicopter monke [ud]", //64
        "force tag lag [lm] [ud]", //65
        "remove all plats [t] [thatonemodder] [ud]", //66
        "plat gun [thatonemodder] [ud]", //67
        "enable pushtotalk [ud]", //68
        "remove leaves [cs] [nw]", //69
        "head spin [ud]", //70
        "trigger to toggle platforms [ud]", //71
        "panic [turns off all mods] [ud]", //72
        "removed due to detected", //73
        "jupiter walk [ud]", //74
        "roll monke [headspin but up] [ud]", //75
        "show playerid on player [nw]", //76
        "copy gun [nw]", //77
        "checkpoint [ud]", //78
        "scare gun / rig gun [ud]", //79
        "lag all [t] [ud]", //80
        "magnet gun [ud] [cs]", //81
        "download antimodchecker [ud]", //82
        "activate cosmetics mirror [cs] [ud]", //83
        "put rig under floor [ud]", //84
        "give slingshot self [ud]", //85
        "vibrate yourself [cs] [ud]", //86
        "steam long arms [ud]", //87
        "fake lag [t] [ud]", //88
        "draw [cs] [ud]", //89
        "make drawing bigger [ud]", //90
        "make drawing smaller [ud]", //91
        "destroy all drawings [t] [ud]", //92
        "swim anywhere [cs] [ud]", //93
        "walk on water [cs] [ud]", //94
        "disable water [cs] [ud]", //95
        "fix water [cs] [ud]", //96
        "ride edwardo [t] [ud]", //97
        "tp to player gun [d?]", //98
        "low graphics [ud]", //99
        "dont destroy menu [ud]", //100
        "loud hand taps [ud]", //101
        "no tap cooldown [ud]", //102
        "no vibrations on hand taps [ud]", //103
        "mute all [cs] [ud]", //104
        "report all [ud]", //105
        "custom plats [p] [ud]" //106
    };

    public static bool?[] buttonsActive = new bool?[]
    {
        false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false
    };

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
    private static System.Random random = new System.Random();
    private static bool gripDown;

    public static float scale2 = 1f;

    public static byte[] byte1;
    public static byte[] byte2;
    public static Texture2D texture;
    public static Texture2D texture2;

    public static bool dontdestroy = false;

    public static float vel = 0f;

    private static void ProcessCarMonke()
    {
        float max = 15f;
        bool trig = false;
        List<InputDevice> list = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list);
        list[0].TryGetFeatureValue(CommonUsages.triggerButton, out trig);
        if (trig)
        {
            if (vel < max)
            {
                vel += 0.1f;
            }
        }
        else if (vel > 0f)
        {
            vel -= 0.1f;
        }
        if (vel != 0f)
        {
            GorillaLocomotion.Player.Instance.transform.position += GorillaLocomotion.Player.Instance.bodyCollider.transform.forward * Time.deltaTime * vel;
        }
    }

    private static GameObject menu = null;

    public static bool yourmotheronce = true;

    private static GameObject canvasObj = null;

    private static GameObject reference = null;

    public static int framePressCooldown = 0;

    private static GameObject pointer = null;

    private static bool gravityToggled = false;

    private static bool flying = false;

    public static int btnCooldown = 0;

    public static GameObject treeroom;

    public static GameObject lowerlevel = GameObject.Find("Level/lower level");

    private static int soundCooldown = 0;

    private static float? maxJumpSpeed = null;

    private static float? jumpMultiplier = null;

    private static object index;

    public static int BlueMaterial = 5;

    public static int TransparentMaterial = 6;

    public static int LavaMaterial = 2;

    public static int RockMaterial = 1;

    public static int DefaultMaterial = 5;

    public static int NeonRed = 3;

    public static int RedTransparent = 4;

    public static int self = 0;

    private static Vector3? leftHandOffsetInitial = null;

    private static Vector3? rightHandOffsetInitial = null;

    private static float? maxArmLengthInitial = null;

    private static bool noClipDisabledOneshot = false;

    private static bool noClipEnabledAtLeastOnce = false;

    private static bool ghostToggle = false;

    private static bool bigMonkeyEnabled = false;

    private static int bigMonkeCooldown = 0;

    private static bool ghostMonkeEnabled = false;

    private static bool ghostMonkeAntiRepeat = false;

    public static int thememnumber = 0;

    public static Color maincolor = black;

    public static Color buttoncolor = black;

    public static bool animated = false;

    public static Color firstcolor = black;

    public static Color secondcolor = black;

    public static Color activatedcolor = purple;

    private static int ghostMonkeCooldown = 0;

    private static bool teleportGunAntiRepeat = false;

    private static Color colorRgbMonke = new Color(0f, 0f, 0f);

    private static float hueRgbMonke = 0f;

    private static void ProcessTagGun()
    {
        bool trig = false;
        bool grip = false;
        List<InputDevice> list = new List<InputDevice>();
        InputDevices.GetDevices(list);
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, list);
        list[0].TryGetFeatureValue(CommonUsages.triggerButton, out trig);
        list[0].TryGetFeatureValue(CommonUsages.gripButton, out grip);
        if (grip)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit) && theactualmenu.pointer == null)
            {
                theactualmenu.pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                UnityEngine.Object.Destroy(theactualmenu.pointer.GetComponent<Rigidbody>());
                UnityEngine.Object.Destroy(theactualmenu.pointer.GetComponent<SphereCollider>());
                theactualmenu.pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            }
            theactualmenu.pointer.transform.position = raycastHit.point;
            Photon.Realtime.Player owner = raycastHit.collider.GetComponentInParent<PhotonView>().Owner;
            if (trig)
            {
                GorillaTagger.Instance.myVRRig.enabled = false;
                GorillaTagger.Instance.myVRRig.transform.position = theactualmenu.FindVRRigForPlayer(owner).transform.position;
                PhotonView.Get(GorillaGameManager.instance.GetComponent<GorillaGameManager>()).RPC("ReportTagRPC", RpcTarget.MasterClient, new object[]
                {
                        owner
                });
                GorillaTagger.Instance.myVRRig.enabled = true;
            }
        }
        else
        {
            GorillaTagger.Instance.myVRRig.enabled = true;
            GameObject.Destroy(pointer);
        }
    }

    private static void playergun()
    {
        bool trig = false;
        bool grip = false;
        List<InputDevice> list = new List<InputDevice>();
        InputDevices.GetDevices(list);
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, list);
        list[0].TryGetFeatureValue(CommonUsages.triggerButton, out trig);
        list[0].TryGetFeatureValue(CommonUsages.gripButton, out grip);
        if (grip)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit) && theactualmenu.pointer == null)
            {
                theactualmenu.pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                UnityEngine.Object.Destroy(theactualmenu.pointer.GetComponent<Rigidbody>());
                UnityEngine.Object.Destroy(theactualmenu.pointer.GetComponent<SphereCollider>());
                theactualmenu.pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            }
            theactualmenu.pointer.transform.position = raycastHit.point;
            Photon.Realtime.Player owner = raycastHit.collider.GetComponentInParent<PhotonView>().Owner;
            if (trig)
            {
                GorillaLocomotion.Player.Instance.transform.position = FindVRRigForPlayer(owner).transform.position;
            }
        }
        else
        {
            GorillaTagger.Instance.myVRRig.enabled = true;
            GameObject.Destroy(pointer);
        }
    }

    private static void scaregun()
    {
        bool trig = false;
        bool grip = false;
        List<InputDevice> list = new List<InputDevice>();
        InputDevices.GetDevices(list);
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, list);
        list[0].TryGetFeatureValue(CommonUsages.triggerButton, out trig);
        list[0].TryGetFeatureValue(CommonUsages.gripButton, out grip);
        if (grip)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit) && theactualmenu.pointer == null)
            {
                theactualmenu.pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                UnityEngine.Object.Destroy(theactualmenu.pointer.GetComponent<Rigidbody>());
                UnityEngine.Object.Destroy(theactualmenu.pointer.GetComponent<SphereCollider>());
                theactualmenu.pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            }
            theactualmenu.pointer.transform.position = raycastHit.point;
            if (trig)
            {
                GorillaTagger.Instance.myVRRig.enabled = false;
                GorillaTagger.Instance.myVRRig.transform.position = pointer.transform.position;
            }
        }
        else
        {
            GorillaTagger.Instance.myVRRig.enabled = true;
            GameObject.Destroy(pointer);
        }
    }

    private static float timerRgbMonke = 0f;

    private static float updateRateRgbMonke = 0f;

    private static float updateTimerRgbMonke = 0f;

    private static bool flag2 = false;

    private static bool flag1 = true;

    private static Vector3 scale = new Vector3(0.0125f, 0.28f, 0.3825f);

    private static bool gripDown_left;

    private static bool gripDown_right;

    private static bool once_left;

    private static bool once_right;

    private static bool once_left_false;

    private static bool once_right_false;

    private static bool ghostToggled;

    private static bool once_networking;

    private static GameObject[] jump_left_network = new GameObject[9999];

    private static GameObject[] jump_right_network = new GameObject[9999];

    private static GameObject jump_left_local = null;

    private static GameObject jump_right_local = null;

    private static GradientColorKey[] colorKeysPlatformMonke = new GradientColorKey[4];

    private static Vector3? checkpointPos;

    private static bool checkpointTeleportAntiRepeat = false;

    private static bool foundPlayer = false;

    private static int btnTagSoundCooldown = 0;

    private static float timeSinceLastChange = 0f;

    private static float myVarY1 = 0f;

    public static bool triggerpress = false;

    private static float myVarY2 = 0f;

    private static bool gain = false;

    private static bool less = false;

    private static bool reset = false;

    private static bool hasDummyProjectile;

    private static GameObject dummyProjectile;

    private static bool fastr = false;

    private static bool speed1 = true;

    private static float gainSpeed = 1f;

    private static int pageSize = 7;

    private static int pageNumber = 0;

    public static bool gripDownactual = false;

    public static bool leftresetbutton = false;

    public static bool leftsecondarybutton = false;

    public static bool righttriggerpress = false;

    public static bool leftgrippress = false;

    public static bool leftprimarypress = false;

    public static bool flipped = false;

    public static bool leftthumbstickclick = false;

    public static string githubversion = null;

    public static bool rightsecondarybutton = false;

    public static bool didthethingwebhook = false;

    public static bool righthand = false;

    public static string currentverison = "9.0";

    public static bool lefttriggerpress = false;

    public static bool changedboards = false;

    public static WebClient webClient;

    private static void Prefix()
    {
        try
        {
            if (!File.Exists("usedshibaz.txt"))
            {
                Process.Start("https://discord.gg/shibagtmodmenu");
                Process.Start("https://youtube.com/@shibagtag/");
                File.WriteAllText("usedshibaz.txt", "uh yes thanks very for use! delete if you wanna see my discord and youtube again hehe\n- mr shibe");
            }
            if (!maxArmLengthInitial.HasValue)
            {
                maxArmLengthInitial = GorillaLocomotion.Player.Instance.maxArmLength;
                leftHandOffsetInitial = GorillaLocomotion.Player.Instance.leftHandOffset;
                rightHandOffsetInitial = GorillaLocomotion.Player.Instance.rightHandOffset;
            }
            List<InputDevice> list = new List<InputDevice>();
            List<InputDevice> list2 = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, list);
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list2);
            InputDevices.GetDeviceAtXRNode(lNode).TryGetFeatureValue(CommonUsages.triggerButton, out lefttriggerpress);
            InputDevices.GetDeviceAtXRNode(lNode).TryGetFeatureValue(CommonUsages.secondaryButton, out leftresetbutton);
            InputDevices.GetDeviceAtXRNode(lNode).TryGetFeatureValue(CommonUsages.secondaryButton, out leftsecondarybutton);
            InputDevices.GetDeviceAtXRNode(lNode).TryGetFeatureValue(CommonUsages.primaryButton, out leftprimarypress);
            InputDevices.GetDeviceAtXRNode(lNode).TryGetFeatureValue(CommonUsages.gripButton, out leftgrippress);
            InputDevices.GetDeviceAtXRNode(lNode).TryGetFeatureValue(CommonUsages.primary2DAxisClick, out leftthumbstickclick);
            InputDevices.GetDeviceAtXRNode(rNode).TryGetFeatureValue(CommonUsages.secondaryButton, out rightsecondarybutton);
            InputDevices.GetDeviceAtXRNode(rNode).TryGetFeatureValue(CommonUsages.gripButton, out rightgrippress);
            InputDevices.GetDeviceAtXRNode(rNode).TryGetFeatureValue(CommonUsages.triggerButton, out righttriggerpress);
            list[0].TryGetFeatureValue(CommonUsages.secondaryButton, out gripDown);
            list[0].TryGetFeatureValue(CommonUsages.gripButton, out gripDownactual);
            if (gripDown && menu == null && !righthand) //left
            {
                Draw();
                if (reference == null)
                {
                    reference = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    reference.transform.parent = GorillaLocomotion.Player.Instance.rightControllerTransform;
                    reference.transform.localPosition = new Vector3(0f, -0.1f, 0f);
                    reference.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                    reference.GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 0, 0));
                }
            }
            else if (!gripDown && menu != null && !dontdestroy && !righthand) //left
            {
                UnityEngine.Object.Destroy(menu);
                menu = null;
                UnityEngine.Object.Destroy(reference);
                reference = null;

            }
            if (gripDown && menu != null && !righthand) //left
            {
                menu.transform.position = GorillaLocomotion.Player.Instance.leftControllerTransform.position;
                if (flipped)
                {
                    menu.transform.RotateAround(menu.transform.position, menu.transform.forward, 180f);
                }
                else
                {
                    menu.transform.rotation = GorillaLocomotion.Player.Instance.leftControllerTransform.rotation;
                }
            }


            //BEGIN RIGHT


            if (rightsecondarybutton && menu == null && righthand) //right
            {
                Draw();
                if (reference == null)
                {
                    reference = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    reference.transform.parent = GorillaLocomotion.Player.Instance.leftControllerTransform;
                    reference.transform.localPosition = new Vector3(0f, -0.1f, 0f);
                    reference.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                    reference.GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 0, 0));
                }
            }
            else if (!rightsecondarybutton && menu != null && !dontdestroy && righthand) //right
            {
                UnityEngine.Object.Destroy(menu);
                menu = null;
                UnityEngine.Object.Destroy(reference);
                reference = null;

            }
            if (rightsecondarybutton && menu != null && righthand) //right
            {
                menu.transform.position = GorillaLocomotion.Player.Instance.rightControllerTransform.position;
                if (flipped)
                {
                    menu.transform.RotateAround(menu.transform.position, menu.transform.forward, 180f);
                }
                else
                {
                    menu.transform.rotation = GorillaLocomotion.Player.Instance.rightControllerTransform.rotation;
                    menu.transform.RotateAround(menu.transform.position, menu.transform.forward, 180f);
                }
            }
            if (githubversion == currentverison && githubversion != null)
            {
                if (buttonsActive[0] == true)
                {
                    PhotonNetwork.Disconnect();
                    buttonsActive[0] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (buttonsActive[1] == true)
                {
                    PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(RandomString(4));
                    buttonsActive[1] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (buttonsActive[2] == true)
                {
                    PhotonNetwork.JoinRandomRoom();
                    buttonsActive[2] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (buttonsActive[3] == true)
                {
                    ProcessPlatformMonke();
                }
                if (buttonsActive[4] == true)
                {
                    ProcessInvisPlatformMonke();
                }
                if (buttonsActive[5] == true)
                {
                    bool sec = false;
                    list = new List<InputDevice>();
                    InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list);
                    list[0].TryGetFeatureValue(CommonUsages.secondaryButton, out sec);
                    if (!sec)
                    {
                        if (theactualmenu.flying)
                        {
                            GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = GorillaLocomotion.Player.Instance.headCollider.transform.forward * Time.deltaTime * 26f;
                            theactualmenu.flying = false;
                        }
                    }
                    else
                    {
                        GorillaLocomotion.Player.Instance.transform.position += GorillaLocomotion.Player.Instance.headCollider.transform.forward * Time.deltaTime * 20f;
                        GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        if (!theactualmenu.flying)
                        {
                            theactualmenu.flying = true;
                        }
                    }
                }
                if (buttonsActive[6] == true)
                {
                    GorillaLocomotion.Player.Instance.transform.localScale = new Vector3(2f, 2f, 2f);
                }
                else
                {
                    if (buttonsActive[8] == false && buttonsActive[87] == false)
                    {
                        GorillaLocomotion.Player.Instance.transform.localScale = new Vector3(1f, 1f, 1f);
                    }
                }
                if (buttonsActive[7] == true)
                {
                    stealgun();
                }
                if (buttonsActive[8] == true)
                {
                    SizeManager sizeManager;
                    GorillaLocomotion.Player.Instance.TryGetComponent<SizeManager>(out sizeManager);
                    InputDevices.GetDeviceAtXRNode(theactualmenu.lNode).TryGetFeatureValue(CommonUsages.secondaryButton, out theactualmenu.resetbutton);
                    InputDevices.GetDeviceAtXRNode(theactualmenu.rNode).TryGetFeatureValue(CommonUsages.triggerButton, out theactualmenu.triggerpress2);
                    bool flag24 = theactualmenu.resetbutton;
                    bool flag25 = flag24;
                    if (flag25)
                    {
                        sizeManager.enabled = false;
                        GorillaLocomotion.Player.Instance.scale = 1f;
                        theactualmenu.monkescale = 1f;
                        theactualmenu.stopgrowing = true;
                    }
                    bool flag26 = theactualmenu.lefttriggerpress;
                    bool flag27 = flag26;
                    if (flag27)
                    {
                        theactualmenu.stopgrowing = false;
                        bool flag28 = !theactualmenu.stopgrowing;
                        bool flag29 = flag28;
                        if (flag29)
                        {
                            sizeManager.enabled = false;
                            theactualmenu.monkescale += 0.1f;
                            GorillaLocomotion.Player.Instance.scale = theactualmenu.monkescale;
                        }
                    }
                    bool flag30 = theactualmenu.triggerpress2;
                    bool flag31 = flag30;
                    if (flag31)
                    {
                        theactualmenu.stopgrowing = false;
                        bool flag32 = !theactualmenu.stopgrowing;
                        bool flag33 = flag32;
                        if (flag33)
                        {
                            sizeManager.enabled = false;
                            bool flag34 = (double)(GorillaLocomotion.Player.Instance.scale - 0.1f) >= 0.01;
                            bool flag35 = flag34;
                            if (flag35)
                            {
                                theactualmenu.monkescale -= 0.1f;
                            }
                            else
                            {
                                bool flag36 = (double)GorillaLocomotion.Player.Instance.scale - 0.02 <= 0.02 || (double)GorillaLocomotion.Player.Instance.scale - 0.02 == 0.02;
                                bool flag37 = !flag36;
                                if (flag37)
                                {
                                    theactualmenu.monkescale -= 0.01f;
                                }
                            }
                            GorillaLocomotion.Player.Instance.scale = theactualmenu.monkescale;
                        }
                    }
                    bool flag38 = !theactualmenu.lefttriggerpress && !theactualmenu.lefttriggerpress && !theactualmenu.resetbutton;
                    bool flag39 = flag38;
                    if (flag39)
                    {
                        theactualmenu.stopgrowing = true;
                    }
                }
                else
                {
                    if (theactualmenu.buttonsActive[6] == false)
                    {
                        GorillaLocomotion.Player.Instance.scale = 1f;
                        theactualmenu.monkescale = 1f;
                    }
                    theactualmenu.stopgrowing = true;
                }
                if (buttonsActive[9] == true)
                {
                    ProcessStickyPlatforms();
                }
                if (buttonsActive[10] == true)
                {
                    ProcessNoClip();
                }
                if (buttonsActive[11] == true)
                {

                }
                if (buttonsActive[12] == true)
                {
                    bool trig = false;
                    bool sec = false;
                    list = new List<InputDevice>();
                    InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list);
                    list[0].TryGetFeatureValue(CommonUsages.triggerButton, out trig);
                    list[0].TryGetFeatureValue(CommonUsages.secondaryButton, out sec);
                    if (!trig)
                    {
                        if (theactualmenu.flying)
                        {
                            GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = GorillaLocomotion.Player.Instance.headCollider.transform.forward * Time.deltaTime * 26f;
                            foreach (MeshCollider meshCollider2 in Resources.FindObjectsOfTypeAll<MeshCollider>())
                            {
                                meshCollider2.transform.localScale = meshCollider2.transform.localScale * 10000f;
                            }
                            theactualmenu.flying = false;
                        }
                    }
                    else
                    {
                        GorillaLocomotion.Player.Instance.transform.position += GorillaLocomotion.Player.Instance.headCollider.transform.forward * Time.deltaTime * 20f;
                        GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        if (!theactualmenu.flying)
                        {
                            foreach (MeshCollider meshCollider2 in Resources.FindObjectsOfTypeAll<MeshCollider>())
                            {
                                meshCollider2.transform.localScale = meshCollider2.transform.localScale / 10000f;
                            }
                            theactualmenu.flying = true;
                        }
                    }
                }
                if (buttonsActive[13] == true)
                {
                    foreach (VRRig vrrig in (VRRig[])UnityEngine.Object.FindObjectsOfType(typeof(VRRig)))
                    {
                        if (!vrrig.isOfflineVRRig)
                        {
                            if (!vrrig.isMyPlayer)
                            {
                                if (!vrrig.photonView.IsMine)
                                {
                                    GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                                    UnityEngine.Object.Destroy(gameObject2.GetComponent<BoxCollider>());
                                    UnityEngine.Object.Destroy(gameObject2.GetComponent<Rigidbody>());
                                    UnityEngine.Object.Destroy(gameObject2.GetComponent<Collider>());
                                    gameObject2.transform.rotation = Quaternion.identity;
                                    gameObject2.transform.localScale = new Vector3(0.04f, 200f, 0.04f);
                                    gameObject2.transform.position = vrrig.transform.position;
                                    gameObject2.GetComponent<MeshRenderer>().material = vrrig.mainSkin.material;
                                    UnityEngine.Object.Destroy(gameObject2, Time.deltaTime);
                                    UnityEngine.Object.Destroy(gameObject2, Time.deltaTime);
                                }
                            }
                        }
                    }
                }
                if (buttonsActive[14] == true)
                {
                    thememnumber++;
                    if (thememnumber == 5)
                    {
                        thememnumber = 0;
                        maincolor = black;
                        buttoncolor = black;
                        activatedcolor = purple;
                        shibaimage = false;
                        binaryimage = false;
                    } //change to main theme and 0
                    if (thememnumber == 0)
                    {
                        maincolor = Color.black;
                        buttoncolor = Color.black;
                        activatedcolor = purple;
                        shibaimage = false;
                        binaryimage = false;
                    } //main theme
                    if (thememnumber == 1)
                    {
                        maincolor = purple;
                        buttoncolor = Color.black;
                        activatedcolor = purple;
                        shibaimage = false;
                        binaryimage = false;
                    } //original theme
                    if (thememnumber == 2)
                    {
                        maincolor = Color.white;
                        buttoncolor = Color.white;
                        activatedcolor = Color.grey;
                        shibaimage = false;
                        binaryimage = false;
                    } //white
                    if (thememnumber == 3)
                    {
                        maincolor = Color.black;
                        buttoncolor = Color.black;
                        activatedcolor = purple;
                        shibaimage = true;
                        binaryimage = false;
                    } //shiba
                    if (thememnumber == 4)
                    {
                        maincolor = Color.black;
                        buttoncolor = Color.black;
                        activatedcolor = Color.green;
                        shibaimage = false;
                        binaryimage = true;
                    } //binary
                    buttonsActive[14] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (buttonsActive[15] == true)
                {
                    disableafk();
                }
                if (buttonsActive[16] == true)
                {
                    if (lefttriggerpress)
                    {
                        GorillaTagger.Instance.myVRRig.enabled = false;
                        GorillaTagger.Instance.myVRRig.transform.position = GorillaLocomotion.Player.Instance.transform.position;
                    }
                    else
                    {
                        GorillaTagger.Instance.myVRRig.enabled = true;
                    }
                }
                if (buttonsActive[17] == true)
                {
                    {
                        Material material = new Material(Shader.Find("GUI/Text Shader"));

                        foreach (VRRig vrrig in (VRRig[])UnityEngine.Object.FindObjectsOfType(typeof(VRRig)))
                        {
                            if (!vrrig.isOfflineVRRig && !vrrig.isMyPlayer && !vrrig.photonView.IsMine)
                            {
                                GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                                UnityEngine.Object.Destroy(gameObject.GetComponent<BoxCollider>());
                                UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
                                UnityEngine.Object.Destroy(gameObject.GetComponent<Collider>());
                                gameObject.transform.rotation = Quaternion.identity;
                                gameObject.GetComponent<MeshRenderer>().material = vrrig.mainSkin.material;
                                gameObject.transform.localScale = new Vector3(0.3f, 0.6f, 0.3f);
                                gameObject.transform.position = vrrig.headMesh.transform.position;
                                UnityEngine.Object.Destroy(gameObject, Time.deltaTime);
                            }
                        }
                    }
                }
                if (buttonsActive[18] == true)
                {
                    norotateplats();
                }
                if (buttonsActive[19] == true)
                {
                    new Thread(yourmotherdidsomethinginthebackofyourcarthatdoesntexistwhichisverysussybecauseyouhavenoideaofwhatyoudololgumthedevisverysussyandidontknowwhatheisdoinginthebackofthecarsadlyyourmotherisalsothereaslongwithshibabecauseshibaistheretheycantdoanythingsussybutshibadoessomethingsussyfirstandhesayshiiamgoingtodosomethingsussytoyouandshibaputshishandsinsideofgumthedevspantsandgetssomethingoutfromgumspocketnothingelsejustthepocketbutthenthethinginthepocketwasasussothengumthedev).Start();
                }
                else
                {
                    new Thread(yourmotherdidsomethinginthebackofyourcarthatdoesntexistwhichisverysussybecauseyouhavenoideaofwhatyoudololgumthedevisverysussyandidontknowwhatheisdoinginthebackofthecarsadlyyourmotherisalsothereaslongwithshibabecauseshibaistheretheycantdoanythingsussybutshibadoessomethingsussyfirstandhesayshiiamgoingtodosomethingsussytoyouandshibaputshishandsinsideofgumthedevspantsandgetssomethingoutfromgumspocketnothingelsejustthepocketbutthenthethinginthepocketwasasussothengumthedev).Abort();
                }
                if (buttonsActive[20] == true)
                {
                    foreach (GorillaTagManager manager in UnityEngine.Object.FindObjectsOfType<GorillaTagManager>())
                    {
                        manager.checkCooldown = 0;
                        manager.tagCoolDown = 0;
                    }
                    buttonsActive[20] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (buttonsActive[21] == true)
                {
                    foreach (GorillaTagManager manager in UnityEngine.Object.FindObjectsOfType<GorillaTagManager>())
                    {
                        manager.infectedModeThreshold = 11;
                    }
                    buttonsActive[21] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (buttonsActive[22] == true)
                {
                    if (rightsecondarybutton)
                    {
                        if (!theactualmenu.ghostToggled && GorillaTagger.Instance.myVRRig.enabled)
                        {
                            GorillaTagger.Instance.myVRRig.enabled = false;
                            theactualmenu.ghostToggled = true;
                        }
                        else
                        {
                            if (!theactualmenu.ghostToggled && !GorillaTagger.Instance.myVRRig.enabled)
                            {
                                GorillaTagger.Instance.myVRRig.enabled = true;
                                theactualmenu.ghostToggled = true;
                            }
                        }
                    }
                    else
                    {
                        theactualmenu.ghostToggled = false;
                    }
                }
                if (buttonsActive[23] == true)
                {
                    bool trig = false;
                    bool sec = false;
                    list = new List<InputDevice>();
                    InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, list);
                    list[0].TryGetFeatureValue(CommonUsages.triggerButton, out trig);
                    list[0].TryGetFeatureValue(CommonUsages.secondaryButton, out sec);
                    if (!trig)
                    {
                        GorillaTagger.Instance.myVRRig.enabled = true;
                    }
                    else
                    {
                        GorillaTagger.Instance.myVRRig.transform.position = new Vector3(200f, 200f, 200f);
                        GorillaTagger.Instance.myVRRig.enabled = false;
                    }
                }
                if (buttonsActive[24] == true)
                {
                    GorillaLocomotion.Player.Instance.disableMovement = false;
                }
                if (buttonsActive[25] == true)
                {
                    ProcessTeleportGun();
                }
                if (buttonsActive[26] == true)
                {
                    Time.timeScale = 0.5f;
                }
                else
                {
                    if (buttonsActive[74] == false)
                    {
                        Time.timeScale = 1f;
                    }
                }
                if (buttonsActive[27] == true)
                {

                    foreach (GorillaTagManager manager in UnityEngine.Object.FindObjectsOfType<GorillaTagManager>())
                    {
                        manager.infectedModeThreshold = 1;
                    }
                    buttonsActive[27] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();

                }
                if (buttonsActive[28] == true)
                {
                    bool trig = false;
                    bool sec = false;
                    list = new List<InputDevice>();
                    InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list);
                    list[0].TryGetFeatureValue(CommonUsages.triggerButton, out trig);
                    list[0].TryGetFeatureValue(CommonUsages.secondaryButton, out sec);
                    if (!trig)
                    {
                        if (theactualmenu.flying)
                        {
                            GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = GorillaLocomotion.Player.Instance.headCollider.transform.forward * Time.deltaTime * 26f;
                            theactualmenu.flying = false;
                        }
                    }
                    else
                    {
                        GorillaLocomotion.Player.Instance.transform.position += GorillaLocomotion.Player.Instance.headCollider.transform.forward * Time.deltaTime * 20f;
                        GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        if (!theactualmenu.flying)
                        {
                            theactualmenu.flying = true;
                        }
                    }
                }
                if (buttonsActive[29] == true)
                {
                    GameObject gameObject = GameObject.Find("Shoulder Camera");
                    gameObject.GetComponent<Camera>().transform.position = new Vector3(0f, 0.10f, 0) + GorillaTagger.Instance.offlineVRRig.transform.position;
                    gameObject.GetComponent<Camera>().fieldOfView = 88f;
                }
                else
                {
                    GameObject gameObject2 = GameObject.Find("Shoulder Camera");
                    gameObject2.GetComponent<Camera>().fieldOfView = 60f;
                }
                if (buttonsActive[30] == true)
                {

                }
                if (buttonsActive[31] == true)
                {
                    buttonsActive[31] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                    homemadeesp();
                }
                if (buttonsActive[32] == true)
                {
                    flipped = true;
                }
                else
                {
                    flipped = false;
                }
                if (buttonsActive[33] == true)
                {
                    GameObject.Find("Global/Local VRRig/Local Gorilla Player/rig/body/WinterJan2023 Body/LBACP.").SetActive(true);
                }
                else
                {
                    GameObject.Find("Global/Local VRRig/Local Gorilla Player/rig/body/WinterJan2023 Body/LBACP.").SetActive(false);
                }
                if (buttonsActive[34] == true)
                {
                    ProcessPlankPlatformMonke();
                }
                if (buttonsActive[35] == true)
                {
                    ProcessTagAll();
                }
                if (buttonsActive[36] == true)
                {
                    ProcessTagGun();
                }
                if (buttonsActive[37] == true)
                {
                    if (lefttriggerpress)
                    {
                        PlaySoundInteger(84);
                    }
                }
                if (buttonsActive[38] == true)
                {
                    if (lefttriggerpress)
                    {
                        PlaySoundInteger(65);
                    }
                }
                if (buttonsActive[39] == true)
                {
                    if (lefttriggerpress)
                    {
                        PlaySoundInteger(UnityEngine.Random.Range(140, 141));
                    }
                }
                if (buttonsActive[40] == true)
                {
                    if (lefttriggerpress)
                    {
                        PlaySoundInteger(83);
                    }
                }
                if (buttonsActive[41] == true)
                {
                    GorillaNot.instance.photonView.RequestOwnership();
                    GorillaNot.instance.photonView.RequestOwnership();
                    GorillaNot.instance.photonView.RequestOwnership();
                    GorillaNot.instance.photonView.RequestOwnership();
                    GorillaNot.instance.photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
                    GorillaNot.instance.photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
                    GorillaNot.instance.photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
                    GorillaNot.instance.photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
                    GorillaNot.instance.photonView.AmOwner.Equals(PhotonNetwork.LocalPlayer);
                    GorillaNot.instance.photonView.AmOwner.Equals(PhotonNetwork.LocalPlayer);
                    GorillaNot.instance.photonView.AmOwner.Equals(PhotonNetwork.LocalPlayer);
                    GorillaNot.instance.photonView.AmOwner.Equals(PhotonNetwork.LocalPlayer);
                    GorillaGameManager.instance.photonView.OwnerActorNr = 1;
                    GorillaGameManager.instance.photonView.OwnerActorNr = 1;
                    GorillaGameManager.instance.photonView.OwnerActorNr = 1;
                    GorillaGameManager.instance.photonView.OwnerActorNr = 1;
                }
                if (buttonsActive[42] == true)
                {
                    bool trig = false;
                    bool grip = false;
                    List<InputDevice> list8 = new List<InputDevice>();
                    InputDevices.GetDevices(list);
                    InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list);
                    list[0].TryGetFeatureValue(CommonUsages.triggerButton, out trig);
                    list[0].TryGetFeatureValue(CommonUsages.gripButton, out grip);
                    if (grip)
                    {
                        RaycastHit raycastHit4;
                        Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit4);
                        if (theactualmenu.pointer == null)
                        {
                            theactualmenu.pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                            UnityEngine.Object.Destroy(theactualmenu.pointer.GetComponent<Rigidbody>());
                            UnityEngine.Object.Destroy(theactualmenu.pointer.GetComponent<SphereCollider>());
                            theactualmenu.pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                        }
                        theactualmenu.pointer.transform.position = raycastHit4.point;
                        new Color(0f, 0f, 0f);
                        PhotonView componentInParent2 = raycastHit4.collider.GetComponentInParent<PhotonView>();
                        if (componentInParent2 != null && PhotonNetwork.LocalPlayer != componentInParent2.Owner)
                        {
                            GorillaTagger.Instance.StartVibration(true, GorillaTagger.Instance.tagHapticStrength, GorillaTagger.Instance.tagHapticDuration);
                            GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.tagHapticStrength, GorillaTagger.Instance.tagHapticDuration);
                            Photon.Realtime.Player owner2 = componentInParent2.Owner;
                            if (trig)
                            {
                                theactualmenu.pointer.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                            }
                            if (!trig)
                            {
                                theactualmenu.pointer.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                            }
                            if (trig)
                            {
                                theactualmenu.pointer.GetComponent<Renderer>().material.SetColor("_Color", Color.magenta);
                                foreach (GorillaTagManager gorillaTagManager8 in UnityEngine.Object.FindObjectsOfType<GorillaTagManager>())
                                {
                                    gorillaTagManager8.currentInfected.Remove(owner2);
                                    gorillaTagManager8.currentInfected.Remove(owner2);
                                    gorillaTagManager8.currentInfected.Remove(owner2);
                                    gorillaTagManager8.currentInfected.Remove(owner2);
                                    gorillaTagManager8.currentInfected.Remove(owner2);
                                }
                            }
                        }
                    }
                }
                if (buttonsActive[43] == true)
                {
                    notag();
                }
                if (buttonsActive[44] == true)
                {
                    
                }
                if (buttonsActive[45] == true)
                {
                    foreach (GorillaBattleManager gorillaBattleManager in UnityEngine.Object.FindObjectsOfType<GorillaBattleManager>())
                    {
                        gorillaBattleManager.AddPlayerToCorrectTeam(PhotonNetwork.LocalPlayer);
                        gorillaBattleManager.OnBlueTeam(PhotonNetwork.LocalPlayer);
                        gorillaBattleManager.OnBlueTeam(PhotonNetwork.LocalPlayer);
                        gorillaBattleManager.OnBlueTeam(PhotonNetwork.LocalPlayer);
                        gorillaBattleManager.OnBlueTeam(PhotonNetwork.LocalPlayer);
                        gorillaBattleManager.playerLivesArray = new int[3];
                        gorillaBattleManager.testAssault = false;
                    }
                }
                if (buttonsActive[46] == true)
                {
                    foreach (GorillaBattleManager gorillaBattleManager2 in UnityEngine.Object.FindObjectsOfType<GorillaBattleManager>())
                    {
                        gorillaBattleManager2.AddPlayerToCorrectTeam(PhotonNetwork.LocalPlayer);
                        gorillaBattleManager2.OnRedTeam(PhotonNetwork.LocalPlayer);
                        gorillaBattleManager2.OnRedTeam(PhotonNetwork.LocalPlayer);
                        gorillaBattleManager2.OnRedTeam(PhotonNetwork.LocalPlayer);
                        gorillaBattleManager2.OnRedTeam(PhotonNetwork.LocalPlayer);
                        gorillaBattleManager2.playerLivesArray = new int[3];
                        gorillaBattleManager2.testAssault = false;
                    }
                }
                if (buttonsActive[47] == true)
                {
                    foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
                    {
                        ids = ids + "\nname: " + player.NickName + " \nid: " + player.UserId + "\n";
                    }
                    File.WriteAllText("ids.txt", ids);
                    if (!opened)
                    {
                        Process.Start("ids.txt");
                        opened = true;
                    }
                    buttonsActive[47] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                else
                {
                    opened = false;
                }
                if (buttonsActive[48] == true)
                {
                    GameObject.Find("Level/forest/ForestObjects/WeatherDayNight/rain").SetActive(false);
                }
                if (buttonsActive[49] == true)
                {
                    
                }
                else
                {
                    opened = false;
                }
                if (buttonsActive[50] == true)
                {
                    GameObject.Find("Global/NetworkTriggers/QuitBox").SetActive(false);
                }
                if (buttonsActive[51] == true)
                {

                }
                if (buttonsActive[52] == true)
                {
                    System.Random random = new System.Random();
                    if (PhotonNetwork.InRoom)
                    {
                        GorillaTagger.Instance.myVRRig.head.rigTarget.eulerAngles = new Vector3(random.Next(0, 360), random.Next(0, 360), random.Next(0, 360));
                        GorillaTagger.Instance.myVRRig.leftHand.rigTarget.eulerAngles = new Vector3(random.Next(0, 360), random.Next(0, 360), random.Next(0, 360));
                        GorillaTagger.Instance.myVRRig.rightHand.rigTarget.eulerAngles = new Vector3(random.Next(0, 360), random.Next(0, 360), random.Next(0, 360));
                    }
                    else
                    {
                        GorillaTagger.Instance.offlineVRRig.head.rigTarget.eulerAngles = new Vector3(random.Next(0, 360), random.Next(0, 360), random.Next(0, 360));
                        GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.eulerAngles = new Vector3(random.Next(0, 360), random.Next(0, 360), random.Next(0, 360));
                        GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.eulerAngles = new Vector3(random.Next(0, 360), random.Next(0, 360), random.Next(0, 360));
                    }

                }
                if (buttonsActive[53] == true)
                {
                    ProcessTagAura();
                }
                if (buttonsActive[54] == true)
                {
                    ProcessCarMonke();
                }
                if (buttonsActive[55] == true)
                {
                    
                        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.triggerButton, out theactualmenu.up);
                        if (theactualmenu.up)
                        {
                            GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 250f, 0f), ForceMode.Acceleration);
                        }
                        InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.triggerButton, out theactualmenu.down);
                        if (theactualmenu.down)
                        {
                            GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().AddForce(new Vector3(0f, -250f, 0f), ForceMode.Acceleration);
                        }
                    
                }
                if (buttonsActive[56] == true)
                {
                    literalgun();
                }
                if (buttonsActive[57] == true)
                {
                    mutegun();
                }
                if (buttonsActive[58] == true)
                {
                    reportgun();
                }
                if (buttonsActive[59] == true)
                {
                    bool joystickpress = false;
                    InputDevices.GetDeviceAtXRNode(lNode).TryGetFeatureValue(CommonUsages.primary2DAxisClick, out joystickpress);
                    if (joystickpress)
                    {
                        PhotonNetwork.Disconnect();
                    }
                }
                if (buttonsActive[60] == true)
                {
                    triggerplats();
                }
                if (buttonsActive[61] == true)
                {
                    foreach (VRRig vrrig in (VRRig[])UnityEngine.Object.FindObjectsOfType(typeof(VRRig)))
                    {
                        if (!vrrig.isOfflineVRRig && !vrrig.isMyPlayer)
                        {
                            foreach (GorillaTagManager manager in UnityEngine.Object.FindObjectsOfType<GorillaTagManager>())
                            {
                                if (!vrrig.photonView.IsMine && manager.currentInfected.Contains(vrrig.photonView.Controller))
                                {
                                    GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                                    UnityEngine.Object.Destroy(gameObject2.GetComponent<BoxCollider>());
                                    UnityEngine.Object.Destroy(gameObject2.GetComponent<Rigidbody>());
                                    UnityEngine.Object.Destroy(gameObject2.GetComponent<Collider>());
                                    gameObject2.transform.rotation = Quaternion.identity;
                                    gameObject2.transform.localScale = new Vector3(0.04f, 200f, 0.04f);
                                    gameObject2.transform.position = vrrig.transform.position;
                                    gameObject2.GetComponent<MeshRenderer>().material = vrrig.mainSkin.material;
                                    UnityEngine.Object.Destroy(gameObject2, Time.deltaTime);
                                }
                            }
                        }
                    }
                }
                if (buttonsActive[62] == true)
                {
                    foreach (VRRig vrrig in (VRRig[])UnityEngine.Object.FindObjectsOfType(typeof(VRRig)))
                    {
                        if (!vrrig.isOfflineVRRig && !vrrig.isMyPlayer)
                        {
                            foreach (GorillaTagManager manager in UnityEngine.Object.FindObjectsOfType<GorillaTagManager>())
                            {
                                if (!vrrig.photonView.IsMine && !manager.currentInfected.Contains(vrrig.photonView.Controller))
                                {
                                    GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                                    UnityEngine.Object.Destroy(gameObject2.GetComponent<BoxCollider>());
                                    UnityEngine.Object.Destroy(gameObject2.GetComponent<Rigidbody>());
                                    UnityEngine.Object.Destroy(gameObject2.GetComponent<Collider>());
                                    gameObject2.transform.rotation = Quaternion.identity;
                                    gameObject2.transform.localScale = new Vector3(0.04f, 200f, 0.04f);
                                    gameObject2.transform.position = vrrig.transform.position;
                                    gameObject2.GetComponent<MeshRenderer>().material = vrrig.mainSkin.material;
                                    UnityEngine.Object.Destroy(gameObject2, Time.deltaTime);
                                }
                            }
                        }
                    }
                }
                if (buttonsActive[63] == true)
                {
                    followgunnoway();
                }
                if (buttonsActive[64] == true)
                {
                    foreach (UnityEngine.XR.Interaction.Toolkit.GorillaSnapTurn unitybro2 in (UnityEngine.XR.Interaction.Toolkit.GorillaSnapTurn[])UnityEngine.Object.FindObjectsOfType(typeof(UnityEngine.XR.Interaction.Toolkit.GorillaSnapTurn)))
                    {
                        unitybro2.turnSpeed = 9999;
                        unitybro2.ChangeTurnMode("SMOOTH", 9999);
                    }
                }
                if (buttonsActive[65] == true)
                {
                    if (hasbeenenabled == false)
                    {
                        hasbeenenabled = true;
                        foreach (GorillaTagManager manager in UnityEngine.Object.FindObjectsOfType<GorillaTagManager>())
                        {
                            manager.checkCooldown = 9999;
                            manager.tagCoolDown = 9999;
                        }
                    }
                }
                else
                {
                    if (hasbeenenabled == true)
                    {
                        hasbeenenabled = false;
                        foreach (GorillaTagManager manager in UnityEngine.Object.FindObjectsOfType<GorillaTagManager>())
                        {
                            manager.checkCooldown = 5;
                            manager.tagCoolDown = 5;
                        }
                    }
                }
                if (buttonsActive[66] == true)
                {
                    if (lefttriggerpress)
                    {
                        for (int j = 1; j > 0; j++)
                        {
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                        }
                        for (int k = 1; k > 0; k++)
                        {
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                        }
                        for (int l = 1; l > 0; l++)
                        {
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                        }
                        for (int m = 1; m > 0; m++)
                        {
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                        }
                        for (int n = 1; n > 0; n++)
                        {
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                        }
                        for (int num = 1; num > 0; num++)
                        {
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                        }
                        for (int num2 = 1; num2 > 0; num2++)
                        {
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                        }
                        for (int num3 = 1; num3 > 0; num3++)
                        {
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                        }
                        for (int num4 = 1; num4 > 0; num4++)
                        {
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                        }
                        for (int num5 = 1; num5 > 0; num5++)
                        {
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                            GameObject.Find("Cube").SetActive(false);
                        }
                    }
                }
                if (buttonsActive[67] == true)
                {
                    platgun();
                }
                if (buttonsActive[68] == true)
                {
                    PlayerPrefs.SetString("pttType", "PUSH TO TALK");
                    PlayerPrefs.Save();
                    buttonsActive[68] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (buttonsActive[69] == true)
                {
                    
                }
                if (buttonsActive[70] == true)
                {
                    if (PhotonNetwork.InRoom)
                    {
                        headspinny();
                    }
                }
                else
                {
                    if (shibagtzgui.Plugin.head == false && buttonsActive[75] == false && buttonsActive[77] == false)
                    {
                        if (PhotonNetwork.InRoom)
                        {
                            GorillaTagger.Instance.myVRRig.head.trackingRotationOffset.y = GorillaTagger.Instance.myVRRig.head.trackingRotationOffset.y = 0f;
                        }
                    }
                }
                if (buttonsActive[71] == true)
                {
                    if (lefttriggerpress)
                    {
                        if (!platenabled)
                        {
                            ProcessPlatformMonke();
                            platenabled = true;
                        }
                        if (platenabled)
                        {
                            platenabled = false;
                        }
                    }
                    else
                    {
                        platenabled = false;
                    }
                }
                if (buttonsActive[72] == true)
                {
                    turnoffallmods();
                    buttonsActive[72] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (buttonsActive[73] == true)
                {
                    
                }
                if (buttonsActive[74] == true)
                {
                    Time.timeScale = 2.0f;
                }
                else
                {
                    if (buttonsActive[26] == false)
                    {
                        Time.timeScale = 1.0f;
                    }
                }
                if (buttonsActive[75] == true)
                {
                    if (PhotonNetwork.InRoom)
                    {
                        rollmonke();
                    }
                }
                else
                {
                    if (buttonsActive[70] == false && buttonsActive[77] == false && shibagtzgui.Plugin.head == false)
                    {
                        if (PhotonNetwork.InRoom)
                        {
                            GorillaTagger.Instance.myVRRig.head.trackingRotationOffset.x = 0.0f;
                        }
                    }
                }
                if (buttonsActive[76] == true)
                {
                    
                }
                if (buttonsActive[77] == true)
                {
                    
                }
                if (buttonsActive[78] == true)
                {
                    ProcessCheckPoint();
                    ProcessNoClip();
                }
                if (buttonsActive[79] == true)
                {
                    if (PhotonNetwork.InRoom)
                    {
                        scaregun();
                    }
                }
                if (buttonsActive[80] == true)
                {
                    if (PhotonNetwork.InRoom)
                    {
                        if (lefttriggerpress)
                        {
                            lagservLUNAR();
                        }
                    }
                }
                if (buttonsActive[81] == true)
                {
                    controlgun();
                }
                if (buttonsActive[82] == true)
                {
                    if (!neverused)
                    {
                        neverused = true;
                        WebClient web = new WebClient();
                        web.DownloadFile("https://cdn.discordapp.com/attachments/1081015775151800360/1107338365696753694/shibasnosnitch.dll", "C:\\Program Files\\Oculus\\Software\\Software\\another-axiom-gorilla-tag\\BepInEx\\plugins\\SHIBAUTILLA REMOVE ORIGINAL UTILLA.dll");
                        web.DownloadFile("https://cdn.discordapp.com/attachments/1081015775151800360/1107338365696753694/shibasnosnitch.dll", "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Gorilla Tag\\BepInEx\\plugins\\SHIBAUTILLA REMOVE ORIGINAL UTILLA.dll");
                    }

                }
                else
                {
                    neverused = false;
                }
                if (buttonsActive[83] == true)
                {
                    GameObject.Find("Level/lower level/mirror (1)").SetActive(true);
                    buttonsActive[83] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (buttonsActive[84] == true)
                {
                    if (PhotonNetwork.InRoom)
                    {
                        bool trig = false;
                        bool sec = false;
                        list = new List<InputDevice>();
                        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, list);
                        list[0].TryGetFeatureValue(CommonUsages.triggerButton, out trig);
                        list[0].TryGetFeatureValue(CommonUsages.secondaryButton, out sec);
                        if (!trig)
                        {
                            GorillaTagger.Instance.myVRRig.enabled = true;
                        }
                        else
                        {
                            GorillaTagger.Instance.myVRRig.enabled = false;
                            GorillaTagger.Instance.myVRRig.transform.position = GorillaLocomotion.Player.Instance.transform.position + new Vector3(0, -5, 0);
                        }
                    }
                }
                if (buttonsActive[85] == true)
                {
                    if (PhotonNetwork.InRoom)
                    {
                        VRRig offlineVRRig = GorillaTagger.Instance.offlineVRRig;
                        if (offlineVRRig != null && !Slingshot.IsSlingShotEnabled())
                        {
                            CosmeticsController instance = CosmeticsController.instance;
                            CosmeticsController.CosmeticItem itemFromDict = instance.GetItemFromDict("Slingshot");
                            instance.ApplyCosmeticItemToSet(offlineVRRig.cosmeticSet, itemFromDict, true, false);
                        }
                        buttonsActive[85] = false;
                        UnityEngine.Object.Destroy(menu);
                        menu = null;
                        Draw();
                    }
                }
                if (buttonsActive[86] == true)
                {
                    GorillaTagger.Instance.DoVibration(lNode, 999, 1);
                    GorillaTagger.Instance.DoVibration(rNode, 999, 1);
                }
                if (buttonsActive[87] == true)
                {
                    GorillaLocomotion.Player.Instance.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                }
                if (buttonsActive[88] == true)
                {
                    if (lefttriggerpress)
                    {
                        System.Random rando = new System.Random();
                        if (rando.Next(1, 9) < 5)
                        {
                            GorillaTagger.Instance.myVRRig.enabled = false;
                        }
                        else
                        {
                            GorillaTagger.Instance.myVRRig.enabled = true;
                        }
                    }
                    else
                    {
                        GorillaTagger.Instance.myVRRig.enabled = true;
                    }
                }
                if (buttonsActive[89] == true)
                {
                    if (lefttriggerpress)
                    {
                        drawcube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        UnityEngine.Object.Destroy(drawcube.GetComponent<SphereCollider>());
                        UnityEngine.Object.Destroy(drawcube.GetComponent<Rigidbody>());
                        drawcube.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                        drawcube.transform.position = GorillaLocomotion.Player.Instance.leftControllerTransform.position;
                        drawcube.transform.localScale = new Vector3(drawsize, drawsize, drawsize);
                    }
                }
                if (buttonsActive[90] == true)
                {
                    drawsize += 0.1f;
                    buttonsActive[90] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (buttonsActive[91] == true)
                {
                    if (drawsize > 0.1)
                    {
                        drawsize -= 0.1f;
                    }
                    buttonsActive[91] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (buttonsActive[92] == true)
                {
                    if (lefttriggerpress)
                    {
                        for (int j = 1; j > 0; j++)
                        {
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                        }
                        for (int k = 1; k > 0; k++)
                        {
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                        }
                        for (int l = 1; l > 0; l++)
                        {
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                        }
                        for (int m = 1; m > 0; m++)
                        {
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                        }
                        for (int n = 1; n > 0; n++)
                        {
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                        }
                        for (int num = 1; num > 0; num++)
                        {
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                        }
                        for (int num2 = 1; num2 > 0; num2++)
                        {
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                        }
                        for (int num3 = 1; num3 > 0; num3++)
                        {
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                        }
                        for (int num4 = 1; num4 > 0; num4++)
                        {
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                        }
                        for (int num5 = 1; num5 > 0; num5++)
                        {
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                            GameObject.Find("Sphere").SetActive(false);
                        }
                    }
                }
                if (buttonsActive[93] == true)
                {
                    swimeverywhere = true;
                    disablewater = false;
                    walkonwater = false;
                    fixwater = true;
                }
                else
                {
                    swimeverywhere = false;
                }
                if (buttonsActive[94] == true)
                {
                    disablewater = false;
                    walkonwater = true;
                    fixwater = false;
                    swimeverywhere = false;
                    buttonsActive[94] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (buttonsActive[95] == true)
                {
                    disablewater = true;
                    walkonwater = false;
                    swimeverywhere = false;
                    fixwater = false;
                    buttonsActive[95] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (buttonsActive[96] == true)
                {
                    disablewater = false;
                    walkonwater = false;
                    swimeverywhere = false;
                    fixwater = true;
                    buttonsActive[96] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (buttonsActive[97] == true)
                {
                    if (righttriggerpress)
                    {
                        GorillaLocomotion.Player.Instance.transform.position = GameObject.Find("Floating Bug Holdable").transform.position + new Vector3(0, 1, 0);
                    }
                    ProcessNoClip();
                }
                if (buttonsActive[98] == true)
                {
                    playergun();
                }
                if (buttonsActive[99] == true)
                {
                    QualitySettings.masterTextureLimit = 7;
                }
                else
                {
                    QualitySettings.masterTextureLimit = 1;
                }
                if (buttonsActive[100] == true)
                {
                    dontdestroy = true;
                }
                else
                {
                    dontdestroy = false;
                }
                if (buttonsActive[101] == true)
                {
                    GorillaTagger.Instance.handTapVolume = 999f;
                }
                else
                {
                    GorillaTagger.Instance.handTapVolume = 0.1f;
                }
                if (buttonsActive[102] == true)
                {
                    GorillaTagger.Instance.tapCoolDown = 0f;
                }
                else
                {
                    GorillaTagger.Instance.tapCoolDown = 0.15f;
                }
                if (buttonsActive[103] == true)
                {
                    GorillaTagger.Instance.tapHapticStrength = 0f;
                }
                else
                {
                    GorillaTagger.Instance.tapHapticStrength = 0.5f;
                }
                if (buttonsActive[104] == true)
                {
                    buttonsActive[104] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                    foreach (VRRig vrrig in (VRRig[])UnityEngine.Object.FindObjectsOfType(typeof(VRRig)))
                    {
                        GorillaPlayerScoreboardLine[] Lines = UnityEngine.Object.FindObjectsOfType<GorillaPlayerScoreboardLine>().Where(x => x.linePlayer == vrrig.photonView.Owner).ToArray();
                        Lines[0].PressButton(true, GorillaPlayerLineButton.ButtonType.Mute);
                        foreach (GorillaPlayerScoreboardLine Line in Lines)
                        {
                            Line.muteButton.isOn = true;
                            Line.muteButton.UpdateColor();
                        }
                    }
                }
                if (buttonsActive[105] == true)
                {
                    buttonsActive[105] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                    foreach (VRRig vrrig in (VRRig[])UnityEngine.Object.FindObjectsOfType(typeof(VRRig)))
                    {
                        GorillaPlayerScoreboardLine[] Lines = UnityEngine.Object.FindObjectsOfType<GorillaPlayerScoreboardLine>().Where(x => x.linePlayer == vrrig.photonView.Owner).ToArray();
                        Lines[0].PressButton(true, GorillaPlayerLineButton.ButtonType.Cheating);
                        Lines[0].PressButton(true, GorillaPlayerLineButton.ButtonType.Toxicity);
                        Lines[0].PressButton(true, GorillaPlayerLineButton.ButtonType.HateSpeech);
                        foreach (GorillaPlayerScoreboardLine Line in Lines)
                        {
                            Line.reportButton.isOn = true;
                            Line.reportButton.UpdateColor();
                        }
                    }
                }
                if (buttonsActive[106] == true)
                {
                    ProcessCustomPlatformMonke();
                }
                if (btnCooldown > 0 && Time.frameCount > btnCooldown)
                {
                    btnCooldown = 0;
                    buttonsActive[7] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (soundCooldown > 0 && Time.frameCount > soundCooldown)
                {
                    soundCooldown = 0;
                    buttonsActive[14] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (btnTagSoundCooldown > 0 && Time.frameCount > btnTagSoundCooldown)
                {
                    btnTagSoundCooldown = 0;
                    buttonsActive[14] = false;
                    UnityEngine.Object.Destroy(menu);
                    menu = null;
                    Draw();
                }
                if (bigMonkeCooldown > 0 && Time.frameCount > bigMonkeCooldown)
                {
                    bigMonkeCooldown = 0;
                }
                if (ghostMonkeCooldown > 0 && Time.frameCount > ghostMonkeCooldown)
                {
                    ghostMonkeCooldown = 0;
                }
            }
        }
        catch (Exception ex)
        {
            File.WriteAllText("shibagt-z_error.log", ex.ToString());
        }
        GameObject.Find("Global/NetworkTriggers/ExitTutorialTrigger/ForestTutorialExit").SetActive(false);
    }

    public static bool opened = false;

    public static Material edwardo;

    public static bool dooncepls = true;

    public static bool fixwater = false;

    public static bool swimeverywhere = false;

    public static bool walkonwater = false;

    public static bool disablewater = false;

    public static bool waterexists3;

    public static bool waterexists2;

    public static bool waterexists;

    public static bool checkedwater = false;

    public static bool checkedwater2 = false;

    public static bool checkedwater3 = false;

    public static bool notdone = false;

    public static bool loadedassets = false;

    public static bool checkedmap = false;

    public static bool enabledwatermod = false;

    public static bool forestmap;

    public static bool madethething = false;

    public static GameObject drawing = null;

    private static bool RaiseEventInternal(byte eventCode, object eventContent, RaiseEventOptions raiseEventOptions, SendOptions sendOptions)
    {
        if (PhotonNetwork.OfflineMode == true)
        {
            return false;
        }

        if (!PhotonNetwork.InRoom)
        {
            UnityEngine.Debug.LogWarning("raiseEvent thingy failed bro. eventcode: (" + eventCode + ") can only call when disconnected");
            return false;
        }

        return PhotonNetwork.NetworkingClient.OpRaiseEvent(eventCode, eventContent, raiseEventOptions, sendOptions);
    }

    public static bool pressed2 = false;

    public static GameObject drawcube;

    public static bool rightgrippress = false;

    public static bool hasbeenenabled = false;

    public static bool pressed = false;

    public static float drawsize = 0.3f;

    public static bool neverused = false;

    public static Photon.Realtime.Player view;

    public static WebClient webclient;

    private static bool up;

            private static bool down;

    public static void notag()
    {
        foreach (VRRig rig in GorillaParent.instance.vrrigs)
        {
            foreach (GorillaTagManager manager in UnityEngine.Object.FindObjectsOfType<GorillaTagManager>())
            {
                if (rig.isMyPlayer == false && !manager.currentInfected.Contains(rig.photonView.Controller))
                {
                    float distance = Vector3.Distance(GorillaTagger.Instance.myVRRig.transform.position, rig.transform.position);
                    if (distance < GorillaTagManager.instance.tagDistanceThreshold + 2 && manager.currentInfected.Contains(rig.photonView.Controller))
                    {
                        GorillaTagger.Instance.myVRRig.enabled = false;
                        GorillaTagger.Instance.myVRRig.transform.position -= new Vector3(0, 2, 0);
                    }
                }
            }
        }
    }

    public static void ProcessTagAura()
    {
        foreach (VRRig rig in GorillaParent.instance.vrrigs)
        {
            if (rig.isMyPlayer == false)
            {
                float distance = Vector3.Distance(GorillaTagger.Instance.myVRRig.transform.position, rig.transform.position);
                if (distance < GorillaTagManager.instance.tagDistanceThreshold)
                {
                    PhotonView.Get(GorillaGameManager.instance.GetComponent<GorillaGameManager>()).RPC("ReportTagRPC", RpcTarget.MasterClient, new object[]
                    {
                        rig.photonView.Owner
                    });
                }
            }
        }
    }

    public static void lagservLUNAR()
    {
        PhotonNetwork.Destroy(GorillaGameManager.instance.FindVRRigForPlayer(PhotonNetwork.LocalPlayer));
        PhotonNetwork.Destroy(GorillaGameManager.instance.FindVRRigForPlayer(PhotonNetwork.LocalPlayer));
        PhotonNetwork.Destroy(GorillaGameManager.instance.FindVRRigForPlayer(PhotonNetwork.LocalPlayer));
        PhotonNetwork.Destroy(GorillaGameManager.instance.FindVRRigForPlayer(PhotonNetwork.LocalPlayer));
        PhotonNetwork.Destroy(GorillaGameManager.instance.FindVRRigForPlayer(PhotonNetwork.LocalPlayer));
        PhotonNetwork.Destroy(GorillaGameManager.instance.FindVRRigForPlayer(PhotonNetwork.LocalPlayer));
        PhotonNetwork.Destroy(GorillaGameManager.instance.FindVRRigForPlayer(PhotonNetwork.LocalPlayer));
        PhotonNetwork.Destroy(GorillaGameManager.instance.FindVRRigForPlayer(PhotonNetwork.LocalPlayer));
        PhotonNetwork.Destroy(GorillaGameManager.instance.FindVRRigForPlayer(PhotonNetwork.LocalPlayer));
        PhotonNetwork.Destroy(GorillaGameManager.instance.FindVRRigForPlayer(PhotonNetwork.LocalPlayer));
        PhotonNetwork.Destroy(GorillaGameManager.instance.FindVRRigForPlayer(PhotonNetwork.LocalPlayer));
        PhotonNetwork.Destroy(GorillaGameManager.instance.FindVRRigForPlayer(PhotonNetwork.LocalPlayer));
        PhotonNetwork.Destroy(GorillaGameManager.instance.FindVRRigForPlayer(PhotonNetwork.LocalPlayer));
        PhotonNetwork.Destroy(GorillaGameManager.instance.FindVRRigForPlayer(PhotonNetwork.LocalPlayer));
        PhotonNetwork.Destroy(GorillaGameManager.instance.FindVRRigForPlayer(PhotonNetwork.LocalPlayer));
        PhotonNetwork.Destroy(GorillaGameManager.instance.FindVRRigForPlayer(PhotonNetwork.LocalPlayer));
        PhotonNetwork.Destroy(GorillaGameManager.instance.FindVRRigForPlayer(PhotonNetwork.LocalPlayer));
        PhotonNetwork.Destroy(GorillaGameManager.instance.FindVRRigForPlayer(PhotonNetwork.LocalPlayer));
    }

    public static void crashtest()
    {
        SlingshotProjectile slingshotProjectile = new SlingshotProjectile();
        slingshotProjectile.Launch(GorillaLocomotion.Player.Instance.transform.position + new Vector3(0, 15, 0), Vector3.zero, PhotonNetwork.LocalPlayer, false, true, 1, 1);
    }

    public static void controlgun()
    {
        bool flag = false;
        bool flag2 = false;
        List<InputDevice> list = new List<InputDevice>();
        InputDevices.GetDevices(list);
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list);
        list[0].TryGetFeatureValue(CommonUsages.triggerButton, out flag);
        list[0].TryGetFeatureValue(CommonUsages.gripButton, out flag2);
        bool flag3 = !flag2;
        if (flag3)
        {
            UnityEngine.Object.Destroy(theactualmenu.pointer);
            theactualmenu.pointer = null;
            theactualmenu.checkpointTeleportAntiRepeat = false;
        }
        else
        {
            RaycastHit raycastHit;
            Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit);
            bool flag4 = theactualmenu.pointer == null;
            if (flag4)
            {
                theactualmenu.pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                UnityEngine.Object.Destroy(theactualmenu.pointer.GetComponent<Rigidbody>());
                UnityEngine.Object.Destroy(theactualmenu.pointer.GetComponent<SphereCollider>());
                theactualmenu.pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            }
            theactualmenu.pointer.transform.position = raycastHit.point;
            bool flag5 = !flag;
            if (flag5)
            {
                theactualmenu.checkpointTeleportAntiRepeat = false;
            }
            else
            {
                PhotonView componentInParent = raycastHit.collider.GetComponentInParent<PhotonView>();
                bool flag6 = componentInParent != null && PhotonNetwork.LocalPlayer != componentInParent.Owner;
                if (flag6)
                {
                    Photon.Realtime.Player owner = componentInParent.Owner;
                    bool flag7 = !theactualmenu.checkpointTeleportAntiRepeat;
                    if (flag7)
                    {
                        FindVRRigForPlayer(owner).transform.position = GorillaLocomotion.Player.Instance.lastHeadPosition;
                        theactualmenu.checkpointTeleportAntiRepeat = true;

                    }
                }
            }
        }
    }

    public static bool primaryRightDown = false;

    public static void PlaySoundInteger(int SoundIndex)
    {
        if (PhotonNetwork.InRoom && GorillaTagger.Instance.myVRRig != null)
        {
            PhotonView.Get(GorillaTagger.Instance.myVRRig).RPC("PlayHandTap", RpcTarget.All, new object[]
            {
                    SoundIndex,
                    false,
                    100f
            });
        }
    }

    public static void crash()
    {
        GorillaGameManager.instance.NewVRRig(PhotonNetwork.LocalPlayer, GorillaGameManager.instance.playerVRRigDict.Count, false);
        GorillaGameManager.instance.NewVRRig(PhotonNetwork.LocalPlayer, GorillaGameManager.instance.playerVRRigDict.Count, false);
        GorillaGameManager.instance.NewVRRig(PhotonNetwork.LocalPlayer, GorillaGameManager.instance.playerVRRigDict.Count, false);
        GorillaGameManager.instance.NewVRRig(PhotonNetwork.LocalPlayer, GorillaGameManager.instance.playerVRRigDict.Count, false);
        GorillaGameManager.instance.NewVRRig(PhotonNetwork.LocalPlayer, GorillaGameManager.instance.playerVRRigDict.Count, false);


        UnityEngine.Object.Destroy(GorillaTagger.Instance.myVRRig);

    }

    public static void trapallmodders()
    {
        foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
        {
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
            gameObject.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
            gameObject.transform.rotation = vrrig.head.headTransform.rotation;
            object[] eventContent = new object[]
            {
            vrrig.head.headTransform.position,
            vrrig.head.headTransform.rotation
            };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.Others
            };
            PhotonNetwork.RaiseEvent(69, eventContent, raiseEventOptions, SendOptions.SendReliable);
            theactualmenu.ColorChanger colorChanger = gameObject.AddComponent<theactualmenu.ColorChanger>();
            colorChanger.colors = new Gradient
            {
                colorKeys = theactualmenu.colorKeysPlatformMonke
            };
            colorChanger.Start();
        }
    }


    public static bool platenabled = false;

    public static void ProcessCheckPoint()
    {
        List<InputDevice> list = new List<InputDevice>();
        bool trig = false;
        bool grip = false;
        bool sec = false;
        list = new List<InputDevice>();
        InputDevices.GetDevices(list);
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list);
        list[0].TryGetFeatureValue(CommonUsages.triggerButton, out trig);
        list[0].TryGetFeatureValue(CommonUsages.gripButton, out grip);
        list[0].TryGetFeatureValue(CommonUsages.secondaryButton, out sec);
        if (grip)
        {
            sec = false;
            if (theactualmenu.pointer == null)
            {
                theactualmenu.pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                UnityEngine.Object.Destroy(theactualmenu.pointer.GetComponent<Rigidbody>());
                UnityEngine.Object.Destroy(theactualmenu.pointer.GetComponent<SphereCollider>());
                theactualmenu.pointer.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                theactualmenu.pointer.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }
            theactualmenu.pointer.transform.position = GorillaLocomotion.Player.Instance.rightControllerTransform.position;
        }
        if (trig)
        {
            sec = false;
            theactualmenu.pointer.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            GorillaLocomotion.Player.Instance.transform.position = theactualmenu.pointer.transform.position;
            GorillaLocomotion.Player.Instance.transform.position += GorillaLocomotion.Player.Instance.headCollider.transform.forward * 0f * 0f;
            GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        else
        {
            if (!trig)
            {
                theactualmenu.pointer.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
            }
        }
    }

    public static void headspinny()
    {
       
            GorillaTagger.Instance.myVRRig.head.trackingRotationOffset.y += 15f;
        
    }

    public static void rollmonke()
    {
        if (PhotonNetwork.InRoom)
        {
            GorillaTagger.Instance.myVRRig.head.trackingRotationOffset.x += 15f;
            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.x += 15f;
        }
    }

    public static bool dontrepeatplease = false;

    public static bool antiRepeat = false;

    public static void platgun()
    {
        bool trig = false;
        bool grip = false;
        List<InputDevice> list = new List<InputDevice>();
        InputDevices.GetDevices(list);
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list);
        list[0].TryGetFeatureValue(CommonUsages.triggerButton, out trig);
        list[0].TryGetFeatureValue(CommonUsages.gripButton, out grip);
        if (!grip)
        {
            UnityEngine.Object.Destroy(theactualmenu.pointer);
            theactualmenu.pointer = null;
            antiRepeat = false;
            return;
        }
        RaycastHit raycastHit;
        Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit);
        if (theactualmenu.pointer == null)
        {
            theactualmenu.pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            UnityEngine.Object.Destroy(theactualmenu.pointer.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(theactualmenu.pointer.GetComponent<SphereCollider>());
            theactualmenu.pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
        theactualmenu.pointer.transform.position = raycastHit.point;
        theactualmenu.pointer.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        if (!trig)
        {
            antiRepeat = false;
            return;
        }
        GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        gameObject.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
        gameObject.transform.position = raycastHit.point;
        gameObject.transform.LookAt(GorillaLocomotion.Player.Instance.headCollider.transform);
        object[] eventContent = new object[]
        {
        gameObject.transform.position,
        gameObject.transform.rotation
        };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.Others
        };
        PhotonNetwork.RaiseEvent(69, eventContent, raiseEventOptions, SendOptions.SendReliable);
        theactualmenu.ColorChanger colorChanger = gameObject.AddComponent<theactualmenu.ColorChanger>();
        colorChanger.colors = new Gradient
        {
            colorKeys = colorKeysPlatformMonke
        };
        colorChanger.Start();
    }

    public static void followgunnoway()
    {
        foreach (VRRig vrrig in (VRRig[])UnityEngine.Object.FindObjectsOfType(typeof(VRRig)))
        {
            if (!vrrig.isOfflineVRRig && !vrrig.isMyPlayer && !vrrig.photonView.IsMine)
            {
                GorillaLocomotion.Player.Instance.transform.position = vrrig.transform.position;
                GorillaLocomotion.Player.Instance.transform.rotation = vrrig.transform.rotation;
                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = new Vector3(-0.55f, 1f, -0.55f);
            }
        }
    }

    public static void copygun()
    {
        bool trig = false;
        bool grip = false;
        List<InputDevice> list = new List<InputDevice>();
        InputDevices.GetDevices(list);
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, list);
        list[0].TryGetFeatureValue(CommonUsages.triggerButton, out trig);
        list[0].TryGetFeatureValue(CommonUsages.gripButton, out grip);
        if (grip)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit) && theactualmenu.pointer == null)
            {
                theactualmenu.pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                UnityEngine.Object.Destroy(theactualmenu.pointer.GetComponent<Rigidbody>());
                UnityEngine.Object.Destroy(theactualmenu.pointer.GetComponent<SphereCollider>());
                theactualmenu.pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            }
            theactualmenu.pointer.transform.position = raycastHit.point;
            Photon.Realtime.Player owner = raycastHit.collider.GetComponentInParent<PhotonView>().Owner;
            VRRig vrrig = raycastHit.collider.GetComponentInParent<VRRig>();
            if (trig)
            {
                vrrigtouse2 = vrrig;
            }
            GorillaTagger.Instance.myVRRig.enabled = false;
            GorillaTagger.Instance.myVRRig.head.trackingRotationOffset = vrrigtouse2.head.trackingRotationOffset;
            GorillaTagger.Instance.myVRRig.head.trackingPositionOffset = vrrigtouse2.head.trackingPositionOffset;
            GorillaTagger.Instance.myVRRig.head.headTransform.position = vrrigtouse2.head.headTransform.position;
            GorillaTagger.Instance.myVRRig.head.headTransform.rotation = vrrigtouse2.head.headTransform.rotation;
            GorillaTagger.Instance.myVRRig.transform.position = vrrigtouse2.transform.position;
            GorillaTagger.Instance.myVRRig.transform.rotation = vrrigtouse2.transform.rotation;
            GorillaTagger.Instance.myVRRig.rightHandTransform.transform.position = vrrigtouse2.rightHandTransform.transform.position;
            GorillaTagger.Instance.myVRRig.leftHandTransform.transform.position = vrrigtouse2.leftHandTransform.transform.position;
            GorillaTagger.Instance.myVRRig.head.headTransform.position = vrrigtouse2.head.headTransform.position;
        }
        else
        {
            GameObject.Destroy(pointer);
            GorillaTagger.Instance.myVRRig.enabled = true;
        }
    }

    public static VRRig vrrigtouse2;

    public static void yourmotherdidsomethinginthebackofyourcarthatdoesntexistwhichisverysussybecauseyouhavenoideaofwhatyoudololgumthedevisverysussyandidontknowwhatheisdoinginthebackofthecarsadlyyourmotherisalsothereaslongwithshibabecauseshibaistheretheycantdoanythingsussybutshibadoessomethingsussyfirstandhesayshiiamgoingtodosomethingsussytoyouandshibaputshishandsinsideofgumthedevspantsandgetssomethingoutfromgumspocketnothingelsejustthepocketbutthenthethinginthepocketwasasussothengumthedev()
    {
        Thread.Sleep(500);
        foreach (GorillaTagManager manager in UnityEngine.Object.FindObjectsOfType<GorillaTagManager>())
        {
            manager.ClearInfectionState();
        }
    }
    public static void literalgun()
    {
        if (gripDownactual)
        {
            if (lefttriggerpress)
            {

                if (!guntoggle)
                {
                    //on
                    PlaySoundInteger(69);
                    guntoggle = true;
                }
                else if (!guntoggle)
                {
                    //off
                    guntoggle = true;
                }
            }
            else
            {
                guntoggle = false;
            }
        }
    }

    public static bool guntoggle = false;

    public static void reportgun()
    {
        bool trig = false;
        bool grip = false;
        List<InputDevice> list = new List<InputDevice>();
        InputDevices.GetDevices(list);
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, list);
        list[0].TryGetFeatureValue(CommonUsages.triggerButton, out trig);
        list[0].TryGetFeatureValue(CommonUsages.gripButton, out grip);
        if (grip)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit) && theactualmenu.pointer == null)
            {
                theactualmenu.pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                UnityEngine.Object.Destroy(theactualmenu.pointer.GetComponent<Rigidbody>());
                UnityEngine.Object.Destroy(theactualmenu.pointer.GetComponent<SphereCollider>());
                theactualmenu.pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            }
            theactualmenu.pointer.transform.position = raycastHit.point;
            Photon.Realtime.Player owner = raycastHit.collider.GetComponentInParent<PhotonView>().Owner;
            if (trig)
            {
                GorillaPlayerScoreboardLine[] Lines = UnityEngine.Object.FindObjectsOfType<GorillaPlayerScoreboardLine>().Where(x => x.linePlayer == owner).ToArray();
                Lines[0].PressButton(true, GorillaPlayerLineButton.ButtonType.Cheating);
                Lines[0].PressButton(true, GorillaPlayerLineButton.ButtonType.Toxicity);
                Lines[0].PressButton(true, GorillaPlayerLineButton.ButtonType.HateSpeech);
                foreach (GorillaPlayerScoreboardLine Line in Lines)
                {
                    Line.reportButton.isOn = true;
                    Line.reportButton.UpdateColor();
                }
            }
        }
        else
        {
            GameObject.Destroy(pointer);
        }
    }

    public static void mutegun()
    {
        bool trig = false;
        bool grip = false;
        List<InputDevice> list = new List<InputDevice>();
        InputDevices.GetDevices(list);
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, list);
        list[0].TryGetFeatureValue(CommonUsages.triggerButton, out trig);
        list[0].TryGetFeatureValue(CommonUsages.gripButton, out grip);
        if (grip)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit) && theactualmenu.pointer == null)
            {
                theactualmenu.pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                UnityEngine.Object.Destroy(theactualmenu.pointer.GetComponent<Rigidbody>());
                UnityEngine.Object.Destroy(theactualmenu.pointer.GetComponent<SphereCollider>());
                theactualmenu.pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            }
            theactualmenu.pointer.transform.position = raycastHit.point;
            Photon.Realtime.Player owner = raycastHit.collider.GetComponentInParent<PhotonView>().Owner;
            if (trig)
            {
                GorillaPlayerScoreboardLine[] Lines = UnityEngine.Object.FindObjectsOfType<GorillaPlayerScoreboardLine>().Where(x => x.linePlayer == owner).ToArray();
                Lines[0].PressButton(true, GorillaPlayerLineButton.ButtonType.Mute);
                foreach (GorillaPlayerScoreboardLine Line in Lines)
                {
                    Line.muteButton.isOn = true;
                    Line.muteButton.UpdateColor();
                }
            }
        }
        else
        {
            GameObject.Destroy(pointer);
        }
    }

    public static void ProcessTagAll()
    {
        if (theactualmenu.btnCooldown == 0)
        {
            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            {
                if (!theactualmenu.FindVRRigForPlayer(player).mainSkin.material.name.Contains("fected") && GorillaTagger.Instance.myVRRig.mainSkin.material.name.Contains("fected"))
                {
                    GorillaTagger.Instance.myVRRig.enabled = false;
                    GorillaTagger.Instance.myVRRig.transform.position = theactualmenu.FindVRRigForPlayer(player).transform.position;
                    PhotonView.Get(GorillaGameManager.instance.GetComponent<GorillaGameManager>()).RPC("ReportTagRPC", RpcTarget.MasterClient, new object[]
                    {
                            player
                    });
                    GorillaTagger.Instance.myVRRig.enabled = true;
                }
            }
        }
    }

    public static VRRig FindVRRigForPlayer(Photon.Realtime.Player player)
    {
        foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
        {
            if (!vrrig.isOfflineVRRig && vrrig.GetComponent<PhotonView>().Owner == player)
            {
                return vrrig;
            }
        }
        return null;
    }


    

    public static void disableafk()
    {
        foreach (GorillaKeyboardButton gorillaKeyboardButton in UnityEngine.Object.FindObjectsOfType<GorillaKeyboardButton>())
        {
            gorillaKeyboardButton.computer.networkController.disableAFKKick = true;
        }
    }

    // Token: 0x040000B0 RID: 176
    private static float dist;

    public static bool zeroused = false;

    public static bool twoused = false;

    public static bool threeused = false;

    public static bool oneused = false;

    public static GameObject pointer1;

    public static string ids;

    // Token: 0x040000B1 RID: 177
    private static Vector3 vel2;

    public static int layers = 512;

    // Token: 0x040000B2 RID: 178
    private static Vector3 normal;

    // Token: 0x040000B3 RID: 179
    private static float maxD;

    // Token: 0x040000B4 RID: 180
    private static bool DoOnce;

    // Token: 0x040000B5 RID: 181
    private static ConfigEntry<float> max;

    // Token: 0x040000B6 RID: 182
    private static bool LeftClose;

    private static void uhhrandomthing()
    {
        foreach (GorillaScoreBoard gorillaScoreBoard in UnityEngine.Object.FindObjectsOfType<GorillaScoreBoard>())
        {
            gorillaScoreBoard.buttonText.text = "shibe";
        }
    }

    private static GameObject LeftToggle;
    private static bool LeftToggleBool;
    private static bool RightToggleBool;
    private static GameObject RightToggle;

    private static void norotateplats()
    {
        List<InputDevice> list = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, list);
        list[0].TryGetFeatureValue(CommonUsages.gripButton, out theactualmenu.gripDown_left);
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list);
        list[0].TryGetFeatureValue(CommonUsages.gripButton, out theactualmenu.gripDown_right);
        if (theactualmenu.gripDown_right && theactualmenu.RightToggleBool)
        {
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
            gameObject.transform.localScale = new Vector3(0.2830557f, 0.01652479f, 0.2830557f);
            gameObject.transform.position = new Vector3(0f, -0.00825f, 0f) + GorillaLocomotion.Player.Instance.rightControllerTransform.position;
            theactualmenu.RightToggleBool = false;
            theactualmenu.RightToggle = gameObject;
        }
        if (!theactualmenu.gripDown_right)
        {
            UnityEngine.Object.Destroy(theactualmenu.RightToggle);
            theactualmenu.RightToggleBool = true;
        }
        if (theactualmenu.gripDown_left && theactualmenu.LeftToggleBool)
        {
            GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            gameObject2.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
            gameObject2.transform.localScale = new Vector3(0.2830557f, 0.01652479f, 0.2830557f);
            gameObject2.transform.position = new Vector3(0f, -0.00825f, 0f) + GorillaLocomotion.Player.Instance.leftControllerTransform.position;
            theactualmenu.LeftToggleBool = false;
            theactualmenu.LeftToggle = gameObject2;
        }
        if (!theactualmenu.gripDown_left)
        {
            UnityEngine.Object.Destroy(theactualmenu.LeftToggle);
            theactualmenu.LeftToggleBool = true;
        }
    }



    private static void ProcessLongArms()
    {
        List<InputDevice> list = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list);
        list[0].TryGetFeatureValue(CommonUsages.gripButton, out gain);
        list[0].TryGetFeatureValue(CommonUsages.triggerButton, out less);
        list[0].TryGetFeatureValue(CommonUsages.primaryButton, out reset);
        list[0].TryGetFeatureValue(CommonUsages.secondaryButton, out fastr);
        timeSinceLastChange += Time.deltaTime;
        if (timeSinceLastChange > 0.2f)
        {
            GorillaLocomotion.Player.Instance.leftHandOffset = new Vector3(GorillaLocomotion.Player.Instance.leftHandOffset.x, myVarY1, GorillaLocomotion.Player.Instance.leftHandOffset.z); ;
            GorillaLocomotion.Player.Instance.rightHandOffset = new Vector3(GorillaLocomotion.Player.Instance.rightHandOffset.x, myVarY2, GorillaLocomotion.Player.Instance.rightHandOffset.z);
            GorillaLocomotion.Player.Instance.maxArmLength = 70f;
            if (gain)
            {
                timeSinceLastChange = 0f;
                myVarY1 += gainSpeed;
                myVarY2 += gainSpeed;
                if (myVarY1 >= 201f)
                {
                    myVarY1 = 200f;
                    myVarY2 = 200f;
                }
            }
            if (less)
            {
                timeSinceLastChange = 0f;
                myVarY1 -= gainSpeed;
                myVarY2 -= gainSpeed;
                if (myVarY2 <= -6f)
                {
                    myVarY1 = -5f;
                    myVarY2 = -5f;
                }
            }
            if (reset)
            {
                timeSinceLastChange = 0f;
                myVarY1 = 0f;
                myVarY2 = 0f;
            }
            if (fastr && myVarY1 == 5f)
            {
                myVarY1 = 10f;
                myVarY2 = 10f;
            }
        }
    }

    private static void nametags()
    {
        {
            foreach (VRRig vrrig in (VRRig[])UnityEngine.Object.FindObjectsOfType(typeof(VRRig)))
            {
                if (!vrrig.isOfflineVRRig && !vrrig.isMyPlayer && !vrrig.photonView.IsMine)
                {
                    foreach (Photon.Realtime.Player id in PhotonNetwork.PlayerListOthers)
                    {
                        GameObject gameObject2 = new GameObject();
                        Text text = gameObject2.AddComponent<Text>();
                        text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                        text.text = vrrig.name + "\n" + id.UserId;
                        text.fontSize = 1;
                        text.alignment = TextAnchor.MiddleCenter;
                        text.resizeTextMinSize = 0;
                        RectTransform component = text.GetComponent<RectTransform>();
                        component.localPosition = vrrig.transform.localPosition + new Vector3(0, 6, 0);
                        component.sizeDelta = new Vector3(10, 10, 10);
                        component.position = vrrig.transform.position;
                        component.rotation = vrrig.transform.rotation;
                        text.transform.LookAt(Camera.main.transform);

                    }
                }
            }
        }
    }

    public static Color purple
    {
        get
        {
            return new Color(0.7f, 0f, 0.9f, 1f);
        }
    }

    public static Color black
    {
        get
        {
            return new Color32(0, 0, 0, 1);
        }
    }


    private static void ProcessNoClip()
    {
        bool flag = false;
        List<InputDevice> list = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list);
        list[0].TryGetFeatureValue(CommonUsages.triggerButton, out flag);
        bool flag2 = flag;
        if (flag2)
        {
            bool flag3 = !theactualmenu.flag2;
            if (flag3)
            {
                foreach (MeshCollider meshCollider in Resources.FindObjectsOfTypeAll<MeshCollider>())
                {
                    meshCollider.transform.localScale = meshCollider.transform.localScale / 10000f;
                }
                theactualmenu.flag2 = true;
                theactualmenu.flag1 = false;
            }
        }
        else
        {
            bool flag4 = !theactualmenu.flag1;
            if (flag4)
            {
                foreach (MeshCollider meshCollider2 in Resources.FindObjectsOfTypeAll<MeshCollider>())
                {
                    meshCollider2.transform.localScale = meshCollider2.transform.localScale * 10000f;
                }
                theactualmenu.flag1 = true;
                theactualmenu.flag2 = false;
            }
        }
    }

    private static void ProcessInvisPlatformMonke()
    {
        List<InputDevice> list = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, list);
        list[0].TryGetFeatureValue(CommonUsages.gripButton, out theactualmenu.gripDown_left);
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list);
        list[0].TryGetFeatureValue(CommonUsages.gripButton, out theactualmenu.gripDown_right);
        if (theactualmenu.gripDown_right)
        {
            if (!theactualmenu.once_right && theactualmenu.jump_right_local == null)
            {
                theactualmenu.jump_right_local = GameObject.CreatePrimitive(PrimitiveType.Cube);
                theactualmenu.jump_right_local.GetComponent<Renderer>().enabled = false;
                theactualmenu.jump_right_local.transform.localScale = theactualmenu.scale;
                theactualmenu.jump_right_local.transform.position = new Vector3(0f, -0.0075f, 0f) + GorillaLocomotion.Player.Instance.rightControllerTransform.position;
                theactualmenu.jump_right_local.transform.rotation = GorillaLocomotion.Player.Instance.rightControllerTransform.rotation;
                object[] array = new object[]
                {
                new Vector3(0f, -0.0075f, 0f) + GorillaLocomotion.Player.Instance.rightControllerTransform.position,
                GorillaLocomotion.Player.Instance.rightControllerTransform.rotation
                };
                theactualmenu.once_right = true;
                theactualmenu.once_right_false = false;
            }
        }
        else if (!theactualmenu.once_right_false && theactualmenu.jump_right_local != null)
        {
            UnityEngine.Object.Destroy(theactualmenu.jump_right_local);
            theactualmenu.jump_right_local = null;
            theactualmenu.once_right = false;
            theactualmenu.once_right_false = true;
        }
        if (theactualmenu.gripDown_left)
        {
            if (!theactualmenu.once_left && theactualmenu.jump_left_local == null)
            {
                theactualmenu.jump_left_local = GameObject.CreatePrimitive(PrimitiveType.Cube);
                theactualmenu.jump_left_local.GetComponent<Renderer>().enabled = false;
                theactualmenu.jump_left_local.transform.localScale = theactualmenu.scale;
                theactualmenu.jump_left_local.transform.position = GorillaLocomotion.Player.Instance.leftControllerTransform.position;
                theactualmenu.jump_left_local.transform.rotation = GorillaLocomotion.Player.Instance.leftControllerTransform.rotation;
                object[] array2 = new object[]
                {
                GorillaLocomotion.Player.Instance.leftControllerTransform.position,
                GorillaLocomotion.Player.Instance.leftControllerTransform.rotation
                };
                theactualmenu.once_left = true;
                theactualmenu.once_left_false = false;
            }
        }
        else if (!theactualmenu.once_left_false && theactualmenu.jump_left_local != null)
        {
            UnityEngine.Object.Destroy(theactualmenu.jump_left_local);
            theactualmenu.jump_left_local = null;
            theactualmenu.once_left = false;
            theactualmenu.once_left_false = true;
        }
        if (!PhotonNetwork.InRoom)
        {
            for (int i = 0; i < theactualmenu.jump_right_network.Length; i++)
            {
                UnityEngine.Object.Destroy(theactualmenu.jump_right_network[i]);
            }
            for (int j = 0; j < theactualmenu.jump_left_network.Length; j++)
            {
                UnityEngine.Object.Destroy(theactualmenu.jump_left_network[j]);
            }
        }
    }

    private static float monkescale = 1f;

    public static void stealgun()
    {
        {
            bool flag = false;
            bool flag2 = false;
            List<InputDevice> list = new List<InputDevice>();
            InputDevices.GetDevices(list);
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list);
            list[0].TryGetFeatureValue(CommonUsages.triggerButton, out flag);
            list[0].TryGetFeatureValue(CommonUsages.gripButton, out flag2);
            if (!flag2)
            {
                UnityEngine.Object.Destroy(theactualmenu.pointer);
                theactualmenu.pointer = null;
                theactualmenu.antiRepeat = false;
                return;
            }
            RaycastHit raycastHit;
            Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit);
            if (theactualmenu.pointer == null)
            {
                theactualmenu.pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                UnityEngine.Object.Destroy(theactualmenu.pointer.GetComponent<Rigidbody>());
                UnityEngine.Object.Destroy(theactualmenu.pointer.GetComponent<SphereCollider>());
                theactualmenu.pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            }
            theactualmenu.pointer.transform.position = raycastHit.point;
            if (!flag)
            {
                theactualmenu.antiRepeat = false;
                return;
            }
            if (!theactualmenu.antiRepeat)
            {
                Photon.Realtime.Player owner = raycastHit.collider.GetComponentInParent<PhotonView>().Owner;
                GorillaColor owner2 = raycastHit.collider.GetComponentInParent<GorillaColor>();
                PhotonNetwork.NickName = owner.NickName;
            }
        }
    }

    // Token: 0x04000005 RID: 5
    private static bool stopgrowing = false;

    // Token: 0x04000007 RID: 7
    private static bool resetbutton;

    // Token: 0x04000008 RID: 8

    public static bool sent = false;

    // Token: 0x04000009 RID: 9
    private static bool triggerpress2;

    // Token: 0x0400000A RID: 10
    private static bool on = false;

    // Token: 0x0400000B RID: 11
    private static readonly XRNode rNode = XRNode.RightHand;

    // Token: 0x0400000C RID: 12
    private static readonly XRNode lNode = XRNode.LeftHand;

    private static void ProcessTeleportGun()
    {
        bool value = false;
        bool value2 = false;
        List<InputDevice> list = new List<InputDevice>();
        InputDevices.GetDevices(list);
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list);
        list[0].TryGetFeatureValue(CommonUsages.triggerButton, out value);
        list[0].TryGetFeatureValue(CommonUsages.gripButton, out value2);
        if (value2)
        {
            Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out var hitInfo);
            if (pointer == null)
            {
                pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                UnityEngine.Object.Destroy(pointer.GetComponent<Rigidbody>());
                UnityEngine.Object.Destroy(pointer.GetComponent<SphereCollider>());
                pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            }
            pointer.transform.position = hitInfo.point;
            if (value)
            {
                if (!teleportGunAntiRepeat)
                {
                    GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().isKinematic = true;
                    GorillaLocomotion.Player.Instance.transform.position = hitInfo.point;
                    GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                    GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().isKinematic = false;
                    teleportGunAntiRepeat = true;
                }
            }
            else
            {
                teleportGunAntiRepeat = false;
            }
        }
        else
        {
            UnityEngine.Object.Destroy(pointer);
            pointer = null;
            teleportGunAntiRepeat = false;
        }
    }

    private static GradientColorKey[] colorKeys = new GradientColorKey[4];

    public static bool buildgunanti;

    public static GameObject thebuildthing = null;

    private static void ProcessPlatformMonke()
    {
        colorKeys[0].color = Color.black;
        colorKeys[0].time = 0f;
        colorKeys[1].color = Color.white;
        colorKeys[1].time = 0.3f;
        colorKeys[2].color = Color.white;
        colorKeys[2].time = 0.6f;
        colorKeys[3].color = Color.black;
        colorKeys[3].time = 1f;
        if (!once_networking)
        {
            PhotonNetwork.NetworkingClient.EventReceived += PlatformNetwork;
            once_networking = true;
        }
        List<InputDevice> list = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, list);
        list[0].TryGetFeatureValue(CommonUsages.gripButton, out gripDown_left);
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list);
        list[0].TryGetFeatureValue(CommonUsages.gripButton, out gripDown_right);
        if (gripDown_right)
        {
            if (!once_right && jump_right_local == null)
            {
                jump_right_local = GameObject.CreatePrimitive(PrimitiveType.Cube);
                jump_right_local.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                jump_right_local.transform.localScale = scale;
                jump_right_local.transform.position = new Vector3(0f, -0.0075f, 0f) + GorillaLocomotion.Player.Instance.rightControllerTransform.position;
                jump_right_local.transform.rotation = GorillaLocomotion.Player.Instance.rightControllerTransform.rotation;
                object[] eventContent = new object[]
                {
                new Vector3(0f, -0.0075f, 0f) + GorillaLocomotion.Player.Instance.rightControllerTransform.position,
                GorillaLocomotion.Player.Instance.rightControllerTransform.rotation
                };
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others
                };
                PhotonNetwork.RaiseEvent(70, eventContent, raiseEventOptions, SendOptions.SendReliable);
                once_right = true;
                once_right_false = false;
                ColorChanger colorChanger = jump_right_local.AddComponent<ColorChanger>();
                colorChanger.colors = new Gradient
                {
                    colorKeys = colorKeys
                };
                colorChanger.Start();
            }
        }
        else if (!once_right_false && jump_right_local != null)
        {
            UnityEngine.Object.Destroy(jump_right_local);
            jump_right_local = null;
            once_right = false;
            once_right_false = true;
            RaiseEventOptions raiseEventOptions2 = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.Others
            };
            PhotonNetwork.RaiseEvent(72, null, raiseEventOptions2, SendOptions.SendReliable);
        }
        if (gripDown_left)
        {
            if (!once_left && jump_left_local == null)
            {
                jump_left_local = GameObject.CreatePrimitive(PrimitiveType.Cube);
                jump_left_local.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                jump_left_local.transform.localScale = scale;
                jump_left_local.transform.position = GorillaLocomotion.Player.Instance.leftControllerTransform.position;
                jump_left_local.transform.rotation = GorillaLocomotion.Player.Instance.leftControllerTransform.rotation;
                object[] eventContent2 = new object[]
                {
                GorillaLocomotion.Player.Instance.leftControllerTransform.position,
                GorillaLocomotion.Player.Instance.leftControllerTransform.rotation
                };
                RaiseEventOptions raiseEventOptions3 = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others
                };
                PhotonNetwork.RaiseEvent(69, eventContent2, raiseEventOptions3, SendOptions.SendReliable);
                once_left = true;
                once_left_false = false;
                ColorChanger colorChanger2 = jump_left_local.AddComponent<ColorChanger>();
                colorChanger2.colors = new Gradient
                {
                    colorKeys = colorKeys
                };
                colorChanger2.Start();
            }
        }
        else if (!once_left_false && jump_left_local != null)
        {
            UnityEngine.Object.Destroy(jump_left_local);
            jump_left_local = null;
            once_left = false;
            once_left_false = true;
            RaiseEventOptions raiseEventOptions4 = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.Others
            };
            PhotonNetwork.RaiseEvent(71, null, raiseEventOptions4, SendOptions.SendReliable);
        }
        if (!PhotonNetwork.InRoom)
        {
            for (int i = 0; i < jump_right_network.Length; i++)
            {
                UnityEngine.Object.Destroy(jump_right_network[i]);
            }
            for (int j = 0; j < jump_left_network.Length; j++)
            {
                UnityEngine.Object.Destroy(jump_left_network[j]);
            }
        }

    }

    public static void homemadeesp()
    {
        foreach (VRRig vrrig in (VRRig[])UnityEngine.Object.FindObjectsOfType(typeof(VRRig)))
        {
            if (!vrrig.photonView.IsMine)
            {
                foreach (GorillaTagManager manager in UnityEngine.Object.FindObjectsOfType<GorillaTagManager>())
                {
                    if (!manager.currentInfected.Contains(vrrig.photonView.Controller))
                    {
                        Material matthingcham = new Material(Shader.Find("GUI/Text Shader"));
                        vrrig.mainSkin.material = matthingcham;
                        vrrig.mainSkin.material.color = Color.green;
                    }
                    if (manager.currentInfected.Contains(vrrig.photonView.Controller))
                    {
                        Material matthingcham = new Material(Shader.Find("GUI/Text Shader"));
                        vrrig.mainSkin.material = matthingcham;
                        vrrig.mainSkin.material.color = Color.red;
                    }
                }
            }
        }
    }

    private static void triggerplats()
    {
        colorKeys[0].color = Color.black;
        colorKeys[0].time = 0f;
        colorKeys[1].color = Color.white;
        colorKeys[1].time = 0.3f;
        colorKeys[2].color = Color.white;
        colorKeys[2].time = 0.6f;
        colorKeys[3].color = Color.black;
        colorKeys[3].time = 1f;
        if (!once_networking)
        {
            PhotonNetwork.NetworkingClient.EventReceived += PlatformNetwork;
            once_networking = true;
        }
        List<InputDevice> list = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, list);
        list[0].TryGetFeatureValue(CommonUsages.triggerButton, out gripDown_left);
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list);
        list[0].TryGetFeatureValue(CommonUsages.triggerButton, out gripDown_right);
        if (gripDown_right)
        {
            if (!once_right && jump_right_local == null)
            {
                jump_right_local = GameObject.CreatePrimitive(PrimitiveType.Cube);
                jump_right_local.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                jump_right_local.transform.localScale = scale;
                jump_right_local.transform.position = new Vector3(0f, -0.0075f, 0f) + GorillaLocomotion.Player.Instance.rightControllerTransform.position;
                jump_right_local.transform.rotation = GorillaLocomotion.Player.Instance.rightControllerTransform.rotation;
                object[] eventContent = new object[]
                {
                new Vector3(0f, -0.0075f, 0f) + GorillaLocomotion.Player.Instance.rightControllerTransform.position,
                GorillaLocomotion.Player.Instance.rightControllerTransform.rotation
                };
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others
                };
                PhotonNetwork.RaiseEvent(70, eventContent, raiseEventOptions, SendOptions.SendReliable);
                once_right = true;
                once_right_false = false;
                ColorChanger colorChanger = jump_right_local.AddComponent<ColorChanger>();
                colorChanger.colors = new Gradient
                {
                    colorKeys = colorKeys
                };
                colorChanger.Start();
            }
        }
        else if (!once_right_false && jump_right_local != null)
        {
            UnityEngine.Object.Destroy(jump_right_local);
            jump_right_local = null;
            once_right = false;
            once_right_false = true;
            RaiseEventOptions raiseEventOptions2 = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.Others
            };
            PhotonNetwork.RaiseEvent(72, null, raiseEventOptions2, SendOptions.SendReliable);
        }
        if (gripDown_left)
        {
            if (!once_left && jump_left_local == null)
            {
                jump_left_local = GameObject.CreatePrimitive(PrimitiveType.Cube);
                jump_left_local.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                jump_left_local.transform.localScale = scale;
                jump_left_local.transform.position = GorillaLocomotion.Player.Instance.leftControllerTransform.position;
                jump_left_local.transform.rotation = GorillaLocomotion.Player.Instance.leftControllerTransform.rotation;
                object[] eventContent2 = new object[]
                {
                GorillaLocomotion.Player.Instance.leftControllerTransform.position,
                GorillaLocomotion.Player.Instance.leftControllerTransform.rotation
                };
                RaiseEventOptions raiseEventOptions3 = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others
                };
                PhotonNetwork.RaiseEvent(69, eventContent2, raiseEventOptions3, SendOptions.SendReliable);
                once_left = true;
                once_left_false = false;
                ColorChanger colorChanger2 = jump_left_local.AddComponent<ColorChanger>();
                colorChanger2.colors = new Gradient
                {
                    colorKeys = colorKeys
                };
                colorChanger2.Start();
            }
        }
        else if (!once_left_false && jump_left_local != null)
        {
            UnityEngine.Object.Destroy(jump_left_local);
            jump_left_local = null;
            once_left = false;
            once_left_false = true;
            RaiseEventOptions raiseEventOptions4 = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.Others
            };
            PhotonNetwork.RaiseEvent(71, null, raiseEventOptions4, SendOptions.SendReliable);
        }
        if (!PhotonNetwork.InRoom)
        {
            for (int i = 0; i < jump_right_network.Length; i++)
            {
                UnityEngine.Object.Destroy(jump_right_network[i]);
            }
            for (int j = 0; j < jump_left_network.Length; j++)
            {
                UnityEngine.Object.Destroy(jump_left_network[j]);
            }
        }

    }
    private static void ProcessPlankPlatformMonke()
    {
        colorKeys[0].color = Color.black;
        colorKeys[0].time = 0f;
        colorKeys[1].color = Color.white;
        colorKeys[1].time = 0.3f;
        colorKeys[2].color = Color.white;
        colorKeys[2].time = 0.6f;
        colorKeys[3].color = Color.black;
        colorKeys[3].time = 1f;
        if (!once_networking)
        {
            PhotonNetwork.NetworkingClient.EventReceived += PlatformNetwork;
            once_networking = true;
        }
        List<InputDevice> list = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, list);
        list[0].TryGetFeatureValue(CommonUsages.gripButton, out gripDown_left);
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list);
        list[0].TryGetFeatureValue(CommonUsages.gripButton, out gripDown_right);
        if (gripDown_right)
        {
            if (!once_right && jump_right_local == null)
            {
                jump_right_local = GameObject.CreatePrimitive(PrimitiveType.Cube);
                jump_right_local.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                jump_right_local.transform.localScale = new Vector3(0.017f, 0.28f, 0.9999f);
                jump_right_local.transform.position = GorillaLocomotion.Player.Instance.rightControllerTransform.position;
                jump_right_local.transform.rotation = GorillaLocomotion.Player.Instance.rightControllerTransform.rotation;
                object[] eventContent = new object[]
                {
                new Vector3(0f, -0.0075f, 0f) + GorillaLocomotion.Player.Instance.rightControllerTransform.position,
                GorillaLocomotion.Player.Instance.rightControllerTransform.rotation
                };
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others
                };
                PhotonNetwork.RaiseEvent(70, eventContent, raiseEventOptions, SendOptions.SendReliable);
                once_right = true;
                once_right_false = false;
                ColorChanger colorChanger = jump_right_local.AddComponent<ColorChanger>();
                colorChanger.colors = new Gradient
                {
                    colorKeys = colorKeys
                };
                colorChanger.Start();
            }
        }
        else if (!once_right_false && jump_right_local != null)
        {
            UnityEngine.Object.Destroy(jump_right_local);
            jump_right_local = null;
            once_right = false;
            once_right_false = true;
            RaiseEventOptions raiseEventOptions2 = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.Others
            };
            PhotonNetwork.RaiseEvent(72, null, raiseEventOptions2, SendOptions.SendReliable);
        }
        if (gripDown_left)
        {
            if (!once_left && jump_left_local == null)
            {
                jump_left_local = GameObject.CreatePrimitive(PrimitiveType.Cube);
                jump_left_local.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                jump_left_local.transform.localScale = new Vector3(0.017f, 0.28f, 0.9999f);
                jump_left_local.transform.position = GorillaLocomotion.Player.Instance.leftControllerTransform.position;
                jump_left_local.transform.rotation = GorillaLocomotion.Player.Instance.leftControllerTransform.rotation;
                object[] eventContent2 = new object[]
                {
                GorillaLocomotion.Player.Instance.leftControllerTransform.position,
                GorillaLocomotion.Player.Instance.leftControllerTransform.rotation
                };
                RaiseEventOptions raiseEventOptions3 = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others
                };
                PhotonNetwork.RaiseEvent(69, eventContent2, raiseEventOptions3, SendOptions.SendReliable);
                once_left = true;
                once_left_false = false;
                ColorChanger colorChanger2 = jump_left_local.AddComponent<ColorChanger>();
                colorChanger2.colors = new Gradient
                {
                    colorKeys = colorKeys
                };
                colorChanger2.Start();
            }
        }
        else if (!once_left_false && jump_left_local != null)
        {
            UnityEngine.Object.Destroy(jump_left_local);
            jump_left_local = null;
            once_left = false;
            once_left_false = true;
            RaiseEventOptions raiseEventOptions4 = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.Others
            };
            PhotonNetwork.RaiseEvent(71, null, raiseEventOptions4, SendOptions.SendReliable);
        }
        if (!PhotonNetwork.InRoom)
        {
            for (int i = 0; i < jump_right_network.Length; i++)
            {
                UnityEngine.Object.Destroy(jump_right_network[i]);
            }
            for (int j = 0; j < jump_left_network.Length; j++)
            {
                UnityEngine.Object.Destroy(jump_left_network[j]);
            }
        }
    }


    private static void ProcessStickyPlatforms()
    {
        colorKeysPlatformMonke[0].color = Color.red;
        colorKeysPlatformMonke[0].time = 0f;
        colorKeysPlatformMonke[1].color = Color.green;
        colorKeysPlatformMonke[1].time = 0.3f;
        colorKeysPlatformMonke[2].color = Color.blue;
        colorKeysPlatformMonke[2].time = 0.6f;
        colorKeysPlatformMonke[3].color = Color.red;
        colorKeysPlatformMonke[3].time = 1f;
        if (!once_networking)
        {
            PhotonNetwork.NetworkingClient.EventReceived += PlatformNetwork;
            once_networking = true;
        }
        List<InputDevice> list = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, list);
        list[0].TryGetFeatureValue(CommonUsages.gripButton, out gripDown_left);
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list);
        list[0].TryGetFeatureValue(CommonUsages.gripButton, out gripDown_right);
        if (gripDown_right)
        {
            if (!once_right && jump_right_local == null)
            {
                jump_right_local = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                jump_right_local.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                jump_right_local.transform.localScale = scale;
                jump_right_local.transform.position = new Vector3(0f, -0.0075f, 0f) + GorillaLocomotion.Player.Instance.rightControllerTransform.position;
                jump_right_local.transform.rotation = GorillaLocomotion.Player.Instance.rightControllerTransform.rotation;
                object[] eventContent = new object[2]
                {
                    new Vector3(0f, -0.0075f, 0f) + GorillaLocomotion.Player.Instance.rightControllerTransform.position,
                    GorillaLocomotion.Player.Instance.rightControllerTransform.rotation
                };
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others
                };
                PhotonNetwork.RaiseEvent(70, eventContent, raiseEventOptions, SendOptions.SendReliable);
                once_right = true;
                once_right_false = false;
                ColorChanger colorChanger = jump_right_local.AddComponent<ColorChanger>();
                Gradient gradient = new Gradient();
                gradient.colorKeys = colorKeysPlatformMonke;
                colorChanger.colors = gradient;
                colorChanger.Start();
            }
        }
        else if (!once_right_false && jump_right_local != null)
        {
            UnityEngine.Object.Destroy(jump_right_local);
            jump_right_local = null;
            once_right = false;
            once_right_false = true;
            RaiseEventOptions raiseEventOptions2 = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.Others
            };
            PhotonNetwork.RaiseEvent(72, null, raiseEventOptions2, SendOptions.SendReliable);
        }
        if (gripDown_left)
        {
            if (!once_left && jump_left_local == null)
            {
                jump_left_local = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                jump_left_local.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                jump_left_local.transform.localScale = scale;
                jump_left_local.transform.position = GorillaLocomotion.Player.Instance.leftControllerTransform.position;
                jump_left_local.transform.rotation = GorillaLocomotion.Player.Instance.leftControllerTransform.rotation;
                object[] eventContent2 = new object[2]
                {
                    GorillaLocomotion.Player.Instance.leftControllerTransform.position,
                    GorillaLocomotion.Player.Instance.leftControllerTransform.rotation
                };
                RaiseEventOptions raiseEventOptions3 = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others
                };
                PhotonNetwork.RaiseEvent(69, eventContent2, raiseEventOptions3, SendOptions.SendReliable);
                once_left = true;
                once_left_false = false;
                ColorChanger colorChanger2 = jump_left_local.AddComponent<ColorChanger>();
                Gradient gradient2 = new Gradient();
                gradient2.colorKeys = colorKeysPlatformMonke;
                colorChanger2.colors = gradient2;
                colorChanger2.Start();
            }
        }
        else if (!once_left_false && jump_left_local != null)
        {
            UnityEngine.Object.Destroy(jump_left_local);
            jump_left_local = null;
            once_left = false;
            once_left_false = true;
            RaiseEventOptions raiseEventOptions4 = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.Others
            };
            PhotonNetwork.RaiseEvent(71, null, raiseEventOptions4, SendOptions.SendReliable);
        }
        if (!PhotonNetwork.InRoom)
        {
            for (int i = 0; i < jump_right_network.Length; i++)
            {
                UnityEngine.Object.Destroy(jump_right_network[i]);
            }
            for (int j = 0; j < jump_left_network.Length; j++)
            {
                UnityEngine.Object.Destroy(jump_left_network[j]);
            }
        }
    }

    private static void PlatformNetwork(EventData eventData)
    {
        switch (eventData.Code)
        {
            case 69:
                {
                    object[] array2 = (object[])eventData.CustomData;
                    jump_left_network[eventData.Sender] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    jump_left_network[eventData.Sender].GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                    jump_left_network[eventData.Sender].transform.localScale = scale;
                    jump_left_network[eventData.Sender].transform.position = (Vector3)array2[0];
                    jump_left_network[eventData.Sender].transform.rotation = (Quaternion)array2[1];
                    ColorChanger colorChanger2 = jump_left_network[eventData.Sender].AddComponent<ColorChanger>();
                    Gradient gradient2 = new Gradient();
                    gradient2.colorKeys = colorKeysPlatformMonke;
                    colorChanger2.colors = gradient2;
                    colorChanger2.Start();
                    break;
                }
            case 70:
                {
                    object[] array = (object[])eventData.CustomData;
                    jump_right_network[eventData.Sender] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    jump_right_network[eventData.Sender].GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                    jump_right_network[eventData.Sender].transform.localScale = scale;
                    jump_right_network[eventData.Sender].transform.position = (Vector3)array[0];
                    jump_right_network[eventData.Sender].transform.rotation = (Quaternion)array[1];
                    ColorChanger colorChanger = jump_right_network[eventData.Sender].AddComponent<ColorChanger>();
                    Gradient gradient = new Gradient();
                    gradient.colorKeys = colorKeysPlatformMonke;
                    colorChanger.colors = gradient;
                    colorChanger.Start();
                    break;
                }
            case 71:
                UnityEngine.Object.Destroy(jump_left_network[eventData.Sender]);
                jump_left_network[eventData.Sender] = null;
                break;
            case 72:
                UnityEngine.Object.Destroy(jump_right_network[eventData.Sender]);
                jump_right_network[eventData.Sender] = null;
                break;
        }
    }

    private static void AddButton(float offset, string text)
    {
        GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        gameObject.transform.parent = menu.transform;
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.transform.localScale = new Vector3(0.09f, 0.8f, 0.08f);
        gameObject.transform.localPosition = new Vector3(0.56f, 0f, 0.6f - offset);
        gameObject.AddComponent<BtnCollider>().relatedText = text;
        int num = -1;
        for (int i = 0; i < buttons.Length; i++)
        {
            if (text == buttons[i])
            {
                num = i;
                break;
            }
        }
        if (buttonsActive[num] == false)
        {
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        }
        else if (buttonsActive[num] == true)
        {
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.magenta);
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        }
        GameObject gameObject2 = new GameObject();
        gameObject2.transform.parent = canvasObj.transform;
        Text text2 = gameObject2.AddComponent<Text>();
        text2.font = (Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font);
        if (githubversion == currentverison && githubversion != null)
        {
            text2.text = text;
        }
        else
        {
            text2.text = custommessage;
        }
        text2.fontSize = 1;
        text2.alignment = TextAnchor.MiddleCenter;
        text2.resizeTextForBestFit = true;
        text2.resizeTextMinSize = 0;
        RectTransform component = text2.GetComponent<RectTransform>();
        component.localPosition = Vector3.zero;
        component.sizeDelta = new Vector2(0.2f, 0.03f);
        component.localPosition = new Vector3(0.064f, 0f, 0.237f - offset / 2.55f);
        component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
    }

    

    public static void checkverr()
    {
        WebRequest request = WebRequest.Create("https://pastebin.com/raw/nd32xHsa");
        WebResponse response = request.GetResponse();
        Stream data = response.GetResponseStream();
        string html = String.Empty;
        using (StreamReader sr = new StreamReader(data))
        {
            html = sr.ReadToEnd();
        }
        theactualmenu.githubversion = html;
        new Thread(checkverr).Abort();
    }

    public static void checkmes()
    {
        try
        {
            string aaaaaaaa = webClient.DownloadString("https://pastebin.com/raw/pH6uSMhB");
            custommessage = aaaaaaaa;
        }
        catch (WebException ex)
        {
            File.WriteAllText("ewwrors.log", ex.ToString());
        }
        new Thread(checkmes).Abort();
    }

    public static string custommessage;

    public static string motd;

    public static bool shibaimage = false;

    public static bool binaryimage = false;

    public static void checkmotd()
    {
        WebRequest request = WebRequest.Create("https://shibagt.github.io/motdshit.html");
        WebResponse response = request.GetResponse();
        Stream data = response.GetResponseStream();
        string html = String.Empty;
        using (StreamReader sr = new StreamReader(data))
        {
            html = sr.ReadToEnd();
        }
        theactualmenu.motd = html;
        new Thread(checkmotd).Abort();
    }

    public static bool didmats = false;

    public static bool loadedstuff = false;

    public static Texture2D shibaimagea = new Texture2D(2, 2);

    public static Texture2D binaryimagea = new Texture2D(2, 2);

    public static int platnum = 0;

    public static Color platcolor = Color.black;

    private static void ProcessCustomPlatformMonke()
    {
        if (leftprimarypress)
        {
            platnum++;
            if (platnum == 8)
            {
                platnum = 0;
                platcolor = Color.black;
            } //change to black and 0
            if (platnum == 0)
            {
                platcolor = Color.magenta;
            } //purple
            if (platnum == 1)
            {
                platcolor = Color.green;
            } //green
            if (platnum == 2)
            {
                platcolor = Color.white;
            } //white
            if (platnum == 3)
            {
                platcolor = Color.gray;
            } //gray
            if (platnum == 4)
            {
                platcolor = Color.red;
            } //red
            if (platnum == 5)
            {
                platcolor = Color.yellow;
            } //yellow
            if (platnum == 6)
            {
                platcolor = Color.blue;
            } //blue
            if (platnum == 7)
            {
                platcolor = Color.cyan;
            } //cyan
        }
        if (!once_networking)
        {
            PhotonNetwork.NetworkingClient.EventReceived += PlatformNetwork;
            once_networking = true;
        }
        List<InputDevice> list = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, list);
        list[0].TryGetFeatureValue(CommonUsages.gripButton, out gripDown_left);
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list);
        list[0].TryGetFeatureValue(CommonUsages.gripButton, out gripDown_right);
        if (gripDown_right)
        {
            if (!once_right && jump_right_local == null)
            {
                jump_right_local = GameObject.CreatePrimitive(PrimitiveType.Cube);
                jump_right_local.GetComponent<Renderer>().material.SetColor("_Color", platcolor);
                jump_right_local.transform.localScale = scale;
                jump_right_local.transform.position = new Vector3(0f, -0.0075f, 0f) + GorillaLocomotion.Player.Instance.rightControllerTransform.position;
                jump_right_local.transform.rotation = GorillaLocomotion.Player.Instance.rightControllerTransform.rotation;
                object[] eventContent = new object[]
                {
                new Vector3(0f, -0.0075f, 0f) + GorillaLocomotion.Player.Instance.rightControllerTransform.position,
                GorillaLocomotion.Player.Instance.rightControllerTransform.rotation
                };
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others
                };
                PhotonNetwork.RaiseEvent(70, eventContent, raiseEventOptions, SendOptions.SendReliable);
                once_right = true;
                once_right_false = false;
                ColorChanger colorChanger = jump_right_local.AddComponent<ColorChanger>();
                colorChanger.colors = new Gradient
                {
                    colorKeys = colorKeys
                };
                colorChanger.Start();
            }
        }
        else if (!once_right_false && jump_right_local != null)
        {
            UnityEngine.Object.Destroy(jump_right_local);
            jump_right_local = null;
            once_right = false;
            once_right_false = true;
            RaiseEventOptions raiseEventOptions2 = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.Others
            };
            PhotonNetwork.RaiseEvent(72, null, raiseEventOptions2, SendOptions.SendReliable);
        }
        if (gripDown_left)
        {
            if (!once_left && jump_left_local == null)
            {
                jump_left_local = GameObject.CreatePrimitive(PrimitiveType.Cube);
                jump_left_local.GetComponent<Renderer>().material.SetColor("_Color", platcolor);
                jump_left_local.transform.localScale = scale;
                jump_left_local.transform.position = GorillaLocomotion.Player.Instance.leftControllerTransform.position;
                jump_left_local.transform.rotation = GorillaLocomotion.Player.Instance.leftControllerTransform.rotation;
                object[] eventContent2 = new object[]
                {
                GorillaLocomotion.Player.Instance.leftControllerTransform.position,
                GorillaLocomotion.Player.Instance.leftControllerTransform.rotation
                };
                RaiseEventOptions raiseEventOptions3 = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others
                };
                PhotonNetwork.RaiseEvent(69, eventContent2, raiseEventOptions3, SendOptions.SendReliable);
                once_left = true;
                once_left_false = false;
                ColorChanger colorChanger2 = jump_left_local.AddComponent<ColorChanger>();
                colorChanger2.colors = new Gradient
                {
                    colorKeys = colorKeys
                };
                colorChanger2.Start();
            }
        }
        else if (!once_left_false && jump_left_local != null)
        {
            UnityEngine.Object.Destroy(jump_left_local);
            jump_left_local = null;
            once_left = false;
            once_left_false = true;
            RaiseEventOptions raiseEventOptions4 = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.Others
            };
            PhotonNetwork.RaiseEvent(71, null, raiseEventOptions4, SendOptions.SendReliable);
        }
        if (!PhotonNetwork.InRoom)
        {
            for (int i = 0; i < jump_right_network.Length; i++)
            {
                UnityEngine.Object.Destroy(jump_right_network[i]);
            }
            for (int j = 0; j < jump_left_network.Length; j++)
            {
                UnityEngine.Object.Destroy(jump_left_network[j]);
            }
        }

    }

    public static void Draw()
    {
        if (!didmats)
        {
            new Thread(checkmotd).Start();
            WebClient webClient = new WebClient();
            webClient.DownloadFile("https://cdn.discordapp.com/attachments/1116775529807360003/1117944382969102536/image.png", "shibatheme.jpg");
            webClient.DownloadFile("https://cdn.discordapp.com/attachments/1116775529807360003/1117945320266010684/image.png", "101.jpg");
            Material dacolorfordaboards = new Material(Shader.Find("Standard"));
            dacolorfordaboards.color = Color.black;
            byte[] image = System.IO.File.ReadAllBytes("shibatheme.jpg");
            byte[] image2 = System.IO.File.ReadAllBytes("101.jpg");
            ImageConversion.LoadImage(shibaimagea, image);
            ImageConversion.LoadImage(binaryimagea, image2);
            didmats = true;
            if (File.ReadAllText("lastseenmotd.txt") != theactualmenu.motd)
            {
                GameObject.Find("Level/lower level/StaticUnlit/motdscreen").GetComponent<Renderer>().material = dacolorfordaboards;
                GameObject.Find("Level/lower level/StaticUnlit/motdscreen").GetComponent<Renderer>().material.color = Color.red;
                GameObject.Find("Level/lower level/StaticUnlit/motdscreen").GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                GameObject.Find("Level/lower level/StaticUnlit/motdscreen").GetComponent<Renderer>().material = dacolorfordaboards;
                GameObject.Find("Level/lower level/StaticUnlit/motdscreen").GetComponent<Renderer>().material = dacolorfordaboards;
                GameObject.Find("Level/lower level/StaticUnlit/motdscreen").GetComponent<Renderer>().material.color = Color.black;
            }
        }
        new Thread(checkmes).Start();
        new Thread(checkverr).Start();
        if (!dooncee)
        {
            dooncee = true;
            for (int i = 0; i < GorillaComputer.instance.levelScreens.Length; i++)
            {
                Material dacolorfordaboards = new Material(Shader.Find("Standard"));
                dacolorfordaboards.color = Color.black;
                string announcement = "THANKS FOR USING THE SHIBAGT-Z MENU V9.0!\n\nTHANKS FOR ALL THE SUPPORT!\n\nDISCORD: SHIBAGTMODMENU\n\nTHANKS TO ALL MY BETA TESTERS!\n\nDONT BAN YOURSELF PLS, AND ALSO REVIEW ON YOUTUBE :D";
                GorillaComputer.instance.levelScreens[i].goodMaterial = dacolorfordaboards;
                GameObject.Find("Level/lower level/UI/-- PhysicalComputer UI --/monitor").GetComponent<Renderer>().material = dacolorfordaboards;
                GameObject.Find("Level/lower level/StaticUnlit/screen").GetComponent<Renderer>().material = dacolorfordaboards;
                GameObject.Find("Level/lower level/UI/CodeOfConduct").GetComponent<Text>().text = "[<color=yellow>SHIBAGT-Z NEWS</color>]";
                GameObject.Find("Level/lower level/UI/CodeOfConduct/COC Text").GetComponent<Text>().text = announcement;
                GameObject.Find("Level/lower level/UI/motd").GetComponent<Text>().text = "SHIBAGT-Z MOTD";
                GameObject.Find("Level/lower level/UI/motd/motdtext").GetComponent<Text>().text = theactualmenu.motd;
                File.WriteAllText("lastseenmotd.txt", motd);
            }
        }
        menu = GameObject.CreatePrimitive(PrimitiveType.Cube);
        menu.name = "shibagtrealnoway";
        UnityEngine.Object.Destroy(menu.GetComponent<Rigidbody>());
        UnityEngine.Object.Destroy(menu.GetComponent<BoxCollider>());
        UnityEngine.Object.Destroy(menu.GetComponent<Renderer>());
        menu.transform.localScale = new Vector3(0.1f, 0.3f, 0.4f);
        GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gameObject.name = "shibagt-z";
        UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
        UnityEngine.Object.Destroy(gameObject.GetComponent<BoxCollider>());
        gameObject.transform.parent = menu.transform;
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.transform.localScale = new Vector3(0.1f, 1f, 1f);
        if (shibaimage)
        {
            gameObject.GetComponent<Renderer>().material.mainTexture = theactualmenu.shibaimagea;
        }
        if (binaryimage)
        {
            gameObject.GetComponent<Renderer>().material.mainTexture = theactualmenu.binaryimagea;
        }
        if (!shibaimage && !binaryimage)
        {
            gameObject.GetComponent<Renderer>().material.mainTexture = null;
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", maincolor);
        }
        gameObject.transform.position = new Vector3(0.05f, 0f, 0f);
        canvasObj = new GameObject();
        canvasObj.transform.parent = menu.transform;
        canvasObj.name = "shibagtcanvas";
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        CanvasScaler canvasScaler = canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvasScaler.dynamicPixelsPerUnit = 1000f;
        GameObject gameObject2 = new GameObject();
        gameObject2.transform.parent = canvasObj.transform;
        Text text = gameObject2.AddComponent<Text>();
        text.font = (Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font);
        text.fontStyle = FontStyle.Bold;
        text.text = "ShibaGTs Mod Menu-Z v9.0";
        text.fontSize = 1;
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleCenter;
        text.resizeTextForBestFit = true;
        text.resizeTextMinSize = 0;
        RectTransform component = text.GetComponent<RectTransform>();
        component.localPosition = Vector3.zero;
        component.sizeDelta = new Vector2(0.28f, 0.05f);
        component.position = new Vector3(0.06f, 0f, 0.175f);
        component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
        AddPageButtons();
        string[] array2 = buttons.Skip(pageNumber * pageSize).Take(pageSize).ToArray();
        for (int i = 0; i < array2.Length; i++)
        {
            AddButton((float)i * 0.13f + 0.26f, array2[i]);
        }
        Text text2 = new GameObject
        {
            transform =
        {
            parent = canvasObj.transform
        }
        }.AddComponent<Text>();
        text2.font = (Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font);
        text2.text = "By ShibaGT & lunar";
        text2.fontSize = 1;
        text2.color = Color.red;
        text2.alignment = TextAnchor.UpperLeft;
        text2.resizeTextForBestFit = true;
        text2.resizeTextMinSize = 0;
        RectTransform component2 = text2.GetComponent<RectTransform>();
        component2.localPosition = Vector3.zero;
        component2.sizeDelta = new Vector2(0.28f, 0.05f);
        component2.position = new Vector3(0.06f, -0.01f, -0.24f);
        component2.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
        Text texx = new GameObject
        {
            transform =
        {
            parent = canvasObj.transform
        }
        }.AddComponent<Text>();
        texx.font = (Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font);
        texx.text = "PAGE " + pageNumber;
        texx.fontSize = 1;
        texx.color = Color.yellow;
        texx.alignment = TextAnchor.UpperLeft;
        texx.resizeTextForBestFit = true;
        texx.resizeTextMinSize = 0;
        RectTransform component8 = texx.GetComponent<RectTransform>();
        component8.localPosition = Vector3.zero;
        component8.sizeDelta = new Vector2(0.15f, 0.02f);
        component8.position = new Vector3(0.06f, -0.01f, 0.225f);
        component8.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
        Text text4 = new GameObject
        {
            transform =
        {
            parent = canvasObj.transform
        }
        }.AddComponent<Text>();
        float current = 0;
        current = Time.frameCount / Time.time;
        fps = (int)current;
        text4.font = (Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font);
        text4.text = "      FPS [ " + fps + " ]";
        text4.fontSize = 1;
        text4.color = Color.yellow;
        text4.alignment = TextAnchor.UpperLeft;
        text4.resizeTextForBestFit = true;
        text4.resizeTextMinSize = 0;
        RectTransform component3 = text4.GetComponent<RectTransform>();
        component3.localPosition = Vector3.zero;
        component3.sizeDelta = new Vector2(0.15f, 0.02f);
        component3.position = new Vector3(0.06f, -0.01f, 0.200f);
        component3.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
        Text text3 = new GameObject
        {
            transform =
        {
            parent = canvasObj.transform
        }
        }.AddComponent<Text>();
        text3.font = (Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font);
        text3.text = "discord.gg/shibagtmodmenu";
        text3.fontSize = 1;
        text3.color = Color.blue;
        text3.alignment = TextAnchor.UpperLeft;
        text3.resizeTextForBestFit = true;
        text3.resizeTextMinSize = 0;
        RectTransform component4 = text3.GetComponent<RectTransform>();
        component4.localPosition = Vector3.zero;
        component4.sizeDelta = new Vector2(0.28f, 0.05f);
        component4.position = new Vector3(0.06f, -0.01f, -0.28f);
        component4.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

    }

    public static int fps;

	private static void AddPageButtons()
	{
        int num = (buttons.Length + pageSize - 1) / pageSize;
        int num2 = pageNumber + 1;
        int num3 = pageNumber - 1;
        if (num2 > num - 1)
        {
            num2 = 0;
        }
        if (num3 < 0)
        {
            num3 = num - 1;
        }
        float num4 = 0f;
        GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        gameObject.transform.parent = menu.transform;
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.transform.localScale = new Vector3(0.045f, 0.25f, 0.25f);
        gameObject.transform.localPosition = new Vector3(0.56f, 0.37f, 0.645f - num4);
        gameObject.AddComponent<BtnCollider>().relatedText = "PreviousPage";
        gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        num4 = 0.13f;
        GameObject gameObject3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        UnityEngine.Object.Destroy(gameObject3.GetComponent<Rigidbody>());
        gameObject3.GetComponent<BoxCollider>().isTrigger = true;
        gameObject3.transform.parent = menu.transform;
        gameObject3.transform.rotation = Quaternion.identity;
        gameObject3.transform.localScale = new Vector3(0.045f, 0.25f, 0.25f);
        gameObject3.transform.localPosition = new Vector3(0.56f, -0.37f, 0.775f - num4);
        gameObject3.AddComponent<BtnCollider>().relatedText = "NextPage";
        gameObject3.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        num4 = 0.26f;
        GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        UnityEngine.Object.Destroy(gameObject3.GetComponent<Rigidbody>());
        gameObject2.GetComponent<BoxCollider>().isTrigger = true;
        gameObject2.transform.parent = menu.transform;
        gameObject2.transform.rotation = Quaternion.identity;
        gameObject2.transform.localScale = new Vector3(0.045f, 0.55f, 0.16f);
        gameObject2.transform.localPosition = new Vector3(0.56f, -0.8f, 0.35f - num4);
        gameObject2.AddComponent<BtnCollider>().relatedText = "DisconnectingButton";
        gameObject2.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        GameObject gameObject4 = new GameObject();
        gameObject4.transform.parent = canvasObj.transform;
        Text text2 = gameObject4.AddComponent<Text>();
        text2.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        text2.text = "Disconnect";
        text2.fontSize = 1;
        text2.alignment = TextAnchor.MiddleCenter;
        text2.resizeTextForBestFit = true;
        text2.resizeTextMinSize = 0;
        RectTransform component2 = text2.GetComponent<RectTransform>();
        component2.localPosition = Vector3.zero;
        component2.sizeDelta = new Vector2(0.2f, 0.03f);
        component2.localPosition = new Vector3(0.06f, -0.24f, 0.14f - num4 / 2.55f);
        component2.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
    }

    public static bool dooncee = false;

	public static void Toggle(string relatedText)
	{
		int num = (buttons.Length + pageSize - 1) / pageSize;
		if (relatedText == "NextPage")
		{
			if (pageNumber < num - 1)
			{
				pageNumber++;
			}
			else
			{
				pageNumber = 0;
			}
			UnityEngine.Object.Destroy(menu);
			menu = null;
			Draw();
			return;
		}
        if (relatedText == "DisconnectingButton")
        {
            PhotonNetwork.Disconnect();
        }
        if (relatedText == "PanicThingy")
        {
            turnoffallmods();
        }
        if (relatedText == "PreviousPage")
		{
			if (pageNumber > 0)
			{
				pageNumber--;
			}
			else
			{
				pageNumber = num - 1;
			}
			UnityEngine.Object.Destroy(menu);
			menu = null;
			Draw();
			return;
		}
		int num2 = -1;
		for (int i = 0; i < buttons.Length; i++)
		{
			if (relatedText == buttons[i])
			{
				num2 = i;
				break;
			}
		}
		if (buttonsActive[num2].HasValue)
		{
			buttonsActive[num2] = !buttonsActive[num2];
			UnityEngine.Object.Destroy(menu);
			menu = null;
			Draw();
		}
	}
}