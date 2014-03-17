namespace QOAM.Core.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    public class QuestionRepository : Repository, IQuestionRepository
    {
        public QuestionRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public IList<Question> All
        {
            get
            {
                return this.DbContext.Questions.ToList();
            }
        }
    }
}