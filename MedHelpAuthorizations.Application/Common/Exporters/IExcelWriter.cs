using System.Collections.Generic;
using System.IO;

namespace MedHelpAuthorizations.Application.Common.Exporters;

public interface IExcelWriter : ITransientService
{
    Stream WriteToStream<T>(IList<T> data);
}