import { useEffect, useState } from "react";
import { useNavigate, useParams, Link } from "react-router-dom";
import api from "../../../Services/api";
import "../Libros/DetalleGenerico.css";

const UsuariosEditar = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [loading, setLoading] = useState(true);
  const [enviando, setEnviando] = useState(false);

  const [form, setForm] = useState({
    nombre: "",
    cedula: "",
    noCarnet: "",
    tipoPersona: "Fisica",
    estado: true,
  });

  // --------------------- Cargar usuario por ID --------------------
  const cargarUsuario = async () => {
    try {
      const res = await api.get(`/api/usuarios/${id}`);
      const data = res.data;

      setForm({
        nombre: data.nombre || "",
        cedula: data.cedula || "",
        noCarnet: data.noCarnet || "",
        tipoPersona: data.tipoPersona || "Fisica",
        estado: data.estado,
      });
    } catch (error) {
      console.error("Error cargando usuario:", error);
    }

    setLoading(false);
  };

  useEffect(() => {
    cargarUsuario();
  }, [id]);

  // --------------------- Manejar cambios --------------------
  const handleChange = (e) => {
    const { name, value } = e.target;

    let val = value;

    if (value === "true") val = true;
    if (value === "false") val = false;

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

    if (!form.noCarnet.trim()) {
      alert("El número de carnet es obligatorio.");
      return;
    }

    setEnviando(true);

    try {
      await api.put(`/api/usuarios/${id}`, form);
      navigate("/usuarios");
    } catch (error) {
      console.error("Error editando usuario:", error);
      alert("Error al actualizar usuario.");
    }

    setEnviando(false);
  };

  if (loading) return <p className="detalle-loading">Cargando usuario…</p>;

  return (
    <div className="detalle-wrapper">

      <div className="detalle-header">
        <h2 className="detalle-titulo">Editar usuario</h2>

        <div className="detalle-buttons">
          <Link to={`/usuarios/${id}`} className="btn-action btn-back">
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
            <strong>No. Carnet:</strong>
            <input
              type="text"
              name="noCarnet"
              value={form.noCarnet}
              onChange={handleChange}
              required
              className="input-text"
            />
          </label>

          <label>
            <strong>Tipo de Persona:</strong>
            <select
              name="tipoPersona"
              value={form.tipoPersona}
              onChange={handleChange}
              className="input-text"
            >
              <option value="Fisica">Física</option>
              <option value="Juridica">Jurídica</option>
            </select>
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

export default UsuariosEditar;