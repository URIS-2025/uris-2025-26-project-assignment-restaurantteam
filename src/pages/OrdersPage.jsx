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

  const [newOrder, setNewOrder] = useState({ paymentMethod: 0, items: [] })
  const [selectedItem, setSelectedItem] = useState({ idMenuItem: '', quantity: 1, pricePerItem: 0 })

  const statusLabels = {
    0: { label: 'Na čekanju', color: '#f39c12', bg: '#2c2000' },
    1: { label: 'U pripremi', color: '#3498db', bg: '#001a2c' },
    2: { label: 'Završeno', color: '#2ecc71', bg: '#001a00' },
    3: { label: 'Otkazano', color: '#e74c3c', bg: '#2c0000' }
  }

  const paymentLabels = { 0: 'Gotovina', 1: 'Kartica', 2: 'Online' }

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
        <p style={{ color: '#c9a84c', letterSpacing: '4px', fontSize: '0.85rem', marginBottom: '12px' }}>VAŠE NARUDŽBINE</p>
        <h1 style={{ color: '#1a1a1a', fontFamily: 'Georgia, serif', fontSize: '2.8rem', marginBottom: '12px' }}>Narudžbine</h1>
        <p style={{ color: '#6b6457', fontSize: '1rem' }}>Naručite svoja omiljena jela</p>
      </div>

      {error && (
        <div style={{
          backgroundColor: '#3a1a1a', border: '1px solid #c0392b',
          color: '#e74c3c', padding: '12px 16px', borderRadius: '6px', marginBottom: '24px'
        }}>
          {error}
        </div>
      )}

      {/* Nova narudžbina */}
      <div style={cardStyle}>
        <h5 style={cardTitleStyle}>Nova narudžbina</h5>
        <form onSubmit={handleCreateOrder}>
          <div className="row g-3 mb-3">
            <div className="col-md-4">
              <label style={labelStyle}>Način plaćanja</label>
              <select style={inputStyle} value={newOrder.paymentMethod}
                onChange={(e) => setNewOrder({ ...newOrder, paymentMethod: parseInt(e.target.value) })}>
                <option value={0}>Gotovina</option>
                <option value={1}>Kartica</option>
                <option value={2}>Online</option>
              </select>
            </div>
            <div className="col-md-4">
              <label style={labelStyle}>Izaberi jelo</label>
              <select style={inputStyle} value={selectedItem.idMenuItem}
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
              <label style={labelStyle}>Količina</label>
              <input type="number" style={inputStyle} min="1" value={selectedItem.quantity}
                onFocus={e => e.target.style.borderColor = '#c9a84c'}
                onBlur={e => e.target.style.borderColor = '#444'}
                onChange={(e) => setSelectedItem({ ...selectedItem, quantity: e.target.value })} />
            </div>
            <div className="col-md-2 d-flex align-items-end">
              <button type="button" style={cancelButtonStyle} onClick={handleAddItem}
                onMouseEnter={e => { e.target.style.backgroundColor = '#c9a84c'; e.target.style.color = '#1a1a1a' }}
                onMouseLeave={e => { e.target.style.backgroundColor = 'transparent'; e.target.style.color = '#c9a84c' }}>
                + Dodaj
              </button>
            </div>
          </div>

          {/* Izabrana jela */}
          {newOrder.items.length > 0 && (
            <div style={{ marginBottom: '20px' }}>
              <table style={{ width: '100%', borderCollapse: 'collapse' }}>
                <thead>
                  <tr style={{ borderBottom: '1px solid #c9a84c' }}>
                    {['Jelo', 'Količina', 'Cijena', ''].map((h, i) => (
                      <th key={i} style={{ color: '#c9a84c', fontFamily: 'Georgia, serif', padding: '8px', textAlign: 'left', fontSize: '0.85rem' }}>{h}</th>
                    ))}
                  </tr>
                </thead>
                <tbody>
                  {newOrder.items.map(item => {
                    const menuItem = menuItems.find(m => m.idMenuItem === item.idMenuItem)
                    return (
                      <tr key={item.idMenuItem} style={{ borderBottom: '1px solid #2c2c2c' }}>
                        <td style={{ color: '#f5f0e8', padding: '10px 8px' }}>{menuItem?.menuItemName}</td>
                        <td style={{ color: '#f5f0e8', padding: '10px 8px' }}>{item.quantity}</td>
                        <td style={{ color: '#c9a84c', padding: '10px 8px', fontFamily: 'Georgia, serif' }}>{(item.pricePerItem * item.quantity).toFixed(2)} RSD</td>
                        <td style={{ padding: '10px 8px' }}>
                          <button type="button" onClick={() => handleRemoveItem(item.idMenuItem)}
                            style={{ background: 'none', border: 'none', color: '#e74c3c', cursor: 'pointer', fontSize: '1rem' }}>✕</button>
                        </td>
                      </tr>
                    )
                  })}
                  <tr>
                    <td colSpan="2" style={{ color: '#c9a84c', fontFamily: 'Georgia, serif', padding: '12px 8px', fontWeight: 'bold' }}>Ukupno:</td>
                    <td colSpan="2" style={{ color: '#c9a84c', fontFamily: 'Georgia, serif', padding: '12px 8px', fontWeight: 'bold', fontSize: '1.1rem' }}>
                      {newOrder.items.reduce((sum, i) => sum + i.pricePerItem * i.quantity, 0).toFixed(2)} RSD
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          )}

          <button type="submit" style={submitButtonStyle}
            onMouseEnter={e => e.target.style.backgroundColor = '#e8c96d'}
            onMouseLeave={e => e.target.style.backgroundColor = '#c9a84c'}>
            Naruči
          </button>
        </form>
      </div>

      {/* Lista narudžbina */}
      <h4 style={{ color: '#1a1a1a', fontFamily: 'Georgia, serif', marginBottom: '24px' }}>
        Pregled narudžbina
      </h4>

      {orders.length === 0 ? (
        <p style={{ color: '#6b6457', fontFamily: 'Georgia, serif' }}>Nema narudžbina.</p>
      ) : (
        <div className="row g-4">
          {orders.map(order => (
            <div key={order.idOrder} className="col-md-6">
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
                  display: 'flex',
                  justifyContent: 'space-between',
                  alignItems: 'center',
                  borderBottom: '1px solid #c9a84c'
                }}>
                  <span style={{ color: '#f5f0e8', fontFamily: 'Georgia, serif', fontWeight: 'bold' }}>
                    Narudžbina #{order.idOrder}
                  </span>
                  <span style={{ color: '#c9a84c', fontSize: '0.85rem' }}>
                    {paymentLabels[order.paymentMethod]}
                  </span>
                </div>

                {/* Card Body */}
                <div style={{ padding: '20px 24px' }}>
                  <table style={{ width: '100%', borderCollapse: 'collapse', marginBottom: '8px' }}>
                    <thead>
                      <tr style={{ borderBottom: '1px solid #2c2c2c' }}>
                        {['Jelo', 'Kol.', 'Cijena'].map((h, i) => (
                          <th key={i} style={{ color: '#c9a84c', fontFamily: 'Georgia, serif', padding: '6px 4px', textAlign: 'left', fontSize: '0.8rem' }}>{h}</th>
                        ))}
                      </tr>
                    </thead>
                    <tbody>
                      {order.items?.map(item => (
                        <tr key={item.idMenuItem} style={{ borderBottom: '1px solid #2c2c2c' }}>
                          <td style={{ color: '#f5f0e8', padding: '8px 4px', fontSize: '0.9rem' }}>
                            {menuItems.find(m => m.idMenuItem === item.idMenuItem)?.menuItemName || `#${item.idMenuItem}`}
                          </td>
                          <td style={{ color: '#9b9080', padding: '8px 4px', fontSize: '0.9rem' }}>{item.quantity}</td>
                          <td style={{ color: '#c9a84c', padding: '8px 4px', fontSize: '0.9rem' }}>{(item.pricePerItem * item.quantity).toFixed(2)} RSD</td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>

                {/* Card Footer */}
                <div style={{
                  padding: '16px 24px',
                  borderTop: '1px solid #2c2c2c',
                  display: 'flex',
                  justifyContent: 'space-between',
                  alignItems: 'center'
                }}>
                  {isAdmin ? (
                    <select
                      value={order.orderStatus}
                      onChange={(e) => handleStatusChange(order.idOrder, e.target.value)}
                      style={{
                        backgroundColor: '#2c2c2c',
                        color: '#c9a84c',
                        border: '1px solid #c9a84c',
                        padding: '6px 12px',
                        borderRadius: '6px',
                        fontFamily: 'Georgia, serif',
                        fontSize: '0.85rem',
                        cursor: 'pointer',
                        outline: 'none'
                      }}>
                      <option value={0}>Na čekanju</option>
                      <option value={1}>U pripremi</option>
                      <option value={2}>Završeno</option>
                      <option value={3}>Otkazano</option>
                    </select>
                  ) : (
                    <span style={{
                      backgroundColor: statusLabels[order.orderStatus]?.bg,
                      color: statusLabels[order.orderStatus]?.color,
                      padding: '4px 14px',
                      borderRadius: '20px',
                      fontSize: '0.85rem',
                      fontFamily: 'Georgia, serif'
                    }}>
                      {statusLabels[order.orderStatus]?.label}
                    </span>
                  )}
                  <button onClick={() => handleDeleteOrder(order.idOrder)}
                    style={{ background: 'none', border: '1px solid #e74c3c', color: '#e74c3c', padding: '6px 14px', borderRadius: '6px', cursor: 'pointer', fontSize: '0.85rem' }}
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
  )
}

const cardStyle = {
  backgroundColor: '#1a1a1a',
  border: '1px solid #c9a84c',
  borderRadius: '12px',
  padding: '32px',
  marginBottom: '40px'
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
  width: '100%',
  backgroundColor: 'transparent',
  color: '#c9a84c',
  border: '1px solid #c9a84c',
  padding: '10px 14px',
  borderRadius: '6px',
  fontFamily: 'Georgia, serif',
  fontSize: '0.95rem',
  cursor: 'pointer'
}

export default OrdersPage