using System;

namespace SmallWorld.Database.CommandLine
{
    public class ArgQueue
    {
        private int index = 1;
        private readonly string[] src = Environment.GetCommandLineArgs();

        public string At(int i)
        {
            return src.Length > i ? src[i] : null;
        }

        public string Peek()
        {
            return At(index);
        }

        public string Read()
        {
            return At(index++);
        }
    }
}
