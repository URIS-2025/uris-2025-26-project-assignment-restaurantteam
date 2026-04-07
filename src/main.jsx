import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import 'bootstrap/dist/css/bootstrap.min.css'
import App from './App.jsx'

/* Izloguj korisnika pri svakom pokretanju
localStorage.removeItem('token')
localStorage.removeItem('role')*/

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <App />
  </StrictMode>,
)