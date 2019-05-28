namespace MelsecHandler
{
    public abstract class IHandler
    {
        public abstract int[] headers { get; set; }//協議的表頭
        public abstract void Process(byte[] packet);//觸發事件
    }

}
