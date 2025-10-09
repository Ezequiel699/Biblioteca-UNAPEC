import './Editoras.css'
import Item from '../../Crud/Item'
import { BibliografiasList } from './BibliografiasList'

const Editoras = () =>{
    return(
        <div className='editoras-container'>
            <div className='title'>
                <h2>Editoras</h2>
            </div>
            <div className='cover-container'>
                {BibliografiasList.map((editora) =>
            
                <Item item={editora}></Item>
            )}
            </div>
        </div>
    );
}

export default Editoras;