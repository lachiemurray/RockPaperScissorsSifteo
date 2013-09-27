using Sifteo;
using System;
using System.Collections.Generic;

namespace RockPaperScissors
{

    public class RockPaperScissors : BaseApp, CubeWrapperDelegate, ServerDelegate
    {

        private static int NUM_PLAYERS = 2;
        public List<List<CubeWrapper>> mWrappers = new List<List<CubeWrapper>>(NUM_PLAYERS);
        private Queue<Fight> mFights = new Queue<Fight>();
        public static Server mTCPServer;

        override public void Setup()
        {
            mTCPServer = new Server(this);

            for (int i = 0; i < NUM_PLAYERS; i++) {
                mWrappers.Add(new List<CubeWrapper>());
            }

            int j = 0;
            foreach (Cube cube in this.CubeSet)
            {
                int player = (j++ % NUM_PLAYERS);
                mWrappers[player].Add(new CubeWrapper(cube, player, this));
            }
        
            this.CubeSet.NeighborAddEvent += OnNeighborAdd;
			this.CubeSet.NeighborRemoveEvent += OnNeighborRemove;
		}

		private void OnNeighborAdd(Cube cube1, Cube.Side side1, Cube cube2, Cube.Side side2)  {
			Log.Debug("Neighbor add: {0}.{1} <-> {2}.{3}", cube1.UniqueId, side1, cube2.UniqueId, side2);

			CubeWrapper cubeWrapper1 = (CubeWrapper)cube1.userData;
			CubeWrapper cubeWrapper2 = (CubeWrapper)cube2.userData;

            if (cubeWrapper1.mPlayer == cubeWrapper2.mPlayer) {
                //ReevaluateCubePositions(cubeWrapper1.mPlayer);
            } else if (cubeWrapper1.IsHidden() && cubeWrapper2.IsHidden()) {
                mFights.Enqueue(new Fight(cubeWrapper1, cubeWrapper2));
            }
		}

		private void OnNeighborRemove(Cube cube1, Cube.Side side1, Cube cube2, Cube.Side side2)  {
			Log.Debug("Neighbor remove: {0}.{1} <-> {2}.{3}", cube1.UniqueId, side1, cube2.UniqueId, side2);

            CubeWrapper cubeWrapper1 = (CubeWrapper)cube1.userData;
            CubeWrapper cubeWrapper2 = (CubeWrapper)cube2.userData;

            if (cubeWrapper1.mPlayer == cubeWrapper2.mPlayer) {
                //ReevaluateCubePositions(cubeWrapper1.mPlayer);
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

            // Update fights
            if (mFights.Count > 0) {
                Fight fight = mFights.Peek();
                if(!fight.IsFinished()) {
                    fight.Tick();
                } else {
                    mFights.Dequeue();
                }
            }
        }

		public void cubeJoined(CubeWrapper cube) {
			foreach (CubeWrapper wrapper in mWrappers[(cube.mPlayer + 1) % 2]) {
				if (wrapper.mCubeState == CubeWrapper.CubeState.WAITING) {
					cube.JoinGame ();
					wrapper.JoinGame ();
					break;
				}
			}
		}

        public void clientConnected() {
            foreach (List<CubeWrapper> players in mWrappers) {
                foreach (CubeWrapper wrapper in players) {
                    wrapper.Connected();
                }
            }
        }

        // dev only
        static void Main(string[] args) { new RockPaperScissors().Run(); }
    }
	
}