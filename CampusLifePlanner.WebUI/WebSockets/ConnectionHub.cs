using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CampusLifePlanner.WebUI.WebSockets;

[Authorize]
public class ConnectionHub : Hub
{
    public async Task GetQRCode()
    {
        var qrCode = "ABCD123";

        await Clients.Caller.SendAsync("ResultQRCode", qrCode);    
    }

    public override Task OnConnectedAsync()
    {
        var x = Context.ConnectionId;

        return base.OnConnectedAsync();
    }
}
