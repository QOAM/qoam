using System;
using QOAM.Core;

namespace QOAM.Website.ViewModels.Api.ScoreCard
{

    public class SerializableScoreCard
    {
        public int Id { get; set; }
        public string Journal { get; set; }
        public string ISSN { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime? DateExpiration { get; set; }
        public DateTime? DatePublished { get; set; }
        public string Remarks { get; set; }
        public ScoreCardState State { get; set; }
        public bool Submitted { get; set; }
        public bool Editor { get; set; }
    }

    public class SerializableBaseScoreCard : SerializableScoreCard
    {
        public BaseScoreCardScore Score { get; set; }
    }

    public class SerializableValuationScoreCard : SerializableScoreCard
    {
        public ValuationScoreCardScore Score { get; set; }
    }
}