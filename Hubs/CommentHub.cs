using Microsoft.AspNetCore.SignalR;

namespace PCM.Hubs
{
    public class CommentHub : Hub
    {
        public async Task SendComment(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveComment", user, message);
        }
    }
}
