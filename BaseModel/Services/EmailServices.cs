using FluentEmail.Core;

namespace BaseModel.Services
{
    public class EmailServices
    {
        private readonly IFluentEmail _email;
        public EmailServices(IFluentEmail fluentEmail)
        {
            _email = fluentEmail;
        }

        public IFluentEmail To(string recipient)
        {
            return _email.To(recipient);
        }

        public async Task SendAsync()
        {
            await _email.SendAsync();
        }

    }
}
