// src/components/Gestion/Editoras/EditorasAdd.jsx
import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import api from '../../../Services/api';
import '../Libros/DetalleGenerico.css';

const EditorasAdd = () => {
  const [descripcion, setDescripcion] = useState('');
  const navigate = useNavigate();

  const guardar = async (e) => {
    e.preventDefault();

    try {
      await api.post('/api/editoras', { descripcion });
      navigate('/editoras');
    } catch (error) {
      console.error("Error guardando editora:", error);
    }
  };

  return (
    <div className="detalle-wrapper">
      <div className="detalle-header">
        <h2 className="detalle-titulo">Nueva Editora</h2>

        <div className="detalle-buttons">
          <Link to="/editoras" className="btn-action btn-back">
            Volver
          </Link>
        </div>
      </div>

      <div className="detalle-box">
        <form onSubmit={guardar} className="detalle-grid">
          <div>
            <label>Descripción</label>
            <input
              type="text"
              className="input-text"
              value={descripcion}
              onChange={(e) => setDescripcion(e.target.value)}
              required
            />
          </div>

          <button type="submit" className="btn-action btn-edit" style={{ marginTop: "1rem" }}>
            Guardar
          </button>
        </form>
      </div>
    </div>
  );
};

export default EditorasAdd;
