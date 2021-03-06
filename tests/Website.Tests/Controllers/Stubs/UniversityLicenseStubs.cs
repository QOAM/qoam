﻿namespace QOAM.Website.Tests.Controllers.Stubs
{
    using System.Collections.Generic;
    using Core.Import.Licences;

    public static class UniversityLicenseStubs
    {
        public static List<UniversityLicense> Licenses()
        {
            var licenses = new List<UniversityLicense>
            {
                new UniversityLicense
                {
                    Domain = "ru.nl",
                    Licenses = new List<LicenseInfo>
                    {
                        new LicenseInfo { ISSN = "0219-3094", LicenseName = "Sage", Text = "Discount of 100% on publication fee. Please contact your library for more information." },
                        new LicenseInfo { ISSN = "0219-3116", LicenseName = "Sage", Text = "Some random text" },
                        new LicenseInfo { ISSN = "0814-6039", LicenseName = "Sage", Text = "I'm Batman" },
                        new LicenseInfo { ISSN = "0942-0940", LicenseName = "Sage", Text = "No, really." }
                    }
                },
                new UniversityLicense
                {
                    Domain = "uu.nl",
                    Licenses = new List<LicenseInfo>
                    {
                        new LicenseInfo { ISSN = "0219-3094", LicenseName = "Springer", Text = "Discount of 100% on publication fee. Please contact your library for more information." },
                        new LicenseInfo { ISSN = "0219-3116", LicenseName = "Springer", Text = "Some random text" },
                        new LicenseInfo { ISSN = "0814-6039", LicenseName = "Springer", Text = "I'm Batman" },
                        new LicenseInfo { ISSN = "0942-0940", LicenseName = "Springer", Text = "No, really." }
                    }
                },
                new UniversityLicense
                {
                    Domain = "uva.nl",
                    Licenses = new List<LicenseInfo>
                    {
                        new LicenseInfo { ISSN = "0219-3094", LicenseName = "Sage", Text = "Discount of 100% on publication fee. Please contact your library for more information." },
                        new LicenseInfo { ISSN = "0219-3116", LicenseName = "Sage", Text = "Some random text" },
                        new LicenseInfo { ISSN = "0814-6039", LicenseName = "Sage", Text = "I'm Batman" },
                        new LicenseInfo { ISSN = "0942-0940", LicenseName = "Sage", Text = "No, really." },
                        new LicenseInfo { ISSN = "0219-3094", LicenseName = "Springer", Text = "Discount of 100% on publication fee. Please contact your library for more information." },
                        new LicenseInfo { ISSN = "0219-3116", LicenseName = "Springer", Text = "Some random text" },
                        new LicenseInfo { ISSN = "0814-6039", LicenseName = "Springer", Text = "I'm Batman" },
                        new LicenseInfo { ISSN = "0942-0940", LicenseName = "Springer", Text = "No, really." }
                    }
                }
            };

            return licenses;
        }

        public static List<UniversityLicense> SomeLicensesToDelete()
        {
            var licenses = new List<UniversityLicense>
            {
                new UniversityLicense
                {
                    Domain = "ru.nl",
                    Licenses = new List<LicenseInfo>
                    {
                        new LicenseInfo { ISSN = "0219-3094", LicenseName = "Sage", Text = "" },
                        new LicenseInfo { ISSN = "0219-3116", LicenseName = "Sage", Text = "Some random text" },
                        new LicenseInfo { ISSN = "0814-6039", LicenseName = "Sage", Text = "I'm Batman" },
                        new LicenseInfo { ISSN = "0942-0940", LicenseName = "Sage", Text = "No, really." }
                    }
                }
            };

            return licenses;
        }
    }
}