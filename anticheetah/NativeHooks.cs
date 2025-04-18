    using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2Cpp;
using Il2CppDataStructs;
using Il2CppGame.Player;
using Il2CppGame.Player.Aimbot;
using Il2CppInterop.Common;
using MelonLoader;
using MelonLoader.NativeUtils;

namespace anticheetah;

internal class NativeHooks
{
	private static readonly NativeHook<DelegateStore.ApplyRagebot> ApplyRagebotHook = new NativeHook<DelegateStore.ApplyRagebot>();

	private static readonly NativeHook<DelegateStore.SpinbotAm> SpinbotAmHook = new NativeHook<DelegateStore.SpinbotAm>();

	private static readonly NativeHook<DelegateStore.ShouldApplyRagebot> ShouldApplyRagebotHook = new NativeHook<DelegateStore.ShouldApplyRagebot>();

	private static readonly NativeHook<DelegateStore.DeduplicatedMethod> MergedHook = new NativeHook<DelegateStore.DeduplicatedMethod>();

	private static readonly NativeHook<DelegateStore.PreprocessRagebot> PreprocessRagebotHook = new NativeHook<DelegateStore.PreprocessRagebot>();

	private static bool CatchMergedCall = false;

	private static bool pitchJitterFlip = false;

	private static bool yawJitterFlip = false;

	public static void Perform()
	{
		HookApplyRagebot();
		HookSpinbotAm();
		HookDeduplicated();
		HookPreprocessRagebot();
	}

	private unsafe static void HookApplyRagebot()
	{
		IntPtr target = *(IntPtr*)(void*)(IntPtr)Il2CppInteropUtils.GetIl2CppMethodInfoPointerFieldForGeneratedMethod((MethodBase)typeof(Ragebot).GetMethod("ApplyRagebot")).GetValue(null);
		IntPtr detour = (IntPtr)(delegate*<IntPtr, IntPtr, byte, byte, bool*, IntPtr, IntPtr>)(&ApplyRagebotDetour);
		ApplyRagebotHook.Target = target;
		ApplyRagebotHook.Detour = detour;
		MelonLogger.Msg(ConsoleColor.Blue, $"Hooking Ragebot::ApplyRagebot @ 0x{target.ToInt64():X}");
		ApplyRagebotHook.Attach();
		MelonLogger.Msg(ConsoleColor.Green, "Successfully hooked Ragebot::ApplyRagebot");
	}

	private unsafe static void HookSpinbotAm()
	{
		IntPtr target = *(IntPtr*)(void*)(IntPtr)Il2CppInteropUtils.GetIl2CppMethodInfoPointerFieldForGeneratedMethod((MethodBase)typeof(Spinbot).GetMethod("ApplyManipulation")).GetValue(null);
		IntPtr detour = (IntPtr)(delegate*<IntPtr, IntPtr, InputModifier*, IntPtr, void>)(&SpinbotAmDetour);
		SpinbotAmHook.Target = target;
		SpinbotAmHook.Detour = detour;
		MelonLogger.Msg(ConsoleColor.Blue, $"Hooking Spinbot::ApplyManipulation @ 0x{target.ToInt64():X}");
		SpinbotAmHook.Attach();
		MelonLogger.Msg(ConsoleColor.Green, "Successfully hooked Spinbot::ApplyManipulation.");
	}

	private unsafe static void HookDeduplicated()
	{
		IntPtr target = *(IntPtr*)(void*)(IntPtr)Il2CppInteropUtils.GetIl2CppMethodInfoPointerFieldForGeneratedMethod((MethodBase)typeof(CheatModeExtensions).GetMethod("IsLawlessEnabled")).GetValue(null);
		IntPtr detour = (IntPtr)(delegate*<IntPtr, IntPtr, IntPtr, byte>)(&DeduplicatedFix);
		MergedHook.Target = target;
		MergedHook.Detour = detour;
		MelonLogger.Msg(ConsoleColor.Blue, $"Hooking MergedFunction @ 0x{target.ToInt64():X}");
		MergedHook.Attach();
		MelonLogger.Msg(ConsoleColor.Green, "Successfully hooked MergedFunction.");
	}

	private unsafe static void HookPreprocessRagebot()
	{
		IntPtr target = *(IntPtr*)(void*)(IntPtr)Il2CppInteropUtils.GetIl2CppMethodInfoPointerFieldForGeneratedMethod((MethodBase)typeof(Ragebot).GetMethod("PreprocessRagebot")).GetValue(null);
		IntPtr detour = (IntPtr)(delegate*<IntPtr, IntPtr, void>)(&PreprocessRagebotDetour);
		PreprocessRagebotHook.Target = target;
		PreprocessRagebotHook.Detour = detour;
		MelonLogger.Msg(ConsoleColor.Blue, $"Hooking Ragebot::PreprocessRagebot @ 0x{target.ToInt64():X}");
		PreprocessRagebotHook.Attach();
		MelonLogger.Msg(ConsoleColor.Green, "Successfully hooked Ragebot::PreprocessRagebot");
	}

	[UnmanagedCallersOnly(CallConvs = new Type[] { typeof(CallConvCdecl) })]
	private unsafe static IntPtr ApplyRagebotDetour(IntPtr _this, IntPtr moveDataPtr, byte willGetConsumed, byte willGetConsumedIfPrimaryDown, bool* overwritePrimaryDown, IntPtr nativeMethodInfo)
	{
		CatchMergedCall = true;
		IntPtr result = ApplyRagebotHook.Trampoline(_this, moveDataPtr, willGetConsumed, willGetConsumedIfPrimaryDown, overwritePrimaryDown, nativeMethodInfo);
		CatchMergedCall = false;
		return result;
	}

	[UnmanagedCallersOnly(CallConvs = new Type[] { typeof(CallConvCdecl) })]
	private unsafe static void SpinbotAmDetour(IntPtr _this, IntPtr moveData, InputModifier* input, IntPtr methodInfo)
	{
		if (!Config.OverrideAntiaim.GetActiveValue())
		{
			SpinbotAmHook.Trampoline(_this, moveData, input, methodInfo);
			return;
		}
		((InputModifier)input).Pitch = Config.PitchType switch
		{
			0 => Constants.AIM_PITCH_MAX, 
			1 => Constants.AIM_PITCH_MIN, 
			2 => 0f, 
			3 => pitchJitterFlip ? Constants.AIM_PITCH_MIN : Constants.AIM_PITCH_MAX, 
			_ => ((InputModifier)input).Pitch, 
		};
		pitchJitterFlip = !pitchJitterFlip;
		byte back = InputModifier.Back;
		byte forward = InputModifier.Forward;
		byte left = InputModifier.Left;
		byte right = InputModifier.Right;
		switch (Config.YawType)
		{
		case 0:
			((InputModifier)input).Yaw = (float)((double)(int)left * 2.8125 + (double)((InputModifier)input).backingYaw);
			((InputModifier)input).RotateInputDirection((int)left);
			break;
		case 1:
			((InputModifier)input).Yaw = (float)((double)(int)back * 2.8125 + (double)((InputModifier)input).backingYaw);
			((InputModifier)input).RotateInputDirection((int)back);
			break;
		case 2:
			((InputModifier)input).Yaw = (float)((double)(int)forward * 2.8125 + (double)((InputModifier)input).backingYaw);
			((InputModifier)input).RotateInputDirection((int)forward);
			break;
		case 3:
			((InputModifier)input).Yaw = (float)((double)(int)right * 2.8125 + (double)((InputModifier)input).backingYaw);
			((InputModifier)input).RotateInputDirection((int)right);
			break;
		case 4:
			yawJitterFlip = !yawJitterFlip;
			((InputModifier)input).Yaw = (yawJitterFlip ? ((float)((double)(int)back * 2.8125 + (double)((InputModifier)input).backingYaw)) : ((float)((double)(int)forward * 2.8125 + (double)((InputModifier)input).backingYaw)));
			((InputModifier)input).RotateInputDirection((int)(yawJitterFlip ? back : forward));
			break;
		case 5:
			yawJitterFlip = !yawJitterFlip;
			((InputModifier)input).Yaw = (yawJitterFlip ? ((float)((double)(int)right * 2.8125 + (double)((InputModifier)input).backingYaw)) : ((float)((double)(int)left * 2.8125 + (double)((InputModifier)input).backingYaw)));
			((InputModifier)input).RotateInputDirection((int)(yawJitterFlip ? right : left));
			break;
		}
	}

	[UnmanagedCallersOnly(CallConvs = new Type[] { typeof(CallConvCdecl) })]
	private static byte DeduplicatedFix(IntPtr a1, IntPtr a2, IntPtr a3)
	{
		return CatchMergedCall ? ((byte)1) : ((byte)0);
	}

	[UnmanagedCallersOnly(CallConvs = new Type[] { typeof(CallConvCdecl) })]
	private static void PreprocessRagebotDetour(IntPtr moveDataPtr, IntPtr methodPtr)
	{
		CatchMergedCall = true;
		PreprocessRagebotHook.Trampoline(moveDataPtr, methodPtr);
		CatchMergedCall = false;
	}
}
    