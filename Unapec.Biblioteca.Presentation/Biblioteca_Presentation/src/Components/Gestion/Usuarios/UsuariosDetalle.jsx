import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import api from "../../../Services/api";
import "../Libros/DetalleGenerico.css";

const UsuariosDetalle = () => {
  const { id } = useParams();
  const [usuario, setUsuario] = useState(null);
  const [loading, setLoading] = useState(true);

  const cargarDetalle = async () => {
    try {
      const res = await api.get(`/api/usuarios/${id}`);
      setUsuario(res.data);
    } catch (error) {
      console.error("Error cargando detalle del usuario:", error);
    }
    setLoading(false);
  };

  useEffect(() => {
    cargarDetalle();
  }, [id]);

  if (loading) return <p className="detalle-loading">Cargando detalle…</p>;
  if (!usuario) return <p className="detalle-error">No se encontró el usuario.</p>;

  return (
    <div className="detalle-wrapper">
      <div className="detalle-header">
        <h2 className="detalle-titulo">{usuario.nombre}</h2>

        <div className="detalle-buttons">
          <Link to={`/usuarios/editar/${usuario.id}`} className="btn-action btn-edit">
            Editar
          </Link>
          <Link to={`/usuarios`} className="btn-action btn-back">
            Volver
          </Link>
        </div>
      </div>

      <div className="detalle-box">
        <h3 className="detalle-subtitulo">Información general</h3>

        <div className="detalle-grid">
          <div><strong>Cédula:</strong> {usuario.cedula}</div>
          <div><strong>No. Carnet:</strong> {usuario.noCarnet}</div>
          <div><strong>Tipo de Persona:</strong> {usuario.tipoPersona}</div>
          <div><strong>Estado:</strong> {usuario.estado ? "Activo" : "Inactivo"}</div>
          <div><strong>Creado en:</strong> {usuario.creadoEn?.split("T")[0]}</div>
          {usuario.actualizadoEn && (
            <div><strong>Actualizado en:</strong> {usuario.actualizadoEn.split("T")[0]}</div>
          )}
        </div>
      </div>
    </div>
  );
};

export default UsuariosDetalle;