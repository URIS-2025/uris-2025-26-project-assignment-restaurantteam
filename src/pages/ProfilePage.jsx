import { useState, useEffect } from 'react'
import { useAuth } from '../context/AuthContext'
import { getUserById, updateUser } from '../api/accountApi'

function ProfilePage() {
  const { token, userId, username, role } = useAuth()
  const [user, setUser] = useState(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [success, setSuccess] = useState('')
  const [editing, setEditing] = useState(false)
  const [editData, setEditData] = useState(null)

  console.log('userId iz AuthContext:', userId)
  console.log('username iz AuthContext:', username)

  const fetchUser = async () => {
    try {
      const response = await getUserById(userId, token)
      setUser(response.data)
      setEditData(response.data)
    } catch (err) {
      setError('Greška pri učitavanju profila.')
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => { fetchUser() }, [])

  const handleUpdate = async (e) => {
    e.preventDefault()
    setError('')
    setSuccess('')
    try {
      await updateUser(userId, {
        username: editData.username,
        email: editData.email,
        phoneNumber: editData.phoneNumber,
        address: editData.address
      }, token)
      setSuccess('Profil uspješno ažuriran!')
      setEditing(false)
      fetchUser()
    } catch (err) {
      setError('Greška pri ažuriranju profila.')
    }
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
        <p style={{ color: '#c9a84c', letterSpacing: '4px', fontSize: '0.85rem', marginBottom: '12px' }}>MOJ NALOG</p>
        <h1 style={{ color: '#1a1a1a', fontFamily: 'Georgia, serif', fontSize: '2.8rem', marginBottom: '12px' }}>
          Profil
        </h1>
        <p style={{ color: '#6b6457', fontSize: '1rem' }}>Upravljajte svojim nalogom</p>
      </div>

      {error && (
        <div style={{
          backgroundColor: '#3a1a1a', border: '1px solid #c0392b',
          color: '#e74c3c', padding: '12px 16px', borderRadius: '6px', marginBottom: '24px'
        }}>
          {error}
        </div>
      )}

      {success && (
        <div style={{
          backgroundColor: '#1a3a1a', border: '1px solid #27ae60',
          color: '#2ecc71', padding: '12px 16px', borderRadius: '6px', marginBottom: '24px'
        }}>
          {success}
        </div>
      )}

      <div className="row justify-content-center">
        <div className="col-md-8">

          {/* Avatar i osnovne info */}
          <div style={{
            backgroundColor: '#1a1a1a',
            border: '1px solid #c9a84c',
            borderRadius: '12px',
            padding: '32px',
            marginBottom: '24px',
            display: 'flex',
            alignItems: 'center',
            gap: '24px'
          }}>
            <div style={{
              width: '80px', height: '80px',
              backgroundColor: '#2c2c2c',
              border: '2px solid #c9a84c',
              borderRadius: '50%',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              fontSize: '2rem',
              flexShrink: 0
            }}>
              👤
            </div>
            <div>
              <h3 style={{ color: '#f5f0e8', fontFamily: 'Georgia, serif', margin: '0 0 8px' }}>
                {user?.username}
              </h3>
              <span style={{
                backgroundColor: role === 'ADMIN' ? '#2c1a00' : '#001a2c',
                color: role === 'ADMIN' ? '#c9a84c' : '#3498db',
                padding: '3px 14px',
                borderRadius: '20px',
                fontSize: '0.8rem',
                fontFamily: 'Georgia, serif'
              }}>
                {role === 'ADMIN' ? 'Admin' : 'Customer'}
              </span>
            </div>
          </div>

          {/* Podaci */}
          {!editing ? (
            <div style={{
              backgroundColor: '#1a1a1a',
              border: '1px solid #c9a84c',
              borderRadius: '12px',
              padding: '32px'
            }}>
              <div style={{
                display: 'flex',
                justifyContent: 'space-between',
                alignItems: 'center',
                marginBottom: '24px',
                paddingBottom: '12px',
                borderBottom: '1px solid #2c2c2c'
              }}>
                <h5 style={{ color: '#c9a84c', fontFamily: 'Georgia, serif', margin: 0 }}>Lični podaci</h5>
                <button onClick={() => setEditing(true)} style={{
                  backgroundColor: 'transparent',
                  color: '#c9a84c',
                  border: '1px solid #c9a84c',
                  padding: '6px 20px',
                  borderRadius: '6px',
                  fontFamily: 'Georgia, serif',
                  fontSize: '0.9rem',
                  cursor: 'pointer'
                }}
                  onMouseEnter={e => { e.target.style.backgroundColor = '#c9a84c'; e.target.style.color = '#1a1a1a' }}
                  onMouseLeave={e => { e.target.style.backgroundColor = 'transparent'; e.target.style.color = '#c9a84c' }}>
                  Izmeni
                </button>
              </div>

              <div className="row g-4">
                {[
                  { label: 'KORISNIČKO IME', value: user?.username },
                  { label: 'EMAIL', value: user?.email },
                  { label: 'TELEFON', value: user?.phoneNumber || '-' },
                  { label: 'ADRESA', value: user?.address ? `${user.address.street} ${user.address.streetNumber}, ${user.address.postalCode}, ${user.address.country}` : '-' }
                ].map((item, i) => (
                  <div key={i} className="col-md-6">
                    <p style={{ color: '#6b6457', fontSize: '0.75rem', letterSpacing: '1px', margin: '0 0 4px' }}>{item.label}</p>
                    <p style={{ color: '#f5f0e8', fontFamily: 'Georgia, serif', margin: 0 }}>{item.value}</p>
                  </div>
                ))}
              </div>
            </div>
          ) : (
            <div style={{
              backgroundColor: '#1a1a1a',
              border: '1px solid #e8c96d',
              borderRadius: '12px',
              padding: '32px'
            }}>
              <h5 style={{ color: '#c9a84c', fontFamily: 'Georgia, serif', marginBottom: '24px', paddingBottom: '12px', borderBottom: '1px solid #2c2c2c' }}>
                Izmeni podatke
              </h5>
              <form onSubmit={handleUpdate}>
                <div className="row g-3">
                  <div className="col-md-6">
                    <label style={labelStyle}>Korisničko ime</label>
                    <input style={inputStyle} value={editData?.username || ''}
                      onFocus={e => e.target.style.borderColor = '#c9a84c'}
                      onBlur={e => e.target.style.borderColor = '#444'}
                      onChange={(e) => setEditData({ ...editData, username: e.target.value })} />
                  </div>
                  <div className="col-md-6">
                    <label style={labelStyle}>Email</label>
                    <input style={inputStyle} value={editData?.email || ''}
                      onFocus={e => e.target.style.borderColor = '#c9a84c'}
                      onBlur={e => e.target.style.borderColor = '#444'}
                      onChange={(e) => setEditData({ ...editData, email: e.target.value })} />
                  </div>
                  <div className="col-md-6">
                    <label style={labelStyle}>Telefon</label>
                    <input style={inputStyle} value={editData?.phoneNumber || ''}
                      onFocus={e => e.target.style.borderColor = '#c9a84c'}
                      onBlur={e => e.target.style.borderColor = '#444'}
                      onChange={(e) => setEditData({ ...editData, phoneNumber: e.target.value })} />
                  </div>
                  <div className="col-md-3">
                    <label style={labelStyle}>Ulica</label>
                    <input style={inputStyle} value={editData?.address?.street || ''}
                      onFocus={e => e.target.style.borderColor = '#c9a84c'}
                      onBlur={e => e.target.style.borderColor = '#444'}
                      onChange={(e) => setEditData({ ...editData, address: { ...editData.address, street: e.target.value } })} />
                  </div>
                  <div className="col-md-3">
                    <label style={labelStyle}>Broj</label>
                    <input type="number" style={inputStyle} value={editData?.address?.streetNumber || ''}
                      onFocus={e => e.target.style.borderColor = '#c9a84c'}
                      onBlur={e => e.target.style.borderColor = '#444'}
                      onChange={(e) => setEditData({ ...editData, address: { ...editData.address, streetNumber: e.target.value } })} />
                  </div>
                  <div className="col-md-3">
                    <label style={labelStyle}>Poštanski broj</label>
                    <input style={inputStyle} value={editData?.address?.postalCode || ''}
                      onFocus={e => e.target.style.borderColor = '#c9a84c'}
                      onBlur={e => e.target.style.borderColor = '#444'}
                      onChange={(e) => setEditData({ ...editData, address: { ...editData.address, postalCode: e.target.value } })} />
                  </div>
                  <div className="col-md-3">
                    <label style={labelStyle}>Država</label>
                    <input style={inputStyle} value={editData?.address?.country || ''}
                      onFocus={e => e.target.style.borderColor = '#c9a84c'}
                      onBlur={e => e.target.style.borderColor = '#444'}
                      onChange={(e) => setEditData({ ...editData, address: { ...editData.address, country: e.target.value } })} />
                  </div>
                </div>
                <div style={{ marginTop: '24px', display: 'flex', gap: '12px' }}>
                  <button type="submit" style={submitButtonStyle}
                    onMouseEnter={e => e.target.style.backgroundColor = '#e8c96d'}
                    onMouseLeave={e => e.target.style.backgroundColor = '#c9a84c'}>
                    Sačuvaj
                  </button>
                  <button type="button" onClick={() => setEditing(false)} style={cancelButtonStyle}
                    onMouseEnter={e => { e.target.style.backgroundColor = '#c9a84c'; e.target.style.color = '#1a1a1a' }}
                    onMouseLeave={e => { e.target.style.backgroundColor = 'transparent'; e.target.style.color = '#c9a84c' }}>
                    Otkaži
                  </button>
                </div>
              </form>
            </div>
          )}
        </div>
      </div>
    </div>
  )
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

export default ProfilePage