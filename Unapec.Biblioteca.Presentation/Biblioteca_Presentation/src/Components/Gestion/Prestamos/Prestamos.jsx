// src/components/Gestion/Prestamos/Prestamos.jsx
import { FaBook, FaUser, FaPlus, FaSearch } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { useEffect, useState } from 'react';
import ItemList from '../../Crud/Item';
import api from '../../../Services/api';
import ConfirmModal from '../../Crud/ConfirmModal';
import './Prestamos.css';

const Prestamos = () => {
  const [prestamos, setPrestamos] = useState([]);
  const [loading, setLoading] = useState(true);
  const [busqueda, setBusqueda] = useState(''); // Búsqueda por ID de libro, usuario o carnet
  const [modalOpen, setModalOpen] = useState(false);
  const [idEliminar, setIdEliminar] = useState(null);

  const cargarPrestamos = () => {
    setLoading(true);

    const params = {
      page: 1,
      pageSize: 100,
    };

    // Opcional: si más adelante quieres búsqueda por libro/usuario, podrías usar parámetros
    // Por ahora, el backend solo filtra por libroId o usuarioId si los pasas como query params
    // Pero como no tenemos input para eso, dejamos solo búsqueda genérica (opcional)

    api.get('/api/prestamos', { params })
      .then(res => {
        setPrestamos(res.data.items || []);
      })
      .catch(error => {
        console.error('Error cargando préstamos:', error);
        setPrestamos([]);
      })
      .finally(() => setLoading(false));
  };

  useEffect(() => {
    setLoading(true);
    const timer = setTimeout(() => {
      cargarPrestamos();
    }, 500);

    return () => clearTimeout(timer);
  }, [busqueda]); // Si luego añades más filtros, agrégalos aquí

  const pedirConfirmarEliminacion = (id) => {
    setIdEliminar(id);
    setModalOpen(true);
  };

  const confirmarEliminar = async () => {
    try {
      await api.delete(`/api/prestamos/${idEliminar}`);
      cargarPrestamos();
    } catch (error) {
      console.error('Error eliminando préstamo:', error);
    }
    setModalOpen(false);
  };

  // Función para formatear la visualización de un préstamo
  const getDisplayText = (prestamo) => {
    const libro = prestamo.libro?.descripcion || `Libro ID: ${prestamo.libroId}`;
    const usuario = prestamo.usuario?.nombre || `Usuario ID: ${prestamo.usuarioId}`;
    const estado = prestamo.devuelto ? ' (Devuelto)' : ' (Pendiente)';
    return `${libro} — ${usuario}${estado}`;
  };

  return (
    <div className='prestamos-container'>
      <div className='shared-wrapper'>
        
        {/* Header */}
        <div className='header-row'>
          <h2 className='header-title'>Préstamos</h2>

          <div className='header-controls'>
            <div className='search-box'>
              <FaSearch className='search-icon' />
              <input
                type='text'
                placeholder='Buscar (no implementado aún)...'
                value={busqueda}
                onChange={e => setBusqueda(e.target.value)}
                className='search-input'
                disabled // 🔸 Opcional: deshabilitado si no usas búsqueda real
              />
            </div>

            {/* 🔸 Opcional: podrías añadir filtro por "devuelto/no devuelto" aquí */}

            <Link to='/prestamos/nuevo' className='btn-add'>
              <FaPlus /> Nuevo
            </Link>
          </div>
        </div>

        {/* Lista */}
        <div className='list-container'>
          {loading ? (
            <p style={{ padding: '1rem' }}>Cargando...</p>
          ) : prestamos.length === 0 ? (
            <p style={{ padding: '1rem' }}>No hay préstamos registrados.</p>
          ) : (
            prestamos.map(prestamo => (
              <ItemList
                key={prestamo.id}
                item={prestamo}
                icon={
                  prestamo.devuelto ? (
                    <FaBook className='list-icon devuelto' />
                  ) : (
                    <FaBook className='list-icon pendiente' />
                  )
                }
                recurso='prestamos'
                displayField={getDisplayText(prestamo)} // Pasamos texto ya formateado
                onDelete={() => pedirConfirmarEliminacion(prestamo.id)}
              />
            ))
          )}
        </div>
      </div>

      {/* Modal Confirmación */}
      <ConfirmModal
        open={modalOpen}
        onClose={() => setModalOpen(false)}
        onConfirm={confirmarEliminar}
        message="¿Realmente deseas eliminar este préstamo? Esta acción no se puede deshacer."
      />
    </div>
  );
};

export default Prestamos;