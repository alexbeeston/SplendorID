public void PlayGame()
{
	while (!isLastTurn)
	{
		foreach (var client in Client)
		{
			S: ask for client state (GetClientStateRequest)
			C: give their state (GetClientStateResponse)
			-> Server validates that the client state is correct
			S: give game state (GetTurnChoiceRequest)
			C: give server their choice (GetTurnChoiceResponse)
			-> Server updates the client and game state
			-> isLastTurn = client.Points >= 15
		}
	}
}


// next iteration: server writes game play down to a database, bash/batch script plays lots of games, then another program can process the datao
