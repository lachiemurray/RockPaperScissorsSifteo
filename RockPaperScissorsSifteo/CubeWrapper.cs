using Sifteo;
using System;
using System.Timers;

namespace RockPaperScissors
{
	public interface CubeWrapperDelegate
	{
		void cubeJoined (CubeWrapper wrapper);
	}

	public class CubeWrapper
	{
        private int BORDER_WIDTH = 10;
        private int NUM_HAND_STATES = 3;
        private static Color COLOR_RED = new Color(255, 0, 0);
        private static Color COLOR_GREEN = new Color(0, 255, 0);
        private static Color COLOR_BLUE = new Color(0, 0, 255);
        private static Color POSITION_COLOR = new Color(0, 180, 0);
        private static Color[] BACKGROUNDS = { COLOR_GREEN, COLOR_RED, COLOR_BLUE };
        public enum CubeState { DISCONNECTED, UNUSED, WAITING, VISIBLE, HIDDEN, SELECTED, START_FIGHT, FIGHTING, RESULT, WINNER, LOSER }; // more ...

        private static Random random = new Random();
		public Cube mCube;
        public int mPlayer;
		public int mHandState;
		public int mPosition = 0;
        public CubeState mCubeState = CubeState.DISCONNECTED;
        private Color mBackgroundColor = Color.White;
		private int joinCounter = 0;
		private CubeWrapperDelegate mDelegate;

        private Timer mTimer = new Timer();

		// This flag tells the wrapper to redraw the current image on the cube. (See Tick, below).
		public bool mNeedDraw = false;

        public CubeWrapper(Cube cube, int player, CubeWrapperDelegate theDelegate)
        {
			mCube = cube;
            mPlayer = player;
            mHandState = random.Next(3);
			mDelegate = theDelegate;

			mCube.userData = this;

			mCube.ButtonEvent += OnButton;
			mCube.TiltEvent += OnTilt;
			mCube.ShakeStartedEvent += OnShakeStarted;
			mCube.ShakeStoppedEvent += OnShakeStopped;
			mCube.FlipEvent += OnFlip;

            Init(); 
		}

		public int GetPosition() {
			return this.mPosition;
		}

		public void SetPosition(int position)
		{
			this.mPosition = position;
			this.mNeedDraw = true;
		}

        private void Init()
        {
			DrawState();
        }

		private void DrawState()
		{
			// Clear off whatever was previously on the display before drawing the new image.
			mCube.FillScreen(Color.Black);

			DrawBorder((mPlayer == 0) ? COLOR_RED : COLOR_BLUE, mBackgroundColor, BORDER_WIDTH);
            switch (mCubeState) {
				case CubeState.DISCONNECTED:
					DrawDisconnected();
                    break;
				case CubeState.UNUSED:
					if (joinCounter < 10) {
                        DrawUtils.DrawJoin(mCube, Color.Black);
					}
                    break;
				case CubeState.WAITING:
					DrawUtils.DrawWait (mCube, Color.Black);
					break;
                case CubeState.WINNER:
                    if (joinCounter < 10) {
                        DrawUtils.DrawWin(mCube, Color.Black);
                    }
                    break;
                case CubeState.LOSER:
                    if (joinCounter < 10) {
                        DrawUtils.DrawLose(mCube, Color.Black);
                    }
                    break;
                case CubeState.START_FIGHT:
                    DrawCrossHair();
                    break;
                case CubeState.FIGHTING:
                case CubeState.VISIBLE:
                    DrawHand();
                    if (mPosition > 0) {
                        DrawPosition();
                    }
                    break;
                case CubeState.HIDDEN:
                    DrawQuestionMark();
                    break;
            }

			mCube.Paint ();
		}

		private void DrawHand() {
			int spriteYPos = mHandState * Cube.SCREEN_HEIGHT;
			mCube.Image("sprites", 0, 0, 0, spriteYPos, Cube.SCREEN_WIDTH, Cube.SCREEN_HEIGHT, 0, 0);
		}

        private void DrawQuestionMark() {
			int spriteYPos = (Cube.SCREEN_HEIGHT * 5);
            mCube.Image("sprites", 0, 0, 0, spriteYPos, Cube.SCREEN_WIDTH, Cube.SCREEN_HEIGHT, 0, 0);
        }

        private void DrawCrossHair() {
            int spriteYPos = (Cube.SCREEN_HEIGHT * 4);
            mCube.Image("sprites", 0, 0, 0, spriteYPos, Cube.SCREEN_WIDTH, Cube.SCREEN_HEIGHT, 0, 0);
        }

		private void DrawDisconnected() {
			int spriteYPos = (Cube.SCREEN_HEIGHT * 3);
			mCube.Image("sprites", 0, 0, 0, spriteYPos, Cube.SCREEN_WIDTH, Cube.SCREEN_HEIGHT, 0, 0);
		}

        private void DrawBorder(Color color1, Color color2, int width) {
            mCube.FillRect(color1, 0, 0, Cube.SCREEN_MAX_X, Cube.SCREEN_MAX_Y);
            mCube.FillRect(color2, width, width,
                Cube.SCREEN_MAX_X - 2 * width, Cube.SCREEN_MAX_Y - 2 * width);
        }

		private void DrawPosition() {
			DrawUtils.DrawNumber (mCube, mPosition, Cube.SCREEN_MAX_X - 30, 5, 20, POSITION_COLOR);
		}

		private void OnButton(Cube cube, bool pressed) {
			Log.Debug("OnButton()");
            switch (mCubeState) {
				case CubeState.UNUSED:
					if (!pressed) {
						mCubeState = CubeState.WAITING;
						mNeedDraw = true;
                        mDelegate.cubeJoined(this);
					}
					break;
	            case CubeState.VISIBLE:
	                if (pressed) {
	                    mHandState = (++mHandState % NUM_HAND_STATES);
	                    mCubeState = CubeState.VISIBLE;
	                    mNeedDraw = true;
	                }
	                break;
            }
		}
		
		private void OnTilt(Cube cube, int tiltX, int tiltY, int tiltZ) {
			Log.Debug("OnTilt()");
		}
		
		private void OnShakeStarted(Cube cube) {
			Log.Debug("OnShakeStarted()");
		}

		private void OnShakeStopped(Cube cube, int duration) {
			Log.Debug("OnShakeStopped()");
		}

		private void OnFlip(Cube cube, bool newOrientationIsUp) {
			Log.Debug("OnFlip()");
            if (!newOrientationIsUp) {
                switch (mCubeState) {
                    case CubeState.VISIBLE:
                        mCubeState = CubeState.HIDDEN;
                        mNeedDraw = true;
                        break;
                    case CubeState.HIDDEN:
                        mCubeState = CubeState.VISIBLE;
                        mNeedDraw = true;
                        break;
                }
            }
		}
		
		public void Tick () {
			if (mCubeState == CubeState.UNUSED || mCubeState == CubeState.WINNER || mCubeState == CubeState.LOSER) {
				joinCounter = (joinCounter < 20) ? joinCounter + 1 : 0;
				if (joinCounter == 0 || joinCounter == 10) {
					mNeedDraw = true;
				}
			}

			if (mNeedDraw) {
				mNeedDraw = false;
				DrawState();
			}
		}

        public bool HasValidLeftNeighbour() {
            return ValidLeftNeighbour() != null;
        }

        public bool HasValidRightNeighbour() {
            return ValidRightNeighbour() != null;
        }

        public CubeWrapper ValidLeftNeighbour() {
            CubeWrapper neighbour = (mCube.Neighbors.Left != null) ? (CubeWrapper)mCube.Neighbors.Left.userData : null;
            if (neighbour != null && neighbour.mPlayer == mPlayer &&
                neighbour.mCube.Neighbors.Right != null &&
                neighbour.mCube.Neighbors.Right.UniqueId == mCube.UniqueId) {
                return neighbour;
            } else {
                return null;
            }
        }

        public CubeWrapper ValidRightNeighbour() {
            CubeWrapper neighbour = (mCube.Neighbors.Right != null) ? (CubeWrapper)mCube.Neighbors.Right.userData : null;
            if (neighbour != null && neighbour.mPlayer == mPlayer &&
                neighbour.mCube.Neighbors.Left != null &&
                neighbour.mCube.Neighbors.Left.UniqueId == mCube.UniqueId) {
                return neighbour;
            } else {
                return null;
            }
        }

        public void StartFight() {
            mCubeState = CubeState.START_FIGHT;
            mNeedDraw = true;
        }

        public bool isSelected() {
            return mCubeState == CubeState.SELECTED;
        }

        public void Connected() {
            mCubeState = CubeState.UNUSED;
            mNeedDraw = true;
            mBackgroundColor = Color.White;
        }

        public void JoinGame() {
            mCubeState = CubeState.VISIBLE;
            mNeedDraw = true;
            mBackgroundColor = Color.White;
        }

        public bool IsFighting() {
            return mCubeState == CubeState.FIGHTING || mCubeState == CubeState.START_FIGHT;
        }

        public bool IsHidden() {
            return mCubeState == CubeState.HIDDEN;
        }

        public void EndFight() {
            mCubeState = CubeState.VISIBLE;
            mBackgroundColor = Color.White;
            mNeedDraw = true;
        }

        public void ShowResult(int result) {
            mBackgroundColor = BACKGROUNDS[result];
            mNeedDraw = true;
        }

        public void ShowHand() {
            mCubeState = CubeState.FIGHTING;
            mBackgroundColor = Color.Black;
            mNeedDraw = true;
        }

        public void Celebrate() {
            mCubeState = CubeState.WINNER;
            mBackgroundColor = Color.White;
            mNeedDraw = true;
        }

        public void Weep() {
            mCubeState = CubeState.LOSER;
            mBackgroundColor = Color.White;
            mNeedDraw = true;
        }
    }
}

