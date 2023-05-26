using GorillaLocomotion;
using HarmonyLib;

namespace ModMenuPatch.HarmonyPatches;

[HarmonyPatch(typeof(GorillaLocomotion.Player))]
[HarmonyPatch("LateUpdate", MethodType.Normal)]
internal class JumpPatch
{
	private static void Prefix()
	{
	}
}
