using System.Net.Http.Json;
using System.Text.Json;

namespace SafeComms.Client;

/// <summary>
/// Client for the SafeComms API.
/// </summary>
public class SafeCommsClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    /// <summary>
    /// Initializes a new instance of the <see cref="SafeCommsClient"/> class.
    /// </summary>
    /// <param name="apiKey">The API key for authentication.</param>
    /// <param name="baseUrl">The base URL of the API.</param>
    public SafeCommsClient(string apiKey, string baseUrl = "https://safecomms.dev/api/v1/public")
    {
        _baseUrl = baseUrl.TrimEnd('/');
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
    }

    /// <summary>
    /// Moderates the provided text content.
    /// </summary>
    /// <param name="content">The text content to moderate.</param>
    /// <param name="language">The language of the content (default: "en").</param>
    /// <param name="replace">Whether to replace flagged content.</param>
    /// <param name="pii">Whether to check for PII.</param>
    /// <param name="replaceSeverity">The severity level for replacement.</param>
    /// <param name="moderationProfileId">The ID of the moderation profile to use.</param>
    /// <returns>The moderation result.</returns>
    public async Task<JsonElement> ModerateTextAsync(string content, string language = "en", bool replace = false, bool pii = false, string? replaceSeverity = null, string? moderationProfileId = null)
    {
        var request = new
        {
            content,
            language,
            replace,
            pii,
            replaceSeverity,
            moderationProfileId
        };

        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/moderation/text", request);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<JsonElement>();
    }

    /// <summary>
    /// Retrieves the current usage statistics.
    /// </summary>
    /// <returns>The usage statistics.</returns>
    public async Task<JsonElement> GetUsageAsync()
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/usage");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<JsonElement>();
    }
}
