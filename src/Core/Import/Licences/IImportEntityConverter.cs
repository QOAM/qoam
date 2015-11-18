using System.Collections.Generic;
using System.Data;

namespace QOAM.Core.Import.Licences
{
    public interface IImportEntityConverter
    {
        List<UniversityLicense> Execute(DataSet data);
    }

    //public interface IImportCommand<in TArgs, out TResult> : IImportCommand
    //{
    //    TResult Execute(TArgs args);
    //}
}