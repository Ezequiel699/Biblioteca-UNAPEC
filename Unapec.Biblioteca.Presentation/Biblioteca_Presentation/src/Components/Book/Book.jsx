import './Book.css'


const Book = ({book}) =>{
    return(
        <div className='book'>
            <div className='book-cover-container'>
                <img className='book-cover' 
                src={book.cover} 
                alt="Book cover" />
                <div className='book-overlay'>
                    <div className='btn-container'>
                        <button type='button' className='overlay-btn'>Read now</button>
                        <button type='button' className='overlay-btn'>more info</button>
                    </div>
                </div> 
            </div>
            <div className='title-container'>
                <h3>{book.title}</h3>
             </div>
        </div>

    );
}

export default Book;
