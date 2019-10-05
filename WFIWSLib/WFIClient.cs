using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;
using WFIWLib.Models;

namespace WFIWLib
{
    public class WFIClient
    {
        private HttpWebRequest _request;
        private HttpWebResponse _response;
        private StreamReader _streamReader;

        public string DomainName { get; set; }

        public WFIClient(string domainName)
        {
            DomainName = domainName;
        }

        public ApiResult CreateUser(string username, string password, string mail)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new UserCreationObject(username, password, mail)));
            _request = WebRequest.Create(string.Format("http://{0}/users/register", DomainName)) as HttpWebRequest;
            _request.Method = "POST";
            _request.ContentType = "application/json";
            _request.ContentLength = bytes.Length;
            WritePostParameters(bytes);
            return GetJsonObjectResult();
        }

        public ApiResult LoginUser(string username, string password)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new UserLoginObject(username, password)));
            _request = WebRequest.Create(string.Format("http://{0}/users/auth", DomainName)) as HttpWebRequest;
            _request.Method = "POST";
            _request.ContentType = "application/json";
            _request.ContentLength = bytes.Length;
            WritePostParameters(bytes);
            return GetJsonObjectResult();
        }

        public ApiResult CreateReview(string token, int movieId, int status)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new ReviewCreationObject(movieId, status)));
            _request = WebRequest.Create(string.Format("http://{0}/reviews/create", DomainName)) as HttpWebRequest;
            _request.Headers.Add("Authorization", string.Format("Bearer {0}", token));
            _request.Method = "POST";
            _request.ContentType = "application/json";
            _request.ContentLength = bytes.Length;
            WritePostParameters(bytes);
            return GetJsonObjectResult();
        }

        public ApiResult SendReview(string token, int movieId, int status)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new ReviewCreationObject(movieId, status)));
            _request = WebRequest.Create(string.Format("http://{0}/reviews/save", DomainName)) as HttpWebRequest;
            _request.Headers.Add("Authorization", string.Format("Bearer {0}", token));
            _request.Method = "POST";
            _request.ContentType = "application/json";
            _request.ContentLength = bytes.Length;
            WritePostParameters(bytes);
            return GetJsonObjectResult();
        }

        public ApiResult UpdateReview(string token, int movieId, int status)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new ReviewCreationObject(movieId, status)));
            _request = WebRequest.Create(string.Format("http://{0}/reviews/{1}/update", DomainName, movieId)) as HttpWebRequest;
            _request.Headers.Add("Authorization", string.Format("Bearer {0}", token));
            _request.Method = "POST";
            _request.ContentType = "application/json";
            _request.ContentLength = bytes.Length;
            WritePostParameters(bytes);
            return GetJsonObjectResult();
        }

        public ApiResult GetReviewsByMovie(int movieId)
        {
            _request = WebRequest.Create(string.Format("http://{0}/reviews/movies/{1}", DomainName, movieId)) as HttpWebRequest;
            _request.Method = "GET";
            _request.ContentType = "application/json";
            return GetJsonObjectResult();
        }

        public ApiResult GetReviewsByUser(string token)
        {
            _request = WebRequest.Create(string.Format("http://{0}/reviews", DomainName)) as HttpWebRequest;
            _request.Headers.Add("Authorization", string.Format("Bearer {0}", token));
            _request.Method = "GET";
            _request.ContentType = "application/json";
            return GetJsonObjectResult();
        }

        public ApiResult GetUserReview(string token, int movieId)
        {
            _request = WebRequest.Create(string.Format("http://{0}/reviews?movie_id=", DomainName, movieId)) as HttpWebRequest;
            _request.Headers.Add("Authorization", string.Format("Bearer {0}", token));
            _request.Method = "GET";
            _request.ContentType = "application/json";
            return GetJsonObjectResult();
        }

        public bool UserHasReview(string token, int movieId)
        {
            return GetUserReview(token, movieId).Reviews.Count > 0;
        }

        private ApiResult GetJsonObjectResult()
        {
            _response = _request.GetResponse() as HttpWebResponse;
            _streamReader = new StreamReader(_response.GetResponseStream());
            return JsonConvert.DeserializeObject<ApiResult>(_streamReader.ReadToEnd());
        }

        private void WritePostParameters(byte[] buffer)
        {
            Stream requestStream = _request.GetRequestStream();
            requestStream.Write(buffer, 0, buffer.Length);
            requestStream.Close();
        }
    }
}
