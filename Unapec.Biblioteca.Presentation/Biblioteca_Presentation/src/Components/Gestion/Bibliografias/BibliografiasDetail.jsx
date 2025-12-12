// src/components/Gestion/Bibliografias/BibliografiasDetail.jsx

import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import api from "../../../Services/api";
import "../Libros/DetalleGenerico.css";

const BibliografiasDetail = () => {
  const { id } = useParams();
  const [biblio, setBiblio] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    api.get(`/api/tipos-bibliografia/${id}`)
      .then(res => {
        setBiblio(res.data);
        setLoading(false);
      })
      .catch(() => {
        setError("No se pudo cargar el tipo de bibliografía.");
        setLoading(false);
      });
  }, [id]);

  if (loading) return <p className="detalle-loading">Cargando detalle…</p>;
  if (error) return <p className="detalle-error">{error}</p>;
  if (!biblio) return <p className="detalle-error">No encontrado.</p>;

  return (
    <div className="detalle-wrapper">

      {/* Encabezado */}
      <div className="detalle-header">
        <h2 className="detalle-titulo">Detalle:</h2>

        <div className="detalle-buttons">
          <Link
            to={`/tipos-bibliografia/editar/${biblio.id}`}
            className="btn-action btn-edit"
          >
            Editar
          </Link>

          <Link
            to="/tipos-bibliografia"
            className="btn-action btn-back"
          >
            Volver
          </Link>
        </div>
      </div>

      {/* Caja principal */}
      <div className="detalle-box">
        <h3 className="detalle-subtitulo">Información general</h3>

        <div className="detalle-grid">
          <div>
            <strong>ID:</strong>
            <p>{biblio.id}</p>
          </div>

          <div>
            <strong>Descripción:</strong>
            <p>{biblio.descripcion}</p>
          </div>

          <div>
            <strong>Estado:</strong>
            <p>{biblio.estado ? "Activo" : "Inactivo"}</p>
          </div>

          <div>
            <strong>Creado en:</strong>
            <p>{new Date(biblio.creadoEn).toLocaleString()}</p>
          </div>

          <div>
            <strong>Actualizado en:</strong>
            <p>
              {biblio.actualizadoEn
                ? new Date(biblio.actualizadoEn).toLocaleString()
                : "—"}
            </p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default BibliografiasDetail;
