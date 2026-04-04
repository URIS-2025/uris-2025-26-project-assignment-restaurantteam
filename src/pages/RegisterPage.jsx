import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { registerUser } from '../api/authApi'

function RegisterPage() {
  const [username, setUsername] = useState('')
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const [success, setSuccess] = useState('')
  const [loading, setLoading] = useState(false)

  const navigate = useNavigate()

  const handleRegister = async (e) => {
    e.preventDefault()
    setError('')
    setSuccess('')
    setLoading(true)

    try {
      await registerUser(username, email, password)
      setSuccess('Registracija uspješna! Preusmjeravamo na login...')
      setTimeout(() => navigate('/login'), 2000)
    } catch (err) {
      setError('Greška pri registraciji. Pokušaj ponovo.')
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
          Registracija
        </h2>
        <p style={{ color: '#ede8dc', textAlign: 'center', marginBottom: '32px', fontSize: '0.95rem' }}>
          Kreirajte novi nalog
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

        {success && (
          <div style={{
            backgroundColor: '#1a3a1a',
            border: '1px solid #27ae60',
            color: '#2ecc71',
            padding: '12px 16px',
            borderRadius: '6px',
            marginBottom: '20px',
            fontSize: '0.9rem'
          }}>
            {success}
          </div>
        )}

        <form onSubmit={handleRegister}>
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
          <div style={{ marginBottom: '20px' }}>
            <label style={labelStyle}>Email</label>
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
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
            {loading ? 'Učitavanje...' : 'Registruj se'}
          </button>
        </form>

        <p style={{ color: '#ede8dc', textAlign: 'center', marginTop: '24px', fontSize: '0.9rem' }}>
          Već imaš nalog?{' '}
          <span
            onClick={() => navigate('/login')}
            style={{ color: '#c9a84c', cursor: 'pointer', textDecoration: 'underline' }}
          >
            Prijavi se
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

export default RegisterPage