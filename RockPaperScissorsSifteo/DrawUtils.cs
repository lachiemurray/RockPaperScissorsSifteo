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
	}
}

