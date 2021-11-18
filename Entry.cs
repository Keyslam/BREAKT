using System;
using Love;

namespace LoveJam2021
{
    public class Entry
    {
        static void Main(string[] args)
        {
            Boot.Init(new BootConfig() {
                WindowDisplay = 0,
                WindowWidth = 900,
                WindowHeight = 976,
                WindowTitle = "BREAKT'",
            });

            Boot.Run(new Program());
        }
    }
}
