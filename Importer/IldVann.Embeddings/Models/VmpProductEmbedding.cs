namespace IldVann.Embeddings.Models;

public class VmpProductEmbedding
{
    public string? Code { get; init; }
    public ReadOnlyMemory<float> NameVectorEmbedding { get; init; }
}