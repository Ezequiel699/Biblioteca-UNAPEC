import './Item.css'


const Item = ({item}) =>{
    return(
        <div className='item'>
            <div className='item-cover-container'>
                <img className='item-cover' 
                src={item.Image}
                alt="item cover" />
                <div className='item-overlay'>
                    <div className='btn-container'>
                        <a className='overlay-btn' href='https://www.youtube.com/' target='_self' rel='noreferrer'>Details</a>
                        <a className='overlay-btn' href='https://es.wikipedia.org/wiki/Wikipedia:Portada' target='_self'>Edit</a>
                    </div>
                </div>
            </div>
            <div className='title-container'>
                <h3>{item.Name}</h3>
            </div>
        </div>

    );
}

export default Item;
