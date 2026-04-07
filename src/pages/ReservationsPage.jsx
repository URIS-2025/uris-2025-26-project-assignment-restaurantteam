import { useState, useEffect } from 'react'
import { useAuth } from '../context/AuthContext'
import {
  getAllReservations, createReservation, updateReservation, deleteReservation,
  getAllTables, createTable, updateTable, deleteTable
} from '../api/reservationApi'

function ReservationsPage() {
  const { token, role } = useAuth()
  const isAdmin = role === 'ADMIN'

  const [reservations, setReservations] = useState([])
  const [tables, setTables] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [activeTab, setActiveTab] = useState('reservations')

  const [newReservation, setNewReservation] = useState({
    reservationDate: '', numberOfGuests: 1, idTable: ''
  })
  const [editReservation, setEditReservation] = useState(null)
  const [newTable, setNewTable] = useState({ numberOfSeats: 2, status: 0 })
  const [editTable, setEditTable] = useState(null)

  const tableStatusLabels = {
    0: { label: 'Slobodan', color: '#2ecc71', bg: '#001a00' },
    1: { label: 'Zauzet', color: '#e74c3c', bg: '#2c0000' }
  }

  const fetchAll = async () => {
    try {
      const [resRes, tabRes] = await Promise.all([
        getAllReservations(token),
        getAllTables()
      ])
      setReservations(resRes.data)
      setTables(tabRes.data)
    } catch (err) {
      setError('Greška pri učitavanju podataka.')
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => { fetchAll() }, [])

  const handleCreateReservation = async (e) => {
    e.preventDefault()
    try {
      await createReservation({
        reservationDate: new Date(newReservation.reservationDate).toISOString(),
        numberOfGuests: parseInt(newReservation.numberOfGuests),
        idTable: parseInt(newReservation.idTable)
      }, token)
      setNewReservation({ reservationDate: '', numberOfGuests: 1, idTable: '' })
      fetchAll()
    } catch (err) { setError('Greška pri kreiranju rezervacije.') }
  }

  const handleUpdateReservation = async () => {
    try {
      await updateReservation(editReservation.idReservation, {
        reservationDate: new Date(editReservation.reservationDate).toISOString(),
        numberOfGuests: parseInt(editReservation.numberOfGuests)
      }, token)
      setEditReservation(null)
      fetchAll()
    } catch (err) { setError('Greška pri izmeni rezervacije.') }
  }

  const handleDeleteReservation = async (id) => {
    if (!window.confirm('Obrisati rezervaciju?')) return
    try {
      await deleteReservation(id, token)
      fetchAll()
    } catch (err) { setError('Greška pri brisanju rezervacije.') }
  }

  const handleCreateTable = async (e) => {
    e.preventDefault()
    try {
      await createTable({ numberOfSeats: parseInt(newTable.numberOfSeats), status: parseInt(newTable.status) }, token)
      setNewTable({ numberOfSeats: 2, status: 0 })
      fetchAll()
    } catch (err) { setError('Greška pri kreiranju stola.') }
  }

  const handleUpdateTable = async () => {
    try {
      await updateTable(editTable.idTable, {
        idTable: editTable.idTable,
        numberOfSeats: parseInt(editTable.numberOfSeats),
        status: parseInt(editTable.status)
      }, token)
      setEditTable(null)
      fetchAll()
    } catch (err) { setError('Greška pri izmeni stola.') }
  }

  const handleDeleteTable = async (id) => {
    if (!window.confirm('Obrisati sto?')) return
    try {
      await deleteTable(id, token)
      fetchAll()
    } catch (err) { setError('Greška pri brisanju stola.') }
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
        <p style={{ color: '#c9a84c', letterSpacing: '4px', fontSize: '0.85rem', marginBottom: '12px' }}>REZERVIŠITE MESTO</p>
        <h1 style={{ color: '#1a1a1a', fontFamily: 'Georgia, serif', fontSize: '2.8rem', marginBottom: '12px' }}>Rezervacije</h1>
        <p style={{ color: '#6b6457', fontSize: '1rem' }}>Osigurajte svoj sto unapred</p>
      </div>

      {error && (
        <div style={{
          backgroundColor: '#3a1a1a', border: '1px solid #c0392b',
          color: '#e74c3c', padding: '12px 16px', borderRadius: '6px', marginBottom: '24px'
        }}>
          {error}
        </div>
      )}

      {/* Tabs */}
      {isAdmin && (
        <div style={{ display: 'flex', gap: '12px', marginBottom: '36px' }}>
          {['reservations', 'tables'].map(tab => (
            <button key={tab} onClick={() => setActiveTab(tab)} style={{
              backgroundColor: activeTab === tab ? '#c9a84c' : 'transparent',
              color: activeTab === tab ? '#1a1a1a' : '#c9a84c',
              border: '1px solid #c9a84c',
              padding: '8px 24px',
              borderRadius: '6px',
              fontFamily: 'Georgia, serif',
              fontSize: '0.95rem',
              cursor: 'pointer',
              letterSpacing: '0.5px'
            }}>
              {tab === 'reservations' ? 'Rezervacije' : 'Stolovi'}
            </button>
          ))}
        </div>
      )}

      {/* REZERVACIJE TAB */}
      {activeTab === 'reservations' && (
        <div>
          {/* Nova rezervacija */}
          <div style={cardStyle}>
            <h5 style={cardTitleStyle}>Nova rezervacija</h5>
            <form onSubmit={handleCreateReservation}>
              <div className="row g-3">
                <div className="col-md-4">
                  <label style={labelStyle}>Datum i vrijeme</label>
                  <input type="datetime-local" style={inputStyle}
                    value={newReservation.reservationDate}
                    onFocus={e => e.target.style.borderColor = '#c9a84c'}
                    onBlur={e => e.target.style.borderColor = '#444'}
                    onChange={(e) => setNewReservation({ ...newReservation, reservationDate: e.target.value })}
                    required />
                </div>
                <div className="col-md-3">
                  <label style={labelStyle}>Broj gostiju</label>
                  <input type="number" style={inputStyle} min="1"
                    value={newReservation.numberOfGuests}
                    onFocus={e => e.target.style.borderColor = '#c9a84c'}
                    onBlur={e => e.target.style.borderColor = '#444'}
                    onChange={(e) => setNewReservation({ ...newReservation, numberOfGuests: e.target.value })}
                    required />
                </div>
                <div className="col-md-3">
                  <label style={labelStyle}>Sto</label>
                  <select style={inputStyle} value={newReservation.idTable}
                    onChange={(e) => setNewReservation({ ...newReservation, idTable: e.target.value })}
                    required>
                    <option value="">-- Izaberi sto --</option>
                    {tables.filter(t => t.status === 0).map(t => (
                      <option key={t.idTable} value={t.idTable}>
                        Sto #{t.idTable} ({t.numberOfSeats} mesta)
                      </option>
                    ))}
                  </select>
                </div>
                <div className="col-md-2 d-flex align-items-end">
                  <button type="submit" style={{ ...submitButtonStyle, width: '100%' }}
                    onMouseEnter={e => e.target.style.backgroundColor = '#e8c96d'}
                    onMouseLeave={e => e.target.style.backgroundColor = '#c9a84c'}>
                    Rezerviši
                  </button>
                </div>
              </div>
            </form>
          </div>

          {/* Edit rezervacija */}
          {editReservation && (
            <div style={{ ...cardStyle, borderColor: '#e8c96d' }}>
              <h5 style={cardTitleStyle}>Izmeni rezervaciju</h5>
              <div className="row g-3">
                <div className="col-md-4">
                  <label style={labelStyle}>Datum i vrijeme</label>
                  <input type="datetime-local" style={inputStyle}
                    value={editReservation.reservationDate?.slice(0, 16)}
                    onFocus={e => e.target.style.borderColor = '#c9a84c'}
                    onBlur={e => e.target.style.borderColor = '#444'}
                    onChange={(e) => setEditReservation({ ...editReservation, reservationDate: e.target.value })} />
                </div>
                <div className="col-md-3">
                  <label style={labelStyle}>Broj gostiju</label>
                  <input type="number" style={inputStyle} min="1"
                    value={editReservation.numberOfGuests}
                    onFocus={e => e.target.style.borderColor = '#c9a84c'}
                    onBlur={e => e.target.style.borderColor = '#444'}
                    onChange={(e) => setEditReservation({ ...editReservation, numberOfGuests: e.target.value })} />
                </div>
              </div>
              <div style={{ marginTop: '20px', display: 'flex', gap: '12px' }}>
                <button style={submitButtonStyle} onClick={handleUpdateReservation}
                  onMouseEnter={e => e.target.style.backgroundColor = '#e8c96d'}
                  onMouseLeave={e => e.target.style.backgroundColor = '#c9a84c'}>
                  Sačuvaj
                </button>
                <button onClick={() => setEditReservation(null)} style={cancelButtonStyle}
                  onMouseEnter={e => { e.target.style.backgroundColor = '#c9a84c'; e.target.style.color = '#1a1a1a' }}
                  onMouseLeave={e => { e.target.style.backgroundColor = 'transparent'; e.target.style.color = '#c9a84c' }}>
                  Otkaži
                </button>
              </div>
            </div>
          )}

          {/* Lista rezervacija */}
          {reservations.length === 0 ? (
            <p style={{ color: '#6b6457', fontFamily: 'Georgia, serif' }}>Nema rezervacija.</p>
          ) : (
            <div className="row g-4">
              {reservations.map(r => (
                <div key={r.idReservation} className="col-md-6">
                  <div style={{
                    backgroundColor: '#1a1a1a',
                    border: '1px solid #c9a84c',
                    borderRadius: '12px',
                    overflow: 'hidden'
                  }}>
                    <div style={{
                      backgroundColor: '#2c2c2c',
                      padding: '16px 24px',
                      borderBottom: '1px solid #c9a84c',
                      display: 'flex',
                      justifyContent: 'space-between',
                      alignItems: 'center'
                    }}>
                      <span style={{ color: '#f5f0e8', fontFamily: 'Georgia, serif', fontWeight: 'bold' }}>
                        Rezervacija #{r.idReservation}
                      </span>
                      <span style={{ color: '#c9a84c', fontSize: '0.85rem' }}>
                        Sto #{r.idTable}
                      </span>
                    </div>
                    <div style={{ padding: '20px 24px' }}>
                      <div style={{ display: 'flex', gap: '32px', marginBottom: '8px' }}>
                        <div>
                          <p style={{ color: '#6b6457', fontSize: '0.8rem', marginBottom: '4px', letterSpacing: '1px' }}>DATUM I VRIJEME</p>
                          <p style={{ color: '#f5f0e8', fontFamily: 'Georgia, serif', margin: 0 }}>
                            {new Date(r.reservationDate).toLocaleString('sr-RS')}
                          </p>
                        </div>
                        <div>
                          <p style={{ color: '#6b6457', fontSize: '0.8rem', marginBottom: '4px', letterSpacing: '1px' }}>BROJ GOSTIJU</p>
                          <p style={{ color: '#f5f0e8', fontFamily: 'Georgia, serif', margin: 0 }}>{r.numberOfGuests}</p>
                        </div>
                      </div>
                    </div>
                    <div style={{
                      padding: '16px 24px',
                      borderTop: '1px solid #2c2c2c',
                      display: 'flex',
                      justifyContent: 'flex-end',
                      gap: '12px'
                    }}>
                      <button onClick={() => setEditReservation({ ...r })} style={smallButtonStyle}
                        onMouseEnter={e => { e.target.style.backgroundColor = '#c9a84c'; e.target.style.color = '#1a1a1a' }}
                        onMouseLeave={e => { e.target.style.backgroundColor = 'transparent'; e.target.style.color = '#c9a84c' }}>
                        Izmeni
                      </button>
                      <button onClick={() => handleDeleteReservation(r.idReservation)}
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
          )}
        </div>
      )}

      {/* STOLOVI TAB */}
      {activeTab === 'tables' && isAdmin && (
        <div>
          <div style={cardStyle}>
            <h5 style={cardTitleStyle}>Dodaj novi sto</h5>
            <form onSubmit={handleCreateTable}>
              <div className="row g-3">
                <div className="col-md-3">
                  <label style={labelStyle}>Broj mesta</label>
                  <input type="number" style={inputStyle} min="1"
                    value={newTable.numberOfSeats}
                    onFocus={e => e.target.style.borderColor = '#c9a84c'}
                    onBlur={e => e.target.style.borderColor = '#444'}
                    onChange={(e) => setNewTable({ ...newTable, numberOfSeats: e.target.value })} required />
                </div>
                <div className="col-md-3">
                  <label style={labelStyle}>Status</label>
                  <select style={inputStyle} value={newTable.status}
                    onChange={(e) => setNewTable({ ...newTable, status: e.target.value })}>
                    <option value={0}>Slobodan</option>
                    <option value={1}>Zauzet</option>
                  </select>
                </div>
                <div className="col-md-2 d-flex align-items-end">
                  <button type="submit" style={{ ...submitButtonStyle, width: '100%' }}
                    onMouseEnter={e => e.target.style.backgroundColor = '#e8c96d'}
                    onMouseLeave={e => e.target.style.backgroundColor = '#c9a84c'}>
                    Dodaj
                  </button>
                </div>
              </div>
            </form>
          </div>

          {/* Edit sto */}
          {editTable && (
            <div style={{ ...cardStyle, borderColor: '#e8c96d' }}>
              <h5 style={cardTitleStyle}>Izmeni sto</h5>
              <div className="row g-3">
                <div className="col-md-3">
                  <label style={labelStyle}>Broj mesta</label>
                  <input type="number" style={inputStyle} min="1"
                    value={editTable.numberOfSeats}
                    onFocus={e => e.target.style.borderColor = '#c9a84c'}
                    onBlur={e => e.target.style.borderColor = '#444'}
                    onChange={(e) => setEditTable({ ...editTable, numberOfSeats: e.target.value })} />
                </div>
                <div className="col-md-3">
                  <label style={labelStyle}>Status</label>
                  <select style={inputStyle} value={editTable.status}
                    onChange={(e) => setEditTable({ ...editTable, status: parseInt(e.target.value) })}>
                    <option value={0}>Slobodan</option>
                    <option value={1}>Zauzet</option>
                  </select>
                </div>
              </div>
              <div style={{ marginTop: '20px', display: 'flex', gap: '12px' }}>
                <button style={submitButtonStyle} onClick={handleUpdateTable}
                  onMouseEnter={e => e.target.style.backgroundColor = '#e8c96d'}
                  onMouseLeave={e => e.target.style.backgroundColor = '#c9a84c'}>
                  Sačuvaj
                </button>
                <button onClick={() => setEditTable(null)} style={cancelButtonStyle}
                  onMouseEnter={e => { e.target.style.backgroundColor = '#c9a84c'; e.target.style.color = '#1a1a1a' }}
                  onMouseLeave={e => { e.target.style.backgroundColor = 'transparent'; e.target.style.color = '#c9a84c' }}>
                  Otkaži
                </button>
              </div>
            </div>
          )}

          {/* Lista stolova */}
          <div className="row g-4">
            {tables.map(t => (
              <div key={t.idTable} className="col-md-3">
                <div style={{
                  backgroundColor: '#1a1a1a',
                  border: '1px solid #c9a84c',
                  borderRadius: '12px',
                  overflow: 'hidden',
                  textAlign: 'center'
                }}>
                  <div style={{
                    backgroundColor: '#2c2c2c',
                    padding: '16px',
                    borderBottom: '1px solid #c9a84c'
                  }}>
                    <span style={{ color: '#c9a84c', fontFamily: 'Georgia, serif', fontWeight: 'bold', fontSize: '1.1rem' }}>
                      Sto #{t.idTable}
                    </span>
                  </div>
                  <div style={{ padding: '20px' }}>
                    <p style={{ color: '#f5f0e8', fontFamily: 'Georgia, serif', fontSize: '1.5rem', margin: '0 0 8px' }}>
                      🪑 {t.numberOfSeats}
                    </p>
                    <p style={{ color: '#6b6457', fontSize: '0.8rem', margin: '0 0 12px' }}>mesta</p>
                    <span style={{
                      backgroundColor: tableStatusLabels[t.status]?.bg,
                      color: tableStatusLabels[t.status]?.color,
                      padding: '4px 14px',
                      borderRadius: '20px',
                      fontSize: '0.8rem',
                      fontFamily: 'Georgia, serif'
                    }}>
                      {tableStatusLabels[t.status]?.label}
                    </span>
                  </div>
                  <div style={{
                    padding: '12px 16px',
                    borderTop: '1px solid #2c2c2c',
                    display: 'flex',
                    gap: '8px',
                    justifyContent: 'center'
                  }}>
                    <button onClick={() => setEditTable({ ...t })} style={smallButtonStyle}
                      onMouseEnter={e => { e.target.style.backgroundColor = '#c9a84c'; e.target.style.color = '#1a1a1a' }}
                      onMouseLeave={e => { e.target.style.backgroundColor = 'transparent'; e.target.style.color = '#c9a84c' }}>
                      Izmeni
                    </button>
                    <button onClick={() => handleDeleteTable(t.idTable)}
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
      )}
    </div>
  )
}

const cardStyle = {
  backgroundColor: '#1a1a1a',
  border: '1px solid #c9a84c',
  borderRadius: '12px',
  padding: '32px',
  marginBottom: '32px'
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

export default ReservationsPage