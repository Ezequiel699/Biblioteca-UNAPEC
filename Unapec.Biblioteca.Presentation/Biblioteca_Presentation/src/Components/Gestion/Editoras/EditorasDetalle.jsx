// src/components/Gestion/Editoras/EditorasDetalle.jsx
import { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import api from '../../../Services/api';
import '../Libros/DetalleGenerico.css';

const EditorasDetalle = () => {
  const { id } = useParams();
  const [editora, setEditora] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    api.get(`/api/editoras/${id}`)
      .then(res => setEditora(res.data))
      .catch(err => console.error("Error cargando detalle:", err))
      .finally(() => setLoading(false));
  }, [id]);

  if (loading) return <p className="detalle-loading">Cargando datos...</p>;
  
  if (!editora) return <p className="detalle-error">No se encontró la editora.</p>;

  return (
    <div className="detalle-wrapper">
      {/* Header */}
      <div className="detalle-header">
        <h2 className="detalle-titulo">Detalle de Editora</h2>

        <div className="detalle-buttons">
          <Link to={`/editoras/editar/${editora.id}`} className="btn-action btn-edit">
            Editar
          </Link>
          <Link to="/editoras" className="btn-action btn-back">
            Volver
          </Link>
        </div>
      </div>

      {/* Caja de datos */}
      <div className="detalle-box">
        <h3 className="detalle-subtitulo">Información General</h3>

        <div className="detalle-grid">
          <p><strong>ID:</strong> {editora.id}</p>
          <p><strong>Estado:</strong> {editora.estado ? 'Activo' : 'Inactivo'}</p>
          <p><strong>Descripción:</strong> {editora.descripcion}</p>
          <p><strong>Creado en:</strong> {editora.creadoEn}</p>
        </div>
      </div>
    </div>
  );
};

export default EditorasDetalle;
