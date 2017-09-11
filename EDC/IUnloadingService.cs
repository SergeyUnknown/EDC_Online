using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace EDC
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IUnloadingService" в коде и файле конфигурации.
    [ServiceContract]
    public interface IUnloadingService
    {
        [OperationContract]
        void ExportToExcel(int upID, string userName);
    }
}
