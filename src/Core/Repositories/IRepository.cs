namespace QOAM.Core.Repositories
{
    public interface IRepository
    {
        void RefreshContext(bool autoDetectChangesEnabled = false);
    }
}