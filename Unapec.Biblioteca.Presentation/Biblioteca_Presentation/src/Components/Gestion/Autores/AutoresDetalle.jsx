// src/components/Gestion/Autores/AutoresDetails.jsx
import { Link, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import api from "../../../Services/api";
import '../Libros/DetalleGenerico.css';

const AutoresDetails = () => {
  const { id } = useParams();
  const [autor, setAutor] = useState(null);

  useEffect(() => {
    api.get(`/api/autores/${id}`)
      .then(res => setAutor(res.data))
      .catch(console.error);
  }, [id]);

  if (!autor) return <p>Cargando detalles…</p>;

  return (
    <div className="detalle-wrapper">

      <div className="detalle-header">
        <h2 className="detalle-titulo">Detalles del Autor</h2>
        <div className="detalle-buttons">
          <Link className="btn-edit" to={`/autores/editar/${id}`}>Editar</Link>
          <Link className="btn-action btn-back" to="/autores">Volver</Link>
        </div>
      </div>

      <div className="detalle-box">

        <h3 className="detalle-subtitulo">Información General</h3>

        <div className="detalle-grid">
          <p><strong>Nombre:</strong> {autor.nombre}</p>
          <p><strong>País de Origen:</strong> {autor.paisOrigen}</p>
          <p><strong>Idioma Nativo:</strong> {autor.idiomaNativo?.descripcion}</p>
          <p><strong>Estado:</strong> {autor.estado ? 'Activo' : 'Inactivo'}</p>
        </div>

      </div>
    </div>
  );
};

export default AutoresDetails;
