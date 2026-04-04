import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

function Navbar() {
  const { token, role, logout } = useAuth()
  const navigate = useNavigate()

  const handleLogout = () => {
    logout()
    navigate('/')
  }

  const handleMouseEnter = (e) => {
    e.target.style.backgroundColor = '#c9a84c'
    e.target.style.color = '#1a1a1a'
  }

  const handleMouseLeave = (e) => {
    e.target.style.backgroundColor = 'transparent'
    e.target.style.color = '#c9a84c'
  }

  return (
    <nav style={{
      backgroundColor: '#1a1a1a',
      borderBottom: '2px solid #c9a84c',
      position: 'sticky',
      top: 0,
      zIndex: 1000
    }}>
      <div className="container d-flex justify-content-between align-items-center" style={{ height: '80px' }}>

        {/* Logo */}
        <Link to="/" style={{
          color: '#c9a84c',
          textDecoration: 'none',
          fontSize: '1.8rem',
          fontWeight: 'bold',
          fontFamily: 'Georgia, serif',
          letterSpacing: '1px'
        }}>
          🍽️ Restaurant
        </Link>

        {/* Links */}
        <div className="d-flex align-items-center gap-3">
          <NavLink to="/" label="Početna" onMouseEnter={handleMouseEnter} onMouseLeave={handleMouseLeave} />
          <NavLink to="/menu" label="Meni" onMouseEnter={handleMouseEnter} onMouseLeave={handleMouseLeave} />
          {token && <NavLink to="/orders" label="Narudžbine" onMouseEnter={handleMouseEnter} onMouseLeave={handleMouseLeave} />}
          {token && <NavLink to="/reservations" label="Rezervacije" onMouseEnter={handleMouseEnter} onMouseLeave={handleMouseLeave} />}
          {token && role === 'ADMIN' && (
            <NavLink to="/users" label="Korisnici" onMouseEnter={handleMouseEnter} onMouseLeave={handleMouseLeave} />
          )}
          {!token && <NavLink to="/login" label="Login" onMouseEnter={handleMouseEnter} onMouseLeave={handleMouseLeave} />}
          {!token && <NavLink to="/register" label="Registracija" onMouseEnter={handleMouseEnter} onMouseLeave={handleMouseLeave} />}
          {token && (
            <button
              onClick={handleLogout}
              onMouseEnter={handleMouseEnter}
              onMouseLeave={handleMouseLeave}
              style={buttonStyle}
            >
              Odjavi se
            </button>
          )}
        </div>
      </div>
    </nav>
  )
}

function NavLink({ to, label, onMouseEnter, onMouseLeave }) {
  return (
    <Link
      to={to}
      onMouseEnter={onMouseEnter}
      onMouseLeave={onMouseLeave}
      style={buttonStyle}
    >
      {label}
    </Link>
  )
}

const buttonStyle = {
  color: '#c9a84c',
  textDecoration: 'none',
  fontSize: '1rem',
  letterSpacing: '1px',
  fontFamily: 'Georgia, serif',
  border: '1px solid #c9a84c',
  padding: '8px 20px',
  borderRadius: '6px',
  backgroundColor: 'transparent',
  cursor: 'pointer',
  transition: 'all 0.2s'
}

export default Navbar