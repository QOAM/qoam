namespace RU.Uci.OAMarket.Domain.Import
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Validation;

    public class UlrichsCache
    {
        private readonly UlrichsSettings ulrichsSettings;

        public UlrichsCache(UlrichsSettings ulrichsSettings)
        {
            Requires.NotNull(ulrichsSettings, "ulrichsSettings");
            
            this.ulrichsSettings = ulrichsSettings;
        }

        public IEnumerable<string> GetAll()
        {
            return this.GetFilenames().Select(File.ReadAllText);
        }

        private IEnumerable<string> GetFilenames()
        {
            return Directory.EnumerateFiles(this.ulrichsSettings.CacheDirectory, "*page*.xml");
        }

        public bool HasExpired(int page)
        {
            var expiredTime = DateTime.Now.Date - File.GetLastWriteTime(this.GetJournalsXmlFilename(page)).Date;
            return expiredTime.Days >= this.ulrichsSettings.CacheLifetimeInDays;
        }

        public void Add(string journalsXml, int page)
        {
            File.WriteAllText(this.GetJournalsXmlFilename(page), journalsXml);
        }

        private string GetJournalsXmlFilename(int page)
        {
            return Path.Combine(this.ulrichsSettings.CacheDirectory, "page" + page + ".xml");
        }
    }
}