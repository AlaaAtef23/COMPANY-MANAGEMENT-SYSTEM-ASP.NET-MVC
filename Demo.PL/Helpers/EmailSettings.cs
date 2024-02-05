using Demo.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helpers
{
	public class EmailSettings
	{
        public static void SenEmail(Email email )
		{
			var client= new SmtpClient("smtp.gmail.com",587);
			client.EnableSsl = true;
			client.Credentials = new NetworkCredential("alaaatefe@gmail.com", "P@swo0rd");
			client.Send("alaaatefe@gmail.com", email.Recipients, email.Subject, email.Body);
		}
	}
}
