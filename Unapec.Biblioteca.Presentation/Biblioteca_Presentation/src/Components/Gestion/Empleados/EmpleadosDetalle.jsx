import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import api from "../../../Services/api";
import "../Libros/DetalleGenerico.css";

const EmpleadosDetalle = () => {
  const { id } = useParams();
  const [empleado, setEmpleado] = useState(null);
  const [loading, setLoading] = useState(true);

  const cargarDetalle = async () => {
    try {
      const res = await api.get(`/api/empleados/${id}`);
      setEmpleado(res.data);
    } catch (error) {
      console.error("Error cargando detalle del empleado:", error);
    }
    setLoading(false);
  };

  useEffect(() => {
    cargarDetalle();
  }, [id]);

  if (loading) return <p className="detalle-loading">Cargando detalle…</p>;
  if (!empleado) return <p className="detalle-error">No se encontró el empleado.</p>;

  return (
    <div className="detalle-wrapper">
      <div className="detalle-header">
        <h2 className="detalle-titulo">{empleado.nombre}</h2>

        <div className="detalle-buttons">
          <Link to={`/empleados/editar/${empleado.id}`} className="btn-action btn-edit">
            Editar
          </Link>
          <Link to={`/empleados`} className="btn-action btn-back">
            Volver
          </Link>
        </div>
      </div>

      <div className="detalle-box">
        <h3 className="detalle-subtitulo">Información general</h3>

        <div className="detalle-grid">
          <div><strong>Cédula:</strong> {empleado.cedula}</div>
          <div><strong>Tanda de Labor:</strong> {empleado.tandaLabor}</div>
          <div><strong>% Comisión:</strong> {empleado.porcientoComision}%</div>
          <div><strong>Fecha de Ingreso:</strong> {empleado.fechaIngreso?.split("T")[0]}</div>
          <div><strong>Estado:</strong> {empleado.estado ? "Activo" : "Inactivo"}</div>
        </div>
      </div>
    </div>
  );
};

export default EmpleadosDetalle;