using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ZdaszToApp.Services;

public class ApiService
{
    private static ApiService? _instance;
    public static ApiService Instance => _instance ??= new ApiService();

    private static readonly string BaseUrl = "https://api-zdaszto.000000404.xyz/";//"";
    private readonly HttpClient _httpClient;
    private string? _token;

    private ApiService() 
    { 
        _httpClient = new HttpClient();
        _httpClient.Timeout = TimeSpan.FromSeconds(10);
    }

    public void SetToken(string token)
    {
        _token = token;
    }

    public string? GetToken() => _token;

    public async Task<string?> LoginAsync(string username, string password)
    {
        try
        {
            var url = $"{BaseUrl}login";
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("username", username),
                new KeyValuePair<string,string>("password", password)
            });

            var response = await _httpClient.PostAsync(url, content);
            var body = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Login HTTP {(int)response.StatusCode}: {body}");

            if (!response.IsSuccessStatusCode)
                return body;

            if (body.StartsWith("TOKEN: "))
                _token = body.Substring(7);
            else
                _token = body;
            
            Console.WriteLine($"Token set to: {_token}");

            return _token;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"[ApiService] Network error: {ex.Message}");
            return "Error:NoInternet";
        }
        catch (TaskCanceledException ex)
        {
            Console.WriteLine($"[ApiService] Timeout: {ex.Message}");
            return "Error:NoInternet";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ApiService] Unexpected error: {ex.Message}");
            return "Error:NoInternet";
        }
    }

    public async Task<string?> SignupAsync(string email, string username, string password)
    {
        try
        {
            var url = $"{BaseUrl}signup";
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("email", email),
                new KeyValuePair<string,string>("username", username),
                new KeyValuePair<string,string>("password", password)
            });
            
            var response = await _httpClient.PostAsync(url, content);
            var body = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Signup HTTP {(int)response.StatusCode}: {body}");
            return body;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"[ApiService] Signup Network error: {ex.Message}");
            return "Error:NoInternet";
        }
        catch (TaskCanceledException ex)
        {
            Console.WriteLine($"[ApiService] Signup Timeout: {ex.Message}");
            return "Error:NoInternet";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ApiService] Signup Unexpected error: {ex.Message}");
            return "Error:NoInternet";
        }
    }

    public async Task<Question?> GetQuestionAsync(int questionId)
    {
        if (string.IsNullOrEmpty(_token)) return null;
        
        var url = $"{BaseUrl}question/{questionId}.json";
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string,string>("token", _token)
        });

        var response = await _httpClient.PostAsync(url, content);
        var body = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"GetQuestion HTTP {(int)response.StatusCode}: {body}");

        if (!response.IsSuccessStatusCode || body == "Invalid question ID") 
        {
            return null;
        }
        
        try
        {
            var json = JsonSerializer.Deserialize<JsonElement>(body);
            
            if (json.ValueKind != JsonValueKind.Array || json.GetArrayLength() < 8)
            {
                return null;
            }

            var answersJson = json[4].GetString() ?? "{}";
            answersJson = answersJson.Replace("\r\n", "\\n").Replace("\n", "\\n").Replace("\r", "\\n");
            
            return new Question
            {
                Id = json[0].GetInt32(),
                Text = json[1].GetString() ?? "",
                Type = json[2].GetString() ?? "",
                Difficulty = json[3].GetInt32(),
                Answers = JsonSerializer.Deserialize<Dictionary<string, string>>(answersJson) ?? new(),
                CorrectAnswer = json[5].GetString() ?? "",
                ImagePath = json[6].ValueKind == JsonValueKind.Null ? null : json[6].GetString(),
                CreatedAt = json[7].ValueKind == JsonValueKind.Null ? null : json[7].GetString(),
                DeletedAt = json[8].ValueKind == JsonValueKind.Null ? null : json[8].GetString()
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Parse error: {ex.Message}");
            return null;
        }
    }

    public async Task<Question?> GetRandomQuestionAsync(int collectionId)
    {
        if (string.IsNullOrEmpty(_token)) return null;
        
        Console.WriteLine($"Fetching random question for collection {collectionId}");
        
        var url = $"{BaseUrl}question/random/{collectionId}.json";
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string,string>("token", _token)
        });

        var response = await _httpClient.PostAsync(url, content);
        var body = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"GetRandomQuestion HTTP {(int)response.StatusCode}: {body}");

        if (!response.IsSuccessStatusCode || body == "Invalid collection ID") 
        {
            return null;
        }
        
        try
        {
            var json = JsonSerializer.Deserialize<JsonElement>(body);
            
            if (json.ValueKind != JsonValueKind.Array || json.GetArrayLength() < 8)
            {
                return null;
            }

            var answersJson = json[4].GetString() ?? "{}";
            answersJson = answersJson.Replace("\r\n", "\\n").Replace("\n", "\\n").Replace("\r", "\\n");
            
            return new Question
            {
                Id = json[0].GetInt32(),
                Text = json[1].GetString() ?? "",
                Type = json[2].GetString() ?? "",
                Difficulty = json[3].GetInt32(),
                Answers = JsonSerializer.Deserialize<Dictionary<string, string>>(answersJson) ?? new(),
                CorrectAnswer = json[5].GetString() ?? "",
                ImagePath = json[6].ValueKind == JsonValueKind.Null ? null : json[6].GetString(),
                CreatedAt = json[7].ValueKind == JsonValueKind.Null ? null : json[7].GetString(),
                DeletedAt = json[8].ValueKind == JsonValueKind.Null ? null : json[8].GetString()
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Parse error: {ex.Message}");
            return null;
        }
    }

    public async Task<List<QuestionCollection>> GetCollectionsAsync()
    {
        if (string.IsNullOrEmpty(_token)) return new();
        
        var url = $"{BaseUrl}collections.json";
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string,string>("token", _token)
        });

        var response = await _httpClient.PostAsync(url, content);
        var body = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"GetCollections HTTP {(int)response.StatusCode}: {body}");

        if (!response.IsSuccessStatusCode) return new();
        
        try
        {
            var json = JsonSerializer.Deserialize<JsonElement>(body);
            var collections = new List<QuestionCollection>();
            
            foreach (var item in json.EnumerateArray())
            {
                collections.Add(new QuestionCollection
                {
                    Id = item[0].GetInt32(),
                    Name = item[1].GetString() ?? "",
                    Description = item[2].GetString() ?? "",
                    CreatedAt = item[3].GetString(),
                    DeletedAt = item[4].ValueKind == JsonValueKind.Null ? null : item[4].GetString()
                });
            }
            return collections;
        }
        catch
        {
            return new();
        }
    }

    public async Task<byte[]?> GetQuestionImageAsync(int questionId)
    {
        if (string.IsNullOrEmpty(_token)) return null;
        
        try
        {
            var url = $"{BaseUrl}question/image/{questionId}.json";
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("token", _token)
            });

            var response = await _httpClient.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            return null;
        }
        catch
        {
            return null;
        }
    }
}

public class Question
{
    public int Id { get; set; }
    public string Text { get; set; } = "";
    public string Type { get; set; } = "";
    public int Difficulty { get; set; }
    public Dictionary<string, string> Answers { get; set; } = new();
    public string CorrectAnswer { get; set; } = "";
    public string? ImagePath { get; set; }
    public string? CreatedAt { get; set; }
    public string? DeletedAt { get; set; }
}

public class QuestionCollection
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string? CreatedAt { get; set; }
    public string? DeletedAt { get; set; }
}
