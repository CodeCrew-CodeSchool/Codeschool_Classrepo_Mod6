using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;
using RestSharp.Authenticators;
using System.Net.Mail;

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
			MailMessage mm = new MailMessage();
			SmtpClient smtp = new SmtpClient();

			mm.From = new MailAddress("dj@code-crew.org", "DJ", System.Text.Encoding.UTF8);
			mm.To.Add(new MailAddress("dj@dij.io"));
			mm.Subject = "Email Demo";
			mm.Body = "Email works";

			mm.IsBodyHtml = false;
			smtp.Host = "smtp.mailgun.org";
			smtp.EnableSsl = false;
			System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
			NetworkCred.UserName = "postmaster@mg.dij.io";//gmail user name
			NetworkCred.Password = "85be821b7931c9f9a1373f1237dc3571";// password
			smtp.UseDefaultCredentials = true;
			smtp.Credentials = NetworkCred;
			smtp.Port = 587; //Gmail port for e-mail 465 or 587
			smtp.Send(mm);


			//API
			RestClient client = new RestClient();
			client.BaseUrl = new Uri(_config.GetSection("Mailgun")["Base"]);
			client.Authenticator = new HttpBasicAuthenticator("api", _config.GetSection("Mailgun")["Key"]);
			RestRequest request = new RestRequest();
			request.AddParameter("domain", _config.GetSection("Mailgun")["Domain"], ParameterType.UrlSegment);
			request.Resource = "{domain}/messages";
			request.AddParameter("from", "Excited User <" + _config.GetSection("Mailgun")["Domain"] + ">");
			//request.AddParameter("to", "bar@example.com");
			request.AddParameter("to", "dj@code-crew.org");
			request.AddParameter("subject", "Hello");
			request.AddParameter("text", "Testing some Mailgun awesomness!");
			request.Method = Method.POST;
			//client.Execute(request);
			var t = await client.ExecutePostAsync(request);
			Console.WriteLine(t);
			/**/
		}
	}
}