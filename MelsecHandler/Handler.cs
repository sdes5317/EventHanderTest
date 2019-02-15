using System;
using System.Collections.Generic;

namespace MelsecHandler
{
    public class Handler : Melsec
    {
        static List<Melsec> handler = new List<Melsec>();

        public override byte header { get ; set ; }

        /// <summary>
        /// 在實體化的時候會把全部需要監控的協議事件加入清單
        /// </summary>
        public Handler()
        {
            var classmember = typeof(Handler).GetNestedTypes();//列出全部的public子類
            foreach (var item in classmember)
            {
                //Console.WriteLine(item);
                Type tp = Type.GetType(item.ToString());//取得子類的名子
                object NewObject = Activator.CreateInstance(tp, true);//實例一個子類 裝箱的狀態
                Melsec MelsecObj = (Melsec)NewObject;//拆箱為Melsec類
                handler.Add(MelsecObj);
                //Console.WriteLine("類型:{0} 表頭:{1}", MelsecObj.GetType(), MelsecObj.header);
                
            }
        }
        /// <summary>
        /// 解析收到的封包表頭
        /// 遍歷在Handler底下的子類的表頭
        /// 如果跟收到的表頭相符觸發該事件
        /// </summary>
        /// <param name="packet"></param>
        public void MelsecFactory(byte[] packet)
        {
            if (packet.Length<2)
            {
                throw new LengthException();
            }
            
            foreach (var item in handler)
            {
                if (item.header == packet[0])
                {
                    item.Process(packet);
                }
            }
        }

        public override void Process(byte[] packet)
        {
            throw new NotImplementedException();
        }

        protected class LengthException : Exception
        {
            public LengthException() : base("Format Error: Length") { }
        }

        /// <summary>
        /// 如果要擴充協議
        /// 只要在這個class
        /// 新增一個實作Melsec的類
        /// 收到封包時的判斷就會有這個類了
        /// </summary>

        public class ACK: Melsec
        {
            public override byte header { get; set; } = 0x06;

            public override void Process(byte[] packet)
            {
                Console.WriteLine("這是ACK協議");
            }
        }
        public class STX : Melsec
        {
            public override byte header { get; set; } = 0x02;

            public override void Process(byte[] packet)
            {
                Console.WriteLine("這是STX協議");
            }
        }
        public class Error : Melsec
        {
            public override byte header { get; set; } = 0x15;

            public override void Process(byte[] packet)
            {
                Console.WriteLine("這是Error協議");
            }
        }
    }

}
