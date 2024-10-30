using SendGrid;
using SendGrid.Helpers.Mail;

namespace SycappsWeb.Server;

public class UserRegisteredEventHandler : IEventHandler<UserRegisteredEvent>
{
    public async Task Handle(UserRegisteredEvent domainEvent)
    {
        var client = new SendGridClient("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");
        var from = new EmailAddress("info@sycapps.net", "Info");
        var subject = "Confirma tu dirección de correo";
        var to = new EmailAddress(domainEvent.Email);
        var templateId = "d-0ca4246253d348268355ae4b85ee7258"; //Un2TrekConfirmacionCorreo

        var msg = new SendGridMessage
        {
            From = from,
            TemplateId = templateId,
            Subject = subject
        };

        msg.AddTo(to);
        msg.SetTemplateData(new
        {
            confirm_url = domainEvent.ConfirmationLink
        });

        var response = await client.SendEmailAsync(msg);
    }
}
