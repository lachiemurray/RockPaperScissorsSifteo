using Sifteo;
using System;

namespace RockPaperScissors
{
	public class CubeWrapper
	{
        private int BORDER_WIDTH = 10;
        private Color PLAYER_1_COLOR = new Color(255, 0, 0);
        private Color PLAYER_2_COLOR = new Color(0, 255, 0);
        private Color TRANSPARENT_COLOR = new Color(72, 255, 170);

		private Cube mCube;
        private int mPlayer;

        public CubeWrapper(Cube cube, int player)
        {
			mCube = cube;
            mPlayer = player;

            Log.Debug(mCube.UniqueId);

			mCube.ButtonEvent += OnButton;
			mCube.TiltEvent += OnTilt;
			mCube.ShakeStartedEvent += OnShakeStarted;
			mCube.ShakeStoppedEvent += OnShakeStopped;
			mCube.FlipEvent += OnFlip;

            Init(); 
		}

        private void Init()
        {
            if (mPlayer == 0)
            {
                DrawBorder(PLAYER_1_COLOR, Color.White, BORDER_WIDTH);
            }
            else
            {
                DrawBorder(PLAYER_2_COLOR, Color.White, BORDER_WIDTH);
            }
        }

        private void DrawBorder(Color color1, Color color2, int width)
        {
            mCube.FillRect(color1, 0, 0, Cube.SCREEN_MAX_X, Cube.SCREEN_MAX_Y);
            mCube.FillRect(color2, width, width,
                Cube.SCREEN_MAX_X - 2 * width, Cube.SCREEN_MAX_Y - 2 * width);
        }

		private void OnButton(Cube cube, bool pressed) {
			Log.Debug("OnButton()");
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
			mCube.Paint ();
		}
	}
}

