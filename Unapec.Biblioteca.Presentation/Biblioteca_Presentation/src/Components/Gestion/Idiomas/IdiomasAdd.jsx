// src/components/Gestion/Idiomas/IdiomasAdd.jsx
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../../../Services/api';
import '../Libros/DetalleGenerico.css';

const IdiomasAdd = () => {
  const [descripcion, setDescripcion] = useState('');
  const [estado, setEstado] = useState(true);
  const navigate = useNavigate();

  const guardar = async (e) => {
    e.preventDefault();
    try {
      await api.post('/api/idiomas', { descripcion, estado });
      navigate('/idiomas');
    } catch (err) {
      console.error("Error creando idioma:", err);
    }
  };

  return (
    <div className='detalle-wrapper'>
      <div className='detalle-header'>
        <h2 className='detalle-titulo'>Registrar Idioma</h2>

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
            <button className='btn-action btn-edit' type="submit">Guardar</button>
            <button className='btn-action btn-back' type="button" onClick={() => navigate('/idiomas')}>
              Cancelar
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default IdiomasAdd;
