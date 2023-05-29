import {Link} from 'react-router-dom';
import Cat from '../../cat.webp'
import './navigation.css'

const Navigation = () => {
    return (<nav>
        <div className="Nav-panel">
            <Link className="Navbar-item" to="/">
                <img src={Cat} alt={Cat} className="App-logo"/>
            </Link>
            <Link className="Navbar-item" to="/">Главная</Link>
            <Link className="Navbar-item" to="/account">Акаунт</Link>
        </div>
    </nav>);
};

export default Navigation;
