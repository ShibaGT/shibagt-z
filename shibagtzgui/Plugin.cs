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
using Utilla;

namespace shibagtzgui
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;
        string stringthing;
        bool disconnect;
        bool panic;
        bool Bubble;
        bool lowquality;
        public static bool head;
        bool lobbyjoin;
        WebClient webclient;
        bool tagall;
        bool trapall;
        bool nameset;

        void Start()
        {
            /* A lot of Gorilla Tag systems will not be set up when start is called /*
			/* Put code in OnGameInitialized to avoid null references */

            Utilla.Events.GameInitialized += OnGameInitialized;
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
            tagall = GUI.Toggle(new Rect(25, 165, 100, 25), tagall ,"Tag All");
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
            /* Code here runs every frame when the mod is enabled */
        }

        /* This attribute tells Utilla to call this method when a modded room is joined */
        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            /* Activate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = true;
        }

        /* This attribute tells Utilla to call this method when a modded room is left */
        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            /* Deactivate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = false;
        }
    }
}
