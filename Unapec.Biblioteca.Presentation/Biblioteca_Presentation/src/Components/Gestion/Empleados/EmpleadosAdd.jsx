import { useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../../../Services/api";
import "../Libros/DetalleGenerico.css";

const EmpleadosAdd = () => {
  const navigate = useNavigate();

  const [form, setForm] = useState({
    nombre: "",
    cedula: "",
    tandaLabor: "Matutina",
    porcientoComision: "",
    fechaIngreso: "",
    estado: true,
  });

  const [loading, setLoading] = useState(false);

  const handleChange = (e) => {
    const { name, value } = e.target;

    let val = value;

    if (value === "true") val = true;
    if (value === "false") val = false;

    if (name === "porcientoComision") {
      val = value === "" ? "" : Number(value);
    }

    setForm({ ...form, [name]: val });
  };

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

    setLoading(true);
    try {
      await api.post("/api/empleados", form);
      navigate("/empleados");
    } catch (err) {
      console.error(err);
      alert("Error al guardar.");
      setLoading(false);
    }
  };

  return (
    <div className="detalle-wrapper">

      {/* HEADER */}
      <div className="detalle-header">
        <h2 className="detalle-titulo">Agregar Empleado</h2>
        <button className="btn-action btn-back" onClick={() => navigate("/empleados")}>
          Volver
        </button>
      </div>

      <form onSubmit={handleSubmit} className="detalle-box">

        <div className="detalle-grid">

          {/* Nombre */}
          <div>
            <label>Nombre</label>
            <input
              type="text"
              className="input-text"
              name="nombre"
              value={form.nombre}
              onChange={handleChange}
              required
            />
          </div>

          {/* Cédula */}
          <div>
            <label>Cédula</label>
            <input
              type="text"
              className="input-text"
              name="cedula"
              value={form.cedula}
              onChange={handleChange}
              required
            />
          </div>

          {/* Tanda de Labor */}
          <div>
            <label>Tanda de Labor</label>
            <select
              className="input-text"
              name="tandaLabor"
              value={form.tandaLabor}
              onChange={handleChange}
            >
              <option value="Matutina">Matutina</option>
              <option value="Vespertina">Vespertina</option>
              <option value="Nocturna">Nocturna</option>
            </select>
          </div>

          {/* Porcentaje de Comisión */}
          <div>
            <label>% Comisión</label>
            <input
              type="number"
              step="0.01"
              className="input-text"
              name="porcientoComision"
              value={form.porcientoComision}
              onChange={handleChange}
            />
          </div>

          {/* Fecha de Ingreso */}
          <div>
            <label>Fecha de Ingreso</label>
            <input
              type="date"
              className="input-text"
              name="fechaIngreso"
              value={form.fechaIngreso}
              onChange={handleChange}
              required
            />
          </div>

          {/* Estado */}
          <div>
            <label>Estado</label>
            <select
              className="input-text"
              name="estado"
              value={form.estado}
              onChange={handleChange}
            >
              <option value={true}>Activo</option>
              <option value={false}>Inactivo</option>
            </select>
          </div>

        </div>

        {/* Botones */}
        <div style={{ marginTop: "1.5rem", textAlign: "right" }}>
          <button
            type="button"
            className="btn-action btn-back"
            onClick={() => navigate("/empleados")}
            style={{ marginRight: ".75rem" }}
          >
            Cancelar
          </button>

          <button type="submit" className="btn-edit" disabled={loading}>
            {loading ? "Guardando..." : "Guardar"}
          </button>
        </div>

      </form>

    </div>
  );
};

export default EmpleadosAdd;