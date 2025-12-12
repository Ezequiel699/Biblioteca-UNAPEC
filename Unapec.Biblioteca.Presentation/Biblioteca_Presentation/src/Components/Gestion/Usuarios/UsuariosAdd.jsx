import { useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../../../Services/api";
import "../Libros/DetalleGenerico.css";

const UsuariosAdd = () => {
  const navigate = useNavigate();

  const [form, setForm] = useState({
    nombre: "",
    cedula: "",
    noCarnet: "",
    tipoPersona: "Fisica",
    estado: true,
  });

  const [loading, setLoading] = useState(false);

  const handleChange = (e) => {
    const { name, value } = e.target;

    let val = value;

    if (value === "true") val = true;
    if (value === "false") val = false;

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

    if (!form.noCarnet.trim()) {
      alert("El número de carnet es obligatorio.");
      return;
    }

    setLoading(true);
    try {
      await api.post("/api/usuarios", form);
      navigate("/usuarios");
    } catch (err) {
      console.error(err);
      alert("Error al guardar.");
    }
    setLoading(false);
  };

  return (
    <div className="detalle-wrapper">

      {/* HEADER */}
      <div className="detalle-header">
        <h2 className="detalle-titulo">Agregar Usuario</h2>
        <button className="btn-action btn-back" onClick={() => navigate("/usuarios")}>
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

          {/* No. Carnet */}
          <div>
            <label>No. Carnet</label>
            <input
              type="text"
              className="input-text"
              name="noCarnet"
              value={form.noCarnet}
              onChange={handleChange}
              required
            />
          </div>

          {/* Tipo de Persona */}
          <div>
            <label>Tipo de Persona</label>
            <select
              className="input-text"
              name="tipoPersona"
              value={form.tipoPersona}
              onChange={handleChange}
            >
              <option value="Fisica">Física</option>
              <option value="Juridica">Juridica</option>
            </select>
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
            onClick={() => navigate("/usuarios")}
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

export default UsuariosAdd;