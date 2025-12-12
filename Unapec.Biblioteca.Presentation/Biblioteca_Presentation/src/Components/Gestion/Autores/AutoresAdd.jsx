// src/components/Gestion/Autores/AutoresAdd.jsx
import { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import api from '../../../Services/api';
import '../Libros/DetalleGenerico.css';

const AutoresAdd = () => {
  const navigate = useNavigate();
  const [idiomas, setIdiomas] = useState([]);

  const [form, setForm] = useState({
    nombre: '',
    paisOrigen: '',
    idiomaNativoId: '',
    estado: true
  });

  useEffect(() => {
    api.get('/api/idiomas')
      .then(res => setIdiomas(res.data.items || []))
      .catch(console.error);
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;
    let val = value;

    if (value === "true") val = true;
    if (value === "false") val = false;

    if (name === "idiomaNativoId") {
      val = value === "" ? "" : Number(value);
    }

    setForm({ ...form, [name]: val });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      await api.post('/api/autores', form);
      navigate('/autores');
    } catch (err) {
      console.error(err);
      alert("Error al guardar el autor.");
    }
  };

  return (
    <div className="detalle-wrapper">

      {/* HEADER */}
      <div className="detalle-header">
        <h2 className="detalle-titulo">Nuevo Autor</h2>
        <Link className="btn-action btn-back" to="/autores">Volver</Link>
      </div>

      <form className="detalle-box" onSubmit={handleSubmit}>
        <h3 className="detalle-subtitulo">Información del Autor</h3>

        <div className="detalle-grid">

          <div>
            <label>Nombre</label>
            <input
              className="input-text"
              name="nombre"
              value={form.nombre}
              onChange={handleChange}
              type="text"
            />
          </div>

          <div>
            <label>País de Origen</label>
            <input
              className="input-text"
              name="paisOrigen"
              value={form.paisOrigen}
              onChange={handleChange}
              type="text"
            />
          </div>

          <div>
            <label>Idioma Nativo</label>
            <select
              className="input-text"
              name="idiomaNativoId"
              value={form.idiomaNativoId}
              onChange={handleChange}
            >
              <option value="">Seleccione</option>
              {idiomas.map(i => (
                <option key={i.id} value={i.id}>{i.descripcion}</option>
              ))}
            </select>
          </div>

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

        {/* BOTONES */}
        <div style={{ marginTop: "1.5rem", textAlign: "right" }}>
          <button
            type="button"
            className="btn-action btn-back"
            onClick={() => navigate("/autores")}
            style={{ marginRight: ".75rem" }}
          >
            Cancelar
          </button>

          <button type="submit" className="btn-edit">
            Guardar
          </button>
        </div>

      </form>
    </div>
  );
};

export default AutoresAdd;
