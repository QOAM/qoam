namespace QOAM.Core.Repositories
{
    public interface IScoreCardVersionRepository
    {
        ScoreCardVersion FindCurrent();
    }
}