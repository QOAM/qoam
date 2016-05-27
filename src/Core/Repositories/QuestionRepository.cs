namespace QOAM.Core.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        public QuestionRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public IList<Question> BaseScoreCardQuestions
        {
            get
            {
                return this.DbContext.Questions.ToList().Where(q => !q.Key.IsOutdated() && q.Key.IsBaseScoreCardQuestion()).ToList();
            }
        }

        public IList<Question> ValuationScoreCardQuestions
        {
            get
            {
                return this.DbContext.Questions.ToList().Where(q => !q.Key.IsOutdated() && q.Key.IsValuationScoreCardQuestion()).ToList();
            }
        }
    }
}