using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EmailDemo.Services.Email.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;
using RestSharp.Authenticators;
using Microsoft.Extensions.Configuration;

namespace EmailDemo.Pages
{
  public class IndexModel : PageModel
  {
    
    public IConfiguration Configuration;

    [BindProperty]
    public Player player { get; set; }

    [BindProperty]
    public string Message { get; set; }

    public IEmailSender emailService { get; set; }

    public IndexModel(IEmailSender service, IConfiguration config)
    {
      emailService = service;
      Configuration = config;
    }

    public void OnGet()
    {
    }

    public async Task OnPost()
    {

      if (ModelState.IsValid)
      {
         //var conf = IndexModel.Configuration.GetSection("Mailgun");
         //conf.Key
         RestClient client = new RestClient();
         client.BaseUrl = new Uri("https://api.mailgun.net/v3");
         client.Authenticator = new HttpBasicAuthenticator("api", "key-a1b3e15c867aa6dacc69e997da5b0e67\r\n");
         RestRequest request = new RestRequest();
         request.AddParameter("domain", "sandbox6e650fc917484c4fb89c282a483f55a2.mailgun.org", ParameterType.UrlSegment);
         request.Resource = "{domain}/messages";
         request.AddParameter("from", "Excited User <mailgun@sandbox6e650fc917484c4fb89c282a483f55a2.mailgun.org>");
         request.AddParameter("to", "bar@example.com");
         request.AddParameter("to", "YOU@sandbox6e650fc917484c4fb89c282a483f55a2.mailgun.org");
         request.AddParameter("subject", "Hello");
         request.AddParameter("text", "Testing some Mailgun awesomness!");
         request.Method = Method.POST;
         client.Execute(request);

				//await emailService.SendEmailAsync(player.Email, $"Welcome to the team, {player.Name}!", $"<p>We can't wait to see you wearing {player.Number} while playing {player.Position} </p>");

				//Message = "Email Sent!";

				//player.Name = string.Empty;
				//player.Email = string.Empty;
				//player.Position = string.Empty;
				//player.Number = null;

			}

    }

    public IRestResponse SendMailGun()
    {
            //      var conf = Configuruation.GetSection("Mailgun");
            //      conf.Key
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v3");
            client.Authenticator = new HttpBasicAuthenticator("api", "key-a1b3e15c867aa6dacc69e997da5b0e67\r\n");
            RestRequest request = new RestRequest();
            request.AddParameter("domain", "sandbox6e650fc917484c4fb89c282a483f55a2.mailgun.org", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "Excited User <mailgun@sandbox6e650fc917484c4fb89c282a483f55a2.mailgun.org>");
            request.AddParameter("to", "bar@example.com");
            request.AddParameter("to", "YOU@sandbox6e650fc917484c4fb89c282a483f55a2.mailgun.org");
            request.AddParameter("subject", "Hello");
            request.AddParameter("text", "Testing some Mailgun awesomness!");
            request.Method = Method.POST;
            return client.Execute(request);
            //sandbox6e650fc917484c4fb89c282a483f55a2.mailgun.org
        }

    public class Player
    {
      [Required]
      public string Name { get; set; }
      [Required]
      public string Email { get; set; }
      [Required]
      public string Position { get; set; }
      [Required]
      public int? Number { get; set; }
    }
  }
}
