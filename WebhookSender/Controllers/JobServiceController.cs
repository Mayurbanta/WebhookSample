using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace WebhookSender.Controllers;
[Route("api/[controller]")]
[ApiController]
public class JobServiceController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public JobServiceController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost]
    public async Task<IActionResult> ProcessJob(int jobId)
    {
        JobPayload payload = new()
        {
            JobId = jobId,
            JobName = "Test Job Name"
        };

        await Task.Delay(1); //simulate job processing here
        payload.JobStatus = "Job Completed";

        //-- Job is completed, now send the post request to the webhook receiver
        string serializedPayload = JsonConvert.SerializeObject(payload);
        HttpContent content = new StringContent(serializedPayload, Encoding.UTF8, "application/json");

        //var client = httpClientFactory.CreateClient(new Uri("https://api.example.com"));
        var client = _httpClientFactory.CreateClient("MyApiClient");

        var response = await client.PostAsync("api/job/JobCompletionAction", content);

        if (response.IsSuccessStatusCode)
        {
            // Handle success
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response: " + responseBody);
            return Ok();
        }
        else
        {
            // Handle error
            Console.WriteLine("Error response: " + response.StatusCode);
            return BadRequest();
        }
    }
}


public class JobPayload
{
    public int JobId { get; set; }
    public string JobName { get; set; } = string.Empty;
    public string JobStatus { get; set; } = string.Empty;
}