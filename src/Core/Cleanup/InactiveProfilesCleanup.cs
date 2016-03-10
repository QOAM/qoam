namespace QOAM.Core.Cleanup
{
    using System;
    using Repositories;
    using Validation;

    public class InactiveProfilesCleanup : ICleanup
    {
        private readonly IUserProfileRepository userProfileRepository;
        private readonly CleanupSettings settings;

        public InactiveProfilesCleanup(IUserProfileRepository userProfileRepository, CleanupSettings settings)
        {
            Requires.NotNull(userProfileRepository, nameof(userProfileRepository));
            Requires.NotNull(settings, nameof(settings));
            
            this.userProfileRepository = userProfileRepository;
            this.settings = settings;
        }

        public void Cleanup()
        {
            var toBeRemovedWindow = TimeSpan.FromDays(settings.NumberOfInactiveDaysBeforeArchivingInactiveProfiles);

            userProfileRepository.RemoveInactive(toBeRemovedWindow);
        }
    }
}