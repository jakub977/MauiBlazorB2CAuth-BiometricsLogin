using System.Net;
using System.Net.Http.Headers;
using System.Text;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Google.Apis.FirebaseCloudMessaging.v1;
using AndroidConfig = Google.Apis.FirebaseCloudMessaging.v1.Data.AndroidConfig;
using AndroidNotification = Google.Apis.FirebaseCloudMessaging.v1.Data.AndroidNotification;
using ApnsConfig = Google.Apis.FirebaseCloudMessaging.v1.Data.ApnsConfig;
using Message = Google.Apis.FirebaseCloudMessaging.v1.Data.Message;
using Notification = Google.Apis.FirebaseCloudMessaging.v1.Data.Notification;

namespace Principal.Telemedicine.Shared.Firebase;

public class FcmNotificationService : IFcmNotificationService
{
    string jsonServiceKey = "{\r\n  \"type\": \"service_account\",\r\n  \"project_id\": \"vanda-25081\",\r\n  \"private_key_id\": \"9debff7e96743b24f7364025ee92c6f4d2cdb95f\",\r\n  \"private_key\": \"-----BEGIN PRIVATE KEY-----\\nMIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQC9KugIjY1RzSq6\\n3aLC3o8cGVwLM7S2/NdMSIWDR9KqpnPRbq3KqC+6WRZ2Lm6+pWsAyezklrkFYnvc\\nLdKixSpKmkMqHg7kwkBiF3VT4dLKOfj0EefVYk+1aNi1LTpTzdJNeurD3oi9oEE0\\nf2imX6JPKNs9OGgj4jKMnm0S2hbz13Ht6nriv8RajSOt4K7jhA4zDUpeCJkoYSFh\\ni5qZBD1IZN+A+2rFG5fZM+FknhiEYV+q9nZheELkHV14PEVxHLp3+NV3Z3UWVxNZ\\n2O2RdncBnYI7/XMp5+dVwBSM3N5e5VZ4N8nynH1J0UN8M4quL+c21qwJCjq9vshZ\\nTQVDq2L7AgMBAAECggEAE75eMG0Bwnds+HWjLr6cO4e5ZSE/loJmQDvTf45oAk/+\\n3O2LLbsxxrvUoyoZJ0R2721OkH8wuXX8IntqitA+/UFY4D+kjfi+T96jQXQw6X11\\nKpg8f7d+mpsM/z/vYLQl49iqlx3rXLX/XZOCCyyxG/JSNki5tIEVMJBlJvy554zi\\nOiff+6yckYwr2Rr0rRX9T0gg5ae7ZYhDhanu26UQSaHcneI9JMo5h0n+MeDl1pEh\\ngHyXZ3b1g6JCy4x48cIlISroU8LIPK66b8XssDOV0P1ezJCEO70mvykH5gOLojtZ\\nUk/mVQGppGIaT19geqYX4GI3nDkx6mzidhH6fD95sQKBgQDhtnB+bnmAquKXqcCg\\ncPwpPDY9/8lf2xtppMRnAAEuCNoqw7a7Q8Eal+/pCnsr0Uwe6qJRGaBK0kMQWBL0\\nvktqENxNY5bWdTG1BA1c4Grv3qWe7+wBD1Kli/C2px67YeXVELIBVPzIA50AlpTy\\nJRLvu5AUUmn69eobqTa7ZWuk0QKBgQDWjRdDf9XnCddktvdmuj0zx4kwd6Xsz9ur\\nTV5u0+E11VPXuTNU08hsJP7gSSduvMo8d8TIgXjoZbIDzYIwknc74bLqZTmOU1ax\\nBYWJUF6k4y/8IB/cvvGOZm8C6cY731av9M9VMo32bmDFtTuybVvsKtM4lCheVZud\\n3NNrMAHuCwKBgQDIfQo9c5nfbWrqdnFQ0itZ4U75EUxpJbGazC1cpEdoAZrwY3nb\\nqxpKqe0DXjj8OePYjmfxi8ayz0Ocr+7ekG1iYGmfIDfqdJzbgibwebLjMCyDGe/T\\nHS43OC9vvtBwd0v0Tukef8QtUfBShm71C3NfmIYpVu8maOsy+MJSAIt2EQKBgQDP\\ncMlObRpe8Sf9693WyDkGiGV/pB+ckPheb0bftyKnzyPqjtkvqji9PyPjrTdgHU8Y\\nGeD80Bob8L+mZ5v55JM+JaG8ebSlhLRsId+T/U4U9MhfaJwF7eRV/3pUAb22A+Gn\\n7PZN9LxHyT7tyaX3hT78YAerS6ygUPtOLcWIfHlRMwKBgBnjp2+GXLoSKMEhMkjj\\nX9J/7r3ApLUfBFJSUbY9luIM9/jsX7QmzOQN4t9Cxcf1+6DnJ2R2BsqB2xUBbgcI\\nxz5uGQZsOMy9L8GzoMNMKfQ6qeKr1SvqtfveW+zQB7H59ZoDGyrJQAOP7sN8RzHu\\nP6OHw8gOoJCkkuSDeXQGF4iv\\n-----END PRIVATE KEY-----\\n\",\r\n  \"client_email\": \"tmfcmmessagingserviceaccount@vanda-25081.iam.gserviceaccount.com\",\r\n  \"client_id\": \"107780887183493434572\",\r\n  \"auth_uri\": \"https://accounts.google.com/o/oauth2/auth\",\r\n  \"token_uri\": \"https://oauth2.googleapis.com/token\",\r\n  \"auth_provider_x509_cert_url\": \"https://www.googleapis.com/oauth2/v1/certs\",\r\n  \"client_x509_cert_url\": \"https://www.googleapis.com/robot/v1/metadata/x509/tmfcmmessagingserviceaccount%40vanda-25081.iam.gserviceaccount.com\"\r\n}\r\n";
    string applicationIdentifier = "projects/vanda-25081";
    private readonly FcmNotificationSettings _fcmNotificationSettings;
    public FcmNotificationService()
    {
        _fcmNotificationSettings = new FcmNotificationSettings();
    }

    public async Task<string> SendFcmNotification(string token, string title, string body)
    {
        FcmNotificationResponse response = new FcmNotificationResponse();
        try
        {
            string jsonServiceKey = "{\r\n  \"type\": \"service_account\",\r\n  \"project_id\": \"vanda-25081\",\r\n  \"private_key_id\": \"9debff7e96743b24f7364025ee92c6f4d2cdb95f\",\r\n  \"private_key\": \"-----BEGIN PRIVATE KEY-----\\nMIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQC9KugIjY1RzSq6\\n3aLC3o8cGVwLM7S2/NdMSIWDR9KqpnPRbq3KqC+6WRZ2Lm6+pWsAyezklrkFYnvc\\nLdKixSpKmkMqHg7kwkBiF3VT4dLKOfj0EefVYk+1aNi1LTpTzdJNeurD3oi9oEE0\\nf2imX6JPKNs9OGgj4jKMnm0S2hbz13Ht6nriv8RajSOt4K7jhA4zDUpeCJkoYSFh\\ni5qZBD1IZN+A+2rFG5fZM+FknhiEYV+q9nZheELkHV14PEVxHLp3+NV3Z3UWVxNZ\\n2O2RdncBnYI7/XMp5+dVwBSM3N5e5VZ4N8nynH1J0UN8M4quL+c21qwJCjq9vshZ\\nTQVDq2L7AgMBAAECggEAE75eMG0Bwnds+HWjLr6cO4e5ZSE/loJmQDvTf45oAk/+\\n3O2LLbsxxrvUoyoZJ0R2721OkH8wuXX8IntqitA+/UFY4D+kjfi+T96jQXQw6X11\\nKpg8f7d+mpsM/z/vYLQl49iqlx3rXLX/XZOCCyyxG/JSNki5tIEVMJBlJvy554zi\\nOiff+6yckYwr2Rr0rRX9T0gg5ae7ZYhDhanu26UQSaHcneI9JMo5h0n+MeDl1pEh\\ngHyXZ3b1g6JCy4x48cIlISroU8LIPK66b8XssDOV0P1ezJCEO70mvykH5gOLojtZ\\nUk/mVQGppGIaT19geqYX4GI3nDkx6mzidhH6fD95sQKBgQDhtnB+bnmAquKXqcCg\\ncPwpPDY9/8lf2xtppMRnAAEuCNoqw7a7Q8Eal+/pCnsr0Uwe6qJRGaBK0kMQWBL0\\nvktqENxNY5bWdTG1BA1c4Grv3qWe7+wBD1Kli/C2px67YeXVELIBVPzIA50AlpTy\\nJRLvu5AUUmn69eobqTa7ZWuk0QKBgQDWjRdDf9XnCddktvdmuj0zx4kwd6Xsz9ur\\nTV5u0+E11VPXuTNU08hsJP7gSSduvMo8d8TIgXjoZbIDzYIwknc74bLqZTmOU1ax\\nBYWJUF6k4y/8IB/cvvGOZm8C6cY731av9M9VMo32bmDFtTuybVvsKtM4lCheVZud\\n3NNrMAHuCwKBgQDIfQo9c5nfbWrqdnFQ0itZ4U75EUxpJbGazC1cpEdoAZrwY3nb\\nqxpKqe0DXjj8OePYjmfxi8ayz0Ocr+7ekG1iYGmfIDfqdJzbgibwebLjMCyDGe/T\\nHS43OC9vvtBwd0v0Tukef8QtUfBShm71C3NfmIYpVu8maOsy+MJSAIt2EQKBgQDP\\ncMlObRpe8Sf9693WyDkGiGV/pB+ckPheb0bftyKnzyPqjtkvqji9PyPjrTdgHU8Y\\nGeD80Bob8L+mZ5v55JM+JaG8ebSlhLRsId+T/U4U9MhfaJwF7eRV/3pUAb22A+Gn\\n7PZN9LxHyT7tyaX3hT78YAerS6ygUPtOLcWIfHlRMwKBgBnjp2+GXLoSKMEhMkjj\\nX9J/7r3ApLUfBFJSUbY9luIM9/jsX7QmzOQN4t9Cxcf1+6DnJ2R2BsqB2xUBbgcI\\nxz5uGQZsOMy9L8GzoMNMKfQ6qeKr1SvqtfveW+zQB7H59ZoDGyrJQAOP7sN8RzHu\\nP6OHw8gOoJCkkuSDeXQGF4iv\\n-----END PRIVATE KEY-----\\n\",\r\n  \"client_email\": \"tmfcmmessagingserviceaccount@vanda-25081.iam.gserviceaccount.com\",\r\n  \"client_id\": \"107780887183493434572\",\r\n  \"auth_uri\": \"https://accounts.google.com/o/oauth2/auth\",\r\n  \"token_uri\": \"https://oauth2.googleapis.com/token\",\r\n  \"auth_provider_x509_cert_url\": \"https://www.googleapis.com/oauth2/v1/certs\",\r\n  \"client_x509_cert_url\": \"https://www.googleapis.com/robot/v1/metadata/x509/tmfcmmessagingserviceaccount%40vanda-25081.iam.gserviceaccount.com\"\r\n}\r\n";
            string applicationIdentifier = "projects/vanda-25081";

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonServiceKey));
            ms.Position = 0;
            ServiceAccountCredential credential = ServiceAccountCredential.FromServiceAccountData(ms); 

            FirebaseApp.Create();

            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromServiceAccountCredential(credential),
                ProjectId = this.applicationIdentifier,
            });

            

            FirebaseAdmin.Messaging.Message message = new FirebaseAdmin.Messaging.Message();
            message.Token = token;
            message.Notification = new FirebaseAdmin.Messaging.Notification();
            message.Notification.Title = title;
            message.Notification.Body = body;

            message.Android = new FirebaseAdmin.Messaging.AndroidConfig();
            message.Android.Notification = new FirebaseAdmin.Messaging.AndroidNotification();
            message.Android.Notification.Sound = "default";
            //message.Android.TimeToLive = "43200s";
            message.Android.Priority = Priority.High;

            message.Apns = new FirebaseAdmin.Messaging.ApnsConfig();
            message.Apns.CustomData = new Dictionary<string, object>();
            Aps aps = new Aps();
            aps.Sound = "default";
            message.Apns.CustomData.Add("aps", aps);
            message.Apns.Headers = new Dictionary<string, string>();
            //string apnsExpiration = ApnSender.ToEpoch(DateTime.UtcNow.AddHours(12)).ToString();
            //msg.Apns.Headers.Add("apns-expiration", apnsExpiration);
            message.Data = new Dictionary<string, string>
            {
                { "AdditionalAttribute", "něco" },
                { "ValidToDate", "něco" }
            };

            string respon = await FirebaseMessaging.DefaultInstance.SendAsync(message);

            FirebaseCloudMessagingService fcmMessagingService = new FirebaseCloudMessagingService();

           

            

            credential.Scopes = new List<string> { "https://www.googleapis.com/auth/firebase.messaging" };
            string accessToken = credential.GetAccessTokenForRequestAsync().Result;

            //_______________________________________________________

            WebRequest tRequest;
            tRequest = WebRequest.Create("https://fcm.googleapis.com/v1/projects/myproject-b5ae1/messages:send");
            tRequest.Method = "post";
            tRequest.ContentType = " application/json";

            tRequest.Headers.Add(string.Format("Authorization: key={0}", _fcmNotificationSettings.ServerKey));
            tRequest.Headers.Add(string.Format("Sender: id={0}", _fcmNotificationSettings.SenderId));

            //var a = new
            //{
            //    notification = new
            //    {
            //        title,
            //        body,
            //        icon = "https://domain/path/to/logo.png",
            //        click_action,
            //        sound = "mySound"
            //    },
            //    to = deviceId
            //};

            byte[] byteArray = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(message));
            tRequest.ContentLength = byteArray.Length;

            Stream dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse tResponse = tRequest.GetResponse();
            dataStream = tResponse.GetResponseStream();

            StreamReader tReader = new StreamReader(dataStream);
            string sResponseFromServer = tReader.ReadToEnd();

            tReader.Close();
            dataStream.Close();
            tResponse.Close();

            return sResponseFromServer;






            //SendMessageRequest messageRequest = new SendMessageRequest();

            //messageRequest.Message = message;
            //ProjectsResource.MessagesResource.SendRequest sendRequest = fcmMessagingService.Projects.Messages.Send(messageRequest, applicationIdentifier);

            //sendRequest.AccessToken = accessToken;
            //messageRequest.Message = sendRequest.Execute();
            //sendRequest.Service.HttpClient.

            //response.IsSuccess = true;

            //return response;
        }
        catch (Exception ex)
        {

            response.IsSuccess = false;
            response.Message = ex.ToString();

            return string.Empty;
        }
    }
}
