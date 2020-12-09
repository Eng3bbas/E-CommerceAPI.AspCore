using System.Text.Json.Serialization;

namespace E_Commerce.Http.Responses
{
    public abstract class BaseResponse
    {
        public enum Statuses
        {
            Failed,
            Succeeded
        }
        
        public string? ErrorMessage { get; set; }
        [JsonIgnore]
        public Statuses Status => !string.IsNullOrEmpty(ErrorMessage) ? Statuses.Failed : Statuses.Succeeded;
    }
}