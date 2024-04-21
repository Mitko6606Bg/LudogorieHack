using BusinessContactsPlatform.Data;
using BusinessContactsPlatform.Data.Entities;
using BusinessContactsPlatform.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BusinessContactsPlatform.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ContactController(AppDbContext context , UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendContactRequest(string receiverId)
        {
            var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var receiverUser = await _userManager.FindByIdAsync(receiverId);
            if (receiverUser == null)
            {
                return NotFound();
            }
            var senderUser = await _userManager.FindByIdAsync(senderId);
            if (senderUser == null)
            {
                return NotFound();
            }

            
            var existingRequest = await _context.ContactRequests
                .FirstOrDefaultAsync(cr => cr.SenderUserId == senderId && cr.ReceiverUserId == receiverId && cr.Status == ContactRequestStatus.Pending);

            if (existingRequest != null)
            {
                TempData["Message"] = "You have already sent a contact request to this user.";
                return BadRequest();
            }

            var contactRequest = new ContactRequest
            {
                SenderUserId = senderId,
                ReceiverUserId = receiverId,
                Status = ContactRequestStatus.Pending,
                IsRead = false,
            };

            _context.ContactRequests.Add(contactRequest);
            await _context.SaveChangesAsync();

            
            var emailService = new Email();
            var subject = "New Contact Request"; 
            var body = $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    margin: 0;
                    padding: 0;
                }}

                .container {{
                    max-width: 600px;
                    margin: 0 auto;
                    padding: 20px;
                    background-color: #fff;
                    border-radius: 10px;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                }}

                h1 {{
                    color: #333;
                }}

                ul {{
                    list-style-type: none;
                    padding: 0;
                }}

                li {{
                    margin-bottom: 10px;
                }}

                strong {{
                    font-weight: bold;
                }}
            </style>
        </head>
       <body>
             <div class='container'>
                 <h1>Contact Request Notification</h1>
                 <p>Hey {receiverUser.UserName},</p>
                 <p>You have received a contact request from {senderUser.UserName}:</p>
                 <p>Please log in to your account to view and respond to the request.</p>
                 <p>Thank you!</p>
             </div>
         </body>
        </html>";

            
            await emailService.SendEmailAsync(receiverUser.Email, subject, body);

            TempData["Message"] = "Contact request sent successfully!";
            return Ok();
        }



        [HttpPost]
        public async Task<IActionResult> AcceptContactRequest(int requestId)
        {
            var contactRequest = await _context.ContactRequests.FindAsync(requestId);
            contactRequest.Status = ContactRequestStatus.Accepted;
            await _context.SaveChangesAsync();

            var senderUser = await _userManager.FindByIdAsync(contactRequest.SenderUserId);
            var receiverUser = await _userManager.FindByIdAsync(contactRequest.ReceiverUserId);

            var emailService = new Email();
            var subject = "Contact Request Accepted";
            var body = @"
                    <html>
                    <head>
                        <style>
                            /* CSS styles for the email body */
                            /* You can add your custom styles here */
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <h1>Contact Request Accepted</h1>
                            <p>Hey " + senderUser.UserName + @",</p>
                            <p>Your contact request sent to " + receiverUser.UserName + @" has been accepted.</p>
                            <p>You can now connect with " + receiverUser.UserName + @".</p>
                            <p>Thank you!</p>
                        </div>
                    </body>
                    </html>";
            await emailService.SendEmailAsync(senderUser.Email, subject, body);

            return RedirectToAction("ContactRequests", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> DeclineContactRequest(int requestId)
        {
            var contactRequest = await _context.ContactRequests.FindAsync(requestId);
            contactRequest.Status = ContactRequestStatus.Declined;
            await _context.SaveChangesAsync();

            var senderUser = await _userManager.FindByIdAsync(contactRequest.SenderUserId);
            var receiverUser = await _userManager.FindByIdAsync(contactRequest.ReceiverUserId);

            var emailService = new Email();
            var subject = "Contact Request Declined";
            var body = @"
                  <html>
                  <head>
                      <style>
                          /* CSS styles for the email body */
                          /* You can add your custom styles here */
                      </style>
                  </head>
                  <body>
                      <div class='container'>
                          <h1>Contact Request Declined</h1>
                          <p>Hey " + receiverUser.UserName + @",</p>
                          <p>Your contact request sent to " + senderUser.UserName + @" has been declined.</p>
                      </div>
                  </body>
                  </html>";

            await emailService.SendEmailAsync(receiverUser.Email, subject, body);

            return RedirectToAction("ContactRequests", "Account");
        }



    }
}
