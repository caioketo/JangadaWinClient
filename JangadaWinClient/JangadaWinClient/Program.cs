using System;

namespace JangadaWinClient
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Jangada game = new Jangada())
            {
                game.Run();
            }
        }
    }
#endif
}

