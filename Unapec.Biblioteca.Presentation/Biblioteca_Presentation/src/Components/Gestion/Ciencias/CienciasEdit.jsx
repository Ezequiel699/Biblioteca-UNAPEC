// src/components/Gestion/Ciencias/CienciasEdit.jsx
import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import api from '../../../Services/api';
import '../Libros/DetalleGenerico.css';

const CienciasEdit = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [descripcion, setDescripcion] = useState('');
  const [estado, setEstado] = useState(true);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    api.get(`/api/ciencias/${id}`)
      .then(res => {
        const c = res.data;
        setDescripcion(c.descripcion);
        setEstado(c.estado);
      })
      .catch(err => console.error("Error:", err))
      .finally(() => setLoading(false));
  }, [id]);

  const guardar = async (e) => {
    e.preventDefault();
    try {
      await api.put(`/api/ciencias/${id}`, { id, descripcion, estado });
      navigate('/ciencias');
    } catch (error) {
      console.error("Error actualizando ciencia:", error);
    }
  };

  if (loading) return <p className="detalle-loading">Cargando…</p>;

  return (
    <div className='detalle-wrapper'>
      <div className='detalle-header'>
        <h2 className='detalle-titulo'>Editar Ciencia</h2>

        <div className='detalle-buttons'>
          <button className='btn-action btn-back' onClick={() => navigate('/ciencias')}>
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
            <button className='btn-action btn-edit' type="submit">
              Guardar Cambios
            </button>

            <button
              className='btn-action btn-back'
              type="button"
              onClick={() => navigate('/ciencias')}
            >
              Cancelar
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default CienciasEdit;
