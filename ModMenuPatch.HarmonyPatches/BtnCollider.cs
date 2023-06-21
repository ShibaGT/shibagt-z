using UnityEngine;

namespace ModMenuPatch.HarmonyPatches;

internal class BtnCollider : MonoBehaviour
{
	public string relatedText;

	private void OnTriggerEnter(Collider collider)
	{
		if (Time.frameCount >= theactualmenu.framePressCooldown + 30)
		{
			theactualmenu.Toggle(relatedText);
			theactualmenu.framePressCooldown = Time.frameCount;
            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, false, 0.25f);
        }
	}
}
