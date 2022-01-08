C: send REGISTER CLIENT REQUEST
S: send REGISTER CLIENT RESPONSE
C: send CREATE GAME REQUEST or JOIN GAME REQUEST
S:
	{ assign client ID; save map of clientId to its socket; } // assumes one of the expected requests was sent
	if CREATE GAME REQUEST
		{ create new GAME_OBJECT in database; save }
		send CREATE GAME RESPONSE
   else
		send JOIN GAME RESPONSE

Create Game flow
C: send NEW GAME
S: Notify C of each new Client that joins, then ask if that is the last client.
C: Tell server if it's the last player to join

Join Game flow:
C: Join game by Id,
S: Send acceptance
S: Send take turn request for first turn

Take turn flow:
S: send YOUR TURN or GAME OVER
	if YOUR TURN:
		C: send TURN DECISION 
	else GAME OVER:
		C: decide what to do



Messages
YOUR TURN:
	GameState gameState;
	bool isLastTurn;
GAME OVER:
	GameInfo finalScores

Objects
GAME_OBJECT:
	List<string> ClientIds;
	string firstClient;


## Stories
A client connents. If they are the first one, they are notified every time someone else connects and are given the choice to start the game or wait for another player until there are up to four players
When clients connect that aren't the first player, they are greeted and then asked to wait until it is their turn to play.
When the first player says that the last player has joined, or when there are four players, the first player is then asked to wait until it is their turn


##
Main
Game game = new Game();
Socket socket = AcceptConnection();
game.AddPlayer(socket, true)

bool lastPlayerAdded;
do
{
	socket = AcceptConnection();
	lastPlayerAdded = game.AddPlayer(socket, false)
} while (!lastPlayerAdded)

## Game Class
{
	List<Client> Clients;

	bool AddPlayer(Socket playerSocket, bool isAdmin)
	{
		var newClient = RegisterPlayer(socket, isAdmin);
		Clients.Add(newClient);
		if (Clients.Count < 4)
		{
			Socket adminSocket = Clients.First(x => x.isAdmin = true).socket;
			sendMessage(adminSocket, new PlayerAddedNotification());
			var rawResponse = recieveMessage(adminSocket);
			var response = Validate<PlayerAddedAcknowledgment>(rawResponse);
			return response.isLastPlayer;
		}
		else
		{
			return true;
		}
	}

	Client RegisterPlayer(Socket clientSocket, bool isAdmin)
	{
		string requestedUserName;
		bool repeat = true;
		do
		{
			var rawRequest = receiveRequest(socket)
			var request = Validate<RegisterNewClientRequest>(mesage);
			if (Clients.Exists(x => x.UserName == request.userName))
			{
				var response = new RegisterNewClientResponse
				{
					success = false,
					error = error.UserNameTaken,
				};
				SendMessage(clientSocket, response);
			}
			else
			{
				requestedUserName = request.UserName;
				repeat = false
			}
		} while (repeat)

		var clientId = new Guid().ToString();
		var authKey = new Guid().ToString();
		var response = new RegisterNewClientResponse // default success to true
		{
			clientId = clientId,
			authKey = authKey,
			isAdmin = isAdmin,
		};
		SendMessage(response, clientSocket); // on client side, wait for turn if not admin, else wait for next player enrollment

		return new Client
		{
			clientId = clientId,
			authKey = authKey,
			socket = clientSocket,
			userName = requestedUserName,
			isAdmin = isAdmin,
		});
	}
}

## Client Class
{
	string clientId;
	Socket socket;
	string userName;
	bool isAdmin;
}



## Main (Socket handler)
List<Task> gameTasks; // MVP: don't support multiple games, just one
Dictionary<string, Socket>> sockets;

while (true)
{
	Socket socket = AcceptConnection();
	new Task(() => 
	{





		BaseMessage message = recieveMessage(socket);
		if (message.type != 'register new client')
		{
			return; // or send 400
		}

		string clientId = newGuid();
		sendMessage(socket, new RegisterClientResponse(clientId));

		message = receieveMessage(socket);
		if (message.Type == 'Create new game')
		{
			Game game = new Game();
			gameTasks.Add(Task.Run(() =>
			{
				game.Start();
			});
		}
		else if (message.Type == 'Join game')
		{
			games.find(message.GameId).AddClient(clientId)
		}
	});
}


Game:
	string ClientIdOfFirstPlayer;

	Init() { wait for other players to join; if more than four, auto start, else ask client if }
	Start() { loops over players' turns in an order }































