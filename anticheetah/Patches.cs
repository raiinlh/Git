    using System.Collections.Generic;
using System.Linq;
using Il2CppDataStructs;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSkateMenu;
using Il2CppSkateMenu.Component;
using Il2CppSkateMenu.Config;
using Il2CppSkateMenu.Content;
using MelonLoader;
using anticheetah.Menu;

namespace anticheetah;

public static class Patches
{
	public static void InjectCategory(ref ImguiManager __instance)
	{
		List<CategoryBase> list = ((IEnumerable<CategoryBase>)__instance.categories).ToList();
		ClassInjector.RegisterTypeInIl2Cpp<InjectedCategory>();
		list.Add((CategoryBase)(object)new InjectedCategory());
		__instance.categories = Il2CppReferenceArray<CategoryBase>.op_Implicit(list.ToArray());
		MelonLogger.Msg("Injected category");
	}

	public static void CorrectLocalize(ref CategoryTab __instance, ref CategoryBase category, int categoryIndex)
	{
		MelonLogger.Msg($"CategoryTab init (id: {categoryIndex}) -> {category.contentName}");
		if (categoryIndex == 4)
		{
			MelonLogger.Msg("Correcting injected category");
			category.contentName = "INJECT##uc hack";
		}
	}

	public static void InjectTranslation(ref string __result)
	{
		if (!__result.StartsWith("No translation found for"))
		{
			return;
		}
		int num = __result.IndexOf('\'') + 1;
		int num2 = __result.LastIndexOf('\'');
		if (num > 0 && num2 > num)
		{
			string text = __result.Substring(num, num2 - num);
			if (text.StartsWith("INJECT##"))
			{
				MelonLogger.Msg("Injecting localized string: " + text.Substring(8));
				__result = text.Substring(8);
			}
		}
	}

	public static void InterceptSliderFloat(ComponentList __instance, ref string label, ref SkateValue<float> value, ref float min, ref float max)
	{
		if (label.Contains("FOV") && max <= 180f)
		{
			MelonLogger.Msg("Unlocking fov slider");
			max = 360f;
		}
	}

	public static bool RebuiltMinDmgThreshold(ref int __result, AimbotValues aimbot, bool manual)
	{
		if (manual)
		{
			__result = 1;
			return false;
		}
		__result = (int)aimbot.MinimumDamage.value;
		return false;
	}

	public static bool ForceRageMode(ref CheatMode __result)
	{
		if (!Config.ForceRageMode.GetActiveValue())
		{
			return true;
		}
		__result = (CheatMode)1;
		return false;
	}

	public static bool FreestandingGetter(ref bool __result)
	{
		__result = CheatConfig.Rage.Movement.EnableFreestanding.GetActiveValue();
		return false;
	}

	public static bool StrafeOptimizerGetter(ref bool __result)
	{
		__result = CheatConfig.Rage.Movement.EnableStrafeOptimizer.GetActiveValue();
		return false;
	}
}