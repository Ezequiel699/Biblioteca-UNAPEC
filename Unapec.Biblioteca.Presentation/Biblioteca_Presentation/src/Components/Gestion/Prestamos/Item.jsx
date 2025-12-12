import { useNavigate } from 'react-router-dom';
import './Item.css';

const Item = ({ item, icon, recurso, displayField = 'descripcion', onDelete }) => {
  const navigate = useNavigate();

  // Determinar qué mostrar como título
  const getDisplayTitle = () => {
    // Si tiene usuario y libro (préstamos)
    if (item.usuario && item.libro) {
      return `${item.usuario.nombre} - ${item.libro.descripcion}`;
    }
    
    // Si solo tiene usuario
    if (item.usuario) {
      return item.usuario.nombre;
    }
    
    // Si solo tiene libro
    if (item.libro) {
      return item.libro.descripcion;
    }
    
    // Por defecto usar el displayField
    return item[displayField] || 'Sin título';
  };

  return (
    <div className='list-row'>
      {icon}

      <span className='list-title'>{getDisplayTitle()}</span>

      <span className={`list-badge ${item.estado || item.devuelto === false ? 'activo' : 'inactivo'}`}>
        {item.devuelto !== undefined 
          ? (item.devuelto ? 'Devuelto' : 'Pendiente')
          : (item.estado ? 'Activo' : 'Inactivo')
        }
      </span>

      <div className='list-actions'>
        <button
          className='btn-edit'
          onClick={() => navigate(`/${recurso}/editar/${item.id}`)}
        >
          Editar
        </button>

        <button
          className='btn-detail'
          onClick={() => navigate(`/${recurso}/detalle/${item.id}`)}
        >
          Detalles
        </button>

        {onDelete && (
          <button
            className='btn-delete'
            onClick={onDelete}
          >
            Eliminar
          </button>
        )}
      </div>
    </div>
  );
};

export default Item;