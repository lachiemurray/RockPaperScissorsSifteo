using Sifteo;
using System;

namespace RockPaperScissors
{
	public class DrawUtils
	{
		public static void DrawNumber(Cube cube, int number, int startX, int startY, int height, Color color)
		{
			for (int i = 0; i < number; i++) {
				cube.FillRect(color, startX + (i * 6), startY, 3, startY + height);
			}
		}

		public static void DrawJoin(Cube cube, Color color) {

			int screenMid = Cube.SCREEN_HEIGHT / 2;
			int letterHeight = 40;
			int letterStroke = 7;
			int letterGap = 7;
			int marginX = 20;

			int nextX = marginX;

			// J
			cube.FillRect (color, nextX + letterStroke, screenMid - (letterHeight / 2), letterStroke, letterHeight);
			cube.FillRect (color, nextX, screenMid + (letterHeight / 2) - letterStroke, letterStroke, letterStroke);
			nextX = marginX + (letterStroke * 2) + letterGap; 

			// O
			cube.FillRect (color, nextX, screenMid - (letterHeight / 2), (letterStroke * 3), letterStroke);
			cube.FillRect (color, nextX, screenMid - (letterHeight / 2), letterStroke, letterHeight);
			cube.FillRect (color, nextX + (letterStroke * 2), screenMid - (letterHeight / 2), letterStroke, letterHeight);
			cube.FillRect (color, nextX, screenMid + (letterHeight / 2) - letterStroke, (letterStroke * 3), letterStroke);
			nextX += (letterStroke * 3) + letterGap; 

			// I
			cube.FillRect (color, nextX, screenMid - (letterHeight / 2), letterStroke, letterHeight);
			nextX += letterStroke + letterGap;

			// N
			cube.FillRect (color, nextX, screenMid - (letterHeight / 2), letterStroke, letterHeight);
			cube.FillRect (color, nextX, screenMid - (letterHeight / 2), (letterStroke * 3), letterStroke);
			cube.FillRect (color, nextX + (letterStroke * 2), screenMid - (letterHeight / 2), letterStroke, letterHeight);
		}

		public static void DrawWait(Cube cube, Color color) {

			int screenMid = Cube.SCREEN_HEIGHT / 2;
			int letterHeight = 40;
			int letterStroke = 6;
			int letterGap = 6;
			int marginX = 20;

			int nextX = marginX;

			// W
			cube.FillRect (color, nextX, screenMid - (letterHeight / 2), letterStroke, letterHeight);
			cube.FillRect (color, nextX + (letterStroke * 2), screenMid - (letterHeight / 2), letterStroke, letterHeight);
			cube.FillRect (color, nextX + (letterStroke * 4), screenMid - (letterHeight / 2), letterStroke, letterHeight);
			cube.FillRect (color, nextX, screenMid + (letterHeight / 2) - letterStroke, (letterStroke * 5), letterStroke);
			nextX = marginX + (letterStroke * 5) + letterGap; 

			// A
			cube.FillRect (color, nextX, screenMid - (letterHeight / 2), letterStroke, letterHeight);
			cube.FillRect (color, nextX + (letterStroke * 2), screenMid - (letterHeight / 2), letterStroke, letterHeight);
			cube.FillRect (color, nextX, screenMid - (letterHeight / 2), (letterStroke * 3), letterStroke);
			cube.FillRect (color, nextX, screenMid - letterStroke, (letterStroke * 3), letterStroke);
			nextX += (letterStroke * 3) + letterGap; 

			// I
			cube.FillRect (color, nextX, screenMid - (letterHeight / 2), letterStroke, letterHeight);
			nextX += letterStroke + letterGap;

			// T
			cube.FillRect (color, nextX + letterStroke, screenMid - (letterHeight / 2), letterStroke, letterHeight);
			cube.FillRect (color, nextX, screenMid - (letterHeight / 2), (letterStroke * 3), letterStroke);
		}

        public static void DrawWin(Cube cube, Color color) {

            int screenMid = Cube.SCREEN_HEIGHT / 2;
            int letterHeight = 40;
            int letterStroke = 6;
            int letterGap = 6;
            int marginX = 30;

            int nextX = marginX;

            // W
            cube.FillRect(color, nextX, screenMid - (letterHeight / 2), letterStroke, letterHeight);
            cube.FillRect(color, nextX + (letterStroke * 2), screenMid - (letterHeight / 2), letterStroke, letterHeight);
            cube.FillRect(color, nextX + (letterStroke * 4), screenMid - (letterHeight / 2), letterStroke, letterHeight);
            cube.FillRect(color, nextX, screenMid + (letterHeight / 2) - letterStroke, (letterStroke * 5), letterStroke);
            nextX = marginX + (letterStroke * 5) + letterGap;

            // I
            cube.FillRect(color, nextX, screenMid - (letterHeight / 2), letterStroke, letterHeight);
            nextX += letterStroke + letterGap;

            // N
            cube.FillRect(color, nextX, screenMid - (letterHeight / 2), letterStroke, letterHeight);
            cube.FillRect(color, nextX, screenMid - (letterHeight / 2), (letterStroke * 3), letterStroke);
            cube.FillRect(color, nextX + (letterStroke * 2), screenMid - (letterHeight / 2), letterStroke, letterHeight);
        }

        public static void DrawLose(Cube cube, Color color) {

            int screenMid = Cube.SCREEN_HEIGHT / 2;
            int letterHeight = 40;
            int letterStroke = 6;
            int letterGap = 6;
            int marginX = 19;

            int nextX = marginX;

            // L
            cube.FillRect(color, nextX, screenMid - (letterHeight / 2), letterStroke, letterHeight);
            cube.FillRect(color, nextX, screenMid + (letterHeight / 2) - letterStroke, (letterStroke * 3), letterStroke);
            nextX = marginX + (letterStroke * 3) + letterGap;

            // O
            cube.FillRect(color, nextX, screenMid - (letterHeight / 2), (letterStroke * 3), letterStroke);
            cube.FillRect(color, nextX, screenMid - (letterHeight / 2), letterStroke, letterHeight);
            cube.FillRect(color, nextX + (letterStroke * 2), screenMid - (letterHeight / 2), letterStroke, letterHeight);
            cube.FillRect(color, nextX, screenMid + (letterHeight / 2) - letterStroke, (letterStroke * 3), letterStroke);
            nextX += (letterStroke * 3) + letterGap;

            // S
            cube.FillRect(color, nextX, screenMid - (letterHeight / 2), letterStroke, letterHeight / 2);
            cube.FillRect(color, nextX + (letterStroke * 2), screenMid, letterStroke, letterHeight / 2);
            cube.FillRect(color, nextX, screenMid - (letterHeight / 2), (letterStroke * 3), letterStroke);
            cube.FillRect(color, nextX, screenMid-(letterStroke/2), (letterStroke * 3), letterStroke);
            cube.FillRect(color, nextX, screenMid + (letterHeight / 2)-letterStroke, (letterStroke * 3), letterStroke);
            nextX += (letterStroke * 3) + letterGap;

            // E
            cube.FillRect(color, nextX, screenMid - (letterHeight / 2), letterStroke, letterHeight);
            cube.FillRect(color, nextX, screenMid - (letterHeight / 2), (letterStroke * 3), letterStroke);
            cube.FillRect(color, nextX, screenMid - (letterStroke / 2), (letterStroke * 3), letterStroke);
            cube.FillRect(color, nextX, screenMid + (letterHeight / 2) - letterStroke, (letterStroke * 3), letterStroke);
            nextX += (letterStroke * 3) + letterGap; 
        }

	}

}

