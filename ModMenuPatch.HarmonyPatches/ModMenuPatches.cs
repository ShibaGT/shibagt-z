using System.Reflection;
using HarmonyLib;

namespace ModMenuPatch.HarmonyPatches;

public class ModMenuPatches
{
	private static Harmony instance;

	public const string InstanceId = "com.shibagt.menu.shibagtz";

	public static bool IsPatched { get; private set; }

	internal static void ApplyHarmonyPatches()
	{
		if (!IsPatched)
		{
			if (instance == null)
			{
				instance = new Harmony("com.shibagt.menu.shibagtz");
			}
			instance.PatchAll(Assembly.GetExecutingAssembly());
			IsPatched = true;
		}
	}

	internal static void RemoveHarmonyPatches()
	{
		if (instance != null && IsPatched)
		{
			instance.UnpatchSelf();
			IsPatched = false;
		}
	}
}
