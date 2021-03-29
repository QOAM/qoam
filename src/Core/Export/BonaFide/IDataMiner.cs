using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace QOAM.Core.Export.BonaFide
{
    public interface IDataMiner
    {
        DataMiningResult GenerateFile(IList<Journal> journals);
    }
}