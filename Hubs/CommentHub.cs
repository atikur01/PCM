using Microsoft.AspNetCore.SignalR;

namespace PCM.Hubs
{
    public class CommentHub : Hub
    {
        // Method to send a comment to all clients connected to a specific item group (identified by itemId)
        public async Task SendComment(Guid itemId, string user, string message)
        {
            // Send the comment only to clients in the group associated with the specific itemId (GUID).
            await Clients.Group($"Item-{itemId}").SendAsync("ReceiveComment", itemId, user, message);
            

        }

        // Method to allow a client to join a group based on the itemId
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        // Method to allow a client to leave a group based on the itemId
        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
