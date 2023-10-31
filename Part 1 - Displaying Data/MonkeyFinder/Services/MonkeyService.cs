using System.Net.Http.Json;

namespace MonkeyFinder.Services;

public class MonkeyService
{
    List<Monkey> monkeyList = new();
    HttpClient httpClient;

    public MonkeyService() 
    {
        this.httpClient = new HttpClient();
    }

    public async Task<List<Monkey>> GetMonkeys()
    {
        if (monkeyList?.Count > 0)
            return monkeyList;

        try
        {
            var response = await httpClient.GetAsync("https://www.montemagno.com/monkeys.json");

            if (response.IsSuccessStatusCode)
            {
                monkeyList = await response.Content.ReadFromJsonAsync(MonkeyContext.Default.ListMonkey);
            }
            else
            {
                // If the network request fails, read from the local JSON file
                using var stream = await FileSystem.OpenAppPackageFileAsync("monkeydata.json");
                using var reader = new StreamReader(stream);
                var contents = await reader.ReadToEndAsync();
                monkeyList = JsonSerializer.Deserialize<List<Monkey>>(contents);
            }
        }
        catch (HttpRequestException)
        {
            // Handle the exception if the network request fails
            // Read from the local JSON file as a fallback
            using var stream = await FileSystem.OpenAppPackageFileAsync("monkeydata.json");
            using var reader = new StreamReader(stream);
            var contents = await reader.ReadToEndAsync();
            monkeyList = JsonSerializer.Deserialize<List<Monkey>>(contents);
        }

        return monkeyList;
    }

}
