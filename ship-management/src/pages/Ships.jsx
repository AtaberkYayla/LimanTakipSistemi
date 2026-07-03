import { useState, useEffect } from 'react';
import api from '../services/api';

function Ships() {
  const [ships, setShips] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [editingShip, setEditingShip] = useState(null);
  const [error, setError] = useState('');
  const [form, setForm] = useState({
    name: '', imo: '', type: '', flag: '', yearBuilt: ''
  });

  const [filter, setFilter] = useState({
    name: '', type: '', flag: '', yearBuiltMin: '', yearBuiltMax: '', page: 1, pageSize: 10
  });
  const [pagination, setPagination] = useState({
    totalCount: 0, totalPages: 1, hasNextPage: false, hasPreviousPage: false
  });

  useEffect(() => { fetchShipsWithFilter(); }, [filter.page]);

  const fetchShipsWithFilter = async (customFilter = filter) => {
    try {
      const params = new URLSearchParams();
      if (customFilter.name) params.append('name', customFilter.name);
      if (customFilter.type) params.append('type', customFilter.type);
      if (customFilter.flag) params.append('flag', customFilter.flag);
      if (customFilter.yearBuiltMin) params.append('yearBuiltMin', customFilter.yearBuiltMin);
      if (customFilter.yearBuiltMax) params.append('yearBuiltMax', customFilter.yearBuiltMax);
      params.append('page', customFilter.page);
      params.append('pageSize', customFilter.pageSize);

      const res = await api.get(`/ships/search?${params.toString()}`);
      setShips(res.data.items);
      setPagination({
        totalCount: res.data.totalCount,
        totalPages: res.data.totalPages,
        hasNextPage: res.data.hasNextPage,
        hasPreviousPage: res.data.hasPreviousPage
      });
    } catch {
      setError('Gemiler yüklenemedi.');
    } finally {
      setLoading(false);
    }
  };

  const handleSearch = () => {
    const newFilter = { ...filter, page: 1 };
    setFilter(newFilter);
    fetchShipsWithFilter(newFilter);
  };

  const handleReset = () => {
    const resetFilter = { name: '', type: '', flag: '', yearBuiltMin: '', yearBuiltMax: '', page: 1, pageSize: 10 };
    setFilter(resetFilter);
    fetchShipsWithFilter(resetFilter);
  };

  const openCreate = () => {
    setForm({ name: '', imo: '', type: '', flag: '', yearBuilt: '' });
    setEditingShip(null);
    setError('');
    setShowModal(true);
  };

  const openEdit = (ship) => {
    setForm({ name: ship.name, imo: ship.imo, type: ship.type, flag: ship.flag, yearBuilt: ship.yearBuilt });
    setEditingShip(ship);
    setError('');
    setShowModal(true);
  };

  const handleSubmit = async () => {
    try {
      if (editingShip) {
        await api.put(`/ships/${editingShip.shipId}`, form);
      } else {
        await api.post('/ships', form);
      }
      setShowModal(false);
      fetchShips();
    } catch (err) {
      const data = err.response?.data;
      if (data?.errors) {
        setError(Object.values(data.errors).flat().join(', '));
      } else if (typeof data === 'string') {
        setError(data);
      } else {
        setError('Bir hata oluştu.');
      }
    }
  };

  const handleDelete = async (id) => {
    if (!confirm('Bu gemiyi silmek istediğinizden emin misiniz?')) return;
    try {
      await api.delete(`/ships/${id}`);
      fetchShips();
    } catch {
      setError('Silme işlemi başarısız.');
    }
  };

  if (loading) return <div className="loading">Yükleniyor...</div>;

  return (
    <div>
      <div className="page-header">
        <h1>Gemi Yönetimi</h1>
        <button className="btn btn-primary" onClick={openCreate}>+ Yeni Gemi</button>
      </div>

      {/* Arama Filtresi */}
      <div style={{ background: 'white', padding: '1rem', borderRadius: '8px', marginBottom: '1rem', boxShadow: '0 1px 3px rgba(0,0,0,0.1)' }}>
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(5, 1fr)', gap: '0.5rem', marginBottom: '0.5rem' }}>
          <input placeholder="Gemi Adı" value={filter.name} onChange={e => setFilter({ ...filter, name: e.target.value })} style={{ padding: '0.5rem', border: '1px solid #ddd', borderRadius: '4px' }} />
          <input placeholder="Tip" value={filter.type} onChange={e => setFilter({ ...filter, type: e.target.value })} style={{ padding: '0.5rem', border: '1px solid #ddd', borderRadius: '4px' }} />
          <input placeholder="Bayrak" value={filter.flag} onChange={e => setFilter({ ...filter, flag: e.target.value })} style={{ padding: '0.5rem', border: '1px solid #ddd', borderRadius: '4px' }} />
          <input type="number" placeholder="Min Yıl" value={filter.yearBuiltMin} onChange={e => setFilter({ ...filter, yearBuiltMin: e.target.value })} style={{ padding: '0.5rem', border: '1px solid #ddd', borderRadius: '4px' }} />
          <input type="number" placeholder="Max Yıl" value={filter.yearBuiltMax} onChange={e => setFilter({ ...filter, yearBuiltMax: e.target.value })} style={{ padding: '0.5rem', border: '1px solid #ddd', borderRadius: '4px' }} />
        </div>
        <div style={{ display: 'flex', gap: '0.5rem' }}>
          <button className="btn btn-primary" onClick={handleSearch}>Ara</button>
          <button className="btn btn-secondary" onClick={handleReset}>Temizle</button>
        </div>
      </div>

      {error && <div className="error-message">{error}</div>}

      <table>
        <thead>
          <tr>
            <th>Ad</th>
            <th>IMO</th>
            <th>Tip</th>
            <th>Bayrak</th>
            <th>İnşa Yılı</th>
            <th>İşlemler</th>
          </tr>
        </thead>
        <tbody>
          {ships.map(ship => (
            <tr key={ship.shipId}>
              <td>{ship.name}</td>
              <td>{ship.imo}</td>
              <td>{ship.type}</td>
              <td>{ship.flag}</td>
              <td>{ship.yearBuilt}</td>
              <td>
                <button className="btn btn-secondary btn-sm" onClick={() => openEdit(ship)}>Düzenle</button>
                {' '}
                <button className="btn btn-danger btn-sm" onClick={() => handleDelete(ship.shipId)}>Sil</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginTop: '1rem' }}>
        <span style={{ color: '#666', fontSize: '0.9rem' }}>Toplam {pagination.totalCount} kayıt</span>
        <div style={{ display: 'flex', gap: '0.5rem' }}>
          <button className="btn btn-secondary btn-sm" disabled={!pagination.hasPreviousPage} onClick={() => setFilter(prev => ({ ...prev, page: prev.page - 1 }))}>← Önceki</button>
          <span style={{ padding: '0.3rem 0.7rem', background: '#1a3c5e', color: 'white', borderRadius: '4px', fontSize: '0.85rem' }}>{filter.page} / {pagination.totalPages}</span>
          <button className="btn btn-secondary btn-sm" disabled={!pagination.hasNextPage} onClick={() => setFilter(prev => ({ ...prev, page: prev.page + 1 }))}>Sonraki →</button>
        </div>
      </div>

      {showModal && (
        <div className="modal-overlay">
          <div className="modal">
            <h2>{editingShip ? 'Gemi Düzenle' : 'Yeni Gemi'}</h2>
            {error && <div className="error-message">{error}</div>}
            <div className="form-group">
              <label>Gemi Adı</label>
              <input value={form.name} onChange={e => setForm({ ...form, name: e.target.value })} />
            </div>
            <div className="form-group">
              <label>IMO Numarası</label>
              <input value={form.imo} onChange={e => setForm({ ...form, imo: e.target.value })} />
            </div>
            <div className="form-group">
              <label>Tip</label>
              <input value={form.type} onChange={e => setForm({ ...form, type: e.target.value })} />
            </div>
            <div className="form-group">
              <label>Bayrak (Ülke)</label>
              <input value={form.flag} onChange={e => setForm({ ...form, flag: e.target.value })} />
            </div>
            <div className="form-group">
              <label>İnşa Yılı</label>
              <input type="number" value={form.yearBuilt} onChange={e => setForm({ ...form, yearBuilt: e.target.value })} />
            </div>
            <div className="modal-actions">
              <button className="btn btn-secondary" onClick={() => setShowModal(false)}>İptal</button>
              <button className="btn btn-primary" onClick={handleSubmit}>
                {editingShip ? 'Güncelle' : 'Kaydet'}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

export default Ships;