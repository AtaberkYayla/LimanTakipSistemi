import { useState, useEffect } from 'react';
import api from '../services/api';

function CrewMembers() {
  const [crew, setCrew] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [editingCrew, setEditingCrew] = useState(null);
  const [error, setError] = useState('');
  const [form, setForm] = useState({
    firstName: '', lastName: '', email: '', phoneNumber: '', role: ''
  });

  useEffect(() => { fetchCrew(); }, []);

  const fetchCrew = async () => {
    try {
      const res = await api.get('/crewmembers');
      setCrew(res.data);
    } catch {
      setError('Mürettebat yüklenemedi.');
    } finally {
      setLoading(false);
    }
  };

  const openCreate = () => {
    setForm({ firstName: '', lastName: '', email: '', phoneNumber: '', role: '' });
    setEditingCrew(null);
    setError('');
    setShowModal(true);
  };

  const openEdit = (member) => {
    setForm({
      firstName: member.firstName,
      lastName: member.lastName,
      email: member.email,
      phoneNumber: member.phoneNumber.replace('+90 ', ''),
      role: member.role
    });
    setEditingCrew(member);
    setError('');
    setShowModal(true);
  };

  const handleSubmit = async () => {
  try {
    const payload = {
      ...form,
      phoneNumber: '+90 ' + form.phoneNumber
    };
    if (editingCrew) {
      await api.put(`/crewmembers/${editingCrew.crewId}`, payload);
    } else {
      await api.post('/crewmembers', payload);
    }
    setShowModal(false);
    fetchCrew();
  } catch (err) {
    const data = err.response?.data;
    if (data?.errors) {
      const messages = Object.values(data.errors).flat().join(', ');
      setError(messages);
    } else if (typeof data === 'string') {
      setError(data);
    } else {
      setError('Bir hata oluştu.');
    }
  }
};

  const handleDelete = async (id) => {
    if (!confirm('Bu mürettebatı silmek istediğinizden emin misiniz?')) return;
    try {
      await api.delete(`/crewmembers/${id}`);
      fetchCrew();
    } catch {
      setError('Silme işlemi başarısız.');
    }
  };

  if (loading) return <div className="loading">Yükleniyor...</div>;

  return (
    <div>
      <div className="page-header">
        <h1>Mürettebat Yönetimi</h1>
        <button className="btn btn-primary" onClick={openCreate}>+ Yeni Mürettebat</button>
      </div>

      {error && <div className="error-message">{error}</div>}

      <table>
        <thead>
          <tr>
            <th>Ad</th>
            <th>Soyad</th>
            <th>E-posta</th>
            <th>Telefon</th>
            <th>Görev</th>
            <th>İşlemler</th>
          </tr>
        </thead>
        <tbody>
          {crew.map(member => (
            <tr key={member.crewId}>
              <td>{member.firstName}</td>
              <td>{member.lastName}</td>
              <td>{member.email}</td>
              <td>{member.phoneNumber}</td>
              <td>{member.role}</td>
              <td>
                <button className="btn btn-secondary btn-sm" onClick={() => openEdit(member)}>Düzenle</button>
                {' '}
                <button className="btn btn-danger btn-sm" onClick={() => handleDelete(member.crewId)}>Sil</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {showModal && (
        <div className="modal-overlay">
          <div className="modal">
            <h2>{editingCrew ? 'Mürettebat Düzenle' : 'Yeni Mürettebat'}</h2>
            {error && <div className="error-message">{error}</div>}
            <div className="form-group">
              <label>Ad</label>
              <input value={form.firstName} onChange={e => setForm({ ...form, firstName: e.target.value })} />
            </div>
            <div className="form-group">
              <label>Soyad</label>
              <input value={form.lastName} onChange={e => setForm({ ...form, lastName: e.target.value })} />
            </div>
            <div className="form-group">
              <label>E-posta</label>
              <input type="email" value={form.email} onChange={e => setForm({ ...form, email: e.target.value })} />
            </div>
            <div className="form-group">
                <label>Telefon</label>
                <input
                    value={form.phoneNumber}
                    onChange={e => {
                        const digits = e.target.value.replace(/\D/g, '').slice(0, 10);
                        let formatted = digits;
                        if (digits.length > 3) formatted = digits.slice(0, 3) + ' ' + digits.slice(3);
                        if (digits.length > 6) formatted = digits.slice(0, 3) + ' ' + digits.slice(3, 6) + ' ' + digits.slice(6);
                        if (digits.length > 8) formatted = digits.slice(0, 3) + ' ' + digits.slice(3, 6) + ' ' + digits.slice(6, 8) + ' ' + digits.slice(8);
                        setForm({ ...form, phoneNumber: formatted });
                    }}
                    placeholder="532 123 45 67"
                />
            </div>
            <div className="form-group">
              <label>Görev</label>
              <input value={form.role} onChange={e => setForm({ ...form, role: e.target.value })} />
            </div>
            <div className="modal-actions">
              <button className="btn btn-secondary" onClick={() => setShowModal(false)}>İptal</button>
              <button className="btn btn-primary" onClick={handleSubmit}>
                {editingCrew ? 'Güncelle' : 'Kaydet'}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

export default CrewMembers;