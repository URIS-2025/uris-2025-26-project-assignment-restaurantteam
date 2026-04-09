import axios from 'axios'

const menuClient = axios.create({
  baseURL: 'https://localhost:44385'
})

const getAuthHeader = (token) => ({
  headers: { Authorization: `Bearer ${token}` }
})

// Menu Items
export const getAllMenuItems = (token) => {
  return token
    ? menuClient.get('/api/menu', getAuthHeader(token))
    : menuClient.get('/api/menu') 
}

export const getMenuItemById = (id, token) => {
  return menuClient.get(`/api/menu/${id}`, getAuthHeader(token))
}

export const createMenuItem = (data, token) => {
  return menuClient.post('/api/menu', data, getAuthHeader(token))
}

export const updateMenuItem = (id, data, token) => {
  return menuClient.put(`/api/menu/${id}`, data, getAuthHeader(token))
}

export const deleteMenuItem = (id, token) => {
  return menuClient.delete(`/api/menu/${id}`, getAuthHeader(token))
}

// Categories
export const getAllCategories = () => {
  return menuClient.get('/api/menu/categories')
}

export const createCategory = (data, token) => {
  return menuClient.post('/api/menu/categories', data, getAuthHeader(token))
}

export const deleteCategory = (id, token) => {
  return menuClient.delete(`/api/menu/categories/${id}`, getAuthHeader(token))
}

// Ingredients
export const getAllIngredients = () => {
  return menuClient.get('/api/menu/ingredients')
}

export const createIngredient = (data, token) => {
  return menuClient.post('/api/menu/ingredients', data, getAuthHeader(token))
}

export const deleteIngredient = (id, token) => {
  return menuClient.delete(`/api/menu/ingredients/${id}`, getAuthHeader(token))
}