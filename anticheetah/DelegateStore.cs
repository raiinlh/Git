    using System;
using System.Runtime.InteropServices;
using Il2CppGame.Player;

namespace anticheetah;

internal static class DelegateStore
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate IntPtr ApplyRagebot(IntPtr _this, IntPtr moveDataPtr, byte willGetConsumed, byte willGetConsumedIfPrimaryDown, bool* overwritePrimaryDown, IntPtr nativeMethodInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate void SpinbotAm(IntPtr _this, IntPtr moveDataPtr, InputModifier* inputPtr, IntPtr methodInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate byte ShouldApplyRagebot(IntPtr moveDataPtr, IntPtr weaponPtr, IntPtr* configPtr, IntPtr methodInfo);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate byte DeduplicatedMethod(IntPtr a1, IntPtr a2, IntPtr a3);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate byte PreprocessRagebot(IntPtr moveDataPtr, IntPtr methodPtr);
}
    