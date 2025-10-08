import './Bibliografias.css'
import Item from '../../Crud/Item'
import { BibliografiasList } from './BibliografiasList'

const Bibliografias = () =>{
    return(
        <div className='Bibliographics-container'>
            <div className='title'>
                <h2>Biblografías</h2>
            </div>
            <div className='cover-container'>
                {BibliografiasList.map((bibliografia) =>
            
                <Item item={bibliografia}></Item>
            )}
            </div>
        </div>
    );
}

export default Bibliografias;