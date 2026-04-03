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

  // Nova rezervacija
  const [newReservation, setNewReservation] = useState({
    reservationDate: '',
    numberOfGuests: 1,
    idTable: ''
  })

  // Edit rezervacija
  const [editReservation, setEditReservation] = useState(null)

  // Novi sto
  const [newTable, setNewTable] = useState({ numberOfSeats: 2, status: 0 })

  // Edit sto
  const [editTable, setEditTable] = useState(null)

  const tableStatusLabels = {
    0: { label: 'Slobodan', badge: 'bg-success' },
    1: { label: 'Zauzet', badge: 'bg-danger' }
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

  // Rezervacije CRUD
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
    } catch (err) {
      setError('Greška pri kreiranju rezervacije.')
    }
  }

  const handleUpdateReservation = async () => {
    try {
      await updateReservation(editReservation.idReservation, {
        reservationDate: new Date(editReservation.reservationDate).toISOString(),
        numberOfGuests: parseInt(editReservation.numberOfGuests)
      }, token)
      setEditReservation(null)
      fetchAll()
    } catch (err) {
      setError('Greška pri izmeni rezervacije.')
    }
  }

  const handleDeleteReservation = async (id) => {
    if (!window.confirm('Obrisati rezervaciju?')) return
    try {
      await deleteReservation(id, token)
      fetchAll()
    } catch (err) {
      setError('Greška pri brisanju rezervacije.')
    }
  }

  // Stolovi CRUD
  const handleCreateTable = async (e) => {
    e.preventDefault()
    try {
      await createTable({ numberOfSeats: parseInt(newTable.numberOfSeats), status: parseInt(newTable.status) }, token)
      setNewTable({ numberOfSeats: 2, status: 0 })
      fetchAll()
    } catch (err) {
      setError('Greška pri kreiranju stola.')
    }
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
    } catch (err) {
      setError('Greška pri izmeni stola.')
    }
  }

  const handleDeleteTable = async (id) => {
    if (!window.confirm('Obrisati sto?')) return
    try {
      await deleteTable(id, token)
      fetchAll()
    } catch (err) {
      setError('Greška pri brisanju stola.')
    }
  }

  if (loading) return <div className="text-center mt-5"><div className="spinner-border"></div></div>

  return (
    <div>
      <h2 className="mb-4">Rezervacije</h2>
      {error && <div className="alert alert-danger">{error}</div>}

      {/* Tabs */}
      <ul className="nav nav-tabs mb-4">
        <li className="nav-item">
          <button className={`nav-link ${activeTab === 'reservations' ? 'active' : ''}`}
            onClick={() => setActiveTab('reservations')}>Rezervacije</button>
        </li>
        {isAdmin && (
          <li className="nav-item">
            <button className={`nav-link ${activeTab === 'tables' ? 'active' : ''}`}
              onClick={() => setActiveTab('tables')}>Stolovi</button>
          </li>
        )}
      </ul>

      {/* REZERVACIJE TAB */}
      {activeTab === 'reservations' && (
        <div>
          {/* Nova rezervacija */}
          <div className="card p-4 mb-4">
            <h5>Nova rezervacija</h5>
            <form onSubmit={handleCreateReservation}>
              <div className="row g-3">
                <div className="col-md-4">
                  <label className="form-label">Datum i vrijeme</label>
                  <input type="datetime-local" className="form-control"
                    value={newReservation.reservationDate}
                    onChange={(e) => setNewReservation({ ...newReservation, reservationDate: e.target.value })}
                    required />
                </div>
                <div className="col-md-3">
                  <label className="form-label">Broj gostiju</label>
                  <input type="number" className="form-control" min="1"
                    value={newReservation.numberOfGuests}
                    onChange={(e) => setNewReservation({ ...newReservation, numberOfGuests: e.target.value })}
                    required />
                </div>
                <div className="col-md-3">
                  <label className="form-label">Sto</label>
                  <select className="form-select" value={newReservation.idTable}
                    onChange={(e) => setNewReservation({ ...newReservation, idTable: e.target.value })}
                    required>
                    <option value="">-- Izaberi sto --</option>
                    {tables.filter(t => t.status === 0).map(t => (
                      <option key={t.idTable} value={t.idTable}>
                        Sto #{t.idTable} ({t.numberOfSeats} mjesta)
                      </option>
                    ))}
                  </select>
                </div>
                <div className="col-md-2 d-flex align-items-end">
                  <button type="submit" className="btn btn-dark w-100">Rezerviši</button>
                </div>
              </div>
            </form>
          </div>

          {/* Edit rezervacija */}
          {editReservation && (
            <div className="card p-4 mb-4 border-warning">
              <h5>Izmeni rezervaciju</h5>
              <div className="row g-3">
                <div className="col-md-4">
                  <label className="form-label">Datum i vrijeme</label>
                  <input type="datetime-local" className="form-control"
                    value={editReservation.reservationDate?.slice(0, 16)}
                    onChange={(e) => setEditReservation({ ...editReservation, reservationDate: e.target.value })} />
                </div>
                <div className="col-md-3">
                  <label className="form-label">Broj gostiju</label>
                  <input type="number" className="form-control" min="1"
                    value={editReservation.numberOfGuests}
                    onChange={(e) => setEditReservation({ ...editReservation, numberOfGuests: e.target.value })} />
                </div>
              </div>
              <div className="mt-3">
                <button className="btn btn-success me-2" onClick={handleUpdateReservation}>Sačuvaj</button>
                <button className="btn btn-secondary" onClick={() => setEditReservation(null)}>Otkaži</button>
              </div>
            </div>
          )}

          {/* Lista rezervacija */}
          {reservations.length === 0 ? (
            <p className="text-muted">Nema rezervacija.</p>
          ) : (
            <table className="table table-bordered table-hover">
              <thead className="table-dark">
                <tr>
                  <th>ID</th>
                  <th>Datum i vrijeme</th>
                  <th>Broj gostiju</th>
                  <th>Sto</th>
                  <th>Akcije</th>
                </tr>
              </thead>
              <tbody>
                {reservations.map(r => (
                  <tr key={r.idReservation}>
                    <td>{r.idReservation}</td>
                    <td>{new Date(r.reservationDate).toLocaleString('sr-RS')}</td>
                    <td>{r.numberOfGuests}</td>
                    <td>Sto #{r.idTable}</td>
                    <td>
                      <button className="btn btn-sm btn-warning me-2"
                        onClick={() => setEditReservation({ ...r })}>Izmeni</button>
                      <button className="btn btn-sm btn-danger"
                        onClick={() => handleDeleteReservation(r.idReservation)}>Obriši</button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>
      )}

      {/* STOLOVI TAB - samo admin */}
      {activeTab === 'tables' && isAdmin && (
        <div>
          {/* Novi sto */}
          <div className="card p-4 mb-4">
            <h5>Dodaj novi sto</h5>
            <form onSubmit={handleCreateTable}>
              <div className="row g-3">
                <div className="col-md-3">
                  <label className="form-label">Broj mjesta</label>
                  <input type="number" className="form-control" min="1"
                    value={newTable.numberOfSeats}
                    onChange={(e) => setNewTable({ ...newTable, numberOfSeats: e.target.value })}
                    required />
                </div>
                <div className="col-md-3">
                  <label className="form-label">Status</label>
                  <select className="form-select" value={newTable.status}
                    onChange={(e) => setNewTable({ ...newTable, status: e.target.value })}>
                    <option value={0}>Slobodan</option>
                    <option value={1}>Zauzet</option>
                  </select>
                </div>
                <div className="col-md-2 d-flex align-items-end">
                  <button type="submit" className="btn btn-dark w-100">Dodaj</button>
                </div>
              </div>
            </form>
          </div>

          {/* Edit sto */}
          {editTable && (
            <div className="card p-4 mb-4 border-warning">
              <h5>Izmeni sto</h5>
              <div className="row g-3">
                <div className="col-md-3">
                  <label className="form-label">Broj mjesta</label>
                  <input type="number" className="form-control" min="1"
                    value={editTable.numberOfSeats}
                    onChange={(e) => setEditTable({ ...editTable, numberOfSeats: e.target.value })} />
                </div>
                <div className="col-md-3">
                  <label className="form-label">Status</label>
                  <select className="form-select" value={editTable.status}
                    onChange={(e) => setEditTable({ ...editTable, status: parseInt(e.target.value) })}>
                    <option value={0}>Slobodan</option>
                    <option value={1}>Zauzet</option>
                  </select>
                </div>
              </div>
              <div className="mt-3">
                <button className="btn btn-success me-2" onClick={handleUpdateTable}>Sačuvaj</button>
                <button className="btn btn-secondary" onClick={() => setEditTable(null)}>Otkaži</button>
              </div>
            </div>
          )}

          {/* Lista stolova */}
          <table className="table table-bordered table-hover">
            <thead className="table-dark">
              <tr><th>ID</th><th>Broj mjesta</th><th>Status</th><th>Akcije</th></tr>
            </thead>
            <tbody>
              {tables.map(t => (
                <tr key={t.idTable}>
                  <td>{t.idTable}</td>
                  <td>{t.numberOfSeats}</td>
                  <td><span className={`badge ${tableStatusLabels[t.status]?.badge}`}>{tableStatusLabels[t.status]?.label}</span></td>
                  <td>
                    <button className="btn btn-sm btn-warning me-2" onClick={() => setEditTable({ ...t })}>Izmeni</button>
                    <button className="btn btn-sm btn-danger" onClick={() => handleDeleteTable(t.idTable)}>Obriši</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  )
}

export default ReservationsPage