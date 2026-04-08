import { useState, useEffect } from 'react'
import { useAuth } from '../context/AuthContext'
import {
  getAllMenuItems, createMenuItem, updateMenuItem, deleteMenuItem,
  getAllCategories, createCategory, deleteCategory,
  getAllIngredients, createIngredient, deleteIngredient
} from '../api/menuApi'

function MenuPage() {
  const { token, role } = useAuth()
  const isAdmin = role === 'ADMIN'

  const [menuItems, setMenuItems] = useState([])
  const [categories, setCategories] = useState([])
  const [ingredients, setIngredients] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [activeTab, setActiveTab] = useState('menu')

  const [newItem, setNewItem] = useState({
    menuItemName: '', description: '', price: 0,
    calories: 0, isAvailable: true, categoryIds: [], ingredientIds: []
  })
  const [editItem, setEditItem] = useState(null)
  const [newCategory, setNewCategory] = useState('')
  const [newIngredient, setNewIngredient] = useState({ ingredientName: '', isAllergen: false })
  const [selectedCategory, setSelectedCategory] = useState(null)

  const fetchAll = async () => {
    try {
      const [catRes, ingRes] = await Promise.all([
        getAllCategories(),
        getAllIngredients()
      ])
      setCategories(catRes.data)
      setIngredients(ingRes.data)

      if (token) {
        const menuRes = await getAllMenuItems(token)
        setMenuItems(menuRes.data)
      }
    } catch (err) {
      setError('Greška pri učitavanju podataka.')
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => { fetchAll() }, [])

  const handleCreateItem = async (e) => {
    e.preventDefault()
    try {
      await createMenuItem(newItem, token)
      setNewItem({ menuItemName: '', description: '', price: 0, calories: 0, isAvailable: true, categoryIds: [], ingredientIds: [] })
      fetchAll()
    } catch (err) { setError('Greška pri kreiranju stavke.') }
  }

  const handleUpdateItem = async () => {
    try {
      await updateMenuItem(editItem.idMenuItem, {
        menuItemName: editItem.menuItemName,
        description: editItem.description,
        price: editItem.price,
        calories: editItem.calories,
        isAvailable: editItem.isAvailable,
        categoryIds: editItem.categories.map(c => c.idCategory),
        ingredientIds: editItem.ingredients.map(i => i.idIngredient)
      }, token)
      setEditItem(null)
      fetchAll()
    } catch (err) { setError('Greška pri izmeni stavke.') }
  }

  const handleDeleteItem = async (id) => {
    if (!window.confirm('Obrisati stavku?')) return
    try {
      await deleteMenuItem(id, token)
      fetchAll()
    } catch (err) { setError('Greška pri brisanju stavke.') }
  }

  const handleCreateCategory = async (e) => {
    e.preventDefault()
    try {
      await createCategory({ categoryName: newCategory }, token)
      setNewCategory('')
      fetchAll()
    } catch (err) { setError('Greška pri kreiranju kategorije.') }
  }

  const handleDeleteCategory = async (id) => {
    if (!window.confirm('Obrisati kategoriju?')) return
    try {
      await deleteCategory(id, token)
      fetchAll()
    } catch (err) { setError('Greška pri brisanju kategorije.') }
  }

  const handleCreateIngredient = async (e) => {
    e.preventDefault()
    try {
      await createIngredient(newIngredient, token)
      setNewIngredient({ ingredientName: '', isAllergen: false })
      fetchAll()
    } catch (err) { setError('Greška pri kreiranju sastojka.') }
  }

  const handleDeleteIngredient = async (id) => {
    if (!window.confirm('Obrisati sastojak?')) return
    try {
      await deleteIngredient(id, token)
      fetchAll()
    } catch (err) { setError('Greška pri brisanju sastojka.') }
  }

  if (loading) return (
    <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: '60vh' }}>
      <div style={{ color: '#c9a84c', fontFamily: 'Georgia, serif', fontSize: '1.2rem' }}>
        Učitavanje...
      </div>
    </div>
  )

  const filteredItems = selectedCategory
  ? menuItems.filter(item => item.categories.some(c => c.idCategory === selectedCategory))
  : menuItems

  return (
    <div style={{ paddingBottom: '60px' }}>

      {/* Header */}
      <div style={{
        textAlign: 'center',
        padding: '48px 0 32px',
        borderBottom: '2px solid #c9a84c',
        marginBottom: '40px'
      }}>
        <p style={{ color: '#c9a84c', letterSpacing: '4px', fontSize: '0.85rem', marginBottom: '12px' }}>
          NAŠA PONUDA
        </p>
        <h1 style={{ color: '#1a1a1a', fontFamily: 'Georgia, serif', fontSize: '2.8rem', marginBottom: '12px' }}>
          Meni
        </h1>
        <p style={{ color: '#6b6457', fontSize: '1rem', maxWidth: '500px', margin: '0 auto' }}>
          Sveže pripremljeno, sa ljubavlju za svaki zalogaj
        </p>
      </div>

      {error && (
        <div style={{
          backgroundColor: '#3a1a1a',
          border: '1px solid #c0392b',
          color: '#e74c3c',
          padding: '12px 16px',
          borderRadius: '6px',
          marginBottom: '24px'
        }}>
          {error}
        </div>
      )}

      {!token && (
        <div style={{
          backgroundColor: '#1a1a1a',
          border: '1px solid #c9a84c',
          color: '#c9a84c',
          padding: '14px 20px',
          borderRadius: '8px',
          marginBottom: '32px',
          textAlign: 'center',
          fontFamily: 'Georgia, serif'
        }}>
          Prijavite se da biste videli kompletan meni →{' '}
          <span onClick={() => window.location.href = '/login'}
            style={{ textDecoration: 'underline', cursor: 'pointer' }}>
            Login
          </span>
        </div>
      )}

      {/* Tabs - samo admin */}
      {isAdmin && (
        <div style={{ display: 'flex', gap: '12px', marginBottom: '36px' }}>
          {['menu', 'categories', 'ingredients'].map(tab => (
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
              {tab === 'menu' ? 'Jela' : tab === 'categories' ? 'Kategorije' : 'Sastojci'}
            </button>
          ))}
        </div>
      )}

      {/* MENU ITEMS TAB */}
      {activeTab === 'menu' && (
        <div>
          {/* Add Form - samo admin */}
          {isAdmin && (
            <div style={cardStyle}>
              <h5 style={cardTitleStyle}>Dodaj novo jelo</h5>
              <form onSubmit={handleCreateItem}>
                <div className="row g-3">
                  <div className="col-md-6">
                    <label style={labelStyle}>Naziv</label>
                    <input style={inputStyle} value={newItem.menuItemName}
                      onFocus={e => e.target.style.borderColor = '#c9a84c'}
                      onBlur={e => e.target.style.borderColor = '#444'}
                      onChange={(e) => setNewItem({ ...newItem, menuItemName: e.target.value })} required />
                  </div>
                  <div className="col-md-6">
                    <label style={labelStyle}>Opis</label>
                    <input style={inputStyle} value={newItem.description}
                      onFocus={e => e.target.style.borderColor = '#c9a84c'}
                      onBlur={e => e.target.style.borderColor = '#444'}
                      onChange={(e) => setNewItem({ ...newItem, description: e.target.value })} />
                  </div>
                  <div className="col-md-3">
                    <label style={labelStyle}>Cijena (RSD)</label>
                    <input type="number" style={inputStyle} value={newItem.price}
                      onFocus={e => e.target.style.borderColor = '#c9a84c'}
                      onBlur={e => e.target.style.borderColor = '#444'}
                      onChange={(e) => setNewItem({ ...newItem, price: parseFloat(e.target.value) })} />
                  </div>
                  <div className="col-md-3">
                    <label style={labelStyle}>Kalorije</label>
                    <input type="number" style={inputStyle} value={newItem.calories}
                      onFocus={e => e.target.style.borderColor = '#c9a84c'}
                      onBlur={e => e.target.style.borderColor = '#444'}
                      onChange={(e) => setNewItem({ ...newItem, calories: parseInt(e.target.value) })} />
                  </div>
                  <div className="col-md-3 d-flex align-items-end">
                    <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
                      <input type="checkbox" checked={newItem.isAvailable}
                        onChange={(e) => setNewItem({ ...newItem, isAvailable: e.target.checked })}
                        style={{ accentColor: '#c9a84c', width: '16px', height: '16px' }} />
                      <label style={{ color: '#c9a84c', fontFamily: 'Georgia, serif' }}>Dostupno</label>
                    </div>
                  </div>
                  <div className="col-md-6">
                    <label style={labelStyle}>Kategorije</label>
                    <div style={{ display: 'flex', flexWrap: 'wrap', gap: '8px' }}>
                      {categories.map(c => (
                        <label key={c.idCategory} style={{ display: 'flex', alignItems: 'center', gap: '6px', color: '#f5f0e8', cursor: 'pointer' }}>
                          <input type="checkbox"
                            style={{ accentColor: '#c9a84c' }}
                            checked={newItem.categoryIds.includes(c.idCategory)}
                            onChange={() => {
                              const updated = newItem.categoryIds.includes(c.idCategory)
                                ? newItem.categoryIds.filter(x => x !== c.idCategory)
                                : [...newItem.categoryIds, c.idCategory]
                              setNewItem({ ...newItem, categoryIds: updated })
                            }} />
                          {c.categoryName}
                        </label>
                      ))}
                    </div>
                  </div>
                  <div className="col-md-6">
                    <label style={labelStyle}>Sastojci</label>
                    <div style={{ display: 'flex', flexWrap: 'wrap', gap: '8px' }}>
                      {ingredients.map(i => (
                        <label key={i.idIngredient} style={{ display: 'flex', alignItems: 'center', gap: '6px', color: '#f5f0e8', cursor: 'pointer' }}>
                          <input type="checkbox"
                            style={{ accentColor: '#c9a84c' }}
                            checked={newItem.ingredientIds.includes(i.idIngredient)}
                            onChange={() => {
                              const updated = newItem.ingredientIds.includes(i.idIngredient)
                                ? newItem.ingredientIds.filter(x => x !== i.idIngredient)
                                : [...newItem.ingredientIds, i.idIngredient]
                              setNewItem({ ...newItem, ingredientIds: updated })
                            }} />
                          {i.ingredientName} {i.isAllergen ? '⚠️' : ''}
                        </label>
                      ))}
                    </div>
                  </div>
                </div>
                <button type="submit" style={submitButtonStyle}
                  onMouseEnter={e => e.target.style.backgroundColor = '#e8c96d'}
                  onMouseLeave={e => e.target.style.backgroundColor = '#c9a84c'}>
                  Dodaj jelo
                </button>
              </form>
            </div>
          )}

          {/* Edit Form */}
          {editItem && (
            <div style={{ ...cardStyle, borderColor: '#e8c96d' }}>
              <h5 style={cardTitleStyle}>Izmeni jelo</h5>
              <div className="row g-3">
                <div className="col-md-6">
                  <label style={labelStyle}>Naziv</label>
                  <input style={inputStyle} value={editItem.menuItemName}
                    onFocus={e => e.target.style.borderColor = '#c9a84c'}
                    onBlur={e => e.target.style.borderColor = '#444'}
                    onChange={(e) => setEditItem({ ...editItem, menuItemName: e.target.value })} />
                </div>
                <div className="col-md-6">
                  <label style={labelStyle}>Opis</label>
                  <input style={inputStyle} value={editItem.description}
                    onFocus={e => e.target.style.borderColor = '#c9a84c'}
                    onBlur={e => e.target.style.borderColor = '#444'}
                    onChange={(e) => setEditItem({ ...editItem, description: e.target.value })} />
                </div>
                <div className="col-md-3">
                  <label style={labelStyle}>Cijena (RSD)</label>
                  <input type="number" style={inputStyle} value={editItem.price}
                    onFocus={e => e.target.style.borderColor = '#c9a84c'}
                    onBlur={e => e.target.style.borderColor = '#444'}
                    onChange={(e) => setEditItem({ ...editItem, price: parseFloat(e.target.value) })} />
                </div>
                <div className="col-md-3">
                  <label style={labelStyle}>Kalorije</label>
                  <input type="number" style={inputStyle} value={editItem.calories}
                    onFocus={e => e.target.style.borderColor = '#c9a84c'}
                    onBlur={e => e.target.style.borderColor = '#444'}
                    onChange={(e) => setEditItem({ ...editItem, calories: parseInt(e.target.value) })} />
                </div>
                <div className="col-md-3 d-flex align-items-end">
                  <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
                    <input type="checkbox" checked={editItem.isAvailable}
                      onChange={(e) => setEditItem({ ...editItem, isAvailable: e.target.checked })}
                      style={{ accentColor: '#c9a84c', width: '16px', height: '16px' }} />
                    <label style={{ color: '#c9a84c', fontFamily: 'Georgia, serif' }}>Dostupno</label>
                  </div>
                </div>
              </div>
              <div style={{ marginTop: '20px', display: 'flex', gap: '12px' }}>
                <button style={submitButtonStyle} onClick={handleUpdateItem}
                  onMouseEnter={e => e.target.style.backgroundColor = '#e8c96d'}
                  onMouseLeave={e => e.target.style.backgroundColor = '#c9a84c'}>
                  Sačuvaj
                </button>
                <button onClick={() => setEditItem(null)} style={cancelButtonStyle}
                  onMouseEnter={e => { e.target.style.backgroundColor = '#c9a84c'; e.target.style.color = '#1a1a1a' }}
                  onMouseLeave={e => { e.target.style.backgroundColor = 'transparent'; e.target.style.color = '#c9a84c' }}>
                  Otkaži
                </button>
              </div>
            </div>
          )}

          {/* Filter po kategorijama */}
          <div style={{ display: 'flex', flexWrap: 'wrap', gap: '10px', marginBottom: '32px' }}>
            <button
              onClick={() => setSelectedCategory(null)}
              style={{
                backgroundColor: selectedCategory === null ? '#c9a84c' : 'transparent',
                color: selectedCategory === null ? '#1a1a1a' : '#c9a84c',
                border: '1px solid #c9a84c',
                padding: '6px 20px',
                borderRadius: '20px',
                fontFamily: 'Georgia, serif',
                fontSize: '0.85rem',
                cursor: 'pointer'
              }}>
              Sve
            </button>
            {categories.map(c => (
              <button key={c.idCategory}
                onClick={() => setSelectedCategory(c.idCategory)}
                style={{
                  backgroundColor: selectedCategory === c.idCategory ? '#c9a84c' : 'transparent',
                  color: selectedCategory === c.idCategory ? '#1a1a1a' : '#c9a84c',
                  border: '1px solid #c9a84c',
                  padding: '6px 20px',
                  borderRadius: '20px',
                  fontFamily: 'Georgia, serif',
                  fontSize: '0.85rem',
                  cursor: 'pointer'
                }}>
                {c.categoryName}
              </button>
            ))}
          </div>

          {/* Menu Items Cards */}
          <div className="row g-4">
            {filteredItems.map(item => (
              <div key={item.idMenuItem} className="col-md-4">
                <div style={{
                  backgroundColor: item.isAvailable ? '#1a1a1a' : '#111',
                  border: '1px solid #c9a84c',
                  borderRadius: '12px',
                  padding: '28px',
                  height: '100%',
                  opacity: item.isAvailable ? 1 : 0.6,
                  display: 'flex',
                  flexDirection: 'column',
                  justifyContent: 'space-between'
                }}>
                  <div>
                    <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', marginBottom: '12px' }}>
                      <h5 style={{ color: '#f5f0e8', fontFamily: 'Georgia, serif', margin: 0 }}>
                        {item.menuItemName}
                      </h5>
                      <span style={{
                        backgroundColor: item.isAvailable ? '#1a3a1a' : '#3a1a1a',
                        color: item.isAvailable ? '#2ecc71' : '#e74c3c',
                        padding: '3px 10px',
                        borderRadius: '20px',
                        fontSize: '0.75rem',
                        whiteSpace: 'nowrap',
                        marginLeft: '8px'
                      }}>
                        {item.isAvailable ? 'Dostupno' : 'Nije dostupno'}
                      </span>
                    </div>
                    <p style={{ color: '#9b9080', fontSize: '0.9rem', marginBottom: '16px' }}>
                      {item.description}
                    </p>
                    <div style={{ marginBottom: '12px' }}>
                      {item.categories.map(c => (
                        <span key={c.idCategory} style={{
                          backgroundColor: '#2c2c2c',
                          color: '#c9a84c',
                          border: '1px solid #c9a84c',
                          padding: '2px 10px',
                          borderRadius: '20px',
                          fontSize: '0.75rem',
                          marginRight: '6px'
                        }}>
                          {c.categoryName}
                        </span>
                      ))}
                    </div>
                    <div style={{ marginBottom: '16px' }}>
                      {item.ingredients.map(i => (
                        <span key={i.idIngredient} style={{
                          backgroundColor: i.isAllergen ? '#3a1a1a' : '#2c2c2c',
                          color: i.isAllergen ? '#e74c3c' : '#9b9080',
                          padding: '2px 8px',
                          borderRadius: '20px',
                          fontSize: '0.75rem',
                          marginRight: '4px',
                          marginBottom: '4px',
                          display: 'inline-block'
                        }}>
                          {i.ingredientName} {i.isAllergen ? '⚠️' : ''}
                        </span>
                      ))}
                    </div>
                  </div>
                  <div style={{ borderTop: '1px solid #2c2c2c', paddingTop: '16px' }}>
                    <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                      <div>
                        <span style={{ color: '#c9a84c', fontFamily: 'Georgia, serif', fontSize: '1.3rem', fontWeight: 'bold' }}>
                          {item.price} RSD
                        </span>
                        <span style={{ color: '#6b6457', fontSize: '0.85rem', marginLeft: '12px' }}>
                          {item.calories} kcal
                        </span>
                      </div>
                      {isAdmin && (
                        <div style={{ display: 'flex', gap: '8px' }}>
                          <button onClick={() => setEditItem({ ...item })} style={smallButtonStyle}
                            onMouseEnter={e => { e.target.style.backgroundColor = '#c9a84c'; e.target.style.color = '#1a1a1a' }}
                            onMouseLeave={e => { e.target.style.backgroundColor = 'transparent'; e.target.style.color = '#c9a84c' }}>
                            Izmeni
                          </button>
                          <button onClick={() => handleDeleteItem(item.idMenuItem)} style={{ ...smallButtonStyle, borderColor: '#e74c3c', color: '#e74c3c' }}
                            onMouseEnter={e => { e.target.style.backgroundColor = '#e74c3c'; e.target.style.color = '#fff' }}
                            onMouseLeave={e => { e.target.style.backgroundColor = 'transparent'; e.target.style.color = '#e74c3c' }}>
                            Obriši
                          </button>
                        </div>
                      )}
                    </div>
                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>
      )}

      {/* CATEGORIES TAB */}
      {activeTab === 'categories' && isAdmin && (
        <div>
          <div style={cardStyle}>
            <h5 style={cardTitleStyle}>Dodaj kategoriju</h5>
            <form onSubmit={handleCreateCategory} style={{ display: 'flex', gap: '12px' }}>
              <input style={{ ...inputStyle, maxWidth: '300px' }} placeholder="Naziv kategorije"
                value={newCategory}
                onFocus={e => e.target.style.borderColor = '#c9a84c'}
                onBlur={e => e.target.style.borderColor = '#444'}
                onChange={(e) => setNewCategory(e.target.value)} required />
              <button type="submit" style={submitButtonStyle}
                onMouseEnter={e => e.target.style.backgroundColor = '#e8c96d'}
                onMouseLeave={e => e.target.style.backgroundColor = '#c9a84c'}>
                Dodaj
              </button>
            </form>
          </div>
          <div style={{ display: 'flex', flexWrap: 'wrap', gap: '12px', marginTop: '24px' }}>
            {categories.map(c => (
              <div key={c.idCategory} style={{
                backgroundColor: '#1a1a1a',
                border: '1px solid #c9a84c',
                borderRadius: '8px',
                padding: '12px 20px',
                display: 'flex',
                alignItems: 'center',
                gap: '16px'
              }}>
                <span style={{ color: '#f5f0e8', fontFamily: 'Georgia, serif' }}>{c.categoryName}</span>
                <button onClick={() => handleDeleteCategory(c.idCategory)}
                  style={{ ...smallButtonStyle, borderColor: '#e74c3c', color: '#e74c3c' }}
                  onMouseEnter={e => { e.target.style.backgroundColor = '#e74c3c'; e.target.style.color = '#fff' }}
                  onMouseLeave={e => { e.target.style.backgroundColor = 'transparent'; e.target.style.color = '#e74c3c' }}>
                  Obriši
                </button>
              </div>
            ))}
          </div>
        </div>
      )}

      {/* INGREDIENTS TAB */}
      {activeTab === 'ingredients' && isAdmin && (
        <div>
          <div style={cardStyle}>
            <h5 style={cardTitleStyle}>Dodaj sastojak</h5>
            <form onSubmit={handleCreateIngredient} style={{ display: 'flex', gap: '12px', alignItems: 'center' }}>
              <input style={{ ...inputStyle, maxWidth: '300px' }} placeholder="Naziv sastojka"
                value={newIngredient.ingredientName}
                onFocus={e => e.target.style.borderColor = '#c9a84c'}
                onBlur={e => e.target.style.borderColor = '#444'}
                onChange={(e) => setNewIngredient({ ...newIngredient, ingredientName: e.target.value })} required />
              <label style={{ display: 'flex', alignItems: 'center', gap: '8px', color: '#c9a84c', fontFamily: 'Georgia, serif', whiteSpace: 'nowrap' }}>
                <input type="checkbox" checked={newIngredient.isAllergen}
                  style={{ accentColor: '#c9a84c' }}
                  onChange={(e) => setNewIngredient({ ...newIngredient, isAllergen: e.target.checked })} />
                Alergen
              </label>
              <button type="submit" style={submitButtonStyle}
                onMouseEnter={e => e.target.style.backgroundColor = '#e8c96d'}
                onMouseLeave={e => e.target.style.backgroundColor = '#c9a84c'}>
                Dodaj
              </button>
            </form>
          </div>
          <div style={{ display: 'flex', flexWrap: 'wrap', gap: '12px', marginTop: '24px' }}>
            {ingredients.map(i => (
              <div key={i.idIngredient} style={{
                backgroundColor: '#1a1a1a',
                border: `1px solid ${i.isAllergen ? '#e74c3c' : '#c9a84c'}`,
                borderRadius: '8px',
                padding: '12px 20px',
                display: 'flex',
                alignItems: 'center',
                gap: '16px'
              }}>
                <span style={{ color: '#f5f0e8', fontFamily: 'Georgia, serif' }}>
                  {i.ingredientName} {i.isAllergen ? '⚠️' : ''}
                </span>
                <button onClick={() => handleDeleteIngredient(i.idIngredient)}
                  style={{ ...smallButtonStyle, borderColor: '#e74c3c', color: '#e74c3c' }}
                  onMouseEnter={e => { e.target.style.backgroundColor = '#e74c3c'; e.target.style.color = '#fff' }}
                  onMouseLeave={e => { e.target.style.backgroundColor = 'transparent'; e.target.style.color = '#e74c3c' }}>
                  Obriši
                </button>
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
  padding: '10px 24px',
  borderRadius: '6px',
  fontFamily: 'Georgia, serif',
  fontSize: '0.95rem',
  fontWeight: 'bold',
  cursor: 'pointer',
  letterSpacing: '0.5px',
  whiteSpace: 'nowrap'
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

export default MenuPage