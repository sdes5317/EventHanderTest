namespace MelsecHandler
{
    public abstract class Melsec
    {
        public abstract byte[] headers { get; set; }//協議的表頭
        public abstract void Process(byte[] packet);//觸發事件
    }

}
