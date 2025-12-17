// src/components/Gestion/Libros/Libros.jsx
import { FaBookOpen, FaPlus, FaSearch } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { useEffect, useState } from 'react';
import ItemList from '../../Crud/Item';
import api from '../../../Services/api';
import ConfirmModal from '../../Crud/ConfirmModal';
import './Libros.css';

const Libros = () => {
  const [libros, setLibros] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filtro, setFiltro] = useState('');
  const [busqueda, setBusqueda] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [idEliminar, setIdEliminar] = useState(null);

  const cargarLibros = () => {
    setLoading(true);

    const params = {
      page: 1,
      pageSize: 100
    };
    
    if (filtro !== '') params.estado = filtro;
    if (busqueda.trim() !== '') params.q = busqueda.trim();

    api.get('/api/libros', { params })
      .then(res => { 
        setLibros(res.data.items || []); 
      })
      .catch((error) => {
        console.error('Error cargando libros:', error);
        setLibros([]);
      })
      .finally(() => setLoading(false));
  };

  useEffect(() => {
    setLoading(true);
    const timer = setTimeout(() => {
      cargarLibros();
    }, 500); // Debounce de 500ms

    return () => clearTimeout(timer);
  }, [filtro, busqueda]);

  // 🟢 Abrir modal y guardar el ID
  const pedirConfirmarEliminacion = (id) => {
    setIdEliminar(id);
    setModalOpen(true);
  };

  // 🔴 Confirmar eliminación
  const confirmarEliminar = async () => {
    try {
      await api.delete(`/api/libros/${idEliminar}`);
      cargarLibros();
    } catch (error) {
      console.error("Error eliminando libro:", error);
    }

    setModalOpen(false);
  };

  return (
    <div className='libros-container'>
      <div className='shared-wrapper'>
        
        {/* Header */}
        <div className='header-row'>
          <h2 className='header-title'>Libros</h2>

          <div className='header-controls'>
            <div className='search-box'>
              <FaSearch className='search-icon' />
              <input
                type='text'
                placeholder='Buscar...'
                value={busqueda}
                onChange={e => setBusqueda(e.target.value)}
                className='search-input'
              />
            </div>

            <select
              value={filtro}
              onChange={e => setFiltro(e.target.value)}
              className='filter-select'
            >
              <option value=''>Todos</option>
              <option value='true'>Activos</option>
              <option value='false'>Inactivos</option>
            </select>

            <Link to='/libros/nuevo' className='btn-add'>
              <FaPlus /> Añadir
            </Link>
          </div>
        </div>

        {/* Lista */}
        <div className='list-container'>
          {loading ? (
            <p style={{ padding: "1rem" }}>Cargando...</p>
          ) : libros.length === 0 ? (
            <p style={{ padding: "1rem" }}>No hay libros registrados.</p>
          ) : (
            libros.map(libro => (
              <ItemList
                key={libro.id}
                item={libro}
                icon={<FaBookOpen className='list-icon' />}
                recurso='libros'
                displayField='descripcion'
                onDelete={() => pedirConfirmarEliminacion(libro.id)}
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
        message="¿Realmente deseas eliminar este libro?"
      />
    </div>
  );
};

export default Libros;