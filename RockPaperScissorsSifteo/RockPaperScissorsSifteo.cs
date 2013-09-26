using Sifteo;
using System;
using System.Collections.Generic;

namespace RockPaperScissors
{
    public class RockPaperScissors : BaseApp
    {
        public List<CubeWrapper> mWrappers = new List<CubeWrapper>();

        override public void Setup()
        {
            int i = 0;
            foreach (Cube cube in this.CubeSet)
            {
                int player = i++ < (this.CubeSet.Count / 2) ? 0 : 1;
                mWrappers.Add(new CubeWrapper(cube, player));
            }
        }

        override public void Tick()
        {
            foreach (CubeWrapper wrapper in mWrappers)
            {
                wrapper.Tick();
            }
        }

        // dev only
        static void Main(string[] args) { new RockPaperScissors().Run(); }
    }
}

