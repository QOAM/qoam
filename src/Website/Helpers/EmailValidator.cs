namespace QOAM.Website.Helpers
{
    using System;
    using System.Net.Mail;

    public static class EmailValidator
    {
        public static bool IsValid(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}