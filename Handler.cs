using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace EventNotifier
{
  public class Handler
  {
    private IConfiguration _configuration;

    public string S3Slack(S3Event evnt, ILambdaContext context)
    {
      InitConfig();

      var s3EventNotification = evnt.Records[0];
      if (s3EventNotification == null) return null;

      // Send the Slack message
      SlackClient slackClient = new SlackClient(_configuration["SlackWebhookUrl"]);
      slackClient.PostMessage($"S3 Event: {s3EventNotification.EventName} {s3EventNotification.S3.Object.Key}");
      return "Slack message sent";
    }

    public string S3SlackDetails(S3Event evnt, ILambdaContext context)
    {
      InitConfig();

      var s3EventNotification = evnt.Records[0];
      if (s3EventNotification == null) return null;

      // Serialize the event object
      string result = JsonConvert.SerializeObject(s3EventNotification);

      // Send the Slack message
      SlackClient slackClient = new SlackClient(_configuration["SlackWebhookUrl"]);
      slackClient.PostMessage(result);
      return "Slack message sent";
    }

    private void InitConfig()
    {
      _configuration = new ConfigurationBuilder()
      //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
      .AddEnvironmentVariables()
      .Build();
    }

  }

  public class Response
  {
    public string Message { get; set; }
    public Request Request { get; set; }

    public Response(string message, Request request)
    {
      Message = message;
      Request = request;
    }
  }

  public class Request
  {
    public string Key1 { get; set; }
    public string Key2 { get; set; }
    public string Key3 { get; set; }
  }
}
