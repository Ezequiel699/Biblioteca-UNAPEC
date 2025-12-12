// src/components/Gestion/Bibliografias/BibliografiasEdit.jsx

import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import api from "../../../Services/api";
import "../Libros/DetalleGenerico.css";

const BibliografiasEdit = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [form, setForm] = useState({
    descripcion: "",
    estado: true,
  });

  const [loading, setLoading] = useState(true);
  const [loadingSave, setLoadingSave] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    api.get(`/api/tipos-bibliografia/${id}`)
      .then(res => {
        setForm({
          descripcion: res.data.descripcion,
          estado: res.data.estado,
        });
        setLoading(false);
      })
      .catch(() => {
        setError("No se pudo cargar la información.");
        setLoading(false);
      });
  }, [id]);

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

    api.put(`/api/tipos-bibliografia/${id}`, form)
      .then(() => navigate("/tipos-bibliografia"))
      .catch(() => {
        setError("No se pudo actualizar.");
        setLoadingSave(false);
      });
  };

  if (loading) return <p className="detalle-loading">Cargando…</p>;

  return (
    <div className="detalle-wrapper">

      <div className="detalle-header">
        <h2 className="detalle-titulo">Editar Bibliografía</h2>

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
          {loadingSave ? "Actualizando..." : "Guardar cambios"}
        </button>
      </form>
    </div>
  );
};

export default BibliografiasEdit;
