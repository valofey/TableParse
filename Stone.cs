using System;

public class Stone
{
	public int X;
	public int Y;
	public bool IsWhite;
	public string StoneData;
	public Stone(int x, int y,bool isWhite, string stoneData)
	{
		X = x;
		Y = y;
		IsWhite = isWhite;
		StoneData = stoneData;
	}
}
