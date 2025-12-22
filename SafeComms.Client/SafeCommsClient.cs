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
    public SafeCommsClient(string apiKey, string baseUrl = "https://api.safecomms.dev")
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
    /// Moderates the provided image content.
    /// </summary>
    /// <param name="image">The image URL or base64 string to moderate.</param>
    /// <param name="language">The language of the content (default: "en").</param>
    /// <param name="moderationProfileId">The ID of the moderation profile to use.</param>
    /// <param name="enableOcr">Whether to extract text (OCR) from the image.</param>
    /// <param name="enhancedOcr">Whether to use enhanced OCR for higher accuracy.</param>
    /// <param name="extractMetadata">Whether to extract metadata (EXIF) from the image.</param>
    /// <returns>The moderation result.</returns>
    public async Task<JsonElement> ModerateImageAsync(string image, string language = "en", string? moderationProfileId = null, bool enableOcr = false, bool enhancedOcr = false, bool extractMetadata = false)
    {
        var request = new
        {
            image,
            language,
            moderationProfileId,
            enableOcr,
            enhancedOcr,
            extractMetadata
        };

        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/moderation/image", request);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<JsonElement>();
    }

    /// <summary>
    /// Moderates the provided image file.
    /// </summary>
    /// <param name="fileStream">The stream of the image file.</param>
    /// <param name="fileName">The name of the image file.</param>
    /// <param name="language">The language of the content (default: "en").</param>
    /// <param name="moderationProfileId">The ID of the moderation profile to use.</param>
    /// <param name="enableOcr">Whether to extract text (OCR) from the image.</param>
    /// <param name="enhancedOcr">Whether to use enhanced OCR for higher accuracy.</param>
    /// <param name="extractMetadata">Whether to extract metadata (EXIF) from the image.</param>
    /// <returns>The moderation result.</returns>
    public async Task<JsonElement> ModerateImageFileAsync(Stream fileStream, string fileName, string language = "en", string? moderationProfileId = null, bool enableOcr = false, bool enhancedOcr = false, bool extractMetadata = false)
    {
        using var content = new MultipartFormDataContent();
        content.Add(new StreamContent(fileStream), "image", fileName);
        content.Add(new StringContent(language), "language");
        content.Add(new StringContent(enableOcr.ToString().ToLower()), "enableOcr");
        content.Add(new StringContent(enhancedOcr.ToString().ToLower()), "enhancedOcr");
        content.Add(new StringContent(extractMetadata.ToString().ToLower()), "extractMetadata");
        
        if (!string.IsNullOrEmpty(moderationProfileId))
        {
            content.Add(new StringContent(moderationProfileId), "moderationProfileId");
        }

        var response = await _httpClient.PostAsync($"{_baseUrl}/moderation/image/upload", content);
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
