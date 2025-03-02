using Microsoft.Extensions.Configuration;

using OpenAI.Embeddings;

namespace IldVann.Embeddings;

public class VectorEmbeddingGenerator
{
    private readonly EmbeddingClient _client;

    public VectorEmbeddingGenerator(IConfiguration configuration)
    {
        var openaiApiKey = configuration["OPENAI_API_KEY"] ?? throw new NullReferenceException("OPENAI_API_KEY");
        _client = new EmbeddingClient("text-embedding-3-small", openaiApiKey);
    }

    public ReadOnlyMemory<float> GenerateEmbedding(string input)
    {
        OpenAIEmbedding embedding = _client.GenerateEmbedding(input);
        ReadOnlyMemory<float> vector = embedding.ToFloats();
        return vector;
    }
}