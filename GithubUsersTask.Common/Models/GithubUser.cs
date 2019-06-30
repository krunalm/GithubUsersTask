using Newtonsoft.Json;
using System.Collections.Generic;

namespace GithubUsersTask.Common.Models
{
    public class GithubUser
    {
        [JsonProperty("login")]
        public string UserName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("avatar_url")]
        public string Avatar { get; set; }

        [JsonProperty("repos_url")]
        public string RepoUrl { get; set; }

        public IEnumerable<GithubUserRepo> Repositories { get; set; }
    }
}
