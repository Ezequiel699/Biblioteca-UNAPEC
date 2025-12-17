// src/components/Gestion/Ciencias/Ciencias.jsx
import { FaFlask, FaPlus, FaSearch } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { useEffect, useState } from 'react';
import ItemList from '../../Crud/Item';
import api from '../../../Services/api';
import ConfirmModal from '../../Crud/ConfirmModal';
import './Ciencias.css';

const Ciencias = () => {
  const [ciencias, setCiencias] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filtro, setFiltro] = useState('');
  const [busqueda, setBusqueda] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [idEliminar, setIdEliminar] = useState(null);

  // 🔄 Cargar Ciencias
  const cargarCiencias = () => {
    setLoading(true);

    const params = {
      page: 1,
      pageSize: 100
    };
    
    if (filtro !== '') params.estado = filtro;
    if (busqueda.trim() !== '') params.q = busqueda.trim();

    api.get('/api/ciencias', { params })
      .then(res => { 
        setCiencias(res.data.items || []); 
      })
      .catch(err => {
        console.error("Error cargando ciencias:", err);
        setCiencias([]);
      })
      .finally(() => setLoading(false));
  };

  useEffect(() => {
    setLoading(true);
    const timer = setTimeout(() => {
      cargarCiencias();
    }, 500); // Debounce de 500ms

    return () => clearTimeout(timer);
  }, [filtro, busqueda]);

  // 🟢 Pedir confirmación antes de eliminar
  const pedirConfirmarEliminacion = (id) => {
    setIdEliminar(id);
    setModalOpen(true);
  };

  // 🔴 Confirmar eliminación
  const confirmarEliminar = async () => {
    try {
      await api.delete(`/api/ciencias/${idEliminar}`);
      cargarCiencias();
    } catch (error) {
      console.error("Error eliminando ciencia:", error);
    }

    setModalOpen(false);
  };

  return (
    <div className='ciencias-container'>
      <div className='shared-wrapper'>

        {/* Header */}
        <div className='header-row'>
          <h2 className='header-title'>Ciencias</h2>

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

            <Link to='/ciencias/nuevo' className='btn-add'>
              <FaPlus /> Añadir
            </Link>
          </div>
        </div>

        {/* Lista */}
        <div className='list-container'>
          {loading ? (
            <p style={{ padding: "1rem" }}>Cargando...</p>
          ) : ciencias.length === 0 ? (
            <p style={{ padding: "1rem" }}>No hay ciencias registradas.</p>
          ) : (
            ciencias.map(c => (
              <ItemList
                key={c.id}
                item={c}
                icon={<FaFlask className='list-icon' />}
                recurso='ciencias'
                displayField='descripcion'
                onDelete={() => pedirConfirmarEliminacion(c.id)}
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
        message="¿Realmente deseas eliminar esta ciencia?"
      />
    </div>
  );
};

export default Ciencias;