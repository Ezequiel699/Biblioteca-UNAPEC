// src/components/Gestion/Editoras/EditorasEdit.jsx
import { useEffect, useState } from 'react';
import { useNavigate, useParams, Link } from 'react-router-dom';
import api from '../../../Services/api';
import '../Libros/DetalleGenerico.css';

const EditorasEdit = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [descripcion, setDescripcion] = useState('');
  const [estado, setEstado] = useState(true);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    api.get(`/api/editoras/${id}`)
      .then(res => {
        const data = res.data;
        setDescripcion(data.descripcion);
        setEstado(data.estado);
      })
      .catch(err => console.error("Error cargando:", err))
      .finally(() => setLoading(false));
  }, [id]);

  const actualizar = async (e) => {
    e.preventDefault();

    try {
      await api.put(`/api/editoras/${id}`, { descripcion, estado });
      navigate('/editoras');
    } catch (err) {
      console.error("Error actualizando editora:", err);
    }
  };

  if (loading) return <p className="detalle-loading">Cargando datos...</p>;

  return (
    <div className="detalle-wrapper">
      <div className="detalle-header">
        <h2 className="detalle-titulo">Editar Editora</h2>

        <div className="detalle-buttons">
          <Link to="/editoras" className="btn-action btn-back">
            Volver
          </Link>
        </div>
      </div>

      <div className="detalle-box">
        <form onSubmit={actualizar} className="detalle-grid">

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

          <div>
            <label>Estado</label>
            <select
              className="input-text"
              value={estado}
              onChange={(e) => setEstado(e.target.value === "true")}
            >
              <option value="true">Activo</option>
              <option value="false">Inactivo</option>
            </select>
          </div>

          <button type="submit" className="btn-action btn-edit" style={{ marginTop: "1rem" }}>
            Guardar Cambios
          </button>

        </form>
      </div>
    </div>
  );
};

export default EditorasEdit;
