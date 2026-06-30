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

  useEffect(() => {
    fetchShips();
  }, []);

  const fetchShips = async () => {
    try {
      const res = await api.get('/ships');
      setShips(res.data);
    } catch {
      setError('Gemiler yüklenemedi.');
    } finally {
      setLoading(false);
    }
  };

  const openCreate = () => {
    setForm({ name: '', imo: '', type: '', flag: '', yearBuilt: '' });
    setEditingShip(null);
    setError('');
    setShowModal(true);
  };

  const openEdit = (ship) => {
    setForm({
      name: ship.name,
      imo: ship.imo,
      type: ship.type,
      flag: ship.flag,
      yearBuilt: ship.yearBuilt
    });
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
      setError(err.response?.data || 'Bir hata oluştu.');
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