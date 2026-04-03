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

  // New item form
  const [newItem, setNewItem] = useState({
    menuItemName: '', description: '', price: 0,
    calories: 0, isAvailable: true, categoryIds: [], ingredientIds: []
  })

  // Edit
  const [editItem, setEditItem] = useState(null)

  // New category/ingredient
  const [newCategory, setNewCategory] = useState('')
  const [newIngredient, setNewIngredient] = useState({ ingredientName: '', isAllergen: false })

  const fetchAll = async () => {
    try {
      const [menuRes, catRes, ingRes] = await Promise.all([
        getAllMenuItems(token),
        getAllCategories(),
        getAllIngredients()
      ])
      setMenuItems(menuRes.data)
      setCategories(catRes.data)
      setIngredients(ingRes.data)
    } catch (err) {
      setError('Greška pri učitavanju podataka.')
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => { fetchAll() }, [])

  // Menu Item CRUD
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

  // Category CRUD
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

  // Ingredient CRUD
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

  const toggleCategoryId = (id, list, setList) => {
    setList(prev => prev.includes(id) ? prev.filter(x => x !== id) : [...prev, id])
  }

  if (loading) return <div className="text-center mt-5"><div className="spinner-border"></div></div>

  return (
    <div>
      <h2 className="mb-4">Meni</h2>
      {error && <div className="alert alert-danger">{error}</div>}

      {/* Tabs */}
      <ul className="nav nav-tabs mb-4">
        <li className="nav-item">
          <button className={`nav-link ${activeTab === 'menu' ? 'active' : ''}`} onClick={() => setActiveTab('menu')}>Jela</button>
        </li>
        {isAdmin && (
          <>
            <li className="nav-item">
              <button className={`nav-link ${activeTab === 'categories' ? 'active' : ''}`} onClick={() => setActiveTab('categories')}>Kategorije</button>
            </li>
            <li className="nav-item">
              <button className={`nav-link ${activeTab === 'ingredients' ? 'active' : ''}`} onClick={() => setActiveTab('ingredients')}>Sastojci</button>
            </li>
          </>
        )}
      </ul>

      {/* MENU ITEMS TAB */}
      {activeTab === 'menu' && (
        <div>
          {/* Add Form - samo admin */}
          {isAdmin && (
            <div className="card p-4 mb-4">
              <h5>Dodaj novo jelo</h5>
              <form onSubmit={handleCreateItem}>
                <div className="row g-3">
                  <div className="col-md-6">
                    <label className="form-label">Naziv</label>
                    <input className="form-control" value={newItem.menuItemName}
                      onChange={(e) => setNewItem({ ...newItem, menuItemName: e.target.value })} required />
                  </div>
                  <div className="col-md-6">
                    <label className="form-label">Opis</label>
                    <input className="form-control" value={newItem.description}
                      onChange={(e) => setNewItem({ ...newItem, description: e.target.value })} />
                  </div>
                  <div className="col-md-3">
                    <label className="form-label">Cijena (KM)</label>
                    <input type="number" className="form-control" value={newItem.price}
                      onChange={(e) => setNewItem({ ...newItem, price: parseFloat(e.target.value) })} />
                  </div>
                  <div className="col-md-3">
                    <label className="form-label">Kalorije</label>
                    <input type="number" className="form-control" value={newItem.calories}
                      onChange={(e) => setNewItem({ ...newItem, calories: parseInt(e.target.value) })} />
                  </div>
                  <div className="col-md-3 d-flex align-items-end">
                    <div className="form-check">
                      <input type="checkbox" className="form-check-input" checked={newItem.isAvailable}
                        onChange={(e) => setNewItem({ ...newItem, isAvailable: e.target.checked })} />
                      <label className="form-check-label">Dostupno</label>
                    </div>
                  </div>
                  <div className="col-md-6">
                    <label className="form-label">Kategorije</label>
                    <div className="d-flex flex-wrap gap-2">
                      {categories.map(c => (
                        <div key={c.idCategory} className="form-check">
                          <input type="checkbox" className="form-check-input"
                            checked={newItem.categoryIds.includes(c.idCategory)}
                            onChange={() => {
                              const updated = newItem.categoryIds.includes(c.idCategory)
                                ? newItem.categoryIds.filter(x => x !== c.idCategory)
                                : [...newItem.categoryIds, c.idCategory]
                              setNewItem({ ...newItem, categoryIds: updated })
                            }} />
                          <label className="form-check-label">{c.categoryName}</label>
                        </div>
                      ))}
                    </div>
                  </div>
                  <div className="col-md-6">
                    <label className="form-label">Sastojci</label>
                    <div className="d-flex flex-wrap gap-2">
                      {ingredients.map(i => (
                        <div key={i.idIngredient} className="form-check">
                          <input type="checkbox" className="form-check-input"
                            checked={newItem.ingredientIds.includes(i.idIngredient)}
                            onChange={() => {
                              const updated = newItem.ingredientIds.includes(i.idIngredient)
                                ? newItem.ingredientIds.filter(x => x !== i.idIngredient)
                                : [...newItem.ingredientIds, i.idIngredient]
                              setNewItem({ ...newItem, ingredientIds: updated })
                            }} />
                          <label className="form-check-label">{i.ingredientName} {i.isAllergen ? '⚠️' : ''}</label>
                        </div>
                      ))}
                    </div>
                  </div>
                </div>
                <button type="submit" className="btn btn-dark mt-3">Dodaj jelo</button>
              </form>
            </div>
          )}

          {/* Edit Form */}
          {editItem && (
            <div className="card p-4 mb-4 border-warning">
              <h5>Izmeni jelo</h5>
              <div className="row g-3">
                <div className="col-md-6">
                  <label className="form-label">Naziv</label>
                  <input className="form-control" value={editItem.menuItemName}
                    onChange={(e) => setEditItem({ ...editItem, menuItemName: e.target.value })} />
                </div>
                <div className="col-md-6">
                  <label className="form-label">Opis</label>
                  <input className="form-control" value={editItem.description}
                    onChange={(e) => setEditItem({ ...editItem, description: e.target.value })} />
                </div>
                <div className="col-md-3">
                  <label className="form-label">Cijena (KM)</label>
                  <input type="number" className="form-control" value={editItem.price}
                    onChange={(e) => setEditItem({ ...editItem, price: parseFloat(e.target.value) })} />
                </div>
                <div className="col-md-3">
                  <label className="form-label">Kalorije</label>
                  <input type="number" className="form-control" value={editItem.calories}
                    onChange={(e) => setEditItem({ ...editItem, calories: parseInt(e.target.value) })} />
                </div>
                <div className="col-md-3 d-flex align-items-end">
                  <div className="form-check">
                    <input type="checkbox" className="form-check-input" checked={editItem.isAvailable}
                      onChange={(e) => setEditItem({ ...editItem, isAvailable: e.target.checked })} />
                    <label className="form-check-label">Dostupno</label>
                  </div>
                </div>
              </div>
              <div className="mt-3">
                <button className="btn btn-success me-2" onClick={handleUpdateItem}>Sačuvaj</button>
                <button className="btn btn-secondary" onClick={() => setEditItem(null)}>Otkaži</button>
              </div>
            </div>
          )}

          {/* Menu Items List */}
          <div className="row g-4">
            {menuItems.map(item => (
              <div key={item.idMenuItem} className="col-md-4">
                <div className={`card h-100 ${!item.isAvailable ? 'opacity-50' : ''}`}>
                  <div className="card-body">
                    <h5 className="card-title">{item.menuItemName}</h5>
                    <p className="card-text text-muted">{item.description}</p>
                    <p className="card-text">
                      <strong>{item.price} KM</strong> · {item.calories} kcal
                    </p>
                    <div className="mb-2">
                      {item.categories.map(c => (
                        <span key={c.idCategory} className="badge bg-secondary me-1">{c.categoryName}</span>
                      ))}
                    </div>
                    <div className="mb-2">
                      {item.ingredients.map(i => (
                        <span key={i.idIngredient} className={`badge me-1 ${i.isAllergen ? 'bg-danger' : 'bg-light text-dark'}`}>
                          {i.ingredientName} {i.isAllergen ? '⚠️' : ''}
                        </span>
                      ))}
                    </div>
                    <span className={`badge ${item.isAvailable ? 'bg-success' : 'bg-danger'}`}>
                      {item.isAvailable ? 'Dostupno' : 'Nije dostupno'}
                    </span>
                  </div>
                  {isAdmin && (
                    <div className="card-footer">
                      <button className="btn btn-sm btn-warning me-2" onClick={() => setEditItem({ ...item })}>Izmeni</button>
                      <button className="btn btn-sm btn-danger" onClick={() => handleDeleteItem(item.idMenuItem)}>Obriši</button>
                    </div>
                  )}
                </div>
              </div>
            ))}
          </div>
        </div>
      )}

      {/* CATEGORIES TAB */}
      {activeTab === 'categories' && isAdmin && (
        <div>
          <form onSubmit={handleCreateCategory} className="d-flex gap-2 mb-4">
            <input className="form-control w-25" placeholder="Nova kategorija"
              value={newCategory} onChange={(e) => setNewCategory(e.target.value)} required />
            <button type="submit" className="btn btn-dark">Dodaj</button>
          </form>
          <table className="table table-bordered">
            <thead className="table-dark">
              <tr><th>ID</th><th>Naziv</th><th>Akcije</th></tr>
            </thead>
            <tbody>
              {categories.map(c => (
                <tr key={c.idCategory}>
                  <td>{c.idCategory}</td>
                  <td>{c.categoryName}</td>
                  <td><button className="btn btn-sm btn-danger" onClick={() => handleDeleteCategory(c.idCategory)}>Obriši</button></td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {/* INGREDIENTS TAB */}
      {activeTab === 'ingredients' && isAdmin && (
        <div>
          <form onSubmit={handleCreateIngredient} className="d-flex gap-2 mb-4 align-items-center">
            <input className="form-control w-25" placeholder="Naziv sastojka"
              value={newIngredient.ingredientName}
              onChange={(e) => setNewIngredient({ ...newIngredient, ingredientName: e.target.value })} required />
            <div className="form-check">
              <input type="checkbox" className="form-check-input" checked={newIngredient.isAllergen}
                onChange={(e) => setNewIngredient({ ...newIngredient, isAllergen: e.target.checked })} />
              <label className="form-check-label">Alergen</label>
            </div>
            <button type="submit" className="btn btn-dark">Dodaj</button>
          </form>
          <table className="table table-bordered">
            <thead className="table-dark">
              <tr><th>ID</th><th>Naziv</th><th>Alergen</th><th>Akcije</th></tr>
            </thead>
            <tbody>
              {ingredients.map(i => (
                <tr key={i.idIngredient}>
                  <td>{i.idIngredient}</td>
                  <td>{i.ingredientName}</td>
                  <td>{i.isAllergen ? '⚠️ Da' : 'Ne'}</td>
                  <td><button className="btn btn-sm btn-danger" onClick={() => handleDeleteIngredient(i.idIngredient)}>Obriši</button></td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  )
}

export default MenuPage