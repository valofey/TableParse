using System;

public class Player
{
	public string Name;
	public string Rating;
	public string FirstGame;
	public string SecondGame;
	public string Exception  =  null;
	public Player(string name, string rating, string firstGame, string secondGame)
	{
		Name = name;
		Rating = rating;
		FirstGame = firstGame;
		SecondGame = secondGame;
	}

	/* public Player()
	{
		Random random = new Random();
		Name = "ChingChong" + random.Next(1000).ToString();
		Rating = (random.Next(9) + 1).ToString() + "d";
		FirstGame = ((random.Next(2) == 1) ? "W(":"L(" )+ random.Next(100).ToString() + ")";
		SecondGame = ((random.Next(2) == 1) ? "W(" : "L(" )+ random.Next(100).ToString() + ")";
	}
	*/
}
