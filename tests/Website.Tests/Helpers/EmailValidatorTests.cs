namespace QOAM.Website.Tests.Helpers
{
    using Website.Helpers;
    using Xunit;

    public class EmailValidatorTests
    {
        [Theory]
        [InlineData("me@example.com")]
        [InlineData("a.nonymous@example.com")]
        [InlineData("name+tag@example.com")]
        [InlineData("spaces are allowed@example.com")]
        [InlineData("\"spaces may be quoted\"@example.com")]
        [InlineData("!#$%&'+-/=.?^`{|}~@[1.0.0.127]")]
        [InlineData("!#$%&'+-/=.?^`{|}~@[IPv6:0123:4567:89AB:CDEF:0123:4567:89AB:CDEF]")]
        [InlineData("me(this is a comment)@example.com")]
        [InlineData("me.example@com")]
        [InlineData("me.@example.com")]
        public void IsValidWithValidEmailAddressReturnsTrue(string mailAddress)
        {
            var isValid = EmailValidator.IsValid(mailAddress);
            Assert.True(isValid);
        }

        [Theory]
        [InlineData("me@")]
        [InlineData("@example.com")]
        [InlineData(".me@example.com")]
        [InlineData("me\\@example.com")]
        public void IsValidWithInvalidEmailAddressReturnsFalse(string mailAddress)
        {
            var isValid = EmailValidator.IsValid(mailAddress);
            Assert.False(isValid);
        }
    }
}