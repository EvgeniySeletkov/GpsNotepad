using Newtonsoft.Json;

namespace GpsNotepad.Models
{
    class FacebookProfileModel
    {
        public string Id { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
    }

}
