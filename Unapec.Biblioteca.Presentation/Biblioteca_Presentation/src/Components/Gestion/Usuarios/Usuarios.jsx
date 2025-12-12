// src/components/Gestion/Usuarios/Usuarios.jsx
import { FaUser, FaPlus } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { useEffect, useState } from 'react';
import ItemList from '../../Crud/Item';
import api from '../../../Services/api';
import ConfirmModal from '../../Crud/ConfirmModal';
import './Usuarios.css';

const Usuarios = () => {
  const [usuarios, setUsuarios] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filtro, setFiltro] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [idEliminar, setIdEliminar] = useState(null);

  const cargarUsuarios = () => {
    setLoading(true);

    const params = filtro === '' ? {} : { estado: filtro };

    api.get('/api/usuarios', { params })
      .then(res => { 
        setUsuarios(res.data.items || res.data || []); 
      })
      .catch((error) => {
        console.error('Error cargando usuarios:', error);
        setUsuarios([]);
      })
      .finally(() => setLoading(false));
  };

  useEffect(() => {
    cargarUsuarios();
  }, [filtro]);

  // 🟢 Abrir modal y guardar el ID
  const pedirConfirmarEliminacion = (id) => {
    setIdEliminar(id);
    setModalOpen(true);
  };

  // 🔴 Confirmar eliminación
  const confirmarEliminar = async () => {
    try {
      await api.delete(`/api/usuarios/${idEliminar}`);
      cargarUsuarios();
    } catch (error) {
      console.error("Error eliminando usuario:", error);
    }

    setModalOpen(false);
  };

  if (loading) return <p>Cargando usuarios…</p>;

  return (
    <div className='usuarios-container'>
      <div className='shared-wrapper'>
        
        {/* Header */}
        <div className='header-row'>
          <h2 className='header-title'>Usuarios</h2>

          <div className='header-controls'>
            <select
              value={filtro}
              onChange={e => setFiltro(e.target.value)}
              className='filter-select'
            >
              <option value=''>Todos</option>
              <option value='true'>Activos</option>
              <option value='false'>Inactivos</option>
            </select>

            <Link to='/usuarios/nuevo' className='btn-add'>
              <FaPlus /> Añadir
            </Link>
          </div>
        </div>

        {/* Lista */}
        <div className='list-container'>
          {usuarios.length === 0 ? (
            <p style={{ padding: "1rem" }}>No hay usuarios registrados.</p>
          ) : (
            usuarios.map(usuario => (
              <ItemList
                key={usuario.id}
                item={usuario}
                icon={<FaUser className='list-icon' />}
                recurso='usuarios'
                displayField='nombre'
                onDelete={() => pedirConfirmarEliminacion(usuario.id)}
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
        message="¿Realmente deseas eliminar este usuario?"
      />
    </div>
  );
};

export default Usuarios;