import { useState, useEffect } from 'react';
import api from '../services/api';

function ShipVisits() {
  const [visits, setVisits] = useState([]);
  const [ships, setShips] = useState([]);
  const [ports, setPorts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [editingVisit, setEditingVisit] = useState(null);
  const [error, setError] = useState('');
  const [form, setForm] = useState({
    shipId: '', portId: '', arrivalDate: '', departureDate: '', purpose: ''
  });

  useEffect(() => { fetchAll(); }, []);

  const fetchAll = async () => {
    try {
      const [visitsRes, shipsRes, portsRes] = await Promise.all([
        api.get('/shipvisits'),
        api.get('/ships'),
        api.get('/ports')
      ]);
      setVisits(visitsRes.data);
      setShips(shipsRes.data);
      setPorts(portsRes.data);
    } catch {
      setError('Veriler yüklenemedi.');
    } finally {
      setLoading(false);
    }
  };

  const openCreate = () => {
    setForm({ shipId: '', portId: '', arrivalDate: '', departureDate: '', purpose: '' });
    setEditingVisit(null);
    setError('');
    setShowModal(true);
  };

  const openEdit = (visit) => {
    setForm({
      shipId: visit.shipId,
      portId: visit.portId,
      arrivalDate: visit.arrivalDate?.slice(0, 16),
      departureDate: visit.departureDate?.slice(0, 16) || '',
      purpose: visit.purpose
    });
    setEditingVisit(visit);
    setError('');
    setShowModal(true);
  };

  const handleSubmit = async () => {
    try {
      const payload = {
        ...form,
        shipId: parseInt(form.shipId),
        portId: parseInt(form.portId),
        departureDate: form.departureDate || null
      };
      if (editingVisit) {
        await api.put(`/shipvisits/${editingVisit.visitId}`, payload);
      } else {
        await api.post('/shipvisits', payload);
      }
      setShowModal(false);
      fetchAll();
    } catch (err) {
      setError(err.response?.data || 'Bir hata oluştu.');
    }
  };

  const handleDelete = async (id) => {
    if (!confirm('Bu ziyareti silmek istediğinizden emin misiniz?')) return;
    try {
      await api.delete(`/shipvisits/${id}`);
      fetchAll();
    } catch {
      setError('Silme işlemi başarısız.');
    }
  };

  if (loading) return <div className="loading">Yükleniyor...</div>;

  return (
    <div>
      <div className="page-header">
        <h1>Ziyaret Kayıtları</h1>
        <button className="btn btn-primary" onClick={openCreate}>+ Yeni Ziyaret</button>
      </div>

      {error && <div className="error-message">{error}</div>}

      <table>
        <thead>
          <tr>
            <th>Gemi</th>
            <th>Liman</th>
            <th>Geliş Tarihi</th>
            <th>Ayrılış Tarihi</th>
            <th>Amaç</th>
            <th>İşlemler</th>
          </tr>
        </thead>
        <tbody>
          {visits.map(visit => (
            <tr key={visit.visitId}>
              <td>{visit.shipName}</td>
              <td>{visit.portName}</td>
              <td>{new Date(visit.arrivalDate).toLocaleString('tr-TR')}</td>
              <td>{visit.departureDate ? new Date(visit.departureDate).toLocaleString('tr-TR') : '-'}</td>
              <td>{visit.purpose}</td>
              <td>
                <button className="btn btn-secondary btn-sm" onClick={() => openEdit(visit)}>Düzenle</button>
                {' '}
                <button className="btn btn-danger btn-sm" onClick={() => handleDelete(visit.visitId)}>Sil</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {showModal && (
        <div className="modal-overlay">
          <div className="modal">
            <h2>{editingVisit ? 'Ziyaret Düzenle' : 'Yeni Ziyaret'}</h2>
            {error && <div className="error-message">{error}</div>}
            <div className="form-group">
              <label>Gemi</label>
              <select value={form.shipId} onChange={e => setForm({ ...form, shipId: e.target.value })}>
                <option value="">Seçiniz</option>
                {ships.map(s => <option key={s.shipId} value={s.shipId}>{s.name}</option>)}
              </select>
            </div>
            <div className="form-group">
              <label>Liman</label>
              <select value={form.portId} onChange={e => setForm({ ...form, portId: e.target.value })}>
                <option value="">Seçiniz</option>
                {ports.map(p => <option key={p.portId} value={p.portId}>{p.name}</option>)}
              </select>
            </div>
            <div className="form-group">
              <label>Geliş Tarihi</label>
              <input type="datetime-local" value={form.arrivalDate} onChange={e => setForm({ ...form, arrivalDate: e.target.value })} />
            </div>
            <div className="form-group">
              <label>Ayrılış Tarihi (opsiyonel)</label>
              <input type="datetime-local" value={form.departureDate} onChange={e => setForm({ ...form, departureDate: e.target.value })} />
            </div>
            <div className="form-group">
              <label>Amaç</label>
              <input value={form.purpose} onChange={e => setForm({ ...form, purpose: e.target.value })} />
            </div>
            <div className="modal-actions">
              <button className="btn btn-secondary" onClick={() => setShowModal(false)}>İptal</button>
              <button className="btn btn-primary" onClick={handleSubmit}>
                {editingVisit ? 'Güncelle' : 'Kaydet'}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

export default ShipVisits;