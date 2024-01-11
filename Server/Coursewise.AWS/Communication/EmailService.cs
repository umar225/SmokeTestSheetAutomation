using Amazon.Runtime;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Coursewise.AWS.Communication.Models;
using Coursewise.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.AWS.Communication
{
    public class EmailService : IEmailService
    {
        private readonly AwsSettings _awsSettings;

        private readonly ICoursewiseLogger<EmailService> _logger;
        public EmailService(ICoursewiseLogger<EmailService> logger, IOptionsSnapshot<AwsSettings> awsSettings)
        {
            _logger = logger;
            Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", awsSettings.Value.AWSAccessKeyID);
            Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", awsSettings.Value.AWSSecretAccessKey);
            Environment.SetEnvironmentVariable("AWS_REGION", awsSettings.Value.AWSRegion);
            _awsSettings = awsSettings.Value;
        }
        public async Task<bool> SendEmail(Email email)
        {
            email.FromEmail = (string.IsNullOrEmpty(email.FromEmail)) ? _awsSettings.FromEmail : email.FromEmail;
            var creds = new EnvironmentVariablesAWSCredentials();
            using (var sesClient = new AmazonSimpleEmailServiceClient(creds, Amazon.RegionEndpoint.EUWest2))
            {
                var destination = new Destination();
                var toLog = "";
                if (email.ToAddress is not null && email.ToAddress.Any())
                {
                    destination.BccAddresses = email.ToAddress;
                    toLog = "Multiple users";
                }
                else
                {
                    destination.ToAddresses = new List<string> { email.To };
                    toLog = email.To;
                }
                var sendRequest = new SendEmailRequest
                {
                    Source = $"GetBoardwise <{email.FromEmail}>",
                    Destination = destination,
                    Message = new Message
                    {
                        Subject = new Content(email.Subject),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = email.Content
                            }
                        },

                    }
                };

                try
                {
                    var response = await sesClient.SendEmailAsync(sendRequest);
                    var isSend = response.HttpStatusCode == System.Net.HttpStatusCode.OK;
                    var messageId = String.IsNullOrEmpty(response.MessageId) ? "" : response.MessageId;
                    if (isSend)
                    {
                        _logger.Info($"Email sent to {toLog} at {DateTime.Now.ToString("dd/MM/yyyy - HH:mm")}, MessageId {messageId}, Subject: {email.Subject}");
                    }
                    else
                    {
                        _logger.Info($"Email not sent to {toLog} at {DateTime.Now.ToString("dd/MM/yyyy - HH:mm")}, MessageId {messageId}, Subject: {email.Subject}");
                    }
                    return isSend;
                }
                catch (Exception ex)
                {
                    _logger.Exception(ex);
                    return false;
                }
            }
        }

        public async Task<bool> SendRawEmail(Email email)
        {

            var creds = new EnvironmentVariablesAWSCredentials();
            using (var sesClient = new AmazonSimpleEmailServiceClient(creds, Amazon.RegionEndpoint.EUWest2))
            {

                using (var messageStream = new MemoryStream())
                {
                    var message = new MimeMessage();
                    var builder = new BodyBuilder() { HtmlBody = email.Content };

                    message.From.Add(MailboxAddress.Parse($"GetBoardwise <{email.FromEmail}>"));
                    message.To.Add(MailboxAddress.Parse(email.To));
                    message.Subject = email.Subject;
                    if (email.Attachments.Any())
                    {
                        foreach (var attachment in email.Attachments)
                        {

                            if (attachment.Data is null)
                            {
                                builder.Attachments.Add(attachment.Name, attachment.DataStream);
                            }
                            else
                            {
                                builder.Attachments.Add(attachment.Name, attachment.Data);
                            }
                        }
                    }

                    message.Body = builder.ToMessageBody();
                    message.WriteTo(messageStream);

                    var request = new SendRawEmailRequest()
                    {
                        RawMessage = new RawMessage() { Data = messageStream }
                    };

                    try
                    {
                        var response = await sesClient.SendRawEmailAsync(request);
                        var isSend = response.HttpStatusCode == System.Net.HttpStatusCode.OK;
                        var messageId = String.IsNullOrEmpty(response.MessageId) ? "" : response.MessageId;
                        if (isSend)
                        {
                            _logger.Info($"Email sent to {email.To} at {DateTime.Now.ToString("dd/MM/yyyy - HH:mm")}, MessageId {messageId}, Subject: {email.Subject}");
                        }
                        else
                        {
                            _logger.Info($"Email not sent to {email.To} at {DateTime.Now.ToString("dd/MM/yyyy - HH:mm")}, MessageId {messageId}, Subject: {email.Subject}");
                        }
                        return isSend;
                    }
                    catch (Exception ex)
                    {
                        _logger.Exception(ex);
                        return false;
                    }
                }
            }
        }
    }
}
