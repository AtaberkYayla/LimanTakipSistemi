import { useState, useEffect } from 'react';
import api from '../services/api';

function ShipCrewAssignments() {
  const [assignments, setAssignments] = useState([]);
  const [ships, setShips] = useState([]);
  const [crew, setCrew] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [editingAssignment, setEditingAssignment] = useState(null);
  const [error, setError] = useState('');
  const [form, setForm] = useState({
    shipId: '', crewId: '', assignmentDate: ''
  });

  useEffect(() => { fetchAll(); }, []);

  const fetchAll = async () => {
    try {
      const [assignRes, shipsRes, crewRes] = await Promise.all([
        api.get('/shipcrewassignments'),
        api.get('/ships'),
        api.get('/crewmembers')
      ]);
      setAssignments(assignRes.data);
      setShips(shipsRes.data);
      setCrew(crewRes.data);
    } catch {
      setError('Veriler yüklenemedi.');
    } finally {
      setLoading(false);
    }
  };

  const openCreate = () => {
    setForm({ shipId: '', crewId: '', assignmentDate: '' });
    setEditingAssignment(null);
    setError('');
    setShowModal(true);
  };

  const openEdit = (assignment) => {
    setForm({
      shipId: assignment.shipId,
      crewId: assignment.crewId,
      assignmentDate: assignment.assignmentDate?.slice(0, 16)
    });
    setEditingAssignment(assignment);
    setError('');
    setShowModal(true);
  };

  const handleSubmit = async () => {
    try {
      const payload = {
        shipId: parseInt(form.shipId),
        crewId: parseInt(form.crewId),
        assignmentDate: form.assignmentDate
      };
      if (editingAssignment) {
        await api.put(`/shipcrewassignments/${editingAssignment.assignmentId}`, payload);
      } else {
        await api.post('/shipcrewassignments', payload);
      }
      setShowModal(false);
      fetchAll();
    } catch (err) {
      setError(err.response?.data || 'Bir hata oluştu.');
    }
  };

  const handleDelete = async (id) => {
    if (!confirm('Bu atamayı silmek istediğinizden emin misiniz?')) return;
    try {
      await api.delete(`/shipcrewassignments/${id}`);
      fetchAll();
    } catch {
      setError('Silme işlemi başarısız.');
    }
  };

  if (loading) return <div className="loading">Yükleniyor...</div>;

  return (
    <div>
      <div className="page-header">
        <h1>Gemi-Mürettebat Atamaları</h1>
        <button className="btn btn-primary" onClick={openCreate}>+ Yeni Atama</button>
      </div>

      {error && <div className="error-message">{error}</div>}

      <table>
        <thead>
          <tr>
            <th>Gemi</th>
            <th>Mürettebat</th>
            <th>Atama Tarihi</th>
            <th>İşlemler</th>
          </tr>
        </thead>
        <tbody>
          {assignments.map(assignment => (
            <tr key={assignment.assignmentId}>
              <td>{assignment.shipName}</td>
              <td>{assignment.crewFullName}</td>
              <td>{new Date(assignment.assignmentDate).toLocaleString('tr-TR')}</td>
              <td>
                <button className="btn btn-secondary btn-sm" onClick={() => openEdit(assignment)}>Düzenle</button>
                {' '}
                <button className="btn btn-danger btn-sm" onClick={() => handleDelete(assignment.assignmentId)}>Sil</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {showModal && (
        <div className="modal-overlay">
          <div className="modal">
            <h2>{editingAssignment ? 'Atama Düzenle' : 'Yeni Atama'}</h2>
            {error && <div className="error-message">{error}</div>}
            <div className="form-group">
              <label>Gemi</label>
              <select value={form.shipId} onChange={e => setForm({ ...form, shipId: e.target.value })}>
                <option value="">Seçiniz</option>
                {ships.map(s => <option key={s.shipId} value={s.shipId}>{s.name}</option>)}
              </select>
            </div>
            <div className="form-group">
              <label>Mürettebat</label>
              <select value={form.crewId} onChange={e => setForm({ ...form, crewId: e.target.value })}>
                <option value="">Seçiniz</option>
                {crew.map(c => <option key={c.crewId} value={c.crewId}>{c.firstName} {c.lastName}</option>)}
              </select>
            </div>
            <div className="form-group">
              <label>Atama Tarihi</label>
              <input type="datetime-local" value={form.assignmentDate} onChange={e => setForm({ ...form, assignmentDate: e.target.value })} />
            </div>
            <div className="modal-actions">
              <button className="btn btn-secondary" onClick={() => setShowModal(false)}>İptal</button>
              <button className="btn btn-primary" onClick={handleSubmit}>
                {editingAssignment ? 'Güncelle' : 'Kaydet'}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

export default ShipCrewAssignments;