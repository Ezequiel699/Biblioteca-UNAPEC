import './Ciencias.css'
import Item from '../../Crud/Item'
import { BibliografiasList } from './BibliografiasList'

const Ciencias = () =>{
    return(
        <div className='ciencias-container'>
            <div className='title'>
                <h2>Ciencias</h2>
            </div>
            <div className='cover-container'>
                {BibliografiasList.map((ciencia) =>
            
                <Item item={ciencia}></Item>
            )}
            </div>
        </div>
    );
}

export default Ciencias;