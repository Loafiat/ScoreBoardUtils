using HarmonyLib;

namespace ScoreboardUtils
{
    [HarmonyPatch(typeof(GorillaPlayerScoreboardLine), "UpdatePlayerText")]
    public class LinePatch
    {
        static void Postfix(GorillaPlayerScoreboardLine __instance)
        {
            __instance.playerNameVisible = ScoreboardUtils.GetNameString(__instance.linePlayer);
        }
    }
    
    [HarmonyPatch(typeof(GorillaScoreBoard), "RedrawPlayerLines")]
    class RichTextPatch
    {
        static void Postfix(GorillaScoreBoard __instance)
        {
            __instance.boardText.richText = true;
        }
    }
}