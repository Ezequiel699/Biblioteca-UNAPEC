import './Carrusel.css'
import { FaChevronLeft, FaChevronRight} from 'react-icons/fa';
import Book from "../Book/Book";
import { books } from "../../data/books";


const Carrusel = () =>{
    return (
        <div className='carrusel-container'>
            <div className='button-container'>
                <button>
                    <FaChevronLeft></FaChevronLeft>
                </button>
            </div>
            <div className='books-container'>
                <div className='book-section'>
                    {books.map((book) =>   //.map Recorre cada elemento del array
                                            //(elemento, índice, arrayOriginal)
                    <Book book={book}></Book>)}
                </div>
            </div>
            <div className='button-container'>
                <button>
                    <FaChevronRight></FaChevronRight>
                </button>
            </div>
        </div>
    );
}

export default Carrusel;