// src/components/Gestion/Idiomas/IdiomasDetails.jsx
import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import api from '../../../Services/api';
import '../Libros/DetalleGenerico.css';

const IdiomasDetails = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [idioma, setIdioma] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    api.get(`/api/idiomas/${id}`)
      .then(res => setIdioma(res.data))
      .catch(() => setIdioma(null))
      .finally(() => setLoading(false));
  }, [id]);

  if (loading) return <p className="detalle-loading">Cargando detalles…</p>;
  if (!idioma) return <p className="detalle-error">No se encontró el idioma.</p>;

  return (
    <div className='detalle-wrapper'>
      <div className='detalle-header'>
        <h2 className='detalle-titulo'>Detalles del Idioma</h2>

        <div className='detalle-buttons'>
          <button className='btn-action btn-edit' onClick={() => navigate(`/idiomas/editar/${id}`)}>
            Editar
          </button>
          <button className='btn-action btn-back' onClick={() => navigate('/idiomas')}>
            Volver
          </button>
        </div>
      </div>

      <div className='detalle-box'>
        <div className="detalle-grid">
          <p><strong>ID:</strong> {idioma.id}</p>
          <p><strong>Descripción:</strong> {idioma.descripcion}</p>
          <p><strong>Estado:</strong> {idioma.estado ? "Activo" : "Inactivo"}</p>
          <p><strong>Creado:</strong> {new Date(idioma.creadoEn).toLocaleString()}</p>
          <p><strong>Actualizado:</strong> {idioma.actualizadoEn ? new Date(idioma.actualizadoEn).toLocaleString() : "—"}</p>
        </div>
      </div>
    </div>
  );
};

export default IdiomasDetails;
