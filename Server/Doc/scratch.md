public void PlayGame()
{
	Clients.ShuffleOrder();
	// draw n + 1 nobles
	// load each tier of mines into memory
	// shuffle each tier
	// draw top four
	// initialize bank
	while (!isLastTurn)
	{
		foreach (var client in Client)
		{
			// validate the server and client states match
			// send game state
			// get client's choice
			// validate the choice is valid
			// set isLastTurn
		}
	}
}


// next iteration: server writes game play down to a database, bash/batch script plays lots of games, then another program can process the data