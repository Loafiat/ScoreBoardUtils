namespace ScoreboardUtils
{
    public static class ScoreboardSettings
    {
        public static NormalizationSettings normalizationSettings = new NormalizationSettings();
    }

    public class NormalizationSettings
    {
        public string BadName;

        public bool CensorBadNames;

        public bool MakeNameUpperCase;

        public bool TruncateNames;
        
        public bool RemoveNonAlphanumericCharacters;

        public NormalizationSettings(string BadName = "BADGORILLA", bool CensorBadNames = true, bool MakeNameUpperCase = true, bool TruncateNames = true, bool RemoveNonAlphanumericCharacters = true)
        {
            this.BadName = BadName;
            this.CensorBadNames = CensorBadNames;
            this.MakeNameUpperCase = MakeNameUpperCase;
            this.TruncateNames = TruncateNames;
            this.RemoveNonAlphanumericCharacters = RemoveNonAlphanumericCharacters;
        }
    }
}