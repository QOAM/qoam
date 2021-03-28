using System.Collections.Generic;
using System.Linq;

namespace QOAM.Core.Export.BonaFide
{
    public abstract class DataMinerBase : IDataMiner
    {
        protected abstract string ContentType { get; }
        protected abstract string FileNameExtension { get; }

        public DataMiningResult GenerateFile(IList<Journal> journals)
        {
            var records = new DataMiningRecord { ISSNs = journals.Select(j => j.ISSN).ToList() };

            var bytes = Serialize(records);

            return new DataMiningResult { Bytes = bytes, ContentType = ContentType, FileNameExtension = FileNameExtension };
        }

        protected abstract byte[] Serialize(DataMiningRecord record);
    }
}