namespace QOAM.Core.Cleanup
{
    using Repositories;
    using Validation;

    public class DuplicateScoreCardsCleanup : ICleanup
    {
        private readonly IBaseScoreCardRepository baseScoreCardRepository;
        private readonly IValuationScoreCardRepository valuationScoreCardRepository;

        public DuplicateScoreCardsCleanup(
            IBaseScoreCardRepository baseScoreCardRepository,
            IValuationScoreCardRepository valuationScoreCardRepository)
        {
            Requires.NotNull(baseScoreCardRepository, nameof(baseScoreCardRepository));
            Requires.NotNull(valuationScoreCardRepository, nameof(valuationScoreCardRepository));
            
            this.valuationScoreCardRepository = valuationScoreCardRepository;
            this.baseScoreCardRepository = baseScoreCardRepository;
        }

        public void Cleanup()
        {
            this.baseScoreCardRepository.ArchiveDuplicateScoreCards();
            this.valuationScoreCardRepository.ArchiveDuplicateScoreCards();
        }
    }
}