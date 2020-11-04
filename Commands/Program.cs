namespace motw
{
    class Program
    {
        static void Main(string[] args){
            var bot = new Bot();
            bot.start().GetAwaiter().GetResult();
        }
    }
}