using System;
using System.IO;
using System.Text.Json;
using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;

namespace ZdaszToApp.Services;

public class AuthService
{
    private static AuthService? _instance;
    public static AuthService Instance => _instance ??= new AuthService();

    private string? _savedUsername;
    private string? _savedPassword;
    private bool _isAuthenticated;

    public string? SavedUsername => _savedUsername;
    public string? SavedPassword => _savedPassword;
    public bool IsAuthenticated => _isAuthenticated;

    public event Action? LoggedIn;
    public event Action? LoggedOut;

    private AuthService()
    {
    }

    public void SaveCredentials(string username, string password)
    {
        Console.WriteLine($"[AuthService] SaveCredentials: username={username}, password={password}");
        _savedUsername = username;
        _savedPassword = password;
        _isAuthenticated = true;
        SaveToFile();
        LoggedIn?.Invoke();
    }

    public void ClearCredentials()
    {
        _savedUsername = null;
        _savedPassword = null;
        _isAuthenticated = false;
        DeleteFile();
        LoggedOut?.Invoke();
    }

    public bool HasSavedCredentials()
    {
        LoadFromFile();
        Console.WriteLine($"[AuthService] HasSavedCredentials: _isAuthenticated={_isAuthenticated}, _savedUsername={_savedUsername}, _savedPassword={(_savedPassword != null ? "***" : "null")}");
        return _isAuthenticated && !string.IsNullOrEmpty(_savedUsername) && !string.IsNullOrEmpty(_savedPassword);
    }

    private string GetFilePath()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appFolder = Path.Combine(appData, "ZdaszToApp");
        Directory.CreateDirectory(appFolder);
        return Path.Combine(appFolder, "auth.json");
    }

    private void SaveToFile()
    {
        try
        {
            var data = new AuthData
            {
                Username = _savedUsername,
                Password = _savedPassword,
                IsAuthenticated = _isAuthenticated
            };
            var json = JsonSerializer.Serialize(data);
            var path = GetFilePath();
            Console.WriteLine($"[AuthService] Saving to: {path}");
            Console.WriteLine($"[AuthService] Data: {json}");
            File.WriteAllText(path, json);
            Console.WriteLine("[AuthService] File saved successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AuthService] SaveToFile error: {ex.Message}");
        }
    }

    private void LoadFromFile()
    {
        try
        {
            var path = GetFilePath();
            Console.WriteLine($"[AuthService] Loading from: {path}");
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                Console.WriteLine($"[AuthService] Loaded json: {json}");
                var data = JsonSerializer.Deserialize<AuthData>(json);
                if (data != null)
                {
                    _savedUsername = data.Username;
                    _savedPassword = data.Password;
                    _isAuthenticated = data.IsAuthenticated;
                    Console.WriteLine($"[AuthService] Loaded - Username: {_savedUsername}, IsAuthenticated: {_isAuthenticated}");
                }
            }
            else
            {
                Console.WriteLine("[AuthService] File does not exist");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AuthService] LoadFromFile error: {ex.Message}");
        }
    }

    private void DeleteFile()
    {
        try
        {
            var path = GetFilePath();
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        catch
        {
            // Ignore errors
        }
    }

    private class AuthData
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
