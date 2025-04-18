    using System;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSkateMenu.Component;
using Il2CppSkateMenu.Config;
using Il2CppSkateMenu.Content;
using Il2CppSystem;
using MelonLoader;

namespace anticheetah.Menu;

internal class InjectedCategory : CategorySimple
{
	public InjectedCategory(IntPtr ptr)
		: base(ptr)
	{
	}

	public InjectedCategory()
		: base(ClassInjector.DerivedConstructorPointer<InjectedCategory>())
	{
		ClassInjector.DerivedConstructorBody((Il2CppObjectBase)(object)this);
	}

	public override void SetupPage(ComponentList list)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		MelonLogger.Msg("InjectedCategory::SetupPage");
		PageSection val = list.CreateSection(AnchorHeight.AutoSize(0.5f, 0f), "hack");
		val.List.CreateCheckbox("override anti-aim (spinbot required)", Config.OverrideAntiaim);
		val.List.CreateCombo("pitch type", DelegateSupport.ConvertDelegate<Func<int>>((Delegate)(Func<int>)(() => Config.PitchType)), DelegateSupport.ConvertDelegate<Action<int>>((Delegate)(Action<int>)delegate(int set)
		{
			Config.PitchType = set;
		}), new Il2CppStringArray(new string[5] { "down", "up", "zero", "jitter", "none" }));
		val.List.CreateCombo("yaw type", DelegateSupport.ConvertDelegate<Func<int>>((Delegate)(Func<int>)(() => Config.YawType)), DelegateSupport.ConvertDelegate<Action<int>>((Delegate)(Action<int>)delegate(int set)
		{
			Config.YawType = set;
		}), new Il2CppStringArray(new string[6] { "backwards", "left", "right", "forward", "jitter (left/right)", "jitter (forward/backward)" }));
		val.List.CreateCheckbox("force rage mode always (aka rage in legit lobbies)", Config.ForceRageMode);
		PageSection obj = list.CreateSection(AnchorHeight.AutoSize(0.5f, 0f), "hidden features");
		obj.List.CreateCheckbox("autofire", CheatConfig.Rage.AimbotBase.EnableAutoFire);
		obj.List.CreateSlider("minimum dmg", CheatConfig.Rage.AimbotBase.MinimumDamage, 0f, 100f, "F1");
		obj.List.CreateSlider("minimum hitchance", CheatConfig.Rage.AimbotBase.MinimumHitChance, 0f, 100f, "F2");
		obj.List.CreateCheckbox("freestanding", CheatConfig.Rage.Movement.EnableFreestanding);
		obj.List.CreateSlider("freestanding resolution", CheatConfig.Rage.Movement.FreestandingResolution, 0f, 50f, "F0");
		obj.List.CreateCheckbox("strafe optimizer", CheatConfig.Rage.Movement.EnableStrafeOptimizer);
	}
}
    