// src/components/Gestion/Bibliografias/BibliografiasAdd.jsx

import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import api from "../../../Services/api";
import "../Libros/DetalleGenerico.css";

const BibliografiasAdd = () => {
  const navigate = useNavigate();

  const [form, setForm] = useState({
    descripcion: "",
    estado: true,
  });

  const [error, setError] = useState(null);
  const [loadingSave, setLoadingSave] = useState(false);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;

    setForm({
      ...form,
      [name]: type === "checkbox" ? checked : value,
    });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    setLoadingSave(true);

    api.post("/api/tipos-bibliografia", form)
      .then(() => navigate("/tipos-bibliografia"))
      .catch(() => {
        setError("No se pudo registrar el tipo de bibliografía.");
        setLoadingSave(false);
      });
  };

  return (
    <div className="detalle-wrapper">

      <div className="detalle-header">
        <h2 className="detalle-titulo">Añadir Bibliografía</h2>

        <div className="detalle-buttons">
          <Link to="/tipos-bibliografia" className="btn-action btn-back">
            Volver
          </Link>
        </div>
      </div>

      <form className="detalle-box" onSubmit={handleSubmit}>
        <h3 className="detalle-subtitulo">Información</h3>

        <div className="detalle-grid">

          <div>
            <label>
              Descripción:
              <input
                type="text"
                name="descripcion"
                className="input-text"
                value={form.descripcion}
                onChange={handleChange}
                required
              />
            </label>
          </div>

          <div>
            <label>
              Estado:
              <select
                name="estado"
                className="input-text"
                value={form.estado}
                onChange={(e) =>
                  setForm({ ...form, estado: e.target.value === "true" })
                }
              >
                <option value="true">Activo</option>
                <option value="false">Inactivo</option>
              </select>
            </label>
          </div>

        </div>

        {error && <p className="detalle-error">{error}</p>}

        <button
          type="submit"
          className="btn-action btn-edit"
          disabled={loadingSave}
          style={{ marginTop: "1rem" }}
        >
          {loadingSave ? "Guardando..." : "Guardar"}
        </button>
      </form>
    </div>
  );
};

export default BibliografiasAdd;
