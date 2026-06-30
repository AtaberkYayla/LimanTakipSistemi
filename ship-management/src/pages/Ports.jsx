import { useState, useEffect } from 'react';
import api from '../services/api';

function Ports() {
  const [ports, setPorts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [editingPort, setEditingPort] = useState(null);
  const [error, setError] = useState('');
  const [form, setForm] = useState({ name: '', country: '', city: '' });

  useEffect(() => { fetchPorts(); }, []);

  const fetchPorts = async () => {
    try {
      const res = await api.get('/ports');
      setPorts(res.data);
    } catch {
      setError('Limanlar yüklenemedi.');
    } finally {
      setLoading(false);
    }
  };

  const openCreate = () => {
    setForm({ name: '', country: '', city: '' });
    setEditingPort(null);
    setError('');
    setShowModal(true);
  };

  const openEdit = (port) => {
    setForm({ name: port.name, country: port.country, city: port.city });
    setEditingPort(port);
    setError('');
    setShowModal(true);
  };

  const handleSubmit = async () => {
    try {
      if (editingPort) {
        await api.put(`/ports/${editingPort.portId}`, form);
      } else {
        await api.post('/ports', form);
      }
      setShowModal(false);
      fetchPorts();
    } catch (err) {
      setError(err.response?.data || 'Bir hata oluştu.');
    }
  };

  const handleDelete = async (id) => {
    if (!confirm('Bu limanı silmek istediğinizden emin misiniz?')) return;
    try {
      await api.delete(`/ports/${id}`);
      fetchPorts();
    } catch {
      setError('Silme işlemi başarısız.');
    }
  };

  if (loading) return <div className="loading">Yükleniyor...</div>;

  return (
    <div>
      <div className="page-header">
        <h1>Liman Yönetimi</h1>
        <button className="btn btn-primary" onClick={openCreate}>+ Yeni Liman</button>
      </div>

      {error && <div className="error-message">{error}</div>}

      <table>
        <thead>
          <tr>
            <th>Ad</th>
            <th>Ülke</th>
            <th>Şehir</th>
            <th>İşlemler</th>
          </tr>
        </thead>
        <tbody>
          {ports.map(port => (
            <tr key={port.portId}>
              <td>{port.name}</td>
              <td>{port.country}</td>
              <td>{port.city}</td>
              <td>
                <button className="btn btn-secondary btn-sm" onClick={() => openEdit(port)}>Düzenle</button>
                {' '}
                <button className="btn btn-danger btn-sm" onClick={() => handleDelete(port.portId)}>Sil</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {showModal && (
        <div className="modal-overlay">
          <div className="modal">
            <h2>{editingPort ? 'Liman Düzenle' : 'Yeni Liman'}</h2>
            {error && <div className="error-message">{error}</div>}
            <div className="form-group">
              <label>Liman Adı</label>
              <input value={form.name} onChange={e => setForm({ ...form, name: e.target.value })} />
            </div>
            <div className="form-group">
              <label>Ülke</label>
              <input value={form.country} onChange={e => setForm({ ...form, country: e.target.value })} />
            </div>
            <div className="form-group">
              <label>Şehir</label>
              <input value={form.city} onChange={e => setForm({ ...form, city: e.target.value })} />
            </div>
            <div className="modal-actions">
              <button className="btn btn-secondary" onClick={() => setShowModal(false)}>İptal</button>
              <button className="btn btn-primary" onClick={handleSubmit}>
                {editingPort ? 'Güncelle' : 'Kaydet'}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

export default Ports;