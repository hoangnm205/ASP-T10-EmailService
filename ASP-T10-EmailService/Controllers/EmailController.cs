using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP_T10_EmailService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {
        [HttpPost]
        public IActionResult Index(IFormFile file)
        {
            // 1. Chuan bi Message

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("NPCETC", "info@hisoft.com.vn"));
            emailMessage.To.Add(new MailboxAddress("Nghiem Hoang", "hoangnm.dev@gmail.com"));
            emailMessage.Subject = "Test Send Email From API With Attachment";

            // 1.1 Setup body

            MemoryStream stream = new MemoryStream();
            file.CopyTo(stream);
            byte[] fileBytes = stream.ToArray();


            BodyBuilder bodyBuilder = new BodyBuilder()
            {
                HtmlBody = "<h1><font color='red'>Day la email thu nghiem</font></h1>",
                TextBody = "Day la email thu nghiem"
            };
            bodyBuilder.Attachments.Add(file.FileName, fileBytes);

            emailMessage.Body = bodyBuilder.ToMessageBody();


            // 2. Ket noi den SMTP Server
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect("e-vdc-01.vinahost.vn", 465, true);
                    client.Authenticate("info@hisoft.com.vn", "mailHh223344@");


                    client.Send(emailMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return Ok();
        }
    }
}

