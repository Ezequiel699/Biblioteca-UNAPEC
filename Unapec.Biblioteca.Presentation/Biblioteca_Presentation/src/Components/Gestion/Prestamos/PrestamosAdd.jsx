// src/components/Gestion/Prestamos/PrestamosAdd.jsx
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../../../Services/api';
import '../Libros/DetalleGenerico.css';

const PrestamosAdd = () => {
  const [usuarioId, setUsuarioId] = useState('');
  const [libroId, setLibroId] = useState('');
  const [usuarios, setUsuarios] = useState([]);
  const [libros, setLibros] = useState([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    const cargarDatos = async () => {
      try {
        const [usuariosRes, librosRes] = await Promise.all([
          api.get('/api/usuarios', { params: { page: 1, pageSize: 1000, estado: true } }),
          api.get('/api/libros', { params: { page: 1, pageSize: 1000, estado: true } })
        ]);
        setUsuarios(usuariosRes.data.items || []);
        setLibros(librosRes.data.items || []);
      } catch (err) {
        console.error('Error cargando usuarios o libros:', err);
      } finally {
        setLoading(false);
      }
    };

    cargarDatos();
  }, []);

  const guardar = async (e) => {
    e.preventDefault();
    try {
      await api.post('/api/prestamos', { usuarioId: parseInt(usuarioId), libroId: parseInt(libroId) });
      navigate('/prestamos');
    } catch (err) {
      console.error('Error creando préstamo:', err);
    }
  };

  if (loading) return <p className="detalle-loading">Cargando usuarios y libros…</p>;

  return (
    <div className='detalle-wrapper'>
      <div className='detalle-header'>
        <h2 className='detalle-titulo'>Registrar Préstamo</h2>
        <div className='detalle-buttons'>
          <button className='btn-action btn-back' onClick={() => navigate('/prestamos')}>
            Volver
          </button>
        </div>
      </div>

      <div className='detalle-box'>
        <form onSubmit={guardar}>
          <label>Usuario</label>
          <select
            className="input-text"
            value={usuarioId}
            onChange={(e) => setUsuarioId(e.target.value)}
            required
          >
            <option value="">-- Selecciona un usuario --</option>
            {usuarios.map(u => (
              <option key={u.id} value={u.id}>
                {u.nombre} ({u.noCarnet})
              </option>
            ))}
          </select>

          <label style={{ marginTop: "1rem" }}>Libro</label>
          <select
            className="input-text"
            value={libroId}
            onChange={(e) => setLibroId(e.target.value)}
            required
          >
            <option value="">-- Selecciona un libro --</option>
            {libros.map(l => (
              <option key={l.id} value={l.id}>
                {l.descripcion} {l.isbn ? ` — ISBN: ${l.isbn}` : ''}
              </option>
            ))}
          </select>

          <div style={{ marginTop: "1.5rem", display: "flex", gap: ".75rem" }}>
            <button className='btn-action btn-edit' type="submit">Registrar Préstamo</button>
            <button className='btn-action btn-back' type="button" onClick={() => navigate('/prestamos')}>
              Cancelar
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default PrestamosAdd;