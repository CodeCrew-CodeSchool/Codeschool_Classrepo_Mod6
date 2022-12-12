using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;
using RestSharp.Authenticators;
using System.Net.Mail;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;

namespace EmailDemo.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		private IConfiguration _config;

		public IndexModel(ILogger<IndexModel> logger, IConfiguration config)
		{
			_logger = logger;
			_config = config;
		}

		public void OnGet()
		{

		}

		public async Task OnPost()
		{
            //SMTP
            // Compose a message
            MimeMessage mail = new MimeMessage();
            mail.From.Add(new MailboxAddress("Excited Admin", _config.GetSection("Mailgun")["From"]));
            mail.To.Add(new MailboxAddress("Excited User", "dj@dij.io"));
            mail.Subject = "Hello";
            mail.Body = new TextPart("plain")
            {
                Text = @"Testing some Mailgun awesomesauce!",
            };

            // Send it!
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                // XXX - Should this be a little different?
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect("smtp.mailgun.org", 587, false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate("postmaster@mg.dij.io", "85be821b7931c9f9a1373f1237dc3571");

                client.Send(mail);
                client.Disconnect(true);
            }


   //         //API
   //         RestClient client = new RestClient();
			//client.BaseUrl = new Uri(_config.GetSection("Mailgun")["Base"]);
			//client.Authenticator = new HttpBasicAuthenticator("api", _config.GetSection("Mailgun")["Key"]);
			//RestRequest request = new RestRequest();
			//request.AddParameter("domain", _config.GetSection("Mailgun")["Domain"], ParameterType.UrlSegment);
			//request.Resource = "{domain}/messages";
			//request.AddParameter("from", "Excited User <" + _config.GetSection("Mailgun")["Domain"] + ">");
			////request.AddParameter("to", "bar@example.com");
			//request.AddParameter("to", "dj@code-crew.org");
			//request.AddParameter("subject", "Hello");
			//request.AddParameter("text", "Testing some Mailgun awesomness!");
			//request.Method = Method.POST;
			////client.Execute(request);
			//var t = await client.ExecutePostAsync(request);
			//Console.WriteLine(t);
			///**/
		}
	}
}