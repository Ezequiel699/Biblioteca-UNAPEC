// src/components/Gestion/Prestamos/PrestamosEdit.jsx
import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import api from '../../../Services/api';
import '../Libros/DetalleGenerico.css';

const PrestamosEdit = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [usuarioId, setUsuarioId] = useState('');
  const [libroId, setLibroId] = useState('');
  const [usuarios, setUsuarios] = useState([]);
  const [libros, setLibros] = useState([]);
  const [loading, setLoading] = useState(true);
  const [prestamoCargado, setPrestamoCargado] = useState(false);

  // Cargar listas y datos del préstamo
  useEffect(() => {
    const cargarTodo = async () => {
      try {
        const [prestamoRes, usuariosRes, librosRes] = await Promise.all([
          api.get(`/api/prestamos/${id}`),
          api.get('/api/usuarios', { params: { page: 1, pageSize: 1000, estado: true } }),
          api.get('/api/libros', { params: { page: 1, pageSize: 1000, estado: true } })
        ]);

        const p = prestamoRes.data;
        if (p.devuelto) {
          alert('No se puede editar un préstamo ya devuelto.');
          navigate('/prestamos');
          return;
        }

        setUsuarioId(p.usuarioId.toString());
        setLibroId(p.libroId.toString());
        setUsuarios(usuariosRes.data.items || []);
        setLibros(librosRes.data.items || []);
        setPrestamoCargado(true);
      } catch (err) {
        console.error('Error cargando datos:', err);
        navigate('/prestamos');
      } finally {
        setLoading(false);
      }
    };

    cargarTodo();
  }, [id, navigate]);

  const guardar = async (e) => {
    e.preventDefault();
    try {
      // Nota: Tu backend no tiene un endpoint PUT para préstamo completo,
      // solo tiene `/devolver/{id}`. Si quieres editar, necesitas implementarlo.
      // Pero por coherencia con el patrón, asumiremos que sí existe.
      await api.put(`/api/prestamos/${id}`, {
        id: parseInt(id),
        usuarioId: parseInt(usuarioId),
        libroId: parseInt(libroId)
        // No enviamos fechas ni devuelto
      });
      navigate('/prestamos');
    } catch (err) {
      console.error('Error actualizando préstamo:', err);
    }
  };

  if (loading) return <p className="detalle-loading">Cargando…</p>;
  if (!prestamoCargado) return null;

  return (
    <div className='detalle-wrapper'>
      <div className='detalle-header'>
        <h2 className='detalle-titulo'>Editar Préstamo</h2>
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
            <button className='btn-action btn-edit' type="submit">Guardar Cambios</button>
            <button className='btn-action btn-back' type="button" onClick={() => navigate('/prestamos')}>
              Cancelar
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default PrestamosEdit;