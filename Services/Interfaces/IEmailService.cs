using SystemBackend.Models.Entities;

namespace SystemBackend.Services.Interfaces
{
    public interface IEmailService
    {
        public void SendEmail(string receiverEmail, string subject, string body);
        public void SendReceiveCreateAccountRequest(PendingUser pendingUser);
        public void SendApproveCreateAccountRequest(PendingUser pendingUser);
        public void SendDenyCreateAccountRequest(PendingUser pendingUser);
    }
}
