using System.Text.Json;
namespace HappyTodo;
public static class SuggestionCatalog {
 public static async Task<List<string>> Load(){
  var path=Path.Combine(AppContext.BaseDirectory,"wwwroot","data","suggestions.json");
  return File.Exists(path)?JsonSerializer.Deserialize<List<string>>(await File.ReadAllTextAsync(path))??new():new();
 }
}