namespace QOAM.Core
{
    using System.ComponentModel.DataAnnotations;

    public enum Score
    {
        [Display(Name = "Score_Undecided", ResourceType = typeof(Strings))]
        Undecided = 0,

        [Display(Name = "Score_Absent", ResourceType = typeof(Strings))]
        Absent,

        [Display(Name = "Score_Poor", ResourceType = typeof(Strings))]
        Poor,

        [Display(Name = "Score_Neutral", ResourceType = typeof(Strings))]
        Neutral,

        [Display(Name = "Score_Good", ResourceType = typeof(Strings))]
        Good,

        [Display(Name = "Score_Excellent", ResourceType = typeof(Strings))]
        Excellent
    }
}