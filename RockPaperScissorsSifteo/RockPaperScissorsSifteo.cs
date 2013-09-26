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

			// ## Event Handlers ##
			// Objects in the Sifteo API (particularly BaseApp, CubeSet, and Cube)
			// fire events to notify an app of various happenings, including actions
			// that the player performs on the cubes.
			//
			// To listen for an event, just add the handler method to the event. The
			// handler method must have the correct signature to be added. Refer to
			// the API documentation or look at the examples below to get a sense of
			// the correct signatures for various events.
			//
			// **NeighborAddEvent** and **NeighborRemoveEvent** are triggered when
			// the player puts two cubes together or separates two neighbored cubes.
			// These events are fired by CubeSet instead of Cube because they involve
			// interaction between two Cube objects. (There are Cube-level neighbor
			// events as well, which comes in handy in certain situations, but most
			// of the time you will find the CubeSet-level events to be more useful.)
			this.CubeSet.NeighborAddEvent += OnNeighborAdd;
			this.CubeSet.NeighborRemoveEvent += OnNeighborRemove;
		}

		// ## Neighbor Add ##
		// This method is a handler for the NeighborAdd event. It is triggered when
		// two cubes are placed side by side.
		//
		// Cube1 and cube2 are the two cubes that are involved in this neighboring.
		// The two cube arguments can be in any order; if your logic depends on
		// cubes being in specific positions or roles, you need to add logic to
		// this handler to sort the two cubes out.
		//
		// Side1 and side2 are the sides that the cubes neighbored on.
		private void OnNeighborAdd(Cube cube1, Cube.Side side1, Cube cube2, Cube.Side side2)  {
			Log.Debug("Neighbor add: {0}.{1} <-> {2}.{3}", cube1.UniqueId, side1, cube2.UniqueId, side2);

			CubeWrapper wrapper1 = (CubeWrapper)cube1.userData;
			CubeWrapper wrapper2 = (CubeWrapper)cube2.userData;

            if (wrapper1.mPlayer == wrapper2.mPlayer) {
                ReevaluateCubePositions(wrapper1.mPlayer);
            }
		}



        private void ReevaluateCubePositions(int player) {

            foreach (CubeWrapper wrapper in mWrappers) {
                if (wrapper.mPlayer == player) {
                    wrapper.SetPosition(0);
                    if(wrapper.HasValidLeftNeighbour()) {
                        if(wrapper.HasValidRightNeighbour()) {
                            wrapper.SetPosition(2);
                        } else if( wrapper.ValidLeftNeighbour().HasValidLeftNeighbour()) {
                            wrapper.SetPosition(3);
                        } else {
                            wrapper.SetPosition(2);
                        }
                    } else if(wrapper.HasValidRightNeighbour()) {
                        wrapper.SetPosition(1);
                    }
                }
            }
        }

		// ## Neighbor Remove ##
		// This method is a handler for the NeighborRemove event. It is triggered
		// when two cubes that were neighbored are separated.
		//
		// The side arguments for this event are the sides that the cubes
		// _were_ neighbored on before they were separated. If you check the
		// current state of their neighbors on those sides, they should of course
		// be NONE.
		private void OnNeighborRemove(Cube cube1, Cube.Side side1, Cube cube2, Cube.Side side2)  {
			Log.Debug("Neighbor remove: {0}.{1} <-> {2}.{3}", cube1.UniqueId, side1, cube2.UniqueId, side2);

			CubeWrapper wrapper1 = (CubeWrapper)cube1.userData;
			CubeWrapper wrapper2 = (CubeWrapper)cube2.userData;

            if (wrapper1.mPlayer == wrapper2.mPlayer) {
                ReevaluateCubePositions(wrapper1.mPlayer);
            }
		}

		private CubeWrapper CubeAtPosition(int position, int player) {
			CubeWrapper matchingCube = null;
			foreach (CubeWrapper wrapper in mWrappers) {
				if (wrapper.mPosition == position && wrapper.mPlayer == player) {
					matchingCube = wrapper;
					break;
				}
			}
			return matchingCube;
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