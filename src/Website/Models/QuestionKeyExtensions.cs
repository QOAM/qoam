namespace QOAM.Website.Models
{
    using System.Resources;

    using QOAM.Core;
    using QOAM.Website.Resources;

    public static class QuestionKeyExtensions
    {
        private static readonly ResourceManager QuestionsResourceManager = new ResourceManager(typeof(Questions));

        public static string ToDescription(this QuestionKey questionKey)
        {
            return QuestionsResourceManager.GetString(questionKey.ToString());
        }
    }
}