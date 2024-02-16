using System.Text;
using MongoDB.Driver;
using SlippageBackend.Models;

namespace SlippageBackend.Services;

public class ModelCommunicationService ( IMongoClient  _client , IHttpClientFactory _httpClientFactory)
{
 public async Task<ModelOutput?> ExecuteInference(ModelInput input, string symbol)
 {
     try
     {
         var client = _httpClientFactory.CreateClient();
         
         client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "1c625a5a-91a1-49c1-ab33-9cf131d06dd5");
         var result = await client.PostAsJsonAsync(Consts.Consts.MODEL_URL + symbol, input);
 
         if (!result.IsSuccessStatusCode)
         {
             throw new Exception("Model server error");
         }
         
         var response = await result.Content.ReadAsStringAsync();
         
         return  System.Text.Json.JsonSerializer.Deserialize<ModelOutput>(response);
     }
     catch (Exception ex)
     {
         Console.WriteLine($"An error occurred: {ex.Message}");
         return null;
     }
 }

}