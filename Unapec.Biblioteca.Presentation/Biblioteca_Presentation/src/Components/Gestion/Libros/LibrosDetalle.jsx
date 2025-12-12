import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import api from "../../../Services/api";
import "./DetalleGenerico.css";

const LibrosDetalle = () => {
  const { id } = useParams();
  const [libro, setLibro] = useState(null);
  const [loading, setLoading] = useState(true);

  const cargarDetalle = async () => {
    try {
      const res = await api.get(`/api/libros/${id}`);
      setLibro(res.data);
    } catch (error) {
      console.error("Error cargando detalle del libro:", error);
    }
    setLoading(false);
  };

  useEffect(() => {
    cargarDetalle();
  }, [id]);

  if (loading) return <p className="detalle-loading">Cargando detalle…</p>;
  if (!libro) return <p className="detalle-error">No se encontró el libro.</p>;

  return (
    <div className="detalle-wrapper">
      <div className="detalle-header">
        <h2 className="detalle-titulo">{libro.descripcion}</h2>

        <div className="detalle-buttons">
          <Link to={`/libros/editar/${libro.id}`} className="btn-action btn-edit">
            Editar
          </Link>
          <Link to={`/libros`} className="btn-action btn-back">
            Volver
          </Link>
        </div>
      </div>

      <div className="detalle-box">
        <h3 className="detalle-subtitulo">Información general</h3>

        <div className="detalle-grid">
          <div><strong>Signatura:</strong> {libro.signaturaTopografica}</div>
          <div><strong>ISBN:</strong> {libro.isbn}</div>
          <div><strong>Año publicación:</strong> {libro.anioPublicacion}</div>
          <div><strong>Estado:</strong> {libro.estado ? "Activo" : "Inactivo"}</div>
          <div><strong>Creado en:</strong> {libro.creadoEn?.split("T")[0]}</div>
        </div>
      </div>

      <div className="detalle-box">
        <h3 className="detalle-subtitulo">Relaciones</h3>

        <div className="detalle-grid">
          <div><strong>Tipo bibliografía:</strong> {libro.tipoBibliografia?.descripcion}</div>
          <div><strong>Editora:</strong> {libro.editora?.descripcion}</div>
          <div><strong>Ciencia:</strong> {libro.ciencia?.descripcion}</div>
          <div><strong>Idioma:</strong> {libro.idioma?.descripcion}</div>
        </div>
      </div>

      <div className="detalle-box">
        <h3 className="detalle-subtitulo">Autores</h3>

        {libro.autores?.length === 0 ? (
          <p>No tiene autores registrados.</p>
        ) : (
          <ul className="detalle-lista">
            {libro.autores.map((autor) => (
              <li key={autor.id} className="detalle-list-item">
                <strong>{autor.nombre}</strong> — {autor.paisOrigen}
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  );
};

export default LibrosDetalle;
