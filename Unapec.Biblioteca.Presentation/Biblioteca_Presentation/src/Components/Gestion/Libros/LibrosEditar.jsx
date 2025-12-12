import { useEffect, useState } from "react";
import { useNavigate, useParams, Link } from "react-router-dom";
import api from "../../../Services/api";
import "./DetalleGenerico.css";

const LibrosEditar = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [loading, setLoading] = useState(true);
  const [enviando, setEnviando] = useState(false);

  const [form, setForm] = useState({
    descripcion: "",
    signaturaTopografica: "",
    isbn: "",
    anioPublicacion: "",
    tipoBibliografiaId: "",
    editoraId: "",
    cienciaId: "",
    idiomaId: "",
    estado: true,
  });

  const [listas, setListas] = useState({
    tipos: [],
    editoras: [],
    ciencias: [],
    idiomas: [],
  });

  // --------------------- Cargar listas necesarias --------------------
  const cargarListas = async () => {
    try {
      const [tipos, editoras, ciencias, idiomas] = await Promise.all([
        api.get("/api/tipo-bibliografia"),
        api.get("/api/editoras"),
        api.get("/api/ciencias"),
        api.get("/api/idiomas"),
      ]);

      setListas({
        tipos: tipos.data.items || [],
        editoras: editoras.data.items || [],
        ciencias: ciencias.data.items || [],
        idiomas: idiomas.data.items || [],
      });
    } catch (error) {
      console.error("Error cargando listas:", error);
    }
  };

  // --------------------- Cargar libro por ID --------------------
  const cargarLibro = async () => {
    try {
      const res = await api.get(`/api/libros/${id}`);
      const data = res.data;

      setForm({
        descripcion: data.descripcion || "",
        signaturaTopografica: data.signaturaTopografica || "",
        isbn: data.isbn || "",
        anioPublicacion: data.anioPublicacion || "",
        tipoBibliografiaId: data.tipoBibliografiaId || "",
        editoraId: data.editoraId || "",
        cienciaId: data.cienciaId || "",
        idiomaId: data.idiomaId || "",
        estado: data.estado,
      });
    } catch (error) {
      console.error("Error cargando libro:", error);
    }

    setLoading(false);
  };

  useEffect(() => {
    cargarListas();
    cargarLibro();
  }, [id]);

  // --------------------- Manejar cambios --------------------
  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  // --------------------- Enviar edición --------------------
  const handleSubmit = async (e) => {
    e.preventDefault();
    setEnviando(true);

    try {
      await api.put(`/api/libros/${id}`, form);
      navigate(`/libros/${id}`);
    } catch (error) {
      console.error("Error editando libro:", error);
    }

    setEnviando(false);
  };

  if (loading) return <p className="detalle-loading">Cargando libro…</p>;

  return (
    <div className="detalle-wrapper">

      <div className="detalle-header">
        <h2 className="detalle-titulo">Editar libro</h2>

        <div className="detalle-buttons">
          <Link to={`/libros/${id}`} className="btn-action btn-back">
            Cancelar
          </Link>
        </div>
      </div>

      <form className="detalle-box" onSubmit={handleSubmit}>
        <h3 className="detalle-subtitulo">Información general</h3>

        <div className="detalle-grid">
          <label>
            <strong>Descripción:</strong>
            <input
              type="text"
              name="descripcion"
              value={form.descripcion}
              onChange={handleChange}
              required
              className="input-text"
            />
          </label>

          <label>
            <strong>Signatura topográfica:</strong>
            <input
              type="text"
              name="signaturaTopografica"
              value={form.signaturaTopografica}
              onChange={handleChange}
              required
              className="input-text"
            />
          </label>

          <label>
            <strong>ISBN:</strong>
            <input
              type="text"
              name="isbn"
              value={form.isbn}
              onChange={handleChange}
              className="input-text"
            />
          </label>

          <label>
            <strong>Año publicación:</strong>
            <input
              type="number"
              name="anioPublicacion"
              value={form.anioPublicacion}
              onChange={handleChange}
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
      </form>

      <form className="detalle-box" onSubmit={handleSubmit}>
        <h3 className="detalle-subtitulo">Relaciones</h3>

        <div className="detalle-grid">
          <label>
            <strong>Tipo bibliografía:</strong>
            <select
              name="tipoBibliografiaId"
              value={form.tipoBibliografiaId}
              onChange={handleChange}
              className="input-text"
              required
            >
              <option value="">Seleccione</option>
              {listas.tipos.map((t) => (
                <option key={t.id} value={t.id}>{t.descripcion}</option>
              ))}
            </select>
          </label>

          <label>
            <strong>Editora:</strong>
            <select
              name="editoraId"
              value={form.editoraId}
              onChange={handleChange}
              className="input-text"
              required
            >
              <option value="">Seleccione</option>
              {listas.editoras.map((e) => (
                <option key={e.id} value={e.id}>{e.descripcion}</option>
              ))}
            </select>
          </label>

          <label>
            <strong>Ciencia:</strong>
            <select
              name="cienciaId"
              value={form.cienciaId}
              onChange={handleChange}
              className="input-text"
              required
            >
              <option value="">Seleccione</option>
              {listas.ciencias.map((c) => (
                <option key={c.id} value={c.id}>{c.descripcion}</option>
              ))}
            </select>
          </label>

          <label>
            <strong>Idioma:</strong>
            <select
              name="idiomaId"
              value={form.idiomaId}
              onChange={handleChange}
              className="input-text"
              required
            >
              <option value="">Seleccione</option>
              {listas.idiomas.map((i) => (
                <option key={i.id} value={i.id}>{i.descripcion}</option>
              ))}
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

export default LibrosEditar;
