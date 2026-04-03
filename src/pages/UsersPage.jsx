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

  useEffect(() => {
    fetchUsers()
  }, [])

  const handleDelete = async (id) => {
    if (!window.confirm('Da li ste sigurni da želite obrisati korisnika?')) return
    try {
      await deleteUser(id, token)
      setUsers(users.filter(u => u.idUser !== id))
    } catch (err) {
      setError('Greška pri brisanju korisnika.')
    }
  }

  const handleEdit = (user) => {
    setEditUser({ ...user })
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

  if (loading) return <div className="text-center mt-5"><div className="spinner-border"></div></div>

  return (
    <div>
      <h2 className="mb-4">Korisnici</h2>
      {error && <div className="alert alert-danger">{error}</div>}

      {/* Edit Modal */}
      {editUser && (
        <div className="card mb-4 p-4">
          <h5>Izmeni korisnika</h5>
          <div className="row g-3">
            <div className="col-md-6">
              <label className="form-label">Korisničko ime</label>
              <input className="form-control" value={editUser.username}
                onChange={(e) => setEditUser({ ...editUser, username: e.target.value })} />
            </div>
            <div className="col-md-6">
              <label className="form-label">Email</label>
              <input className="form-control" value={editUser.email}
                onChange={(e) => setEditUser({ ...editUser, email: e.target.value })} />
            </div>
            <div className="col-md-6">
              <label className="form-label">Telefon</label>
              <input className="form-control" value={editUser.phoneNumber || ''}
                onChange={(e) => setEditUser({ ...editUser, phoneNumber: e.target.value })} />
            </div>
            <div className="col-md-6">
              <label className="form-label">Ulica</label>
              <input className="form-control" value={editUser.address?.street || ''}
                onChange={(e) => setEditUser({ ...editUser, address: { ...editUser.address, street: e.target.value } })} />
            </div>
            <div className="col-md-4">
              <label className="form-label">Broj</label>
              <input className="form-control" type="number" value={editUser.address?.streetNumber || ''}
                onChange={(e) => setEditUser({ ...editUser, address: { ...editUser.address, streetNumber: e.target.value } })} />
            </div>
            <div className="col-md-4">
              <label className="form-label">Poštanski broj</label>
              <input className="form-control" value={editUser.address?.postalCode || ''}
                onChange={(e) => setEditUser({ ...editUser, address: { ...editUser.address, postalCode: e.target.value } })} />
            </div>
            <div className="col-md-4">
              <label className="form-label">Država</label>
              <input className="form-control" value={editUser.address?.country || ''}
                onChange={(e) => setEditUser({ ...editUser, address: { ...editUser.address, country: e.target.value } })} />
            </div>
            <div className="col-md-4">
              <label className="form-label">Rola</label>
              <select className="form-select" value={editUser.role}
                onChange={(e) => setEditUser({ ...editUser, role: parseInt(e.target.value) })}>
                <option value={0}>Korisnik</option>
                <option value={1}>Admin</option>
              </select>
            </div>
          </div>
          <div className="mt-3">
            <button className="btn btn-success me-2" onClick={handleUpdate}>Sačuvaj</button>
            <button className="btn btn-secondary" onClick={() => setEditUser(null)}>Otkaži</button>
          </div>
        </div>
      )}

      {/* Users Table */}
      <table className="table table-bordered table-hover">
        <thead className="table-dark">
          <tr>
            <th>ID</th>
            <th>Korisničko ime</th>
            <th>Email</th>
            <th>Telefon</th>
            <th>Adresa</th>
            <th>Akcije</th>
          </tr>
        </thead>
        <tbody>
          {users.map(user => (
            <tr key={user.idUser}>
              <td>{user.idUser}</td>
              <td>{user.username}</td>
              <td>{user.email}</td>
              <td>{user.phoneNumber || '-'}</td>
              <td>{user.address ? `${user.address.street} ${user.address.streetNumber}, ${user.address.country}` : '-'}</td>
              <td>
                <button className="btn btn-sm btn-warning me-2" onClick={() => handleEdit(user)}>Izmeni</button>
                <button className="btn btn-sm btn-danger" onClick={() => handleDelete(user.idUser)}>Obriši</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}

export default UsersPage