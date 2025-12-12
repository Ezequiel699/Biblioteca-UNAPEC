import { useEffect, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import api from '../../../Services/api';
import '../Libros/DetalleGenerico.css';

const AutoresEdit = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [idiomas, setIdiomas] = useState([]);
  const [form, setForm] = useState(null);

  useEffect(() => {
    api.get('/api/idiomas?page=1&pageSize=100&estado=true')
      .then(res => setIdiomas(res.data.items))
      .catch(console.error);

    api.get(`/api/autores/${id}`)
      .then(res => setForm(res.data))
      .catch(console.error);
  }, [id]);

  if (!form) return <p className="detalle-loading">Cargando autor…</p>;

  const handleSubmit = async (e) => {
    e.preventDefault();
    await api.put(`/api/autores/${id}`, form);
    navigate('/autores');
  };

  return (
    <div className="detalle-wrapper">
      
      <div className="detalle-header">
        <h2 className="detalle-titulo">Editar Autor</h2>
        <div className="detalle-buttons">
          <Link className="btn-action btn-back" to="/autores">
            Volver
          </Link>
        </div>
      </div>

      <form className="detalle-box" onSubmit={handleSubmit}>
        <h3 className="detalle-subtitulo">Información del Autor</h3>

        <div className="detalle-grid">

          <label>
            <strong>Nombre:</strong>
            <input
              className="input-text"
              value={form.nombre}
              onChange={e => setForm({ ...form, nombre: e.target.value })}
            />
          </label>

          <label>
            <strong>País de Origen:</strong>
            <input
              className="input-text"
              value={form.paisOrigen}
              onChange={e => setForm({ ...form, paisOrigen: e.target.value })}
            />
          </label>

          <label>
            <strong>Idioma Nativo:</strong>
            <select
              className="input-text"
              value={form.idiomaNativoId}
              onChange={e => setForm({ ...form, idiomaNativoId: Number(e.target.value) })}
            >
              <option value="">Seleccione</option>
              {idiomas.map(i => (
                <option key={i.id} value={i.id}>{i.descripcion}</option>
              ))}
            </select>
          </label>

          <label>
            <strong>Estado:</strong>
            <select
              className="input-text"
              value={form.estado}
              onChange={e => setForm({ ...form, estado: e.target.value === 'true' })}
            >
              <option value="true">Activo</option>
              <option value="false">Inactivo</option>
            </select>
          </label>
        </div>

        <button className="btn-action btn-edit" type="submit">
          Actualizar
        </button>

      </form>
    </div>
  );
};

export default AutoresEdit;
