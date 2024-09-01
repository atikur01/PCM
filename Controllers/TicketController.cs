using Atlassian.Jira;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using PCM.Data;
using PCM.Models;
using PCM.ViewModels;
using Serilog;
using System.Text;

namespace PCM.Controllers
{
    public class TicketController : Controller
    {
        private readonly AppDbContext _context;
        public TicketController(AppDbContext context)
        {
            _context = context; 
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            var userid = HttpContext.Session.GetString("Id");
            if (userid == null) { return RedirectToAction("Login", "Account"); }

            var referringUrl = Request.Headers["Referer"].ToString();

            ViewBag.ReferringUrl = referringUrl;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult>  Create(SupportTicketViewModel supportTicketViewModel)
        {
            var userid = HttpContext.Session.GetString("Id");
            if (userid == null) { return RedirectToAction("Login", "Account"); }

            var jira = Jira.CreateRestClient("https://pcm-web.atlassian.net/", "coc13259@gmail.com", "ATATT3xFfGF08d7w8X1XAo8dFS08GruQ5HzJ3r4hkoSfzepcKnGD2hV8B32KHCWx12H2fkGJShoejKa66lNxC61Uua10h5Qqbf2qNx3Jw1O2H0aaGs6W-DPcvYy1LJiUOya1errIiyEBIJN1HlVd2kBsV0SDZ5yM3w4oF05Qb-p2ZzHQUQEelGw=4496E0B0");

            try
            {
                await CreateJiraUser(supportTicketViewModel.Reported, supportTicketViewModel.FullName);

                var issue = jira.CreateIssue("KAN");
                issue.Type = "Ticket";

                issue.Summary = supportTicketViewModel.Summary;
                issue.Priority = supportTicketViewModel.Priority;
                issue.CustomFields.Add("Reported", supportTicketViewModel.Reported);
                issue.CustomFields.Add("Collection", supportTicketViewModel.Collection);
                issue.CustomFields.Add("Link", supportTicketViewModel.Link);

                await issue.SaveChangesAsync();


                // Assuming 'issue.Key' contains the key of the created issue
                string issueKey = issue.Key.ToString(); // Replace with the correct property if it's named differently
                string jiraBaseUrl = "https://pcm-web.atlassian.net"; // Replace with your Jira base URL
                string issueUrl = $"{jiraBaseUrl}/browse/{issueKey}";

                Log.Information("Issue created successfully.");

                Log.Information($"Issue URL: {issueUrl}");
                Log.Information($"Issue key: {issueKey}");

                var Ticket = new Tickets
                {
                    TicketId = issueKey,
                    Summary = supportTicketViewModel.Summary,
                    Priority = supportTicketViewModel.Priority,
                    Reported = supportTicketViewModel.Reported,
                    Collection = supportTicketViewModel.Collection,
                    Link = issueUrl,
                    CreatedAt = DateTime.Now.ToString(),
                    Status = "Open"
                };

                _context.Tickets.Add(Ticket);
                await _context.SaveChangesAsync();

                return RedirectToAction("JiraTicketLink", new { link = issueUrl });

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message.ToString());
            }


            return View();
        }

        public static async Task CreateJiraUser(string email, string fullName)
        {
            

            string JiraBaseUrl = "https://pcm-web.atlassian.net";
         string ApiToken = "ATATT3xFfGF08d7w8X1XAo8dFS08GruQ5HzJ3r4hkoSfzepcKnGD2hV8B32KHCWx12H2fkGJShoejKa66lNxC61Uua10h5Qqbf2qNx3Jw1O2H0aaGs6W-DPcvYy1LJiUOya1errIiyEBIJN1HlVd2kBsV0SDZ5yM3w4oF05Qb-p2ZzHQUQEelGw=4496E0B0";  // Replace with your Jira API token
         string Email = "coc13259@gmail.com"; // Replace with your Jira email

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(JiraBaseUrl);
                var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{Email}:{ApiToken}"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authToken);

                var newUser = new
                {
                    emailAddress = email,
                    displayName = fullName,
                    notification = true,
                    password = email,
                    products = new List<string>() { "jira-software" },


                };

                var jsonContent = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("/rest/api/3/user", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("User created successfully.");
                }
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to create user. Status code: {response.StatusCode}");
                    Console.WriteLine($"Response: {responseBody}");
                }
            }
        }

        public async Task<IActionResult>  ViewAllTickets()
        {
            var userid = HttpContext.Session.GetString("Id");
            if (userid == null) { return RedirectToAction("Login", "Account"); }

            var email = HttpContext.Session.GetString("Email");

            var tickets = _context.Tickets.Where(t => t.Reported == email).ToList();
            
            var viewModel = new ViewAllTicketsViewModel
            {
                Tickets = new List<Tickets>()
            };


            foreach (var ticket in tickets)
            {
                var jira = Jira.CreateRestClient("https://pcm-web.atlassian.net/", "coc13259@gmail.com", "ATATT3xFfGF08d7w8X1XAo8dFS08GruQ5HzJ3r4hkoSfzepcKnGD2hV8B32KHCWx12H2fkGJShoejKa66lNxC61Uua10h5Qqbf2qNx3Jw1O2H0aaGs6W-DPcvYy1LJiUOya1errIiyEBIJN1HlVd2kBsV0SDZ5yM3w4oF05Qb-p2ZzHQUQEelGw=4496E0B0");
                try
                {
                    // Fetch the issue using the provided issue key
                    var issue = await jira.Issues.GetIssueAsync(ticket.TicketId);

                    // Check if the issue is not null
                    if (issue != null)
                    {
                        // Retrieve the summary and status of the issue
                        string summary = issue.Summary;
                        var status = issue.CustomFields["Status"].Values;
                        var priority = issue.Priority;
                        var created = issue.Created;

                        // Print or return the summary and status
                        Log.Information($"Issue Key: {ticket.TicketId}");
                        Log.Information($"Summary: {summary}");
                        Log.Information($"Status: {status[0]}");
                        Log.Information($"Priority: {priority}");
                        Log.Information($"Created: {created}");

                        var obj = new Tickets
                        {
                            TicketId = ticket.TicketId,
                            Summary = summary,
                            Priority =  priority.Name,
                            CreatedAt = created.ToString(),
                            Status = status[0],
                            Collection = ticket.Collection,
                            Link = ticket.Link
                        };

                        viewModel.Tickets.Add(obj);

                        
                    }
                    else
                    {
                        Log.Information($"Issue with key '{ticket.TicketId}' not found.");
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"An error occurred: {ex.Message}");
                }





            }


            return View(viewModel);
        }



        public IActionResult JiraTicketLink(string link)
        {
            ViewBag.TicketLink = link;
            return View();
        }

        public static async Task<Tickets>? GetIssueSummaryAndStatusAsync(string issueKey)
        {
            var jira = Jira.CreateRestClient("https://pcm-web.atlassian.net/", "coc13259@gmail.com", "ATATT3xFfGF08d7w8X1XAo8dFS08GruQ5HzJ3r4hkoSfzepcKnGD2hV8B32KHCWx12H2fkGJShoejKa66lNxC61Uua10h5Qqbf2qNx3Jw1O2H0aaGs6W-DPcvYy1LJiUOya1errIiyEBIJN1HlVd2kBsV0SDZ5yM3w4oF05Qb-p2ZzHQUQEelGw=4496E0B0");
            try
            {
                // Fetch the issue using the provided issue key
                var issue = await jira.Issues.GetIssueAsync(issueKey);

                // Check if the issue is not null
                if (issue != null)
                {
                    // Retrieve the summary and status of the issue
                    string summary = issue.Summary;
                    var status = issue.CustomFields["Status"].Values;
                    var priority = issue.Priority;
                    var created = issue.Created;

                    // Print or return the summary and status
                    Console.WriteLine($"Issue Key: {issueKey}");
                    Console.WriteLine($"Summary: {summary}");
                    Console.WriteLine($"Status: {status[0]}");
                    Console.WriteLine($"Priority: {priority}");
                    Console.WriteLine($"Created: {created}");

                    return new Tickets
                    {
                        TicketId = issueKey,
                        Summary = summary,
                        Priority = priority.ToString(),
                        CreatedAt = created.ToString(),
                        Status = status[0]
                    };
                }
                else
                {
                    Console.WriteLine($"Issue with key '{issueKey}' not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return null;

        }



    }
}
