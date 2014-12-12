namespace QOAM.Website.Tests.Controllers
{
    using Moq;

    using QOAM.Core.Repositories;
    using QOAM.Website.Controllers;
    using QOAM.Website.Helpers;
    using QOAM.Website.Models;
    using QOAM.Website.Tests.Controllers.Helpers;

    using Xunit;

    public class AccountControllerTests
    {

        private static AccountController CreateAccountController()
        {
            return new AccountController(Mock.Of<IUserProfileRepository>(), new Mock<IAuthentication>().Object, new Mock<IInstitutionRepository>().Object)
                   {
                       Url = HttpContextHelper.CreateUrlHelper()
                   };
        }
    }
}