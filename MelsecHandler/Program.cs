using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelsecHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            //協議測試
            byte[] packet = new byte[] { 0x02, 0x50 };
            Handler.MelsecFactory(packet);
            packet = new byte[] { 0x06, 0x50 };
            Handler.MelsecFactory(packet);
            packet = new byte[] { 0x15, 0x50 };
            Handler.MelsecFactory(packet);

            //封包長度測試
            packet = new byte[] { 0x15 };
            Handler.MelsecFactory(packet);
        }

    }

}
