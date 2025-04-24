using Microsoft.AspNetCore.SignalR;

namespace BusinessLogic.Services
{
    public class ChatHub :Hub
    {
        CancellationToken token = new CancellationToken();
        public override Task OnConnectedAsync()
        {
            // Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            return base.OnConnectedAsync();
        }

        public async Task SendMessage(string message) 
        {
            await Clients.All.SendAsync("Receive", message, token);
        }

    }
}
