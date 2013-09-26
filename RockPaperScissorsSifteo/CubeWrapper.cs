using Sifteo;
using System;

namespace RockPaperScissors
{
	public class CubeWrapper
	{
        private int BORDER_WIDTH = 10;
        private Color PLAYER_1_COLOR = new Color(255, 0, 0);
        private Color PLAYER_2_COLOR = new Color(0, 0, 255);
		private Color POSITION_COLOR = new Color(0, 180, 0);
        //private Color TRANSPARENT_COLOR = new Color(72, 255, 170);

        private static Random random = new Random();
		public Cube mCube;
        public int mPlayer;
		private int mHandState;
		public int mPosition = 0;

		// This flag tells the wrapper to redraw the current image on the cube. (See Tick, below).
		public bool mNeedDraw = false;

        public CubeWrapper(Cube cube, int player)
        {
			mCube = cube;
            mPlayer = player;
            mHandState = random.Next(3);

            //Log.Debug(mCube.UniqueId);

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

			DrawBorder((mPlayer == 0) ? PLAYER_1_COLOR : PLAYER_2_COLOR, Color.White, BORDER_WIDTH);
			DrawHand();

			if (mPosition > 0) {
				DrawPosition ();
			}

			mCube.Paint ();
		}

		private void DrawHand()
		{
			int spriteYPos = mHandState * Cube.SCREEN_HEIGHT;
			mCube.Image("hands", 0, 0, 0, spriteYPos, Cube.SCREEN_WIDTH, Cube.SCREEN_HEIGHT, 0, 0);
		}

        private void DrawBorder(Color color1, Color color2, int width)
        {
            mCube.FillRect(color1, 0, 0, Cube.SCREEN_MAX_X, Cube.SCREEN_MAX_Y);
            mCube.FillRect(color2, width, width,
                Cube.SCREEN_MAX_X - 2 * width, Cube.SCREEN_MAX_Y - 2 * width);
        }

		private void DrawPosition()
		{
			DrawUtils.DrawNumber (mCube, mPosition, Cube.SCREEN_MAX_X - 30, 5, 20, POSITION_COLOR);
		}

		private void OnButton(Cube cube, bool pressed) {
			Log.Debug("OnButton()");
			if (pressed) {
				mHandState = (mHandState == 2) ? 0 : mHandState + 1;
				mNeedDraw = true;
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
		}
		
		public void Tick ()
		{
			if (mNeedDraw) 
			{
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
    }
}

