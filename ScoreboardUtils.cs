using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using BepInEx;
using GorillaNetworking;
using HarmonyLib;

namespace ScoreboardUtils
{
    [BepInPlugin("Lofiat.ScoreboardUtils", "ScoreboardUtils", "1.0.0")]
    public class ScoreboardUtils : BaseUnityPlugin
    {
        private static Dictionary<string, string> CustomColors = new Dictionary<string, string>();
        private static Dictionary<string, string> Nicknames = new Dictionary<string, string>();
        public static GorillaScoreboardTotalUpdater scoreboardUpdater;

        private void Start()
        {
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "Lofiat.ScoreboardUtils");
            GorillaTagger.OnPlayerSpawned(OnPlayerSpawned);
        }

        void OnPlayerSpawned()
        {
            scoreboardUpdater = FindObjectOfType<GorillaScoreboardTotalUpdater>();
        }

        public static GorillaScoreboardTotalUpdater GetScoreboardUpdater()
        {
            if (scoreboardUpdater == null)
                scoreboardUpdater = FindObjectOfType<GorillaScoreboardTotalUpdater>();
            return scoreboardUpdater;
        }

        public static void UpdateScoreboards()
        {
            GetScoreboardUpdater().UpdateActiveScoreboards();
        }

        public static void SetPlayerNameColor(string id, string hexColor)
        {
            CustomColors[id] = hexColor;
            UpdateScoreboards();
        }
        
        public static void SetPlayerNickname(string id, string newName)
        {
            Nicknames[id] = newName;
            UpdateScoreboards();
        }

        public static void RemovePlayerNameColor(string id)
        {
            if (CustomColors.ContainsKey(id))
                CustomColors.Remove(id);
            UpdateScoreboards();
        }
        
        public static void RemovePlayerNickname(string id)
        {
            if (Nicknames.ContainsKey(id))
                Nicknames.Remove(id);
            UpdateScoreboards();
        }

        //Not used in the mod anymore, for external use, E.X. custom scoreboards.
        public static string GetScoreboardString()
        {
            StringBuilder scoreboardString = new StringBuilder(GorillaScoreboardTotalUpdater.allScoreboards[0].GetBeginningString());
            foreach (NetPlayer player in NetworkSystem.Instance.AllNetPlayers)
            {
                scoreboardString.Append($"\n {GetNameString(player)}");
            }
            return scoreboardString.ToString();
        }

        /// <summary>
        /// Gets the user's name as displayed on the scoreboard.
        /// </summary>
        /// <param name="ID">The Photon user ID of the player you want to change.</param>
        public static string GetNameString(NetPlayer player)
        {
            string retString = string.Empty;
            string output;
            retString = Nicknames.TryGetValue(player.UserId, out output) ? output : NormalizeName(player.NickName);
            if (CustomColors.TryGetValue(player.UserId, out output))
                retString = $"<color={output}>{retString}</color>";
            return retString;
        }
        
        /// <summary>
        /// Returns the string as it would be displayed on the scoreboard as a name. This is normalized according to the ScoreboardSettings.
        /// </summary>
        public static string NormalizeName(string text)
        {
            if (ScoreboardSettings.normalizationSettings.RemoveNonAlphanumericCharacters)
                text = new string(text.ToCharArray().Where(Utils.IsASCIILetterOrDigit).ToArray());
            
            if (ScoreboardSettings.normalizationSettings.TruncateNames && text.Length > 12)
                text = text.Substring(0, 10);

            if (ScoreboardSettings.normalizationSettings.CensorBadNames && !GorillaComputer.instance.CheckAutoBanListForName(text))
                text = ScoreboardSettings.normalizationSettings.BadName;

            if (ScoreboardSettings.normalizationSettings.MakeNameUpperCase)
                text = text.ToUpper();

            return text;
        }
        
        /// <summary>
        /// Returns the string as it would be displayed on the scoreboard as a name.
        /// </summary>
        /// <param name="settings">The normalization settings.</param>
        /// <param name="text">The text to normalize.</param>
        public static string NormalizeName(NormalizationSettings settings, string text)
        {
            if (settings.RemoveNonAlphanumericCharacters)
                text = new string(text.ToCharArray().Where(Utils.IsASCIILetterOrDigit).ToArray());
            
            if (settings.TruncateNames && text.Length > 12)
                text = text.Substring(0, 10);

            if (settings.CensorBadNames && !GorillaComputer.instance.CheckAutoBanListForName(text))
                text = settings.BadName;

            if (settings.MakeNameUpperCase)
                text = text.ToUpper();

            return text;
        }
    }
}
