using Newtonsoft.Json;
using System.ComponentModel;

namespace GithubUsersTask.Common.Models
{
    public class GithubUserRepo
    {
        [DisplayName("Repository")]
        [JsonProperty("name")]
        public string RepositoryName { get; set; }

        [JsonProperty("stargazers_count")]
        public int Stars { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }
    }
}