using Microsoft.Data.Sqlite;

namespace YdkTester.Modules.EdoPro;

public class EdoProCardResolver : ICardResolver
{
    private List<SqliteConnection> _sqliteConnections;

    public EdoProCardResolver(string edoProPath)
    {
        _sqliteConnections = new List<SqliteConnection>();

        var directortyPath = Path.Combine(edoProPath, "repositories/delta-utopia");
        foreach (var filePath in Directory.GetFiles(directortyPath))
        {
            if (filePath.EndsWith(".cdb"))
            {
                var connection = new SqliteConnection("Data Source=" + filePath);
                connection.Open();

                _sqliteConnections.Add(connection);
            }
        }
    }

    public Card Parse(int id)
    {
        var card = new Card();
        card.Id = id;

        foreach (var connection in _sqliteConnections)
        {
            var commandTexts = connection.CreateCommand();
            commandTexts.CommandText = @"SELECT name FROM texts WHERE id = $id";
            commandTexts.Parameters.AddWithValue("$id", id);
            using var readerTexts = commandTexts.ExecuteReader();

            var commandDatas = connection.CreateCommand();
            commandDatas.CommandText = @"SELECT type, atk, def, level, race, attribute FROM datas WHERE id = $id";
            commandDatas.Parameters.AddWithValue("$id", id);
            using var readerDatas = commandDatas.ExecuteReader();
            
            if (readerTexts.Read() && readerDatas.Read())
            {
                card.Title = readerTexts.GetString(0);

                card.Category = (CardCategory)readerDatas.GetInt32(0);
                card.Atk = readerDatas.GetInt32(1);
                card.Def = readerDatas.GetInt32(2);
                card.Level = readerDatas.GetInt32(3) & 0xFFFF; // first 4 byes are pend scales (2 left, 2 right), next 4 bytes are the level
                card.Type = (CardType)readerDatas.GetInt32(4);
                card.Attribute = (CardAttribute)readerDatas.GetInt32(5);
            }
        }

        return card;
    }
}
