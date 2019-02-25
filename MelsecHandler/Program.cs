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
            Handler handler = new Handler();
            //協議測試
            byte[] packet = new byte[] { 0x02, 0x50 };
            handler.MelsecFactory(packet);
            packet = new byte[] { 0x06, 0x50 };
            handler.MelsecFactory(packet);
            packet = new byte[] { 0x15, 0x50 };
            handler.MelsecFactory(packet);

            //封包長度測試 

            //故意做一個長度不符合期待的 測試錯誤訊息有沒有攔截到
            packet = new byte[] { 0x15 };
            //Handler.MelsecFactory(packet);


        }

    }

}
