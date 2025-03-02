using IldVann.Importer.Mappers;
using IldVann.Importer.Models;

using Vinmonopolet.Models;

using Z.Dapper.Plus;

namespace Ildvann.Importer.Repositories;

public class RumXRepository
{
    private readonly RumXMapper _rumXMapper;

    public RumXRepository(RumXMapper rumXMapper)
    {
        _rumXMapper = rumXMapper ?? throw new ArgumentNullException(nameof(rumXMapper));
    }

    public void BulkInsertRums(List<RumXRum> rums)
    {
        var mappedRums = _rumXMapper.MapToDto(rums);

        if (mappedRums.Count == 0)
        {
            Console.WriteLine("No rums to insert, mapping failed");
            return;
        }

        using var connection = DbConnection.CreateConnection();
        connection
            .UseBulkOptions(options =>
            {
                options.InsertIfNotExists = true;
                options.DestinationTableName = "rumx_rums";
            })
            .BulkInsert(mappedRums);

        Console.WriteLine($"Inserted {mappedRums.Count} rums");
    }
}