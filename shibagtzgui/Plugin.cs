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

namespace shibagtzgui
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        string stringthing;
        bool disconnect;
        bool panic;
        bool onn = true;
        bool enable;
        bool Bubble;
        public static bool head;
        bool lobbyjoin;
        bool crashall = false;
        bool tagall;
        bool trapall;
        bool antimodcheck = false;
        bool nameset;

        void Start()
        {
            /* A lot of Gorilla Tag systems will not be set up when start is called /*
			/* Put code in OnGameInitialized to avoid null references */
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
                GUI.Box(new Rect(15, 15, 350, 300), "ShibaGT-Z GUI");

                GUI.color = Color.black;
                GUI.color = Color.white;
                disconnect = GUI.Button(new Rect(25, 45, 100, 25), "Disconnect");
                if (disconnect)
                {
                    PhotonNetwork.Disconnect();
                }
                stringthing = GUI.TextArea(new Rect(25, 75, 100, 25), stringthing);
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
                    foreach (Photon.Realtime.Player player2 in PhotonNetwork.PlayerList)
                    {
                        PhotonView photonView = GorillaGameManager.instance.FindVRRigForPlayer(player2);
                        if (photonView != null)
                        {
                            photonView.RPC("PlayHandTap", RpcTarget.All, new object[]
                            {
                                    84,
                                    false,
                                    100f
                            });
                        }
                    }
                }
                enable = GUI.Toggle(new Rect(125, 45, 100, 25), enable, "wasd[C: KMAN]");
                if (enable)
                {
                    wasdd();
                }
                crashall = GUI.Toggle(new Rect(125, 75, 100, 25), crashall, "lag all [ud]");
                if (crashall)
                {
                    new Thread(MenuPatch.lagservLUNAR).Start();
                }
                else
                {
                    new Thread(MenuPatch.lagservLUNAR).Abort();
                }
                antimodcheck = GUI.Toggle(new Rect(125, 105, 100, 25), antimodcheck, "anti mod check [ud]");
                if (antimodcheck)
                {
                    
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
