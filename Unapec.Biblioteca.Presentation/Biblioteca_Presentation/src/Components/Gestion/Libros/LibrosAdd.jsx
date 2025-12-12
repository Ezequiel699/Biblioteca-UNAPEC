import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../../../Services/api";

const LibrosAdd = () => {
  const navigate = useNavigate();

  const [form, setForm] = useState({
    descripcion: "",
    signaturaTopografica: "",
    isbn: "",
    tipoBibliografiaId: "",
    editoraId: "",
    anioPublicacion: "",
    cienciaId: "",
    idiomaId: "",
    estado: true,
    autorIds: [],
  });

  const [loading, setLoading] = useState(false);

  const [autores, setAutores] = useState([]);
  const [editoras, setEditoras] = useState([]);
  const [tipos, setTipos] = useState([]);
  const [idiomas, setIdiomas] = useState([]);
  const [ciencias, setCiencias] = useState([]);

  useEffect(() => {
    api.get("/api/autores").then((res) => setAutores(res.data.items || []));
    api.get("/api/editoras").then((res) => setEditoras(res.data.items || []));
    api.get("/api/tiposBibliografia").then((res) => setTipos(res.data.items || []));
    api.get("/api/idiomas").then((res) => setIdiomas(res.data.items || []));
    api.get("/api/ciencias").then((res) => setCiencias(res.data.items || []));
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;

    let val = value;

    if (value === "true") val = true;
    if (value === "false") val = false;

    if (["tipoBibliografiaId", "editoraId", "anioPublicacion", "idiomaId", "cienciaId"].includes(name)) {
      val = value === "" ? null : Number(value);
    }

    setForm({ ...form, [name]: val });
  };

  const handleAutoresChange = (e) => {
    const values = Array.from(e.target.selectedOptions).map((o) => Number(o.value));
    setForm({ ...form, autorIds: values });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!form.descripcion.trim()) {
      alert("La descripción es obligatoria.");
      return;
    }

    setLoading(true);
    try {
      await api.post("/api/libros", form);
      navigate("/libros");
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
        <h2 className="detalle-titulo">Agregar Libro</h2>
        <button className="btn-back" onClick={() => navigate("/libros")}>
          Volver
        </button>
      </div>

      <form onSubmit={handleSubmit} className="detalle-box">

        <div className="detalle-grid">

          {/* Descripción */}
          <div>
            <label>Descripción</label>
            <input
              type="text"
              className="input-text"
              name="descripcion"
              value={form.descripcion}
              onChange={handleChange}
            />
          </div>

          {/* Signatura */}
          <div>
            <label>Signatura Topográfica</label>
            <input
              type="text"
              className="input-text"
              name="signaturaTopografica"
              value={form.signaturaTopografica}
              onChange={handleChange}
            />
          </div>

          {/* ISBN */}
          <div>
            <label>ISBN</label>
            <input
              type="text"
              className="input-text"
              name="isbn"
              value={form.isbn}
              onChange={handleChange}
            />
          </div>

          {/* Tipo Bibliografía */}
          <div>
            <label>Tipo de Bibliografía</label>
            <select
              className="input-text"
              name="tipoBibliografiaId"
              value={form.tipoBibliografiaId || ""}
              onChange={handleChange}
            >
              <option value="">Seleccione</option>
              {tipos.map((t) => (
                <option key={t.id} value={t.id}>{t.descripcion}</option>
              ))}
            </select>
          </div>

          {/* Editora */}
          <div>
            <label>Editora</label>
            <select
              className="input-text"
              name="editoraId"
              value={form.editoraId || ""}
              onChange={handleChange}
            >
              <option value="">Seleccione</option>
              {editoras.map((e) => (
                <option key={e.id} value={e.id}>{e.nombre}</option>
              ))}
            </select>
          </div>

          {/* Año Publicación */}
          <div>
            <label>Año de Publicación</label>
            <input
              type="number"
              className="input-text"
              name="anioPublicacion"
              value={form.anioPublicacion || ""}
              onChange={handleChange}
            />
          </div>

          {/* Ciencia */}
          <div>
            <label>Ciencia (Opcional)</label>
            <select
              className="input-text"
              name="cienciaId"
              value={form.cienciaId || ""}
              onChange={handleChange}
            >
              <option value="">Sin ciencia</option>
              {ciencias.map((c) => (
                <option key={c.id} value={c.id}>{c.descripcion}</option>
              ))}
            </select>
          </div>

          {/* Idioma */}
          <div>
            <label>Idioma</label>
            <select
              className="input-text"
              name="idiomaId"
              value={form.idiomaId || ""}
              onChange={handleChange}
            >
              <option value="">Seleccione</option>
              {idiomas.map((i) => (
                <option key={i.id} value={i.id}>{i.descripcion}</option>
              ))}
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

        {/* Autores (fuera del grid porque es multi-select grande) */}
        <div style={{ marginTop: "1rem" }}>
          <label>Autores</label>
          <select
            multiple
            className="input-text"
            name="autorIds"
            onChange={handleAutoresChange}
            style={{ height: "110px" }}
          >
            {autores.map((a) => (
              <option key={a.id} value={a.id}>
                {a.nombre}
              </option>
            ))}
          </select>
        </div>

        {/* Botones */}
        <div style={{ marginTop: "1.5rem", textAlign: "right" }}>
          <button
            type="button"
            className="btn-back"
            onClick={() => navigate("/libros")}
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

export default LibrosAdd;
