using Atlassian.Jira;
using Microsoft.Extensions.Options;

namespace PCM.Services
{
    public class JiraConfig
    {
        public string Endpoint { get; set; }
        public string Email { get; set; }
        public string ApiToken { get; set; }
    }
    public interface IJiraService
    {
        Task<Issue> CreateTicketAsync(string summary, string description, string priority, string collection, string link);
        Task<IEnumerable<Issue>> GetUserTicketsAsync(string userAccountId);
    }
    public class JiraService : IJiraService
    {
        private readonly JiraConfig _config;
        private readonly Jira _jira;
        string Endpoint = "https://pcm-web.atlassian.net";

        public JiraService(IOptions<JiraConfig> config)
        {
            _config = config.Value;
            _jira = Jira.CreateRestClient(Endpoint, _config.Email, _config.ApiToken);
        }

        public async Task<Issue> CreateTicketAsync(string summary, string description, string priority, string collection, string link)
        {
            var issue = _jira.CreateIssue("KAN");

            issue.Summary = summary;
            issue.Type = "Task"; // or "Bug"
            issue.Priority = priority; // "High", "Average", "Low"
            issue.Description = description;
            issue["customfield_10000"] = link; // assumes custom field for "Link"
            issue["customfield_10001"] = collection; // assumes custom field for "Collection"

            return await issue.SaveChangesAsync();
        }

        public async Task<IEnumerable<Issue>> GetUserTicketsAsync(string userAccountId)
        {
            return await _jira.Issues.GetIssuesFromJqlAsync($"reporter = \"{userAccountId}\"");
        }
    }
}
