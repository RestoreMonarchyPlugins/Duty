using Newtonsoft.Json;
using RestoreMonarchy.Duty.Models.Discord;
using Rocket.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace RestoreMonarchy.Duty.Services;

public class WebhookService
{
    public void SendMessage(WebhookMessage message, Dictionary<string, object> param)
    {
        WebhookMessage sendMessage = (WebhookMessage)message.Clone();

        sendMessage.Prepare();
        sendMessage.FormatParameters(param);
        sendMessage.Validate();

        try
        {
            SendMessage(sendMessage.WebhookUrl, sendMessage);
        } catch (Exception e)
        {
            Logger.LogException(e);
        }            
    }

    private WebResponse SendMessage(string url, WebhookMessage message)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

        JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        string content = JsonConvert.SerializeObject(message, jsonSerializerSettings);
        byte[] data = Encoding.UTF8.GetBytes(content);

        request.Method = "POST";
        request.ContentType = "application/json";
        request.ContentLength = data.Length;

        using Stream stream = request.GetRequestStream();

        stream.Write(data, 0, data.Length);

        return request.GetResponse();
    }
}