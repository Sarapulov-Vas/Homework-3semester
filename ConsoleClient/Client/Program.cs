using System.Text.Json;
class Program
{
    public class Response
    {
        public User[]? Users { get; set; }
        
    }
    public class User
    {
        public int id { get; set; }
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public bool can_access_closed { get; set; }
        public bool is_closed { get; set; }
    }
    private static async Task Main()
    {
        var httpClient = new System.Net.Http.HttpClient();
        var request = "https://api.vk.com/method/users.get?user_id=435891485&access_token=...d&v=5.131";
        var resp = await httpClient.GetAsync(request);
        if (resp.IsSuccessStatusCode)
        {
            var content = await resp.Content.ReadAsStringAsync();
            Console.WriteLine(content);
            var response = JsonSerializer.Deserialize<Response>(content);
            Console.WriteLine($"Id: {response.Users[0].id}");
            Console.WriteLine($"Name: {response.Users[0].first_name}");
            Console.WriteLine($"Username: {response.Users[0].last_name}");
        }
    }
}