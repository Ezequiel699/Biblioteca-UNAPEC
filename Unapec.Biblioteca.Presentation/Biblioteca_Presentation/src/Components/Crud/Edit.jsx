// src/components/Crud/Editar.jsx
import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import api from '../../Services/api';
import ConfirmModal from './ConfirmModal';
import './Edit.css';

const Editar = () => {
  const { recurso, id } = useParams();
  const navigate = useNavigate();

  const [form, setForm] = useState({ descripcion: '', estado: true });
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);

  useEffect(() => {
    api.get(`/api/${recurso}/${id}`)
      .then(res => {
        setForm(res.data);
        setLoading(false);
      })
      .catch(() => setLoading(false));
  }, [recurso, id]);

  const handleChange = e => {
    const { name, value, type, checked } = e.target;
    setForm(prev => ({ ...prev, [name]: type === 'checkbox' ? checked : value }));
  };

  const handleSubmit = e => {
    e.preventDefault();
    api.put(`/api/${recurso}/${id}`, form)
      .then(() => navigate(`/${recurso}`))
      .catch(console.error);
  };

  const handleDelete = () => setShowModal(true);

  const confirmDelete = () => {
    api.delete(`/api/${recurso}/${id}`)
      .then(() => navigate(`/${recurso}`))
      .catch(console.error);
  };

  if (loading) return <p>Cargando…</p>;

  return (
    <>
      <form onSubmit={handleSubmit} className='editarForm'>
        <h2>Editar {recurso.slice(0, -1)}</h2>

        <div className='field'>
          <label>Descripción</label>
          <input name='descripcion' type='text' value={form.descripcion} onChange={handleChange} required />
        </div>

        <div className='field'>
          <label>
            <input name='estado' type='checkbox' checked={form.estado} onChange={handleChange} />
            Activo
          </label>
        </div>

        <div className='actions'>
          <button type='submit' className='submitBtn'>Guardar</button>
          <button type='button' className='deleteBtn' onClick={handleDelete}>Eliminar</button>
        </div>
      </form>

      <ConfirmModal
        show={showModal}
        title='¿Confirmar eliminación?'
        onConfirm={confirmDelete}
        onCancel={() => setShowModal(false)}
      >
        Se borrará definitivamente el registro.
      </ConfirmModal>
    </>
  );
};

export default Editar;