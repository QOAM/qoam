namespace RU.Uci.OAMarket.Website.Models
{
    using System.Resources;

    using RU.Uci.OAMarket.Domain;
    using RU.Uci.OAMarket.Website.Resources;

    public static class QuestionKeyExtensions
    {
        private static readonly ResourceManager QuestionsResourceManager = new ResourceManager(typeof(Questions));

        public static string ToDescription(this QuestionKey questionKey)
        {
            return QuestionsResourceManager.GetString(questionKey.ToString());
        }
    }
}