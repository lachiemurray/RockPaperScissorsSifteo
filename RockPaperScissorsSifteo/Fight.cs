using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RockPaperScissors {
    class Fight {

        public static int WIN = 0;
        public static int LOSE = 1;
        public static int DRAW = 2;
        private static int ROCK = 0;
        private static int SCISSORS = 1;
        private static int PAPER = 2;
        int[] WHAT_BEATS_WHAT = new int[] { SCISSORS, PAPER, ROCK };

        CubeWrapper mPlayer1;
        CubeWrapper mPlayer2;
        private bool mIsFinished = false;
        private int mTickCount = 0;
        private int SHOW_HAND = 100;
        private int SHOW_RESULT = 200;
        private int END_FIGHT = 300;

        public Fight(CubeWrapper cube1, CubeWrapper cube2) {

            mPlayer1 = cube1.mPlayer == 0 ? cube1 : cube2;
            mPlayer2 = cube1.mPlayer == 1 ? cube1 : cube2;

            mPlayer1.StartFight();
            mPlayer2.StartFight();
        }

        public void Tick() {

            // TODO: send fight to Alex

            if (mTickCount == SHOW_HAND) {
                mPlayer1.ShowHand();
                mPlayer2.ShowHand();
            } else if (mTickCount == SHOW_RESULT) {
                if (mPlayer1.mHandState == mPlayer2.mHandState) {
                    mPlayer1.ShowResult(DRAW);
                    mPlayer2.ShowResult(DRAW);
                } else if (mPlayer1.mHandState == WHAT_BEATS_WHAT[mPlayer2.mHandState]) {
                    mPlayer1.ShowResult(LOSE);
                    mPlayer2.ShowResult(WIN);
                } else {
                    mPlayer1.ShowResult(WIN);
                    mPlayer2.ShowResult(LOSE);
                }
            } else if (mTickCount == END_FIGHT) {
                mPlayer1.EndFight();
                mPlayer2.EndFight();
                mIsFinished = true;
            }

            mTickCount++;

        }

        public bool IsFinished() {
            return mIsFinished;
        }
    }
}
