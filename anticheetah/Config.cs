    using Il2CppSkateMenu.Config;

namespace anticheetah;

internal class Config
{
	public static SkateValue<bool> OverrideAntiaim = new SkateValue<bool>(false);

	public static SkateValue<bool> ForceRageMode = new SkateValue<bool>(false);

	public static int PitchType = 0;

	public static int YawType = 0;
}