using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

using Global;
using Global.Messaging;
using Global.Types;
using Global.Messaging.Messages;
using System.Threading.Tasks;

namespace Server.Types
{
	public class Game
	{
		private List<AccessibleClient> Clients { get; set; }
		private List<DevelopmentCard> FirstTierCards { get; set; }
		private List<DevelopmentCard> SecondTierCards { get; set; }
		private List<DevelopmentCard> ThirdTierCards { get; set; }
		private List<Noble> UnclaimedNobles { get; set; }
		private GemQuantity GemQuantity { get; set; }
		private int Wilds { get; set; }
		private readonly Socket Listener;

		public Game(Socket listener)
		{
			Clients = new List<AccessibleClient>();
			Listener = listener;
		}

		public void PlayGame(int numPlayers)
		{
			WaitForPlayersToJoin(numPlayers);
			InitializeGameState();
			BeginPlay();
		}

		public void WaitForPlayersToJoin(int numPlayers)
		{
			Task[] addClientTasks = new Task[numPlayers];
			for (int i = 0; i < numPlayers; i++)
			{
				Socket socket = Listener.Accept();
				addClientTasks[i] = Task.Run(() =>
				{
					AddClient(socket);
				});
			}
			Task.WaitAll(addClientTasks);
			if (Clients.Count != numPlayers)
			{
				throw new Exception("Did not add the expected number of players");
			}
		}

		private void AddClient(Socket socket)
		{
			AccessibleClient client = null;
			do
			{
				var request = MessagingUtils.ReceiveMessage<RegisterNewClientRequest>(socket);
				lock (Clients)
				{
					if (Clients.Exists(x => x.UserName == request.RequestedUserName))
					{
						var response = new RegisterNewClientResponse
						{
							Success = false,
							Error = ErrorCode.UserNameTaken
						};
						MessagingUtils.SendMessage(socket, response);
					}
					else
					{
						client = new AccessibleClient
						{
							ClientId = new Guid().ToString(),
							Socket = socket,
							UserName = request.RequestedUserName
						};
						Clients.Add(client);
					}
				}
			} while (client == null);

			Console.WriteLine($"Just added {client.UserName}");

			MessagingUtils.SendMessage(socket, new RegisterNewClientResponse
			{
				ClientId = client.ClientId
			});
		}

		private void InitializeGameState()
		{
			var pathToDataDir = @"..\..\..\..\Global\Data";
			var random = new Random();
			var allCards = Utils.ReadAllDevelopmentCards(pathToDataDir);
			FirstTierCards = allCards.FindAll(x => x.Level == DevelopmentLevel.Low).OrderBy(x => random.Next()).ToList();
			SecondTierCards = allCards.FindAll(x => x.Level == DevelopmentLevel.Middle).OrderBy(x => random.Next()).ToList();
			ThirdTierCards = allCards.FindAll(x => x.Level == DevelopmentLevel.High).OrderBy(x => random.Next()).ToList();
			UnclaimedNobles = Utils.ReadAllNobles(pathToDataDir).OrderBy(x => random.Next()).ToList().GetRange(0, Clients.Count + 1);
			GemQuantity = new GemQuantity(Clients.Count);
			Wilds = 5;
			Clients = Clients.OrderBy(x => random.Next()).ToList();
		}

		private void BeginPlay()
		{
			ToVisibleGameState().Print();
			bool isLastTurn = false;
			do
			{
				foreach (var client in Clients)
				{
					// ask client for its state
					// make sure that it matches what the server has on file
					// send game state
					// get Client's choice
					// validate the client can take the turn
					isLastTurn = client.Points >= 15;
				}

			} while (!isLastTurn);
		}

		private VisibleGameState ToVisibleGameState()
		{
			return new VisibleGameState
			{
				ClientStates = null,
				FirstTierCards = GetTopFourCards(FirstTierCards),
				SecondTierCards = GetTopFourCards(SecondTierCards),
				ThirdTierCards = GetTopFourCards(ThirdTierCards),
				UnclaimedNobles = UnclaimedNobles,
				AvailableGems = GemQuantity,
				AvailableWilds = Wilds,
			};
		}

		private List<DevelopmentCard> GetTopFourCards(List<DevelopmentCard> deck)
		{
			var numCardsToShow = deck.Count > 4 ? 4 : deck.Count;
			return deck.GetRange(0, numCardsToShow - 1);
		}
	}
}
