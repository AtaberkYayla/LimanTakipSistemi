using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace ShipManagement.Services;

public class AISStreamService : BackgroundService
{
    private readonly ILogger<AISStreamService> _logger;
    private readonly IConfiguration _configuration;
    private static readonly List<AISShipData> _recentShips = new();
    private static readonly object _lock = new();

    public AISStreamService(ILogger<AISStreamService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public static List<AISShipData> GetRecentShips()
    {
        lock (_lock)
        {
            return _recentShips.ToList();
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var apiKey = _configuration["AISStream:ApiKey"];
        if (string.IsNullOrEmpty(apiKey))
        {
            _logger.LogWarning("AISStream API key bulunamadı.");
            return;
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ConnectAndListenAsync(apiKey, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AISStream bağlantısı kesildi, 30 saniye sonra yeniden bağlanılacak.");
                await Task.Delay(30000, stoppingToken);
            }
        }
    }

    private async Task ConnectAndListenAsync(string apiKey, CancellationToken stoppingToken)
    {
        using var ws = new ClientWebSocket();
        await ws.ConnectAsync(new Uri("wss://stream.aisstream.io/v0/stream"), stoppingToken);
        _logger.LogInformation("AISStream WebSocket bağlantısı kuruldu.");

        // Türkiye çevresini dinle
        var subscribeMsg = JsonSerializer.Serialize(new
        {
            APIKey = apiKey,
            BoundingBoxes = new[] { new[] { new[] { 35.0, 25.0 }, new[] { 42.0, 45.0 } } },
            FilterMessageTypes = new[] { "PositionReport" }
        });

        var bytes = Encoding.UTF8.GetBytes(subscribeMsg);
        await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, stoppingToken);

        var buffer = new byte[4096];
        while (ws.State == WebSocketState.Open && !stoppingToken.IsCancellationRequested)
        {
            var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), stoppingToken);
            if (result.MessageType == WebSocketMessageType.Close) break;

            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            ProcessMessage(message);
        }
    }

    private void ProcessMessage(string message)
    {
        try
        {
            var doc = JsonDocument.Parse(message);
            var root = doc.RootElement;

            if (!root.TryGetProperty("MetaData", out var meta)) return;

            var shipData = new AISShipData
            {
                MMSI = meta.TryGetProperty("MMSI", out var mmsi) ? mmsi.GetInt32() : 0,
                ShipName = meta.TryGetProperty("ShipName", out var name) ? name.GetString()?.Trim() ?? "Bilinmiyor" : "Bilinmiyor",
                Latitude = meta.TryGetProperty("latitude", out var lat) ? lat.GetDouble() : 0,
                Longitude = meta.TryGetProperty("longitude", out var lon) ? lon.GetDouble() : 0,
                TimeUtc = meta.TryGetProperty("time_utc", out var time) ? time.GetString() ?? "" : "",
            };

            if (root.TryGetProperty("Message", out var msgProp) &&
                msgProp.TryGetProperty("PositionReport", out var posReport))
            {
                shipData.Speed = posReport.TryGetProperty("Sog", out var sog) ? sog.GetDouble() : 0;
                shipData.Heading = posReport.TryGetProperty("TrueHeading", out var heading) ? heading.GetInt32() : 0;
            }

            lock (_lock)
            {
                var existing = _recentShips.FirstOrDefault(s => s.MMSI == shipData.MMSI);
                if (existing != null)
                    _recentShips.Remove(existing);

                _recentShips.Add(shipData);

                if (_recentShips.Count > 100)
                    _recentShips.RemoveAt(0);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AIS mesajı işlenirken hata oluştu.");
        }
    }
}

public class AISShipData
{
    public int MMSI { get; set; }
    public string ShipName { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Speed { get; set; }
    public int Heading { get; set; }
    public string TimeUtc { get; set; } = string.Empty;
}