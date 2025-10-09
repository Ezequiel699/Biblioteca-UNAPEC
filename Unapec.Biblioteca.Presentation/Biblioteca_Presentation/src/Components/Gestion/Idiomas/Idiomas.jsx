import './Idiomas.css'
import Item from '../../Crud/Item'
import { BibliografiasList } from './BibliografiasList'

const Idiomas = () =>{
    return(
        <div className='idiomas-container'>
            <div className='title'>
                <h2>Idiomas</h2>
            </div>
            <div className='cover-container'>
                {BibliografiasList.map((idioma) =>
            
                <Item item={idioma}></Item>
            )}
            </div>
        </div>
    );
}

export default Idiomas;