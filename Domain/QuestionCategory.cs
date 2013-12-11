namespace RU.Uci.OAMarket.Domain
{
    using System.ComponentModel.DataAnnotations;

    public enum QuestionCategory
    {
        [Display(Name = "QuestionCategory_EditorialInformation", ResourceType = typeof(Strings))]
        EditorialInformation,

        [Display(Name = "QuestionCategory_PeerReview", ResourceType = typeof(Strings))]
        PeerReview,

        [Display(Name = "QuestionCategory_Governance", ResourceType = typeof(Strings))]
        Governance,

        [Display(Name = "QuestionCategory_Process", ResourceType = typeof(Strings))]
        Process,

        [Display(Name = "QuestionCategory_Valuation", ResourceType = typeof(Strings))]
        Valuation
    }
}