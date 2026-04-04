import { useState } from 'react'
import { useNavigate, useLocation } from 'react-router-dom'
import { loginUser } from '../api/authApi'
import { useAuth } from '../context/AuthContext'

function LoginPage() {
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)

  const { login } = useAuth()
  const navigate = useNavigate()
  const location = useLocation()
  const from = location.state?.from || '/menu'

  const handleLogin = async (e) => {
    e.preventDefault()
    setError('')
    setLoading(true)

    try {
      const response = await loginUser(username, password)
      const { token, role } = response.data
      login(token, role)
      navigate(from)
    } catch (err) {
      setError('Pogrešno korisničko ime ili lozinka.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div style={{
      minHeight: '80vh',
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'center'
    }}>
      <div style={{
        backgroundColor: '#1a1a1a',
        border: '2px solid #c9a84c',
        borderRadius: '12px',
        padding: '48px',
        width: '100%',
        maxWidth: '440px'
      }}>
        {/* Naslov */}
        <h2 style={{
          color: '#c9a84c',
          fontFamily: 'Georgia, serif',
          textAlign: 'center',
          marginBottom: '8px',
          fontSize: '2rem'
        }}>
          Prijava
        </h2>
        <p style={{ color: '#ede8dc', textAlign: 'center', marginBottom: '32px', fontSize: '0.95rem' }}>
          Dobrodošli nazad!
        </p>

        {error && (
          <div style={{
            backgroundColor: '#3a1a1a',
            border: '1px solid #c0392b',
            color: '#e74c3c',
            padding: '12px 16px',
            borderRadius: '6px',
            marginBottom: '20px',
            fontSize: '0.9rem'
          }}>
            {error}
          </div>
        )}

        <form onSubmit={handleLogin}>
          <div style={{ marginBottom: '20px' }}>
            <label style={labelStyle}>Korisničko ime</label>
            <input
              type="text"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              required
              style={inputStyle}
              onFocus={e => e.target.style.borderColor = '#c9a84c'}
              onBlur={e => e.target.style.borderColor = '#444'}
            />
          </div>
          <div style={{ marginBottom: '28px' }}>
            <label style={labelStyle}>Lozinka</label>
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              style={inputStyle}
              onFocus={e => e.target.style.borderColor = '#c9a84c'}
              onBlur={e => e.target.style.borderColor = '#444'}
            />
          </div>
          <button
            type="submit"
            disabled={loading}
            style={submitButtonStyle}
            onMouseEnter={e => e.target.style.backgroundColor = '#e8c96d'}
            onMouseLeave={e => e.target.style.backgroundColor = '#c9a84c'}
          >
            {loading ? 'Učitavanje...' : 'Prijavi se'}
          </button>
        </form>

        <p style={{ color: '#ede8dc', textAlign: 'center', marginTop: '24px', fontSize: '0.9rem' }}>
          Nemaš nalog?{' '}
          <span
            onClick={() => navigate('/register')}
            style={{ color: '#c9a84c', cursor: 'pointer', textDecoration: 'underline' }}
          >
            Registruj se
          </span>
        </p>
      </div>
    </div>
  )
}

const labelStyle = {
  display: 'block',
  color: '#c9a84c',
  fontFamily: 'Georgia, serif',
  marginBottom: '8px',
  fontSize: '0.95rem',
  letterSpacing: '0.5px'
}

const inputStyle = {
  width: '100%',
  backgroundColor: '#2c2c2c',
  border: '1px solid #444',
  borderRadius: '6px',
  padding: '12px 16px',
  color: '#f5f0e8',
  fontSize: '1rem',
  outline: 'none',
  transition: 'border-color 0.2s',
  boxSizing: 'border-box'
}

const submitButtonStyle = {
  width: '100%',
  backgroundColor: '#c9a84c',
  color: '#1a1a1a',
  border: 'none',
  padding: '14px',
  borderRadius: '6px',
  fontSize: '1rem',
  fontWeight: 'bold',
  fontFamily: 'Georgia, serif',
  cursor: 'pointer',
  letterSpacing: '1px',
  transition: 'background-color 0.2s'
}

export default LoginPage