    using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Il2CppGame.Player;
using Il2CppGame.Player.Aimbot;
using Il2CppGlobalGame;
using Il2CppSkateMenu;
using Il2CppSkateMenu.Component;
using Il2CppSkateMenu.Config;
using MelonLoader;
using UnityEngine.Localization.Settings;

namespace anticheetah;

public class anticheetah : MelonMod
{
	private static readonly Harmony HarmonyInstance = new Harmony("anticheetah");

	public override void OnInitializeMelon()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Expected O, but got Unknown
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Expected O, but got Unknown
		HarmonyInstance.Patch((MethodBase)typeof(ImguiManager).GetMethod("InitializeSingleton"), new HarmonyMethod(typeof(Patches), "InjectCategory", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null);
		foreach (MethodInfo item in from m in typeof(LocalizedStringDatabase).GetMethods()
			where m.Name == "GetLocalizedString"
			select m)
		{
			MelonLogger.Msg(ConsoleColor.Blue, $"Patching GetLocalizedString (param count: {item.GetParameters().Length})");
			HarmonyInstance.Patch((MethodBase)item, (HarmonyMethod)null, new HarmonyMethod(typeof(Patches), "InjectTranslation", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null);
		}
		HarmonyInstance.Patch((MethodBase)typeof(CategoryTab).GetMethod("Initialize"), new HarmonyMethod(typeof(Patches), "CorrectLocalize", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null);
		HarmonyInstance.Patch((MethodBase)typeof(ComponentList).GetMethod("CreateSlider", new Type[5]
		{
			typeof(string),
			typeof(SkateValue<float>),
			typeof(float),
			typeof(float),
			typeof(string)
		}), new HarmonyMethod(typeof(Patches), "InterceptSliderFloat", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null);
		HarmonyInstance.Patch((MethodBase)typeof(Ragebot).GetMethod("GetMinDamageThreshold"), new HarmonyMethod(typeof(Patches), "RebuiltMinDmgThreshold", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null);
		HarmonyInstance.Patch((MethodBase)typeof(RoomState).GetProperty("CheatMode").GetGetMethod(), new HarmonyMethod(typeof(Patches), "ForceRageMode", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null);
		HarmonyInstance.Patch((MethodBase)typeof(Freestanding).GetProperty("Enabled").GetGetMethod(), new HarmonyMethod(typeof(Patches), "FreestandingGetter", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null);
		HarmonyInstance.Patch((MethodBase)typeof(StrafeOptimizer).GetProperty("Enabled").GetGetMethod(), new HarmonyMethod(typeof(Patches), "StrafeOptimizerGetter", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null);
		NativeHooks.Perform();
	}
}
    