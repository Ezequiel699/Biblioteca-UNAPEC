// src/components/Gestion/Empleados/Empleados.jsx
import { FaUserTie, FaPlus } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { useEffect, useState } from 'react';
import ItemList from '../../Crud/Item';
import api from '../../../Services/api';
import ConfirmModal from '../../Crud/ConfirmModal';
import './Empleados.css';

const Empleados = () => {
  const [empleados, setEmpleados] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filtro, setFiltro] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [idEliminar, setIdEliminar] = useState(null);

  const cargarEmpleados = () => {
    setLoading(true);

    const params = filtro === '' ? {} : { estado: filtro };

    api.get('/api/empleados', { params })
      .then(res => { 
        setEmpleados(res.data.items || res.data || []); 
      })
      .catch((error) => {
        console.error('Error cargando empleados:', error);
        setEmpleados([]);
      })
      .finally(() => setLoading(false));
  };

  useEffect(() => {
    cargarEmpleados();
  }, [filtro]);

  // 🟢 Abrir modal y guardar el ID
  const pedirConfirmarEliminacion = (id) => {
    setIdEliminar(id);
    setModalOpen(true);
  };

  // 🔴 Confirmar eliminación
  const confirmarEliminar = async () => {
    try {
      await api.delete(`/api/empleados/${idEliminar}`);
      cargarEmpleados();
    } catch (error) {
      console.error("Error eliminando empleado:", error);
    }

    setModalOpen(false);
  };

  if (loading) return <p>Cargando empleados…</p>;

  return (
    <div className='empleados-container'>
      <div className='shared-wrapper'>
        
        {/* Header */}
        <div className='header-row'>
          <h2 className='header-title'>Empleados</h2>

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

            <Link to='/empleados/nuevo' className='btn-add'>
              <FaPlus /> Añadir
            </Link>
          </div>
        </div>

        {/* Lista */}
        <div className='list-container'>
          {empleados.length === 0 ? (
            <p style={{ padding: "1rem" }}>No hay empleados registrados.</p>
          ) : (
            empleados.map(empleado => (
              <ItemList
                key={empleado.id}
                item={empleado}
                icon={<FaUserTie className='list-icon' />}
                recurso='empleados'
                displayField='nombre'
                onDelete={() => pedirConfirmarEliminacion(empleado.id)}
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
        message="¿Realmente deseas eliminar este empleado?"
      />
    </div>
  );
};

export default Empleados;