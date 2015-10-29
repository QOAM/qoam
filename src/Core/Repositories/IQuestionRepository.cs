namespace QOAM.Core.Repositories
{
    using System.Collections.Generic;

    public interface IQuestionRepository
    {
        IList<Question> BaseScoreCardQuestions { get; }
        IList<Question> ValuationScoreCardQuestions { get; }
    }
}