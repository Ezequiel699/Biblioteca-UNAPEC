import { useEffect, useState } from "react";
import { useNavigate, useParams, Link } from "react-router-dom";
import api from "../../../Services/api";
import "../Libros/DetalleGenerico.css";

const EmpleadosEditar = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [loading, setLoading] = useState(true);
  const [enviando, setEnviando] = useState(false);

  const [form, setForm] = useState({
    nombre: "",
    cedula: "",
    tandaLabor: "Matutina",
    porcientoComision: "",
    fechaIngreso: "",
    estado: true,
  });

  // --------------------- Cargar empleado por ID --------------------
  const cargarEmpleado = async () => {
    try {
      const res = await api.get(`/api/empleados/${id}`);
      const data = res.data;

      setForm({
        nombre: data.nombre || "",
        cedula: data.cedula || "",
        tandaLabor: data.tandaLabor || "Matutina",
        porcientoComision: data.porcientoComision || "",
        fechaIngreso: data.fechaIngreso ? data.fechaIngreso.split("T")[0] : "",
        estado: data.estado,
      });
    } catch (error) {
      console.error("Error cargando empleado:", error);
    }

    setLoading(false);
  };

  useEffect(() => {
    cargarEmpleado();
  }, [id]);

  // --------------------- Manejar cambios --------------------
  const handleChange = (e) => {
    const { name, value } = e.target;

    let val = value;

    if (value === "true") val = true;
    if (value === "false") val = false;

    if (name === "porcientoComision") {
      val = value === "" ? "" : Number(value);
    }

    setForm((prev) => ({
      ...prev,
      [name]: val,
    }));
  };

  // --------------------- Enviar edición --------------------
  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!form.nombre.trim()) {
      alert("El nombre es obligatorio.");
      return;
    }

    if (!form.cedula.trim()) {
      alert("La cédula es obligatoria.");
      return;
    }

    if (!form.fechaIngreso) {
      alert("La fecha de ingreso es obligatoria.");
      return;
    }

    setEnviando(true);

    try {
      await api.put(`/api/empleados/${id}`, form);
      navigate("/empleados");
    } catch (error) {
      console.error("Error editando empleado:", error);
      alert("Error al actualizar empleado.");
    }

    setEnviando(false);
  };

  if (loading) return <p className="detalle-loading">Cargando empleado…</p>;

  return (
    <div className="detalle-wrapper">

      <div className="detalle-header">
        <h2 className="detalle-titulo">Editar empleado</h2>

        <div className="detalle-buttons">
          <Link to={`/empleados/${id}`} className="btn-action btn-back">
            Cancelar
          </Link>
        </div>
      </div>

      <form className="detalle-box" onSubmit={handleSubmit}>
        <h3 className="detalle-subtitulo">Información general</h3>

        <div className="detalle-grid">
          <label>
            <strong>Nombre:</strong>
            <input
              type="text"
              name="nombre"
              value={form.nombre}
              onChange={handleChange}
              required
              className="input-text"
            />
          </label>

          <label>
            <strong>Cédula:</strong>
            <input
              type="text"
              name="cedula"
              value={form.cedula}
              onChange={handleChange}
              required
              className="input-text"
            />
          </label>

          <label>
            <strong>Tanda de Labor:</strong>
            <select
              name="tandaLabor"
              value={form.tandaLabor}
              onChange={handleChange}
              className="input-text"
            >
              <option value="Matutina">Matutina</option>
              <option value="Vespertina">Vespertina</option>
              <option value="Nocturna">Nocturna</option>
            </select>
          </label>

          <label>
            <strong>% Comisión:</strong>
            <input
              type="number"
              step="0.01"
              name="porcientoComision"
              value={form.porcientoComision}
              onChange={handleChange}
              className="input-text"
            />
          </label>

          <label>
            <strong>Fecha de Ingreso:</strong>
            <input
              type="date"
              name="fechaIngreso"
              value={form.fechaIngreso}
              onChange={handleChange}
              required
              className="input-text"
            />
          </label>

          <label>
            <strong>Estado:</strong>
            <select
              name="estado"
              value={form.estado}
              onChange={handleChange}
              className="input-text"
            >
              <option value={true}>Activo</option>
              <option value={false}>Inactivo</option>
            </select>
          </label>
        </div>

        <button className="btn-action btn-edit" disabled={enviando}>
          {enviando ? "Guardando…" : "Guardar cambios"}
        </button>
      </form>
    </div>
  );
};

export default EmpleadosEditar;