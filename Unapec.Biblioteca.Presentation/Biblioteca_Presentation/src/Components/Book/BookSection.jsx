import './BookSection.css'
import Book from "./Book";
import { books } from "../../data/books";


const BookSection = ({children = 'Mas libros'}) =>{
    return(
        <div className='book-section-container'>
            <div className='book-section-title'><h3>{children}</h3></div>
            <div className='book-section'>
                {books.map((book) =>   //.map Recorre cada elemento del array
                                        //(elemento, índice, arrayOriginal)
                <Book book={book}></Book>)}
            </div>
        </div>
    );
}

export default BookSection; 