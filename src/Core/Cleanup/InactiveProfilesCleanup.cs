namespace QOAM.Core.Cleanup
{
    using System;
    using Repositories;
    using Validation;

    public class InactiveProfilesCleanup : ICleanup
    {
        private readonly IUserProfileRepository userProfileRepository;
        private readonly InactiveProfilesCleanupSettings settings;

        public InactiveProfilesCleanup(IUserProfileRepository userProfileRepository, InactiveProfilesCleanupSettings settings)
        {
            Requires.NotNull(userProfileRepository, nameof(userProfileRepository));
            Requires.NotNull(settings, nameof(settings));
            
            this.userProfileRepository = userProfileRepository;
            this.settings = settings;
        }

        public void Cleanup()
        {
            var toBeRemovedWindow = TimeSpan.FromDays(settings.NumberOfInactiveDaysBeforeRemoval);

            userProfileRepository.RemoveInactive(toBeRemovedWindow);
        }
    }
}