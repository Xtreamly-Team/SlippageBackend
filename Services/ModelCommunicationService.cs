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
         
         // Set authorization header
         client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "1c625a5a-91a1-49c1-ab33-9cf131d06dd5");
 
         // Serialize input object to JSON
         var jsonPayload = System.Text.Json.JsonSerializer.Serialize(input);
 
         Console.WriteLine(jsonPayload);
 
         // Send POST request with authorization header
         var result = await client.PostAsJsonAsync(Consts.Consts.MODEL_URL + symbol, input);
 
         if (!result.IsSuccessStatusCode)
         {
             throw new Exception("Model server error");
         }
 
         // Read response content
         var response = await result.Content.ReadAsStringAsync();
 
         // Deserialize response JSON to ModelOutput object
         return  System.Text.Json.JsonSerializer.Deserialize<ModelOutput>(response);
     }
     catch (Exception ex)
     {
         // Handle exceptions
         Console.WriteLine($"An error occurred: {ex.Message}");
         return null;
     }
 }

}