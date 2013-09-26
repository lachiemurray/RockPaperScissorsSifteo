using Sifteo;
using System;
using System.Collections.Generic;

namespace RockPaperScissors
{
    public class RockPaperScissors : BaseApp
    {
        public static int WIN = 0;
        public static int LOSE = 1;
        public static int DRAW = 2;

        private static int NUM_PLAYERS = 2;
        public List<List<CubeWrapper>> mWrappers = new List<List<CubeWrapper>>(NUM_PLAYERS);
        private static int ROCK = 0;
        private static int SCISSORS = 1;
        private static int PAPER = 2;
        int[] WHAT_BEATS_WHAT = new int[] { SCISSORS, PAPER, ROCK };


        override public void Setup()
        {
            for (int i = 0; i < NUM_PLAYERS; i++) {
                mWrappers.Add(new List<CubeWrapper>());
            }

            int j = 0;
            foreach (Cube cube in this.CubeSet)
            {
                int player = (j++ % NUM_PLAYERS);
                mWrappers[player].Add(new CubeWrapper(cube, player));
            }
        
            this.CubeSet.NeighborAddEvent += OnNeighborAdd;
			this.CubeSet.NeighborRemoveEvent += OnNeighborRemove;
		}

		private void OnNeighborAdd(Cube cube1, Cube.Side side1, Cube cube2, Cube.Side side2)  {
			Log.Debug("Neighbor add: {0}.{1} <-> {2}.{3}", cube1.UniqueId, side1, cube2.UniqueId, side2);

			CubeWrapper cubeWrapper1 = (CubeWrapper)cube1.userData;
			CubeWrapper cubeWrapper2 = (CubeWrapper)cube2.userData;

            if (cubeWrapper1.mPlayer == cubeWrapper2.mPlayer) {
                ReevaluateCubePositions(cubeWrapper1.mPlayer);
            } else if(cubeWrapper1.isSelected() && cubeWrapper2.isSelected()) {
                Fight(cubeWrapper1, cubeWrapper2);
            }
		}

		private void OnNeighborRemove(Cube cube1, Cube.Side side1, Cube cube2, Cube.Side side2)  {
			Log.Debug("Neighbor remove: {0}.{1} <-> {2}.{3}", cube1.UniqueId, side1, cube2.UniqueId, side2);

            CubeWrapper cubeWrapper1 = (CubeWrapper)cube1.userData;
            CubeWrapper cubeWrapper2 = (CubeWrapper)cube2.userData;

            if (cubeWrapper1.mPlayer == cubeWrapper2.mPlayer) {
                ReevaluateCubePositions(cubeWrapper1.mPlayer);
            } else if(cubeWrapper1.IsFighting() && cubeWrapper2.IsFighting()){
                cubeWrapper1.ResetCube();
                cubeWrapper2.ResetCube();
            }
		}

        private void Fight(CubeWrapper cubeA, CubeWrapper cubeB) {
            if (cubeA.mHandState == cubeB.mHandState) {
                cubeA.StartFight(DRAW);
                cubeB.StartFight(DRAW);
            } else if (cubeA.mHandState == WHAT_BEATS_WHAT[cubeB.mHandState]) {
                cubeA.StartFight(LOSE);
                cubeB.StartFight(WIN);
            } else {
                cubeA.StartFight(WIN);
                cubeB.StartFight(LOSE);
            }
        }

        private void ReevaluateCubePositions(int player) {
            foreach (CubeWrapper wrapper in mWrappers[player]) {
                if (wrapper.mPlayer == player) {
                    wrapper.SetPosition(0);
                    if (wrapper.HasValidLeftNeighbour()) {
                        if (wrapper.HasValidRightNeighbour()) {
                            wrapper.SetPosition(2);
                        } else if (wrapper.ValidLeftNeighbour().HasValidLeftNeighbour()) {
                            wrapper.SetPosition(3);
                        } else {
                            wrapper.SetPosition(2);
                        }
                    } else if (wrapper.HasValidRightNeighbour()) {
                        wrapper.SetPosition(1);
                    }
                }
            }
        }

        override public void Tick() {
            foreach (List<CubeWrapper> players in mWrappers) {
                foreach (CubeWrapper wrapper in players) {
                    wrapper.Tick();
                }
            }
        }

        // dev only
        static void Main(string[] args) { new RockPaperScissors().Run(); }
    }
	
}