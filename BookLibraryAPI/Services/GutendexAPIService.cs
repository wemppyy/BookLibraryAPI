using BookLibraryAPI.Abstracts;
using System.Text.Json;
using BookLibraryAPI.Models;

namespace BookLibraryAPI.Services
{
    public class GutendexAPIService : IGutendexAPIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _endpoint;

        public GutendexAPIService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _endpoint = configuration.GetValue<string>("GutendexAPI:Endpoint");
        }

        public async Task<GutendexPagedResult> GetAllBooks(int page = 1, string search = null)
        {
            var url = $"{_endpoint}?page={page}";
            if (!string.IsNullOrEmpty(search))
            {
                url += $"&search={Uri.EscapeDataString(search)}";
            }

            var json = await _httpClient.GetStringAsync(url);
            using var document = JsonDocument.Parse(json);

            var books = new List<GutendexAPIModel>();
            var root = document.RootElement;
            
            foreach (var property in root.EnumerateObject())
            {
                if (property.Name == "results" && property.Value.ValueKind == JsonValueKind.Array)
                {
                    foreach (var book in property.Value.EnumerateArray())
                    {
                        books.Add(ParseBook(book));
                    }
                }
            }

            return new GutendexPagedResult
            {
                Books = books,
                NextPage = ExtractPageNumber(root, "next"),
                PreviousPage = ExtractPageNumber(root, "previous")
            };
        }

        public async Task<GutendexAPIModel> GetById(int id)
        {
            var json = await _httpClient.GetStringAsync($"{_endpoint}/{id}");
            using var document = JsonDocument.Parse(json);
            return ParseBook(document.RootElement);
        }

        private int? ExtractPageNumber(JsonElement root, string propertyName)
        {
            JsonElement urlElement;
            if (!root.TryGetProperty(propertyName, out urlElement))
                return null;

            var url = urlElement.GetString();
            if (string.IsNullOrEmpty(url))
                return null;

            var query = new Uri(url).Query;
            var pageParam = query.Split('&', '?')
                .FirstOrDefault(p => p.StartsWith("page="));

            int page;
            if (pageParam != null && int.TryParse(pageParam.Replace("page=", ""), out page))
                return page;

            return null;
        }

        private GutendexAPIModel ParseBook(JsonElement element)
        {
            var authorName = "Unknown Author";
            int? birthYear = null;
            string imageUrl = null;
            
            foreach (var property in element.EnumerateObject())
            {
                if (property.Name == "authors" && property.Value.ValueKind == JsonValueKind.Array)
                {
                    var firstAuthor = property.Value.EnumerateArray().FirstOrDefault();
                    if (firstAuthor.ValueKind != JsonValueKind.Undefined)
                    {
                        foreach (var authorProp in firstAuthor.EnumerateObject())
                        {
                            if (authorProp.Name == "name" && authorProp.Value.ValueKind == JsonValueKind.String)
                            {
                                authorName = authorProp.Value.GetString() ?? authorName;
                            }
                            else if (authorProp.Name == "birth_year" && authorProp.Value.ValueKind == JsonValueKind.Number)
                            {
                                birthYear = authorProp.Value.GetInt32();
                            }
                        }
                    }
                }
                else if (property.Name == "formats" && property.Value.ValueKind == JsonValueKind.Object)
                {
                    foreach (var format in property.Value.EnumerateObject())
                    {
                        if (format.Name == "image/jpeg" && format.Value.ValueKind == JsonValueKind.String)
                        {
                            imageUrl = format.Value.GetString();
                            break;
                        }
                    }
                }
            }

            return new GutendexAPIModel
            {
                Id = element.GetProperty("id").GetInt32(),
                Title = element.GetProperty("title").GetString() ?? "Unknown Title",
                AuthorName = authorName,
                BirthYear = birthYear,
                ImageUrl = imageUrl
            };
        }
    }
}

