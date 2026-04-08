import { useNavigate } from 'react-router-dom'

function NotFoundPage() {
  const navigate = useNavigate()

  return (
    <div style={{
      minHeight: '70vh',
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'center',
      textAlign: 'center'
    }}>
      <div>
        <p style={{ color: '#c9a84c', letterSpacing: '4px', fontSize: '0.85rem', marginBottom: '16px' }}>
          GREŠKA
        </p>
        <h1 style={{
          color: '#1a1a1a',
          fontFamily: 'Georgia, serif',
          fontSize: '8rem',
          fontWeight: 'bold',
          lineHeight: 1,
          marginBottom: '8px'
        }}>
          404
        </h1>
        <h2 style={{
          color: '#c9a84c',
          fontFamily: 'Georgia, serif',
          fontSize: '1.8rem',
          marginBottom: '16px'
        }}>
          Stranica nije pronađena
        </h2>
        <p style={{ color: '#6b6457', fontSize: '1rem', marginBottom: '40px', maxWidth: '400px' }}>
          Stranica koju tražite ne postoji ili je premeštena.
        </p>
        <div style={{ display: 'flex', gap: '16px', justifyContent: 'center' }}>
          <button onClick={() => navigate('/')} style={{
            backgroundColor: '#c9a84c',
            color: '#1a1a1a',
            border: 'none',
            padding: '12px 32px',
            borderRadius: '6px',
            fontFamily: 'Georgia, serif',
            fontSize: '1rem',
            fontWeight: 'bold',
            cursor: 'pointer',
            letterSpacing: '0.5px'
          }}
            onMouseEnter={e => e.target.style.backgroundColor = '#e8c96d'}
            onMouseLeave={e => e.target.style.backgroundColor = '#c9a84c'}>
            Početna
          </button>
          <button onClick={() => navigate(-1)} style={{
            backgroundColor: 'transparent',
            color: '#c9a84c',
            border: '1px solid #c9a84c',
            padding: '12px 32px',
            borderRadius: '6px',
            fontFamily: 'Georgia, serif',
            fontSize: '1rem',
            cursor: 'pointer'
          }}
            onMouseEnter={e => { e.target.style.backgroundColor = '#c9a84c'; e.target.style.color = '#1a1a1a' }}
            onMouseLeave={e => { e.target.style.backgroundColor = 'transparent'; e.target.style.color = '#c9a84c' }}>
            Nazad
          </button>
        </div>
      </div>
    </div>
  )
}

export default NotFoundPage