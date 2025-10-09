import './Sidebar.css'


const SideBar = () =>{
    return(
        <aside className='sidebar'>
            <div className='sidebar-container'>
                <div className='title-container'>
                    <h2 className='title'>Gestión de Bibliotecas</h2>
                </div>
                <div className='options-container'>
                    <ul className='options-list'>
                        <li className='option'><a href="">Contenido</a></li>
                    </ul>
                    <ul className='options-list'>
                        <li className='option'><a href="">Usuarios</a></li>
                    </ul>
                </div>
            </div>
        </aside>
    );
}
export default SideBar;