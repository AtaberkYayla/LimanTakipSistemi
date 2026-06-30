import { useState, useEffect } from 'react';
import api from '../services/api';

function Cargoes() {
  const [cargoes, setCargoes] = useState([]);
  const [ships, setShips] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [editingCargo, setEditingCargo] = useState(null);
  const [error, setError] = useState('');
  const [form, setForm] = useState({
    shipId: '', description: '', weightTon: '', cargoType: ''
  });

  useEffect(() => { fetchAll(); }, []);

  const fetchAll = async () => {
    try {
      const [cargoRes, shipsRes] = await Promise.all([
        api.get('/cargoes'),
        api.get('/ships')
      ]);
      setCargoes(cargoRes.data);
      setShips(shipsRes.data);
    } catch {
      setError('Veriler yüklenemedi.');
    } finally {
      setLoading(false);
    }
  };

  const openCreate = () => {
    setForm({ shipId: '', description: '', weightTon: '', cargoType: '' });
    setEditingCargo(null);
    setError('');
    setShowModal(true);
  };

  const openEdit = (cargo) => {
    setForm({
      shipId: cargo.shipId,
      description: cargo.description,
      weightTon: cargo.weightTon,
      cargoType: cargo.cargoType
    });
    setEditingCargo(cargo);
    setError('');
    setShowModal(true);
  };

  const handleSubmit = async () => {
    try {
      const payload = {
        ...form,
        shipId: parseInt(form.shipId),
        weightTon: parseFloat(form.weightTon)
      };
      if (editingCargo) {
        await api.put(`/cargoes/${editingCargo.cargoId}`, payload);
      } else {
        await api.post('/cargoes', payload);
      }
      setShowModal(false);
      fetchAll();
    } catch (err) {
      setError(err.response?.data || 'Bir hata oluştu.');
    }
  };

  const handleDelete = async (id) => {
    if (!confirm('Bu yükü silmek istediğinizden emin misiniz?')) return;
    try {
      await api.delete(`/cargoes/${id}`);
      fetchAll();
    } catch {
      setError('Silme işlemi başarısız.');
    }
  };

  if (loading) return <div className="loading">Yükleniyor...</div>;

  return (
    <div>
      <div className="page-header">
        <h1>Yük Yönetimi</h1>
        <button className="btn btn-primary" onClick={openCreate}>+ Yeni Yük</button>
      </div>

      {error && <div className="error-message">{error}</div>}

      <table>
        <thead>
          <tr>
            <th>Gemi</th>
            <th>Açıklama</th>
            <th>Ağırlık (Ton)</th>
            <th>Yük Tipi</th>
            <th>İşlemler</th>
          </tr>
        </thead>
        <tbody>
          {cargoes.map(cargo => (
            <tr key={cargo.cargoId}>
              <td>{cargo.shipName}</td>
              <td>{cargo.description}</td>
              <td>{cargo.weightTon}</td>
              <td>{cargo.cargoType}</td>
              <td>
                <button className="btn btn-secondary btn-sm" onClick={() => openEdit(cargo)}>Düzenle</button>
                {' '}
                <button className="btn btn-danger btn-sm" onClick={() => handleDelete(cargo.cargoId)}>Sil</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {showModal && (
        <div className="modal-overlay">
          <div className="modal">
            <h2>{editingCargo ? 'Yük Düzenle' : 'Yeni Yük'}</h2>
            {error && <div className="error-message">{error}</div>}
            <div className="form-group">
              <label>Gemi</label>
              <select value={form.shipId} onChange={e => setForm({ ...form, shipId: e.target.value })}>
                <option value="">Seçiniz</option>
                {ships.map(s => <option key={s.shipId} value={s.shipId}>{s.name}</option>)}
              </select>
            </div>
            <div className="form-group">
              <label>Açıklama</label>
              <input value={form.description} onChange={e => setForm({ ...form, description: e.target.value })} />
            </div>
            <div className="form-group">
              <label>Ağırlık (Ton)</label>
              <input type="number" step="0.01" value={form.weightTon} onChange={e => setForm({ ...form, weightTon: e.target.value })} />
            </div>
            <div className="form-group">
              <label>Yük Tipi</label>
              <input value={form.cargoType} onChange={e => setForm({ ...form, cargoType: e.target.value })} />
            </div>
            <div className="modal-actions">
              <button className="btn btn-secondary" onClick={() => setShowModal(false)}>İptal</button>
              <button className="btn btn-primary" onClick={handleSubmit}>
                {editingCargo ? 'Güncelle' : 'Kaydet'}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

export default Cargoes;