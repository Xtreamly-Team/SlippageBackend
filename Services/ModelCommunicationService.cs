using MongoDB.Driver;
using Newtonsoft.Json;
using SlippageBackend.Models;

namespace SlippageBackend.Services;

public class ModelCommunicationService ( IMongoClient  _client , IHttpClientFactory _httpClientFactory)
{
    public async Task<ModelOutput?> ExecuteInference(ModelInput input)
    {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Bearer" , Consts.Consts.MODEL_KEY);
            var result = await client.PostAsJsonAsync(Consts.Consts.MODEL_URL, _httpClientFactory);
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception("Model server error");
            }
            var response = await result.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<ModelOutput>(response);
        
    } 
}