using Microsoft.AspNetCore.SignalR;

namespace PCM.Hubs
{
    public class CommentHub : Hub
    {
        // This method allows a client to send a comment to all connected clients.
        public async Task SendComment(string user, string message)
        {
            // The Clients.All.SendAsync method sends the received comment to all connected clients.
            // "ReceiveComment" is the name of the client-side method that will handle the comment.
            // The parameters `user` and `message` are passed along to the clients.
            await Clients.All.SendAsync("ReceiveComment", user, message);
        }
    }
}
