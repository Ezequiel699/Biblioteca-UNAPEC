// src/components/Gestion/Idiomas/Idiomas.jsx
import { FaLanguage, FaPlus, FaSearch } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { useEffect, useState } from 'react';
import ItemList from '../../Crud/Item';
import api from '../../../Services/api';
import ConfirmModal from '../../Crud/ConfirmModal';
import './Idiomas.css';

const Idiomas = () => {
  const [idiomas, setIdiomas] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filtro, setFiltro] = useState('');
  const [busqueda, setBusqueda] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [idEliminar, setIdEliminar] = useState(null);

  const cargarIdiomas = () => {
    setLoading(true);

    const params = {
      page: 1,
      pageSize: 100
    };
    
    if (filtro !== '') params.estado = filtro;
    if (busqueda.trim() !== '') params.q = busqueda.trim();

    api.get('/api/idiomas', { params })
      .then(res => setIdiomas(res.data.items || []))
      .catch(() => setIdiomas([]))
      .finally(() => setLoading(false));
  };

  useEffect(() => {
    setLoading(true);
    const timer = setTimeout(() => {
      cargarIdiomas();
    }, 500); // Debounce de 500ms

    return () => clearTimeout(timer);
  }, [filtro, busqueda]);

  const pedirConfirmarEliminacion = (id) => {
    setIdEliminar(id);
    setModalOpen(true);
  };

  const confirmarEliminar = async () => {
    try {
      await api.delete(`/api/idiomas/${idEliminar}`);
      cargarIdiomas();
    } catch (e) {
      console.error("Error eliminando idioma:", e);
    }
    setModalOpen(false);
  };

  return (
    <div className='idiomas-container'>
      <div className='shared-wrapper'>

        {/* Header */}
        <div className='header-row'>
          <h2 className='header-title'>Idiomas</h2>

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

            <Link to='/idiomas/nuevo' className='btn-add'>
              <FaPlus /> Añadir
            </Link>
          </div>
        </div>

        {/* Lista */}
        <div className='list-container'>
          {loading ? (
            <p style={{ padding: "1rem" }}>Cargando...</p>
          ) : idiomas.length === 0 ? (
            <p style={{ padding: "1rem" }}>No hay idiomas registrados.</p>
          ) : (
            idiomas.map(i => (
              <ItemList
                key={i.id}
                item={i}
                icon={<FaLanguage className='list-icon' />}
                recurso='idiomas'
                displayField='descripcion'
                onDelete={() => pedirConfirmarEliminacion(i.id)}
              />
            ))
          )}
        </div>
      </div>

      {/* Modal */}
      <ConfirmModal
        open={modalOpen}
        onClose={() => setModalOpen(false)}
        onConfirm={confirmarEliminar}
        message="¿Realmente deseas eliminar este idioma?"
      />
    </div>
  );
};

export default Idiomas;