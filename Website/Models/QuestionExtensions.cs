namespace QOAM.Website.Models
{
    using System.Resources;

    using QOAM.Core;
    using QOAM.Website.Resources;

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