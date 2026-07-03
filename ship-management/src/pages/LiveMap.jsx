import { useState, useEffect } from 'react';
import { MapContainer, TileLayer, Marker, Popup, useMap } from 'react-leaflet';
import L from 'leaflet';
import 'leaflet/dist/leaflet.css';
import api from '../services/api';

delete L.Icon.Default.prototype._getIconUrl;
L.Icon.Default.mergeOptions({
  iconRetinaUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-icon-2x.png',
  iconUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-icon.png',
  shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-shadow.png',
});

const shipIcon = L.divIcon({
  className: '',
  html: '🚢',
  iconSize: [24, 24],
  iconAnchor: [12, 12],
});

function LiveMap() {
  const [ships, setShips] = useState([]);
  const [loading, setLoading] = useState(true);
  const [lastUpdate, setLastUpdate] = useState(null);

  useEffect(() => {
    fetchShips();
    const interval = setInterval(fetchShips, 10000);
    return () => clearInterval(interval);
  }, []);

  const fetchShips = async () => {
    try {
      const res = await api.get('/ais/live-ships');
      setShips(res.data);
      setLastUpdate(new Date().toLocaleTimeString('tr-TR'));
    } catch (err) {
      console.error('AIS verisi alınamadı', err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <div className="page-header">
        <h1>Canlı Gemi Takibi</h1>
        <div style={{ display: 'flex', alignItems: 'center', gap: '1rem' }}>
          <span style={{ color: '#666', fontSize: '0.9rem' }}>
            {ships.length} gemi takip ediliyor
          </span>
          {lastUpdate && (
            <span style={{ color: '#666', fontSize: '0.9rem' }}>
              Son güncelleme: {lastUpdate}
            </span>
          )}
          <button className="btn btn-primary" onClick={fetchShips}>Yenile</button>
        </div>
      </div>

      {loading ? (
        <div className="loading">Yükleniyor...</div>
      ) : (
        <div style={{ borderRadius: '8px', overflow: 'hidden', boxShadow: '0 1px 3px rgba(0,0,0,0.1)' }}>
          <MapContainer
            center={[38.5, 35.0]}
            zoom={6}
            style={{ height: '600px', width: '100%' }}
          >
            <TileLayer
              attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>'
              url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
            />
            {ships.filter(s => s.latitude !== 0 && s.longitude !== 0).map(ship => (
              <Marker
                key={ship.mmsi}
                position={[ship.latitude, ship.longitude]}
                icon={shipIcon}
              >
                <Popup>
                  <div style={{ minWidth: '150px' }}>
                    <strong>{ship.shipName}</strong><br />
                    <span>MMSI: {ship.mmsi}</span><br />
                    <span>Hız: {ship.speed} knot</span><br />
                    <span>Yön: {ship.heading}°</span><br />
                    <span style={{ fontSize: '0.8rem', color: '#666' }}>
                      {new Date(ship.timeUtc).toLocaleString('tr-TR')}
                    </span>
                  </div>
                </Popup>
              </Marker>
            ))}
          </MapContainer>
        </div>
      )}

      <div style={{ marginTop: '1rem' }}>
        <table>
          <thead>
            <tr>
              <th>Gemi Adı</th>
              <th>MMSI</th>
              <th>Enlem</th>
              <th>Boylam</th>
              <th>Hız (knot)</th>
              <th>Yön</th>
              <th>Son Güncelleme</th>
            </tr>
          </thead>
          <tbody>
            {ships.map(ship => (
              <tr key={ship.mmsi}>
                <td>{ship.shipName}</td>
                <td>{ship.mmsi}</td>
                <td>{ship.latitude.toFixed(4)}</td>
                <td>{ship.longitude.toFixed(4)}</td>
                <td>{ship.speed}</td>
                <td>{ship.heading}°</td>
                <td style={{ fontSize: '0.85rem' }}>
                  {new Date(ship.timeUtc).toLocaleString('tr-TR')}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default LiveMap;