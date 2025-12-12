// src/components/Gestion/Idiomas/IdiomasEdit.jsx
import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import api from '../../../Services/api';
import '../Libros/DetalleGenerico.css';

const IdiomasEdit = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [descripcion, setDescripcion] = useState('');
  const [estado, setEstado] = useState(true);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    api.get(`/api/idiomas/${id}`)
      .then(res => {
        const d = res.data;
        setDescripcion(d.descripcion);
        setEstado(d.estado);
      })
      .finally(() => setLoading(false));
  }, [id]);

  const guardar = async (e) => {
    e.preventDefault();
    try {
      await api.put(`/api/idiomas/${id}`, { id, descripcion, estado });
      navigate('/idiomas');
    } catch (err) {
      console.error("Error actualizando idioma:", err);
    }
  };

  if (loading) return <p className="detalle-loading">Cargando…</p>;

  return (
    <div className='detalle-wrapper'>
      <div className='detalle-header'>
        <h2 className='detalle-titulo'>Editar Idioma</h2>

        <div className='detalle-buttons'>
          <button className='btn-action btn-back' onClick={() => navigate('/idiomas')}>
            Volver
          </button>
        </div>
      </div>

      <div className='detalle-box'>
        <form onSubmit={guardar}>
          <label>Descripción</label>
          <input
            className="input-text"
            type="text"
            value={descripcion}
            onChange={(e) => setDescripcion(e.target.value)}
            required
          />

          <label style={{ marginTop: "1rem" }}>Estado</label>
          <select
            className="input-text"
            value={estado}
            onChange={e => setEstado(e.target.value === 'true')}
          >
            <option value="true">Activo</option>
            <option value="false">Inactivo</option>
          </select>

          <div style={{ marginTop: "1.5rem", display: "flex", gap: ".75rem" }}>
            <button className='btn-action btn-edit' type="submit">Guardar Cambios</button>
            <button className='btn-action btn-back' type="button" onClick={() => navigate('/idiomas')}>
              Cancelar
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default IdiomasEdit;
