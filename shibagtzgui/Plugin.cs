using BepInEx;
using ExitGames.Client.Photon;
using GorillaNetworking;
using ModMenuPatch.HarmonyPatches;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using UnityEngine;
using UnityEngine.XR;
using static ModMenuPatch.HarmonyPatches.MenuPatch;
using System.Threading;
using UnityEngine.UI;
using ExitGames.Client.Photon.StructWrapping;

namespace shibagtzgui
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        string stringthing;
        bool disconnect;
        bool esp = false;
        bool panic;
        bool onn = true;
        bool enable;
        bool afk = false;
        bool Bubble;
        public static bool head;
        bool lobbyjoin;
        bool crashall = false;
        bool tagall;
        bool loadstump = false;
        public static bool fpc = false;
        bool trapall;
        bool aura = false;
        bool antimodcheck = false;
        bool nameset;

        void Start()
        {
            
        }

        void OnEnable()
        {
            /* Set up your mod here */
            /* Code here runs at the start and whenever your mod is enabled*/

            HarmonyPatches.ApplyHarmonyPatches();
        }

        void OnDisable()
        {
            /* Undo mod setup here */
            /* This provides support for toggling mods with ComputerInterface, please implement it :) */
            /* Code here runs whenever your mod is disabled (including if it disabled on startup)*/

            HarmonyPatches.RemoveHarmonyPatches();
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            /* Code here runs after the game initializes (i.e. GorillaLocomotion.Player.Instance != null) */
        }

        void OnGUI()
        {
            if (onn)
            {
                GUI.color = Color.white;
                GUI.Box(new Rect(15, 15, 350, 300), "ShibaGT-Z GUI (F2 to hide)");

                GUI.color = Color.black;
                GUI.color = Color.white;
                disconnect = GUI.Button(new Rect(25, 45, 100, 25), "Disconnect");
                if (disconnect)
                {
                    PhotonNetwork.Disconnect();
                }
                stringthing = GUI.TextField(new Rect(25, 75, 100, 25), stringthing);
                lobbyjoin = GUI.Button(new Rect(25, 105, 100, 25), "Join Lobby");
                if (lobbyjoin)
                {
                    PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(stringthing);
                }
                nameset = GUI.Button(new Rect(25, 135, 100, 25), "Set Name");
                if (nameset)
                {
                    PhotonNetwork.LocalPlayer.NickName = stringthing;
                    PhotonNetwork.NickName = stringthing;
                    PlayerPrefs.SetString("playerName", stringthing);
                    GorillaComputer.instance.currentName = stringthing;
                    GorillaComputer.instance.offlineVRRigNametagText.text = stringthing;
                    PlayerPrefs.Save();
                }
                tagall = GUI.Toggle(new Rect(25, 165, 100, 25), tagall, "Tag All");
                if (tagall)
                {
                    TagAll();
                }
                panic = GUI.Toggle(new Rect(25, 195, 100, 25), panic, "have panic attac");
                if (panic)
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
                head = GUI.Toggle(new Rect(25, 225, 100, 25), head, "head spin");
                if (head)
                {
                    headspinny();
                }
                trapall = GUI.Toggle(new Rect(25, 255, 100, 25), trapall, "trap all modders");
                if (trapall)
                {
                    MenuPatch.trapallmodders();
                }
                Bubble = GUI.Toggle(new Rect(25, 285, 100, 25), Bubble, "bubble pop spam");
                if (Bubble)
                {
                    PlaySoundInteger(84);
                }
                enable = GUI.Toggle(new Rect(125, 45, 100, 25), enable, "wasd[C: KMAN]");
                if (enable)
                {
                    wasdd();
                }
                crashall = GUI.Toggle(new Rect(125, 75, 100, 25), crashall, "lag all [ud]");
                if (crashall)
                {
                    lagservLUNAR();
                }
                antimodcheck = GUI.Toggle(new Rect(125, 105, 200, 25), antimodcheck, "antimodcheck (check plugins)");
                if (antimodcheck)
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
                aura = GUI.Toggle(new Rect(125, 135, 100, 25), aura, "tag aura");
                if (aura)
                {
                    MenuPatch.ProcessTagAura();
                }
                esp = GUI.Toggle(new Rect(125, 165, 100, 25), esp, "esp");
                if (esp)
                {
                    homemadeesp();
                }
                afk = GUI.Toggle(new Rect(125, 195, 100, 25), afk, "disable afk kick");
                if (afk)
                {
                    disableafk();
                }
            }
        }
        public static void headspinny()
        {
            if (PhotonNetwork.InRoom)
            {
                GorillaTagger.Instance.myVRRig.head.trackingRotationOffset.y += 15f;
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.y = 0.0f;
            }
        }

        public static void wasdd()
        {
            if (UnityInput.Current.GetKey(KeyCode.W))
            {
                GorillaLocomotion.Player.Instance.transform.position += GorillaLocomotion.Player.Instance.headCollider.transform.forward * Time.deltaTime * 5;
            }
            if (UnityInput.Current.GetKey(KeyCode.S))
            {
                GorillaLocomotion.Player.Instance.transform.position += GorillaLocomotion.Player.Instance.headCollider.transform.forward * Time.deltaTime * -5;
            }
            if (UnityInput.Current.GetKey(KeyCode.D))
            {
                GorillaLocomotion.Player.Instance.transform.position += GorillaLocomotion.Player.Instance.headCollider.transform.right * Time.deltaTime * 5;
            }
            if (UnityInput.Current.GetKey(KeyCode.A))
            {
                GorillaLocomotion.Player.Instance.transform.position += GorillaLocomotion.Player.Instance.headCollider.transform.right * Time.deltaTime * -5;
            }
            if (UnityInput.Current.GetKey(KeyCode.LeftArrow))
            {
                GorillaLocomotion.Player.Instance.transform.Rotate(0f, -1f, 0f);
            }
            if (UnityInput.Current.GetKey(KeyCode.RightArrow))
            {
                GorillaLocomotion.Player.Instance.transform.Rotate(0f, 1f, 0f);
            }
            if (UnityInput.Current.GetKey(KeyCode.LeftControl))
            {
                GorillaLocomotion.Player.Instance.transform.position += GorillaLocomotion.Player.Instance.headCollider.transform.up * Time.deltaTime * -5;
            }
            if (UnityInput.Current.GetKey(KeyCode.Space))
            {
                GorillaLocomotion.Player.Instance.transform.position += GorillaLocomotion.Player.Instance.headCollider.transform.up * Time.deltaTime * 5;
            }
            GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        private static GradientColorKey[] colorKeysPlatformMonke = new GradientColorKey[4];

        public static void beacons()
        {
            foreach (VRRig vrrig in (VRRig[])UnityEngine.Object.FindObjectsOfType(typeof(VRRig)))
            {
                if (!vrrig.isOfflineVRRig && !vrrig.isMyPlayer && !vrrig.photonView.IsMine)
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

        public static void TagAll()
        {
            if (MenuPatch.btnCooldown == 0)
            {
                foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
                {
                    if (!MenuPatch.FindVRRigForPlayer(player).mainSkin.material.name.Contains("fected") && GorillaTagger.Instance.myVRRig.mainSkin.material.name.Contains("fected"))
                    {
                        GorillaTagger.Instance.myVRRig.enabled = false;
                        GorillaTagger.Instance.myVRRig.transform.position = MenuPatch.FindVRRigForPlayer(player).transform.position;
                        PhotonView.Get(GorillaGameManager.instance.GetComponent<GorillaGameManager>()).RPC("ReportTagRPC", RpcTarget.MasterClient, new object[]
                        {
                            player
                        });
                        GorillaTagger.Instance.myVRRig.enabled = true;
                    }
                    else
                    {
                        PhotonView.Get(GorillaGameManager.instance.GetComponent<GorillaGameManager>()).RPC("ReportTagRPC", RpcTarget.MasterClient, new object[]
                        {
                            player
                        });
                    }
                }
            }
        }

        public static int pastquality = QualitySettings.GetQualityLevel();

        void Update()
        {
            
            if (UnityInput.Current.GetKey(KeyCode.F2))
            {
                onn = !onn;
                Thread.Sleep(250);
            }
        }
    }
}
