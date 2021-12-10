using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

using Global;
using Global.Messaging;
using Global.Messaging.Messages;

namespace Client.Players
{
	class HumanPlayer : BasePlayer
	{
		protected override void CreateNewClient()
		{
			var payload = new CreateNewClientRequest();
			Messenger.SendMessage(Message.CreateMessage(string.Empty, payload));
		}

		protected override void HandleNewClientCreated(Message message)
		{
			var payload = JsonConvert.DeserializeObject<CreateNewClientResponse>(message.SerializedPayload);
			ClientId = payload.ClientId;
			AuthorizationKey = payload.AuthorizationKey;
			Console.WriteLine($"Human Player got\n  ClientId: {ClientId}\n  Authorization Key: {AuthorizationKey}");
		}
	}
}
