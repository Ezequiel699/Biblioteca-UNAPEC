import { useNavigate } from 'react-router-dom';
import './Item.css';

const Item = ({ item, icon, recurso, displayField = 'descripcion', onDelete }) => {
  const navigate = useNavigate();

  return (
    <div className='list-row'>
      {icon}

      <span className='list-title'>{item[displayField]}</span>

      <span className={`list-badge ${item.estado ? 'activo' : 'inactivo'}`}>
        {item.estado ? 'Activo' : 'Inactivo'}
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
