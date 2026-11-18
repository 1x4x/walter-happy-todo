using System.Text.Json;
namespace HappyTodo;
public static class SuggestionCatalog {
 public static async Task<List<string>> Load(string contentRoot){
  var path=Path.Combine(contentRoot,"wwwroot","data","suggestions.json");
  return File.Exists(path)?JsonSerializer.Deserialize<List<string>>(await File.ReadAllTextAsync(path))??new():new();
 }
}