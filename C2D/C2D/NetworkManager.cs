using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using C2D.Shared;
using C2D.Shared.Models;
using C2D.Shared.Models.POST;
using C2D.Shared.Models.Responses;
using Newtonsoft.Json;
using RestSharp;

namespace C2D
{
    class NetworkManager
    {

        #region Basics

        public static RestRequest CreateRequest(Method method)
        {
            var request = new RestRequest(method);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            return request;
        }

        public static async Task<IRestResponse> ExecuteGet(string URL)
        {
            var client = new RestClient(AppSettings.BaseURL + URL);
            var request = CreateRequest(Method.GET);
            return await client.ExecuteTaskAsync(request, new CancellationTokenSource().Token);
        }

        public static async Task<IRestResponse> ExecutePost(string URL, object Body)
        {
            var client = new RestClient(AppSettings.BaseURL + URL);
            var request = CreateRequest(Method.POST);
            Console.WriteLine(JsonConvert.SerializeObject(Body));
            request.AddParameter("undefined", JsonConvert.SerializeObject(Body), ParameterType.RequestBody);
            return await client.ExecuteTaskAsync(request, new CancellationTokenSource().Token);
        }

        public static async Task<IRestResponse> ExecuteAuthorizedGet(string URL)
        {
            var client = new RestClient(AppSettings.BaseURL + URL);
            var request = CreateRequest(Method.GET);

            request.AddHeader("Authorization", App.UserSettings.AccessToken);
            return await client.ExecuteTaskAsync(request, new CancellationTokenSource().Token);
        }

        public static async Task<IRestResponse> ExecuteAuthorizedPost(string URL, object Body)
        {
            var client = new RestClient(AppSettings.BaseURL + URL);
            var request = CreateRequest(Method.POST);

            request.AddHeader("Authorization", App.UserSettings.AccessToken);
            request.AddParameter("undefined", JsonConvert.SerializeObject(Body), ParameterType.RequestBody);

            Console.WriteLine(JsonConvert.SerializeObject(Body));
            Console.WriteLine(AppSettings.BaseURL + URL);
            return await client.ExecuteTaskAsync(request, new CancellationTokenSource().Token);
        }

        #endregion


        public static async Task<AuthResponse> CheckLogin(User user)
        {
            var postUser = new POSTUser
            {
                username = user.Username,
                password = user.Password,
                client_id = user.ClientID,
                client_secret = user.ClientSecret,
                extension = user.Extension
            };

            var response = await ExecutePost("authorize", postUser);

            //todo: fix more options here
            if (response.StatusCode != HttpStatusCode.OK || response.ContentLength == -1)
                return null;

            return JsonConvert.DeserializeObject<AuthResponse>(response.Content);
        }

        #region Contacts

        public static async Task<List<Contact>> GetContacts()
        {
            List<Contact> contacts = new List<Contact>();
            var toPost = new POSTGetContacts(App.UserSettings.ClientID, App.UserSettings.AccessToken);

            var response = await ExecuteAuthorizedPost("phonebook", toPost);
            
            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            try
            {
                var ContactsResponse = JsonConvert.DeserializeObject<GetContactsResponse>(response.Content);
                ContactsResponse.Convert();
                contacts = ContactsResponse.parsedRecords;
            }
            catch (JsonException exception)
            {
                return null;
            }
            return contacts;
        }

        public static async Task<Contact> GetContact(string id)
        {
            var data = await GetContacts();
            var contact = data.Find(x => x.Id == id);
            return contact;
        }

        public static async Task<Contact> GetContactByNumber(string number)
        {
            var data = await GetContacts();
            var contact = data.Find(x => x.PhoneNumberOne == number
            || x.PhoneNumberTwo == number
            || x.PhoneNumberThree == number
            || x.MobilePhoneOne == number);
            return contact;
        }

        public static async Task<POSTResponse> PostContact(POSTContact c)
        {
            var response = await ExecuteAuthorizedPost("phonebook", c);

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            var ContactsResponse = JsonConvert.DeserializeObject<POSTResponse>(response.Content);

            return ContactsResponse;
        }

        public static async Task<POSTResponse> CreateNewContact(Contact c)
        {
            var toPost = new POSTContact(c, App.UserSettings.ClientID, App.UserSettings.AccessToken, "new");
            return await PostContact(toPost);
        }

        public static async Task<POSTResponse> SaveContact(Contact c)
        {
            var toPost = new POSTContact(c, App.UserSettings.ClientID, App.UserSettings.AccessToken, "edit");
            return await PostContact(toPost);
        }

        public static async Task<POSTResponse> DeleteContact(Contact c)
        {
            var toPost = new POSTContact(c, App.UserSettings.ClientID, App.UserSettings.AccessToken, "delete");
            return await PostContact(toPost);
        }
        #endregion

        #region Calls

        public static async Task<POSTResponse> PostCall(POSTCall c)
        {
            var response = await ExecuteAuthorizedPost("calls", c);

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            Console.WriteLine(response.Content);
            var ContactsResponse = JsonConvert.DeserializeObject<POSTResponse>(response.Content);

            return ContactsResponse;
        }

        public static async Task<POSTResponse> StartCall(Contact contact)
        {
            POSTCall call = new POSTCall(App.UserSettings.ClientID, App.UserSettings.AccessToken, "dial", App.UserSettings.Extension);
            call.to.phonebook_id = contact.Id;
            call.to.number = contact.PhoneNumberOne;
            return await PostCall(call);
        }

        public static async Task<POSTResponse> CancelCall(string CallId)
        {
            PostCallExtra call = new PostCallExtra(App.UserSettings.ClientID, App.UserSettings.AccessToken, "terminate", App.UserSettings.Extension, CallId);
            return await PostCall(call);
        }

        public static async Task<POSTResponse> PauseCall(string CallId)
        {
            PostCallExtra call = new PostCallExtra(App.UserSettings.ClientID, App.UserSettings.AccessToken, "hold", App.UserSettings.Extension, CallId);
            return await PostCall(call);
        }

        public static async Task<POSTResponse> ResumeCall(string CallId)
        {
            PostCallExtra call = new PostCallExtra(App.UserSettings.ClientID, App.UserSettings.AccessToken, "resume", App.UserSettings.Extension, CallId);
            return await PostCall(call);
        }

        public static async Task<List<GetCallsResponse>> GetActiveCalls()
        {
            var calls = new List<GetCallsResponse>();
            POSTGetActiveCalls post = new POSTGetActiveCalls(App.UserSettings.ClientID, App.UserSettings.AccessToken, App.UserSettings.Extension);

            var response = await ExecuteAuthorizedPost("calls", post);

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            if (response.Content.Contains("Given call_id not found!"))
                return null;

            calls = JsonConvert.DeserializeObject<List<GetCallsResponse>>(response.Content);

            return calls;
        }

        public static async Task ForceTerminateCall(Call call)
        {
            PostTerminateCall c = new PostTerminateCall();
            c.call_id = call.CallId;

            var response = await ExecuteAuthorizedPost("subscriptions_callback", c);
        }

        #endregion
    }


}
