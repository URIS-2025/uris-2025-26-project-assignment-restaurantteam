import { useState, useEffect } from 'react'
import { useAuth } from '../context/AuthContext'
import { getAllOrders, createOrder, deleteOrder, updateOrderStatus } from '../api/orderApi'
import { getAllMenuItems } from '../api/menuApi'

function OrdersPage() {
  const { token, role } = useAuth()
  const isAdmin = role === 'ADMIN'

  const [orders, setOrders] = useState([])
  const [menuItems, setMenuItems] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  // Nova narudžbina
  const [newOrder, setNewOrder] = useState({
    paymentMethod: 0,
    items: []
  })
  const [selectedItem, setSelectedItem] = useState({ idMenuItem: '', quantity: 1, pricePerItem: 0 })

  const statusLabels = {
    0: { label: 'Na čekanju', badge: 'bg-warning text-dark' },
    1: { label: 'U pripremi', badge: 'bg-info text-dark' },
    2: { label: 'Završeno', badge: 'bg-success' },
    3: { label: 'Otkazano', badge: 'bg-danger' }
  }

  const paymentLabels = {
    0: 'Gotovina',
    1: 'Kartica',
    2: 'Online'
  }

  const fetchAll = async () => {
    try {
      const [ordersRes, menuRes] = await Promise.all([
        getAllOrders(token),
        getAllMenuItems(token)
      ])
      setOrders(ordersRes.data)
      setMenuItems(menuRes.data)
    } catch (err) {
      setError('Greška pri učitavanju narudžbina.')
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => { fetchAll() }, [])

  const handleAddItem = () => {
    if (!selectedItem.idMenuItem) return
    const menuItem = menuItems.find(m => m.idMenuItem === parseInt(selectedItem.idMenuItem))
    const exists = newOrder.items.find(i => i.idMenuItem === parseInt(selectedItem.idMenuItem))
    if (exists) return

    setNewOrder({
      ...newOrder,
      items: [...newOrder.items, {
        idMenuItem: parseInt(selectedItem.idMenuItem),
        quantity: parseInt(selectedItem.quantity),
        pricePerItem: menuItem.price
      }]
    })
    setSelectedItem({ idMenuItem: '', quantity: 1, pricePerItem: 0 })
  }

  const handleRemoveItem = (idMenuItem) => {
    setNewOrder({ ...newOrder, items: newOrder.items.filter(i => i.idMenuItem !== idMenuItem) })
  }

  const handleCreateOrder = async (e) => {
    e.preventDefault()
    if (newOrder.items.length === 0) {
      setError('Dodajte bar jedno jelo u narudžbinu.')
      return
    }
    try {
      await createOrder(newOrder, token)
      setNewOrder({ paymentMethod: 0, items: [] })
      fetchAll()
    } catch (err) {
      setError('Greška pri kreiranju narudžbine.')
    }
  }

  const handleDeleteOrder = async (id) => {
    if (!window.confirm('Obrisati narudžbinu?')) return
    try {
      await deleteOrder(id, token)
      fetchAll()
    } catch (err) {
      setError('Greška pri brisanju narudžbine.')
    }
  }

  const handleStatusChange = async (id, status) => {
  try {
    await updateOrderStatus(id, parseInt(status), token)
    fetchAll()
  } catch (err) {
    setError('Greška pri izmeni statusa.')
  }
}

  if (loading) return <div className="text-center mt-5"><div className="spinner-border"></div></div>

  return (
    <div>
      <h2 className="mb-4">Narudžbine</h2>
      {error && <div className="alert alert-danger">{error}</div>}

      {/* Nova narudžbina forma */}
      <div className="card p-4 mb-4">
        <h5>Nova narudžbina</h5>
        <form onSubmit={handleCreateOrder}>
          <div className="row g-3 mb-3">
            <div className="col-md-4">
              <label className="form-label">Način plaćanja</label>
              <select className="form-select" value={newOrder.paymentMethod}
                onChange={(e) => setNewOrder({ ...newOrder, paymentMethod: parseInt(e.target.value) })}>
                <option value={0}>Gotovina</option>
                <option value={1}>Kartica</option>
                <option value={2}>Online</option>
              </select>
            </div>
            <div className="col-md-4">
              <label className="form-label">Izaberi jelo</label>
              <select className="form-select" value={selectedItem.idMenuItem}
                onChange={(e) => setSelectedItem({ ...selectedItem, idMenuItem: e.target.value })}>
                <option value="">-- Izaberi --</option>
                {menuItems.filter(m => m.isAvailable).map(m => (
                  <option key={m.idMenuItem} value={m.idMenuItem}>
                    {m.menuItemName} ({m.price} RSD)
                  </option>
                ))}
              </select>
            </div>
            <div className="col-md-2">
              <label className="form-label">Količina</label>
              <input type="number" className="form-control" min="1" value={selectedItem.quantity}
                onChange={(e) => setSelectedItem({ ...selectedItem, quantity: e.target.value })} />
            </div>
            <div className="col-md-2 d-flex align-items-end">
              <button type="button" className="btn btn-secondary w-100" onClick={handleAddItem}>Dodaj</button>
            </div>
          </div>

          {/* Lista izabranih jela */}
          {newOrder.items.length > 0 && (
            <div className="mb-3">
              <table className="table table-sm table-bordered">
                <thead className="table-light">
                  <tr><th>Jelo</th><th>Količina</th><th>Cena</th><th></th></tr>
                </thead>
                <tbody>
                  {newOrder.items.map(item => {
                    const menuItem = menuItems.find(m => m.idMenuItem === item.idMenuItem)
                    return (
                      <tr key={item.idMenuItem}>
                        <td>{menuItem?.menuItemName}</td>
                        <td>{item.quantity}</td>
                        <td>{(item.pricePerItem * item.quantity).toFixed(2)} RSD</td>
                        <td>
                          <button type="button" className="btn btn-sm btn-danger"
                            onClick={() => handleRemoveItem(item.idMenuItem)}>✕</button>
                        </td>
                      </tr>
                    )
                  })}
                </tbody>
                <tfoot>
                  <tr>
                    <td colSpan="2"><strong>Ukupno:</strong></td>
                    <td colSpan="2"><strong>{newOrder.items.reduce((sum, i) => sum + i.pricePerItem * i.quantity, 0).toFixed(2)} RSD</strong></td>
                  </tr>
                </tfoot>
              </table>
            </div>
          )}

          <button type="submit" className="btn btn-dark">Naruči</button>
        </form>
      </div>

      {/* Lista narudžbina */}
      <h5>Pregled narudžbina</h5>
      {orders.length === 0 ? (
        <p className="text-muted">Nema narudžbina.</p>
      ) : (
        <div className="row g-4">
          {orders.map(order => (
              console.log('Order status:', order.status, 'StatusLabel:', statusLabels[order.status]),
              console.log('Cela narudžbina:', order),
            <div key={order.idOrder} className="col-md-6">
              <div className="card h-100">
                <div className="card-header d-flex justify-content-between align-items-center">
                  <strong>Narudžbina #{order.idOrder}</strong>
                </div>
                <div className="card-body">
                  <p className="mb-1"><strong>Plaćanje:</strong> {paymentLabels[order.paymentMethod]}</p>
                  <table className="table table-sm">
                    <thead><tr><th>Jelo</th><th>Kol.</th><th>Cena</th></tr></thead>
                    <tbody>
                      {order.items?.map(item => (
                        <tr key={item.idMenuItem}>
                          <td>{menuItems.find(m => m.idMenuItem === item.idMenuItem)?.menuItemName || `#${item.idMenuItem}`}</td>
                          <td>{item.quantity}</td>
                          <td>{(item.pricePerItem * item.quantity).toFixed(2)} RSD</td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
                <div className="card-footer d-flex justify-content-between align-items-center">
                  {isAdmin ? (
                    <select className="form-select form-select-sm w-50"
                      value={order.orderStatus}
                      onChange={(e) => handleStatusChange(order.idOrder, e.target.value)}>
                      <option value={0}>Na čekanju</option>
                      <option value={1}>U pripremi</option>
                      <option value={2}>Završeno</option>
                      <option value={3}>Otkazano</option>
                    </select>
                  ) : (
                    <span className={`badge ${statusLabels[order.orderStatus]?.badge}`}>
                      {statusLabels[order.orderStatus]?.label}
                    </span>
                  )}
                  <button className="btn btn-sm btn-danger ms-auto"
                    onClick={() => handleDeleteOrder(order.idOrder)}>Obriši</button>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  )
}

export default OrdersPage