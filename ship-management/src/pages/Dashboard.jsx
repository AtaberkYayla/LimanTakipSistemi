import { useState, useEffect } from 'react';
import {
  BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer,
  PieChart, Pie, Cell
} from 'recharts';
import api from '../services/api';

const COLORS = ['#1a3c5e', '#2a5a8e', '#3d7ab5', '#5a9fd4', '#7db8e8', '#a8d1f5'];

function Dashboard() {
  const [summary, setSummary] = useState(null);
  const [shipsByType, setShipsByType] = useState([]);
  const [shipsByFlag, setShipsByFlag] = useState([]);
  const [crewByRole, setCrewByRole] = useState([]);
  const [visitsByPort, setVisitsByPort] = useState([]);
  const [cargoByType, setCargoByType] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchAll();
  }, []);

  const fetchAll = async () => {
    try {
      const [summaryRes, typeRes, flagRes, crewRes, visitsRes, cargoRes] = await Promise.all([
        api.get('/analytics/summary'),
        api.get('/analytics/ships-by-type'),
        api.get('/analytics/ships-by-flag'),
        api.get('/analytics/crew-by-role'),
        api.get('/analytics/visits-by-port'),
        api.get('/analytics/cargo-by-type')
      ]);
      setSummary(summaryRes.data);
      setShipsByType(typeRes.data);
      setShipsByFlag(flagRes.data);
      setCrewByRole(crewRes.data);
      setVisitsByPort(visitsRes.data);
      setCargoByType(cargoRes.data);
    } catch (err) {
      console.error('Dashboard verileri yüklenemedi', err);
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <div className="loading">Yükleniyor...</div>;

  return (
    <div>
      <div className="page-header">
        <h1>Dashboard</h1>
      </div>

      {/* Özet Kartlar */}
      {summary && (
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(6, 1fr)', gap: '1rem', marginBottom: '2rem' }}>
          {[
            { label: 'Gemiler', value: summary.totalShips, icon: '🚢' },
            { label: 'Limanlar', value: summary.totalPorts, icon: '⚓' },
            { label: 'Mürettebat', value: summary.totalCrewMembers, icon: '👤' },
            { label: 'Yükler', value: summary.totalCargoes, icon: '📦' },
            { label: 'Ziyaretler', value: summary.totalVisits, icon: '📅' },
            { label: 'Atamalar', value: summary.totalAssignments, icon: '📋' },
          ].map((item, i) => (
            <div key={i} style={{
              background: 'white', borderRadius: '8px', padding: '1.2rem',
              boxShadow: '0 1px 3px rgba(0,0,0,0.1)', textAlign: 'center'
            }}>
              <div style={{ fontSize: '2rem' }}>{item.icon}</div>
              <div style={{ fontSize: '1.8rem', fontWeight: 'bold', color: '#1a3c5e' }}>{item.value}</div>
              <div style={{ color: '#666', fontSize: '0.85rem' }}>{item.label}</div>
            </div>
          ))}
        </div>
      )}

      {/* Grafikler - Satır 1 */}
      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem', marginBottom: '1rem' }}>
        {/* Gemi Tipi Dağılımı */}
        <div style={{ background: 'white', borderRadius: '8px', padding: '1.5rem', boxShadow: '0 1px 3px rgba(0,0,0,0.1)' }}>
          <h3 style={{ marginBottom: '1rem', color: '#1a3c5e' }}>Gemi Tipi Dağılımı</h3>
          <ResponsiveContainer width="100%" height={250}>
            <PieChart>
              <Pie data={shipsByType} dataKey="count" nameKey="type" cx="50%" cy="50%" outerRadius={90} label={({ type, count }) => `${type}: ${count}`}>
                {shipsByType.map((_, i) => <Cell key={i} fill={COLORS[i % COLORS.length]} />)}
              </Pie>
              <Tooltip />
            </PieChart>
          </ResponsiveContainer>
        </div>

        {/* Bayrak Dağılımı */}
        <div style={{ background: 'white', borderRadius: '8px', padding: '1.5rem', boxShadow: '0 1px 3px rgba(0,0,0,0.1)' }}>
          <h3 style={{ marginBottom: '1rem', color: '#1a3c5e' }}>Bayrak Dağılımı</h3>
          <ResponsiveContainer width="100%" height={250}>
            <BarChart data={shipsByFlag}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="flag" />
              <YAxis allowDecimals={false} />
              <Tooltip />
              <Bar dataKey="count" name="Gemi Sayısı" fill="#1a3c5e" />
            </BarChart>
          </ResponsiveContainer>
        </div>
      </div>

      {/* Grafikler - Satır 2 */}
      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem', marginBottom: '1rem' }}>
        {/* Mürettebat Rol Dağılımı */}
        <div style={{ background: 'white', borderRadius: '8px', padding: '1.5rem', boxShadow: '0 1px 3px rgba(0,0,0,0.1)' }}>
          <h3 style={{ marginBottom: '1rem', color: '#1a3c5e' }}>Mürettebat Rol Dağılımı</h3>
          <ResponsiveContainer width="100%" height={250}>
            <BarChart data={crewByRole} layout="vertical">
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis type="number" allowDecimals={false} />
              <YAxis dataKey="role" type="category" width={120} />
              <Tooltip />
              <Bar dataKey="count" name="Kişi Sayısı" fill="#2a5a8e" />
            </BarChart>
          </ResponsiveContainer>
        </div>

        {/* Liman Ziyaret Sayıları */}
        <div style={{ background: 'white', borderRadius: '8px', padding: '1.5rem', boxShadow: '0 1px 3px rgba(0,0,0,0.1)' }}>
          <h3 style={{ marginBottom: '1rem', color: '#1a3c5e' }}>Liman Ziyaret Sayıları</h3>
          <ResponsiveContainer width="100%" height={250}>
            <BarChart data={visitsByPort}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="port" tick={{ fontSize: 11 }} />
              <YAxis allowDecimals={false} />
              <Tooltip />
              <Bar dataKey="count" name="Ziyaret Sayısı" fill="#3d7ab5" />
            </BarChart>
          </ResponsiveContainer>
        </div>
      </div>

      {/* Grafikler - Satır 3 */}
      <div style={{ background: 'white', borderRadius: '8px', padding: '1.5rem', boxShadow: '0 1px 3px rgba(0,0,0,0.1)' }}>
        <h3 style={{ marginBottom: '1rem', color: '#1a3c5e' }}>Yük Tipi Dağılımı (Ton)</h3>
        <ResponsiveContainer width="100%" height={250}>
          <BarChart data={cargoByType}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="cargoType" />
            <YAxis />
            <Tooltip />
            <Legend />
            <Bar dataKey="totalWeight" name="Toplam Ağırlık (Ton)" fill="#1a3c5e" />
            <Bar dataKey="count" name="Yük Sayısı" fill="#5a9fd4" />
          </BarChart>
        </ResponsiveContainer>
      </div>
    </div>
  );
}

export default Dashboard;