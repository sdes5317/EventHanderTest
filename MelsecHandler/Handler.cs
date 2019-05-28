using System;
using System.Collections.Generic;
using System.Linq;

namespace MelsecHandler
{
    public class Handler
    {
        private static List<IHandler> _handler = new List<IHandler>();

        public Handler()
        {
            #region 把全部需要監控的協議事件加入清單
            var classmember = typeof(Handler).GetNestedTypes();//列出全部的public子類
            foreach (var member in classmember)
            {
                //Console.WriteLine(item);
                //Type tp = Type.GetType(item.ToString());//取得子類的名子
                object NewObject = Activator.CreateInstance(member, true);//實例一個子類 裝箱的狀態
                IHandler MelsecObj = NewObject as IHandler;//拆箱為Melsec類 用as來做安全轉換 沒有實作Melsec的會放null
                if (MelsecObj != null)
                {
                    _handler.Add(MelsecObj);
                }
                //Console.WriteLine("類型:{0} 表頭:{1}", MelsecObj.GetType(), MelsecObj.header);
            }

            CheckIsHeadRepeat();//檢查是否有重複的header

            #endregion
        }
        public class MyClass//用來驗證沒實作協議的類有沒有被誤加到事件
        {

        }
        /// <summary>
        /// 解析收到的封包表頭
        /// 遍歷在Handler底下的子類的表頭
        /// 如果跟收到的表頭相符觸發該事件
        /// </summary>
        /// <param name="packet"></param>
        public void MelsecFactory(byte[] packet)
        {
            if (packet.Length < 2)
            {
                throw new LengthException();
            }

            foreach (var item in _handler)
            {
                if (item.headers.Contains(packet[0]))
                {
                    item.Process(packet);
                }
            }
        }

        /// <summary>
        /// 檢查是否有重複的header
        /// </summary>
        private void CheckIsHeadRepeat()
        {
            List<int> head_list = new List<int>();
            foreach (var handle in _handler)
            {
                foreach (var header in handle.headers)
                {
                    if (head_list.Contains(header))
                    {
                        throw new HeaderRepeatException();
                    }
                    head_list.Add(header);
                }
            }
        }

        protected class LengthException : Exception
        {
            public LengthException() : base("Format Error: Length") { }
        }

        /// <summary>
        /// 同一個header只能被使用一次
        /// 否則會觸發這個錯誤
        /// 例如同時有兩個handle的headers包含0x11
        /// </summary>
        protected class HeaderRepeatException : Exception
        {
            public HeaderRepeatException() : base("HeaderRepeat Error: 有重複的header") { }
        }

        /// <summary>
        /// 如果要擴充協議
        /// 只要在這個class
        /// 新增一個實作IHandler的類
        /// 收到封包時的判斷就會有這個類了
        /// </summary>

        public class ACK : IHandler
        {
            public override int[] headers { get; set; } = { 0x06 };

            public override void Process(byte[] packet)
            {
                Console.WriteLine("這是ACK協議");
            }
        }
        public class STX : IHandler
        {
            public override int[] headers { get; set; } = { 0x02 };

            public override void Process(byte[] packet)
            {
                Console.WriteLine("這是STX協議");
            }
        }
        public class Error : IHandler
        {
            public override int[] headers { get; set; } = { 0x15 };

            public override void Process(byte[] packet)
            {
                Console.WriteLine("這是Error協議");
            }
        }
    }

}
