import { useState, useEffect } from 'react'
import { useAuth } from '../context/AuthContext'
import { getAllUsers, updateUser, updateUserRole, deleteUser } from '../api/accountApi'

function UsersPage() {
  const { token } = useAuth()
  const [users, setUsers] = useState([])
  const [error, setError] = useState('')
  const [editUser, setEditUser] = useState(null)
  const [loading, setLoading] = useState(true)

  const fetchUsers = async () => {
    try {
      const response = await getAllUsers(token)
      setUsers(response.data)
    } catch (err) {
      setError('Greška pri učitavanju korisnika.')
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => { fetchUsers() }, [])

  const handleDelete = async (id) => {
    if (!window.confirm('Da li ste sigurni da želite obrisati korisnika?')) return
    try {
      await deleteUser(id, token)
      setUsers(users.filter(u => u.idUser !== id))
    } catch (err) {
      setError('Greška pri brisanju korisnika.')
    }
  }

  const handleUpdate = async () => {
    try {
      await updateUser(editUser.idUser, {
        username: editUser.username,
        email: editUser.email,
        phoneNumber: editUser.phoneNumber,
        address: editUser.address
      }, token)
      await updateUserRole(editUser.idUser, editUser.role, token)
      setEditUser(null)
      fetchUsers()
    } catch (err) {
      setError('Greška pri izmeni korisnika.')
    }
  }

  const roleLabels = {
    0: { label: 'Customer', color: '#3498db', bg: '#001a2c' },
    1: { label: 'Admin', color: '#c9a84c', bg: '#2c1a00' }
  }

  if (loading) return (
    <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: '60vh' }}>
      <div style={{ color: '#c9a84c', fontFamily: 'Georgia, serif', fontSize: '1.2rem' }}>Učitavanje...</div>
    </div>
  )

  return (
    <div style={{ paddingBottom: '60px' }}>

      {/* Header */}
      <div style={{
        textAlign: 'center',
        padding: '48px 0 32px',
        borderBottom: '2px solid #c9a84c',
        marginBottom: '40px'
      }}>
        <p style={{ color: '#c9a84c', letterSpacing: '4px', fontSize: '0.85rem', marginBottom: '12px' }}>UPRAVLJANJE</p>
        <h1 style={{ color: '#1a1a1a', fontFamily: 'Georgia, serif', fontSize: '2.8rem', marginBottom: '12px' }}>Korisnici</h1>
        <p style={{ color: '#6b6457', fontSize: '1rem' }}>Pregled i upravljanje korisničkim nalozima</p>
      </div>

      {error && (
        <div style={{
          backgroundColor: '#3a1a1a', border: '1px solid #c0392b',
          color: '#e74c3c', padding: '12px 16px', borderRadius: '6px', marginBottom: '24px'
        }}>
          {error}
        </div>
      )}

      {/* Edit forma */}
      {editUser && (
        <div style={{ ...cardStyle, borderColor: '#e8c96d', marginBottom: '40px' }}>
          <h5 style={cardTitleStyle}>Izmeni korisnika</h5>
          <div className="row g-3">
            <div className="col-md-6">
              <label style={labelStyle}>Korisničko ime</label>
              <input style={inputStyle} value={editUser.username}
                onFocus={e => e.target.style.borderColor = '#c9a84c'}
                onBlur={e => e.target.style.borderColor = '#444'}
                onChange={(e) => setEditUser({ ...editUser, username: e.target.value })} />
            </div>
            <div className="col-md-6">
              <label style={labelStyle}>Email</label>
              <input style={inputStyle} value={editUser.email}
                onFocus={e => e.target.style.borderColor = '#c9a84c'}
                onBlur={e => e.target.style.borderColor = '#444'}
                onChange={(e) => setEditUser({ ...editUser, email: e.target.value })} />
            </div>
            <div className="col-md-6">
              <label style={labelStyle}>Telefon</label>
              <input style={inputStyle} value={editUser.phoneNumber || ''}
                onFocus={e => e.target.style.borderColor = '#c9a84c'}
                onBlur={e => e.target.style.borderColor = '#444'}
                onChange={(e) => setEditUser({ ...editUser, phoneNumber: e.target.value })} />
            </div>
            <div className="col-md-6">
              <label style={labelStyle}>Rola</label>
              <select style={inputStyle} value={editUser.role}
                onChange={(e) => setEditUser({ ...editUser, role: parseInt(e.target.value) })}>
                <option value={0}>Customer</option>
                <option value={1}>Admin</option>
              </select>
            </div>
            <div className="col-md-3">
              <label style={labelStyle}>Ulica</label>
              <input style={inputStyle} value={editUser.address?.street || ''}
                onFocus={e => e.target.style.borderColor = '#c9a84c'}
                onBlur={e => e.target.style.borderColor = '#444'}
                onChange={(e) => setEditUser({ ...editUser, address: { ...editUser.address, street: e.target.value } })} />
            </div>
            <div className="col-md-3">
              <label style={labelStyle}>Broj</label>
              <input type="number" style={inputStyle} value={editUser.address?.streetNumber || ''}
                onFocus={e => e.target.style.borderColor = '#c9a84c'}
                onBlur={e => e.target.style.borderColor = '#444'}
                onChange={(e) => setEditUser({ ...editUser, address: { ...editUser.address, streetNumber: e.target.value } })} />
            </div>
            <div className="col-md-3">
              <label style={labelStyle}>Poštanski broj</label>
              <input style={inputStyle} value={editUser.address?.postalCode || ''}
                onFocus={e => e.target.style.borderColor = '#c9a84c'}
                onBlur={e => e.target.style.borderColor = '#444'}
                onChange={(e) => setEditUser({ ...editUser, address: { ...editUser.address, postalCode: e.target.value } })} />
            </div>
            <div className="col-md-3">
              <label style={labelStyle}>Država</label>
              <input style={inputStyle} value={editUser.address?.country || ''}
                onFocus={e => e.target.style.borderColor = '#c9a84c'}
                onBlur={e => e.target.style.borderColor = '#444'}
                onChange={(e) => setEditUser({ ...editUser, address: { ...editUser.address, country: e.target.value } })} />
            </div>
          </div>
          <div style={{ marginTop: '24px', display: 'flex', gap: '12px' }}>
            <button style={submitButtonStyle} onClick={handleUpdate}
              onMouseEnter={e => e.target.style.backgroundColor = '#e8c96d'}
              onMouseLeave={e => e.target.style.backgroundColor = '#c9a84c'}>
              Sačuvaj
            </button>
            <button onClick={() => setEditUser(null)} style={cancelButtonStyle}
              onMouseEnter={e => { e.target.style.backgroundColor = '#c9a84c'; e.target.style.color = '#1a1a1a' }}
              onMouseLeave={e => { e.target.style.backgroundColor = 'transparent'; e.target.style.color = '#c9a84c' }}>
              Otkaži
            </button>
          </div>
        </div>
      )}

      {/* Users kartice */}
      <div className="row g-4">
        {users.map(user => (
          <div key={user.idUser} className="col-md-4">
            <div style={{
              backgroundColor: '#1a1a1a',
              border: '1px solid #c9a84c',
              borderRadius: '12px',
              overflow: 'hidden'
            }}>
              {/* Card Header */}
              <div style={{
                backgroundColor: '#2c2c2c',
                padding: '16px 24px',
                borderBottom: '1px solid #c9a84c',
                display: 'flex',
                justifyContent: 'space-between',
                alignItems: 'center'
              }}>
                <span style={{ color: '#f5f0e8', fontFamily: 'Georgia, serif', fontWeight: 'bold', fontSize: '1rem' }}>
                  {user.username}
                </span>
                <span style={{
                  backgroundColor: roleLabels[user.role]?.bg,
                  color: roleLabels[user.role]?.color,
                  padding: '3px 12px',
                  borderRadius: '20px',
                  fontSize: '0.75rem',
                  fontFamily: 'Georgia, serif'
                }}>
                  {roleLabels[user.role]?.label}
                </span>
              </div>

              {/* Card Body */}
              <div style={{ padding: '20px 24px' }}>
                <div style={{ marginBottom: '10px' }}>
                  <p style={{ color: '#6b6457', fontSize: '0.75rem', letterSpacing: '1px', margin: '0 0 3px' }}>EMAIL</p>
                  <p style={{ color: '#f5f0e8', fontSize: '0.9rem', margin: 0 }}>{user.email}</p>
                </div>
                <div style={{ marginBottom: '10px' }}>
                  <p style={{ color: '#6b6457', fontSize: '0.75rem', letterSpacing: '1px', margin: '0 0 3px' }}>TELEFON</p>
                  <p style={{ color: '#f5f0e8', fontSize: '0.9rem', margin: 0 }}>{user.phoneNumber || '-'}</p>
                </div>
                <div>
                  <p style={{ color: '#6b6457', fontSize: '0.75rem', letterSpacing: '1px', margin: '0 0 3px' }}>ADRESA</p>
                  <p style={{ color: '#f5f0e8', fontSize: '0.9rem', margin: 0 }}>
                    {user.address ? `${user.address.street} ${user.address.streetNumber}, ${user.address.country}` : '-'}
                  </p>
                </div>
              </div>

              {/* Card Footer */}
              <div style={{
                padding: '14px 24px',
                borderTop: '1px solid #2c2c2c',
                display: 'flex',
                gap: '10px',
                justifyContent: 'flex-end'
              }}>
                <button onClick={() => setEditUser({ ...user })} style={smallButtonStyle}
                  onMouseEnter={e => { e.target.style.backgroundColor = '#c9a84c'; e.target.style.color = '#1a1a1a' }}
                  onMouseLeave={e => { e.target.style.backgroundColor = 'transparent'; e.target.style.color = '#c9a84c' }}>
                  Izmeni
                </button>
                <button onClick={() => handleDelete(user.idUser)}
                  style={{ ...smallButtonStyle, borderColor: '#e74c3c', color: '#e74c3c' }}
                  onMouseEnter={e => { e.target.style.backgroundColor = '#e74c3c'; e.target.style.color = '#fff' }}
                  onMouseLeave={e => { e.target.style.backgroundColor = 'transparent'; e.target.style.color = '#e74c3c' }}>
                  Obriši
                </button>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  )
}

const cardStyle = {
  backgroundColor: '#1a1a1a',
  border: '1px solid #c9a84c',
  borderRadius: '12px',
  padding: '32px'
}

const cardTitleStyle = {
  color: '#c9a84c',
  fontFamily: 'Georgia, serif',
  fontSize: '1.2rem',
  marginBottom: '24px',
  paddingBottom: '12px',
  borderBottom: '1px solid #2c2c2c'
}

const labelStyle = {
  display: 'block',
  color: '#c9a84c',
  fontFamily: 'Georgia, serif',
  marginBottom: '8px',
  fontSize: '0.9rem',
  letterSpacing: '0.5px'
}

const inputStyle = {
  width: '100%',
  backgroundColor: '#2c2c2c',
  border: '1px solid #444',
  borderRadius: '6px',
  padding: '10px 14px',
  color: '#f5f0e8',
  fontSize: '0.95rem',
  outline: 'none',
  boxSizing: 'border-box'
}

const submitButtonStyle = {
  backgroundColor: '#c9a84c',
  color: '#1a1a1a',
  border: 'none',
  padding: '10px 28px',
  borderRadius: '6px',
  fontFamily: 'Georgia, serif',
  fontSize: '0.95rem',
  fontWeight: 'bold',
  cursor: 'pointer',
  letterSpacing: '0.5px'
}

const cancelButtonStyle = {
  backgroundColor: 'transparent',
  color: '#c9a84c',
  border: '1px solid #c9a84c',
  padding: '10px 24px',
  borderRadius: '6px',
  fontFamily: 'Georgia, serif',
  fontSize: '0.95rem',
  cursor: 'pointer'
}

const smallButtonStyle = {
  backgroundColor: 'transparent',
  border: '1px solid #c9a84c',
  color: '#c9a84c',
  padding: '4px 12px',
  borderRadius: '6px',
  fontSize: '0.8rem',
  cursor: 'pointer',
  fontFamily: 'Georgia, serif'
}

export default UsersPage