using System;

namespace MelsecHandler
{
    public abstract class Handler : Melsec
    {
        /// <summary>
        /// 解析收到的封包表頭
        /// 遍歷在Handler底下的子類的表頭
        /// 如果跟收到的表頭相符觸發該事件
        /// </summary>
        /// <param name="packet"></param>
        public static void MelsecFactory(byte[] packet)
        {
            if (packet.Length<2)
            {
                throw new LengthException();
            }
            var classmember = typeof(Handler).GetNestedTypes();//列出全部的子類
            foreach (var item in classmember)
            {
                //Console.WriteLine(item);
                Type tp = Type.GetType(item.ToString());//取得子類的名子
                object NewObject = Activator.CreateInstance(tp, true);//實例一個子類 裝箱的狀態
                Melsec MelsecObj = (Melsec)NewObject;//拆箱為Melsec類
                //Console.WriteLine("類型:{0} 表頭:{1}", MelsecObj.GetType(), MelsecObj.header);
                if (MelsecObj.header == packet[0])
                {
                    MelsecObj.Process(packet);
                }
            }
        }

        protected class LengthException : Exception
        {
            public LengthException() : base("Format Error: Length") { }
        }

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
