using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TCPBinding
{
   [ServiceContract]
   public interface IStockService
    {
        [OperationContract]
        void PostStockDetail(Stock stock);
    }
}
