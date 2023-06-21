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
using static ModMenuPatch.HarmonyPatches.theactualmenu;
using System.Threading;
using UnityEngine.UI;
using ExitGames.Client.Photon.StructWrapping;

namespace shibagtzgui
{
    [BepInPlugin("com.shibagt.water", "shibagtwater", "1.0.0")]
    public class waterhandler : BaseUnityPlugin
    {
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

        void Update()
        {
            if (theactualmenu.disablewater)
            {
                int defaul2 = LayerMask.NameToLayer("TransparentFX");
                GameObject.Find("OceanWater").layer = defaul2;
            }

            if (theactualmenu.walkonwater)
            {
                int defaul2 = LayerMask.NameToLayer("Default");
                GameObject.Find("OceanWater").layer = defaul2;
            }

            if (theactualmenu.fixwater)
            {
                int defaul2 = LayerMask.NameToLayer("Water");
                GameObject.Find("OceanWater").layer = defaul2;
            }
        }
    }
}
