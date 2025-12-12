// src/components/Gestion/Ciencias/CienciasDetails.jsx
import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import api from '../../../Services/api';
import '../Libros/DetalleGenerico.css';

const CienciasDetails = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [ciencia, setCiencia] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    api.get(`/api/ciencias/${id}`)
      .then(res => setCiencia(res.data))
      .catch(() => setCiencia(null))
      .finally(() => setLoading(false));
  }, [id]);

  if (loading) return <p className="detalle-loading">Cargando detalles…</p>;
  if (!ciencia) return <p className="detalle-error">No se encontró la ciencia.</p>;

  return (
    <div className='detalle-wrapper'>
      <div className='detalle-header'>
        <h2 className='detalle-titulo'>Detalles de Ciencia</h2>

        <div className='detalle-buttons'>
          <button className='btn-action btn-edit' onClick={() => navigate(`/ciencias/editar/${id}`)}>
            Editar
          </button>

          <button className='btn-action btn-back' onClick={() => navigate('/ciencias')}>
            Volver
          </button>
        </div>
      </div>

      <div className='detalle-box'>
        <div className="detalle-grid">
          <p><strong>ID:</strong> {ciencia.id}</p>
          <p><strong>Descripción:</strong> {ciencia.descripcion}</p>
          <p><strong>Estado:</strong> {ciencia.estado ? "Activo" : "Inactivo"}</p>
          <p><strong>Creado:</strong> {new Date(ciencia.creadoEn).toLocaleString()}</p>
          <p><strong>Actualizado:</strong> {ciencia.actualizadoEn ? new Date(ciencia.actualizadoEn).toLocaleString() : "—"}</p>
        </div>
      </div>
    </div>
  );
};

export default CienciasDetails;
