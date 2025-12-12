// src/components/Gestion/Editoras/Editoras.jsx
import { FaBuilding, FaPlus, FaSearch } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { useEffect, useState } from 'react';
import ItemList from '../../Crud/Item';
import api from '../../../Services/api';
import ConfirmModal from '../../Crud/ConfirmModal';
import './Editoras.css';

const Editoras = () => {
  const [editoras, setEditoras] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filtro, setFiltro] = useState('');
  const [busqueda, setBusqueda] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [idEliminar, setIdEliminar] = useState(null);

  const cargarEditoras = () => {
    setLoading(true);

    const params = {
      page: 1,
      pageSize: 100
    };
    
    if (filtro !== '') params.estado = filtro;
    if (busqueda.trim() !== '') params.q = busqueda.trim();

    api.get('/api/editoras', { params })
      .then(res => setEditoras(res.data.items || []))
      .catch(() => setEditoras([]))
      .finally(() => setLoading(false));
  };

  useEffect(() => {
    setLoading(true);
    const timer = setTimeout(() => {
      cargarEditoras();
    }, 500); // Debounce de 500ms

    return () => clearTimeout(timer);
  }, [filtro, busqueda]);

  const pedirConfirmarEliminacion = (id) => {
    setIdEliminar(id);
    setModalOpen(true);
  };

  const confirmarEliminar = async () => {
    try {
      await api.delete(`/api/editoras/${idEliminar}`);
      cargarEditoras();
    } catch (error) {
      console.error("Error eliminando editora:", error);
    }
    setModalOpen(false);
  };

  return (
    <div className='editoras-container'>
      <div className='shared-wrapper'>

        <div className='header-row'>
          <h2 className='header-title'>Editoras</h2>

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

            <Link to='/editoras/nuevo' className='btn-add'>
              <FaPlus /> Añadir
            </Link>
          </div>
        </div>

        <div className='list-container'>
          {loading ? (
            <p style={{ padding: "1rem" }}>Cargando...</p>
          ) : editoras.length === 0 ? (
            <p style={{ padding: "1rem" }}>No hay editoras registradas.</p>
          ) : (
            editoras.map(e => (
              <ItemList
                key={e.id}
                item={e}
                icon={<FaBuilding className='list-icon' />}
                recurso='editoras'
                displayField='descripcion'
                onDelete={() => pedirConfirmarEliminacion(e.id)}
              />
            ))
          )}
        </div>
      </div>

      <ConfirmModal
        open={modalOpen}
        onClose={() => setModalOpen(false)}
        onConfirm={confirmarEliminar}
        message="¿Realmente deseas eliminar esta editora?"
      />
    </div>
  );
};

export default Editoras;