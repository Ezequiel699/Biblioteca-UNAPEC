// src/components/Gestion/Prestamos/PrestamosDetails.jsx
import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import api from '../../../Services/api';
import '../Libros/DetalleGenerico.css';

const PrestamosDetails = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [prestamo, setPrestamo] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    api.get(`/api/prestamos/${id}`)
      .then(res => setPrestamo(res.data))
      .catch(() => setPrestamo(null))
      .finally(() => setLoading(false));
  }, [id]);

  if (loading) return <p className="detalle-loading">Cargando detalles del préstamo…</p>;
  if (!prestamo) return <p className="detalle-error">No se encontró el préstamo.</p>;

  // Formatear fecha con protección contra null
  const formatDate = (dateString) => {
    return dateString ? new Date(dateString).toLocaleString() : "—";
  };

  return (
    <div className='detalle-wrapper'>
      <div className='detalle-header'>
        <h2 className='detalle-titulo'>Detalles del Préstamo</h2>

        <div className='detalle-buttons'>
          {!prestamo.devuelto && (
            <button
              className='btn-action btn-edit'
              onClick={() => navigate(`/prestamos/editar/${id}`)}
            >
              Editar
            </button>
          )}
          <button
            className='btn-action btn-back'
            onClick={() => navigate('/prestamos')}
          >
            Volver
          </button>
        </div>
      </div>

      <div className='detalle-box'>
        <div className="detalle-grid">
          <p><strong>ID:</strong> {prestamo.id}</p>
          <p><strong>Estado:</strong> {prestamo.devuelto ? "Devuelto" : "Pendiente"}</p>
          
          <p><strong>Usuario:</strong> {prestamo.usuario?.nombre || `ID: ${prestamo.usuarioId}`}</p>
          <p><strong>No. Carnet:</strong> {prestamo.usuario?.noCarnet || "—"}</p>
          <p><strong>Cédula:</strong> {prestamo.usuario?.cedula || "—"}</p>

          <p><strong>Libro:</strong> {prestamo.libro?.descripcion || `ID: ${prestamo.libroId}`}</p>
          <p><strong>ISBN:</strong> {prestamo.libro?.isbn || "—"}</p>
          <p><strong>Signatura Topográfica:</strong> {prestamo.libro?.signaturaTopografica || "—"}</p>

          <p><strong>Fecha de Préstamo:</strong> {formatDate(prestamo.fechaPrestamo)}</p>
          <p><strong>Fecha de Devolución:</strong> {formatDate(prestamo.fechaDevolucion)}</p>
          
          <p><strong>Creado:</strong> {formatDate(prestamo.creadoEn)}</p>
          <p><strong>Actualizado:</strong> {formatDate(prestamo.actualizadoEn)}</p>
        </div>
      </div>
    </div>
  );
};

export default PrestamosDetails;