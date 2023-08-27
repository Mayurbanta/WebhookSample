using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebhookReceiver.Controllers;
[Route("api/[controller]")]
[ApiController]
public class JobController : ControllerBase
{
    [HttpPost("JobCompletionAction")]
    public IActionResult JobCompletionAction(JobPayload jobPayload)
    {
        Console.WriteLine($"Job staus: {jobPayload.JobStatus}");

        //--perform some activity like send email, archive data etc when the job gets complete

        return Ok("Email sent");
    }
}

public class JobPayload
{
    public int JobId { get; set; }
    public string JobName { get; set; } = string.Empty;
    public string JobStatus { get; set; } = string.Empty;
}