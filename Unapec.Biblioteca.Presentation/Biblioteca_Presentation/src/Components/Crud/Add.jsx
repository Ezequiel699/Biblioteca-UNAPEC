// src/components/Crud/Add.jsx
import { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import api from '../../Services/api';

const Add = () => {
  const { recurso } = useParams();          // /:recurso/nuevo
  const navigate  = useNavigate();

  const [form, setForm] = useState({ descripcion: '', estado: true });
  const [saving, setSaving] = useState(false);

  const handleChange = e => {
    const { name, value, type, checked } = e.target;
    setForm(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value,
    }));
  };

  const handleSubmit = e => {
    e.preventDefault();
    setSaving(true);
    api.post(`/api/${recurso}`, form)
      .then(() => navigate(`/${recurso}`))
      .catch(console.error)
      .finally(() => setSaving(false));
  };

  return (
    <form onSubmit={handleSubmit} className="editarForm">
      <h2>Añadir: {recurso}</h2>

      <div className="field">
        <label>Descripción</label>
        <input
          name="descripcion"
          type="text"
          value={form.descripcion}
          onChange={handleChange}
          required
        />
      </div>

      <div className="field">
        <label>
          <input
            name="estado"
            type="checkbox"
            checked={form.estado}
            onChange={handleChange}
          />
          Activo
        </label>
      </div>

      <div className="actions">
        <button type="submit" className="submitBtn" disabled={saving}>
          {saving ? 'Guardando…' : 'Guardar'}
        </button>
        <button type="button" className="deleteBtn" onClick={() => navigate(`/${recurso}`)}>
          Cancelar
        </button>
      </div>
    </form>
  );
};

export default Add;