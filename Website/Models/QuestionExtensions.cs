namespace RU.Uci.OAMarket.Website.Models
{
    using System.Resources;

    using RU.Uci.OAMarket.Domain;

    using Resources;

    using Validation;

    public static class QuestionExtensions
    {
        public static string ToLocalizedString(this Question question)
        {
            Requires.NotNull(question, "question");

            var resourceManager = new ResourceManager(typeof(Questions));
            return resourceManager.GetString(question.Key.ToString());
        }
    }
}